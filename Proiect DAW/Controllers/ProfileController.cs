﻿using System.Linq;
using Services;
using System.IO;
using Proiect_DAW.Models.ProfileModels;
using Proiect_DAW.Models.DomainModels;
using System.Collections.Generic;
using Proiect_DAW.Models.JsonModels;
using System.Web.Mvc;
using Proiect_DAW.Code.Base;
using DataAccess;
using Services.InterestsUsers;
using Services.Interest;
using Services.User;
using Services.County;
using Services.FriendShip;
using Services.Album;
using Services.Photo;
using Services.Message;

namespace Proiect_DAW.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly Services.County.CountyService countyService;
        private readonly Services.User.UserService userService;
        private readonly Services.FriendShip.FriendshipService friendService;
        private readonly Services.Interest.InterestService interestService;
        private readonly Services.InterestsUsers.InterestsUsersService interestsUsersService;
        private readonly Services.Album.AlbumService albumService;
        private readonly Services.Photo.PhotoService photoService;
        private readonly Services.Message.MessageService messageService;


        public ProfileController()
            : base()
        {
            interestsUsersService = new InterestsUsersService(new SocializRUnitOfWork(new SocializRContext()));
            interestService = new InterestService(new SocializRUnitOfWork(new SocializRContext()));
            userService = new UserService( new SocializRUnitOfWork(new SocializRContext()));
            countyService = new CountyService(new SocializRUnitOfWork(new SocializRContext()));
            friendService = new FriendshipService( new SocializRUnitOfWork(new SocializRContext()));
            albumService = new AlbumService( new SocializRUnitOfWork(new SocializRContext()));
            photoService = new PhotoService(new SocializRUnitOfWork(new SocializRContext()));
            messageService = new MessageService(new SocializRUnitOfWork(new SocializRContext()));
        }

        [HttpGet]
        public ActionResult Albums()
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            var albums = albumService.GetAll(currentUser.Id,currentUser);
            AlbumsViewModel model = new AlbumsViewModel()
            {
                CanEdit = true,
                Album = albums.Select(e => 
                new AlbumDomainModel() {
                Count=e.Photo.Count,
                CoverPhoto = e.Photo.Count>0?e.Photo.OrderBy(f=>f.Position).FirstOrDefault().Id:-1,
                Id=e.Id,
                Name=e.Name
                }

                ).ToList(),
                AddAlbumModel = new AddAlbumModel()

            };
            return View(model);
        }
        [HttpGet]
        public JsonResult GetInterests()
        {
            MakeCurrentUser();
            var indexes = interestsUsersService.GetAllInterests(currentUser.Id).Select(e=>e.Id).ToList();
            var interests = interestService.GetAll().Select(e =>
             //{
             //    //var item = mapper.Map<InterestSelectJsonModel>(e);
                
             //    item.Selected = indexes.Contains(e.Id);
             //    return item;

             //}
             new InterestSelectJsonModel() { Id=e.Id,
             Text=e.Name,
             Selected=indexes.Contains(e.Id)}
             ).ToList();
            return Json(interests,JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public bool ChangeDescription(int? photoId,string description)
        {
            if (photoId == null || !photoService.HasThisPhoto(photoId.Value, currentUser.Id))
            {
                return false;
            }
            photoService.ChangeDescription(photoId.Value, description);
            return true;
        }

        [HttpGet]
        public JsonResult GetPhotosJson(int? toSkip,int? albumId)
        {
            MakeCurrentUser();
            if (toSkip == null || albumId == null)
            {
                return Json(new List<int>(),JsonRequestBehavior.AllowGet);
            }
            if (albumService.CanSeeAlbum(albumId.Value,currentUser))
            {
                var photos = photoService.GetPhotos(toSkip.Value, PageSize, albumId.Value).Select(e => 
                //mapper.Map<ImageJsonModel>(e)
                new ImageJsonModel() { 
                
                AlbumId=e.AlbumId,
                Description=e.Description,
                Id=e.Id}
                ).ToList();
                return Json(photos, JsonRequestBehavior.AllowGet);
            }
            else return Json(new List<int>(), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Index()
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            var domainUser = userService.GetUserById(currentUser.Id);
            var user = new ProfileViewerModel()
            {
                Album = new List<AlbumDomainModel>(domainUser.Album.Select(e => new AlbumDomainModel() { Id = e.Id, Count = e.Photo.Count, Name = e.Name })),
                Interests = new List<string>(domainUser.InterestsUsers.Select(e => e.Interest.Name)),
                Locality = domainUser.Locality.Name,
                County = domainUser.Locality.County.Name,
                Birthday = domainUser.BirthDay.Year + "." + domainUser.BirthDay.Month + "." + domainUser.BirthDay.Day,
                PhotoId = domainUser.PhotoId,
                IsAdmin = domainUser.RoleId == 1,
                IsBanned = domainUser.IsBanned,
                Id = domainUser.Id,
                Email = domainUser.Email,
                Name = domainUser.Name,
                Surname = domainUser.Surname,
                SexualIdentity = domainUser.SexualIdentity,



            };
            user.Interests = interestsUsersService
                .GetAllInterests(domainUser.Id)
                .Select(e => e.Name)
                .ToList();
            user.Album = albumService
                .GetAll(currentUser.Id,currentUser)
                .Select(
                e=>
                new AlbumDomainModel() {
                Count=e.Photo.Count,
                CoverPhoto=e.Photo.Count>0?e.Photo.OrderBy(e=>e.Position).First().Id:-1,
                Id=e.Id,
                Name=e.Name
                }
                ).ToList();
            return View(user);
        }
        [HttpGet]
        public JsonResult GetPhotos(int? albumId)
        {
            MakeCurrentUser();
            if (albumId == null)
            {
                return Json( new List<PhotoDomainModel>(),JsonRequestBehavior.AllowGet);
            }
            if (albumService.CanSeeAlbum(albumId.Value,currentUser))
            {
                return Json(albumService
                    .GetPhotos(albumId.Value, currentUser)
                    .Select(e =>
                    //mapper.Map<PhotoDomainModel>(e)
                    new PhotoDomainModel()
                    {
                        Description = e.Description,
                        Id = e.Id
                    }
                    )
                    .ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json(new List<PhotoDomainModel>(),JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult MakeProfilePhoto(int? photoId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (photoId == null)
            {
                return NotFoundView();
            }
            if (!photoService.HasThisPhoto(photoId.Value,currentUser.Id))
            {
                return RedirectToAction("Index", "Profile");
            }
            userService.UpdateProfilePhoto(photoId.Value,currentUser);
            return RedirectToAction("Index", "Profile");

        }

        [HttpGet]
        public ActionResult Album(int? albumId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (albumId == null)
            {
                return NotFoundView();
            }
            if (!albumService.CanSeeAlbum(albumId.Value,currentUser))
            {
                return ForbidView();
            }

            var album = albumService.GetAlbum(albumId.Value);
            if (album == null)
            {
                return NotFoundView();
            }

            AlbumViewerModel albumViewerModel = new AlbumViewerModel()
            {
                PhotoModel = new PhotoModel() { AlbumId = albumId },
                HasThisAlbum = albumService.HasThisAlbum(albumId.Value,currentUser),
                Id=albumId.Value,
                Name = album.Name
            };

            return View(albumViewerModel);
        }

        [HttpPost]
        public ActionResult AlbumChangeName(AlbumViewerModel model)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return View("Album",model);
            }
            albumService.ChangeName(model.Id, model.Name);
            return RedirectToAction("Album", "Profile",new {albumId=model.Id });

        }

        [HttpGet]
        public ActionResult RemovePhoto(int? photoId,int? albumId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (photoId == null||albumId==null )
            {
                return NotFoundView();
            }
            if(!photoService.HasThisPhoto(photoId.Value, currentUser.Id))
            {
                return ForbidView();
            }

            photoService.RemovePhoto(photoId.Value);

            return RedirectToAction("Album", new { albumId = albumId.Value });
        }

        [HttpPost]
        public ActionResult AddPhoto(PhotoModel model,int? albumId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (ModelState.IsValid)
            {

                var photo = new Domain.Photo()
                {
                    AlbumId = albumId,
                    PostId = null,
                    MIMEType = model.Binar.ContentType
                };
                using (var memoryStream = new MemoryStream())
                {
                    model.Binar.InputStream.CopyTo(memoryStream);
                    photo.Binary = memoryStream.ToArray();
                    
                }
                photoService.AddPhoto(photo,currentUser);

                return RedirectToAction("Album", "Profile", new { albumId = model.AlbumId });
            }
            if (albumId == null)
            {
                return NotFoundView();
            }
            AlbumViewerModel albumViewerModel = new AlbumViewerModel()
            {
                PhotoModel = model,
                HasThisAlbum = albumService.HasThisAlbum(albumId.Value,currentUser),
                Id = albumId.Value,
                Name=albumService.GetAlbum(albumId.Value).Name
            };
            return View("Album", albumViewerModel);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            var user = userService.GetUserById(currentUser.Id);
            //var model = mapper.Map<EditUserModel>(user);
            var model = new EditUserModel()
            {
                Visibility = user.Confidentiality,
                BirthDay=user.BirthDay,
                Id=user.Id,
                LocalityId=user.LocalityId,
                Name=user.Name,
                PhotoId=user.PhotoId,
                SexualIdentity=user.SexualIdentity,
                Surname=user.Surname
                
            };

            var counties = countyService.GetAll();

            var interests = interestService.GetAll();

            model.Counties = counties
                .Select(c => 
                new SelectListItem() {
                Text=c.Name,
                Value=c.Id.ToString()
                }
                )
                .ToList();
            model.Albume = albumService
                .GetAll(currentUser.Id,currentUser)
                .Select(e => 
                new AlbumDomainModel()
                {
                    Count = e.Photo.Count,
                    CoverPhoto = e.Photo.Count > 0 ? e.Photo.OrderBy(e => e.Position).First().Id : -1,
                    Id = e.Id,
                    Name = e.Name
                }
                )
                .ToList();
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(EditUserModel user)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (ModelState.IsValid)
            {
                var raspunsuri = Request.Form.Get("Interests")?.Split(',').ToList();
                if (raspunsuri == null)
                {
                    raspunsuri = new List<string>();
                }
                interestsUsersService.ChangeInterests(currentUser.Id, raspunsuri.Select(e => int.Parse(e)).ToList());


                var updateUser = new Domain.ApplicationUser()
                {
                    BirthDay = user.BirthDay,
                    LocalityId = user.LocalityId,
                    SexualIdentity = user.SexualIdentity,
                    Confidentiality = user.Visibility,
                    Name = user.Name,
                    Surname = user.Surname,
                    Id = user.Id

                };
                userService.Update(updateUser);

                return RedirectToAction("Index");
            }
            var counties = countyService.GetAll();

            var interests = interestService.GetAll();

            user.Counties = counties
                .Select(c =>
                new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }
                )
                .ToList();
            user.Albume = albumService
                .GetAll(user.Id,currentUser)
                .Select(e => new AlbumDomainModel()
                {
                    Count = e.Photo.Count,
                    CoverPhoto = e.Photo.Count > 0 ? e.Photo.OrderBy(e => e.Position).First().Id : -1,
                    Id = e.Id,
                    Name = e.Name
                })
                .ToList();
            return View(user);

        }

        [HttpPost]
        public ActionResult AddAlbum(AddAlbumModel model)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (ModelState.IsValid)
            {
                int albumId= albumService.AddAlbum(model.Name,currentUser);
                return RedirectToAction("Album", "Profile",new {albumId });
            }
            var albums = albumService.GetAll(currentUser.Id,currentUser);
            AlbumsViewModel modelEdit = new AlbumsViewModel()
            {
                CanEdit = true,
                Album = albums.Select(e => new AlbumDomainModel()
                {
                    Count = e.Photo.Count,
                    CoverPhoto = e.Photo.Count > 0 ? e.Photo.OrderBy(e => e.Position).First().Id : -1,
                    Id = e.Id,
                    Name = e.Name
                }).ToList(),
                AddAlbumModel = model

            };
            return View("Albums", modelEdit);
        }
        [HttpGet]
        public new ActionResult Profile(string userId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (string.IsNullOrWhiteSpace(userId)|| userService.GetUserById(userId) ==null)
            {
                return NotFoundView();
            }
            else
            {
                if (userId == currentUser.Id)
                {
                    return RedirectToAction("Index", "Profile", null);
                }

                var domainUser = userService.GetUserById(userId);
                //ProfileViewerModel user = mapper.Map<ProfileViewerModel>(domainUser);
                var user = new ProfileViewerModel()
                {
                    Album = new List<AlbumDomainModel>(domainUser.Album.Select(e => new AlbumDomainModel() { Id = e.Id, Count = e.Photo.Count, Name = e.Name })),
                    Interests = new List<string>(domainUser.InterestsUsers.Select(e => e.Interest.Name)),
                    Locality = domainUser.Locality.Name,
                    County = domainUser.Locality.County.Name,
                    Birthday = domainUser.BirthDay.Year + "." + domainUser.BirthDay.Month + "." + domainUser.BirthDay.Day,
                    PhotoId = domainUser.PhotoId,
                    IsAdmin = domainUser.RoleId == 1,
                    IsBanned = domainUser.IsBanned,
                    Id = domainUser.Id,
                    Email = domainUser.Email,
                    Name = domainUser.Name,
                    Surname = domainUser.Surname,
                    SexualIdentity = domainUser.SexualIdentity,

                };
                user.CanSee = friendService.CanSee(userId,currentUser);
                user.CanSendRequest = friendService.CanSendRequest(userId,currentUser);
                user.IsRequested = friendService.IsFriendRequested(userId,currentUser);
                user.Interests = interestsUsersService.GetAllInterests(domainUser.Id)
                    .Select(e => e.Name)
                    .ToList();
                user.Album = albumService.GetAll(userId,currentUser).Select(e => new AlbumDomainModel()
                {
                    Count = e.Photo.Count,
                    CoverPhoto = e.Photo.Count > 0 ? e.Photo.First().Id : -1,
                    Id = e.Id,
                    Name = e.Name
                }).ToList();

                return View(user);
            }

        } 

        [HttpGet]
        public ActionResult RemoveAlbum(int? albumId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (albumId == null)
            {
                return NotFoundView();
            }
            if (albumService.CanDeleteAlbum(albumId.Value,currentUser))
            {
                albumService.RemoveAlbum(albumId.Value,currentUser);
            }
            return RedirectToAction("Index", "Profile", null);
        }

        [HttpGet]
        public ActionResult FriendRequests()
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            return View();
        }

        [HttpGet]
        public ActionResult FriendList()
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            return View();
        }
        [HttpGet]
        public ActionResult Accept(string id)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            friendService.AcceptFriendRequest(id,currentUser);
            return RedirectToAction("Profile", "Profile", new { userId = id });
        }
        [HttpGet]
        public ActionResult Refuse(string id)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            friendService.RefuseFriendRequest(id,currentUser);
            return RedirectToAction("FriendRequests");
        }
        [HttpGet]
        public ActionResult Send(string id)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            friendService.SendFriendRequest(id,currentUser);
            return RedirectToAction("Profile", "Profile", new {userId=id });
        }
        [Authorize]
        [HttpGet]
        public JsonResult GetFriends(int toSkip)
        {
            MakeCurrentUser();
            var friends = friendService.GetFriends(toSkip, PageSize,currentUser).Select(e => 
            new FriendJsonModel() { Id=e.Id,Name=e.Name,ProfilePhoto=e.PhotoId}
            ).ToList();
            return Json(friends, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        [HttpGet]
        public JsonResult GetRequesters(int toSkip)
        {
            MakeCurrentUser();
            var friends = friendService.GetRequesters(toSkip, PageSize,currentUser).Select(e => new FriendJsonModel() { Id = e.Id, Name = e.Name, ProfilePhoto = e.PhotoId }).ToList();
            return Json(friends, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ChatBox()
        {

            MakeCurrentUser();
            ViewBag.currentUser = currentUser;
            var chats = messageService.GetMessageBoxes(currentUser);

            if (chats == null) return RedirectToAction("Index");
            return View(chats);
        }
    }
}