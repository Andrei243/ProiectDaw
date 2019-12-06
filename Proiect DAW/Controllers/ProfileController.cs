using System.Linq;
using Services;
using AutoMapper;
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


        public ProfileController()
            : base(mapper)
        {
            interestsUsersService = new InterestsUsersService(new SocializRUnitOfWork(new SocializRContext()));
            interestService = new InterestService(new SocializRUnitOfWork(new SocializRContext()));
            userService = new UserService(currentUser, new SocializRUnitOfWork(new SocializRContext()));
            countyService = new CountyService(new SocializRUnitOfWork(new SocializRContext()));
            friendService = new FriendshipService(currentUser, new SocializRUnitOfWork(new SocializRContext()));
            albumService = new AlbumService(currentUser, new SocializRUnitOfWork(new SocializRContext()));
            photoService = new PhotoService(new SocializRUnitOfWork(new SocializRContext()), currentUser);
        }

        [HttpGet]
        public ActionResult Albums()
        {
            var albums = albumService.GetAll(currentUser.Id);
            AlbumsViewModel model = new AlbumsViewModel()
            {
                CanEdit = true,
                Album = albums.Select(e => mapper.Map<AlbumDomainModel>(e)).ToList(),
                AddAlbumModel = new AddAlbumModel()

            };
            return View(model);
        }
        [HttpGet]
        public JsonResult GetInterests()
        {
            var indexes = interestsUsersService.GetAllInterests(currentUser.Id).Select(e=>e.Id).ToList();
            var interests = interestService.GetAll().Select(e =>
             {
                 var item = mapper.Map<InterestSelectJsonModel>(e);
                 item.Selected = indexes.Contains(e.Id);
                 return item;

             }).ToList();
            return Json(interests);
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
            if (toSkip == null || albumId == null)
            {
                return Json(new List<int>());
            }
            if (albumService.CanSeeAlbum(albumId.Value))
            {
                var photos = photoService.GetPhotos(toSkip.Value, PageSize, albumId.Value).Select(e => mapper.Map<ImageJsonModel>(e)).ToList();
                return Json(photos);
            }
            else return Json(new List<int>());

        }

        [HttpGet]
        public ActionResult Index()
        {
            var domainUser = userService.GetUserById(currentUser.Id);
            var user = mapper.Map<ProfileViewerModel>(domainUser);
            user.Interests = interestsUsersService
                .GetAllInterests(domainUser.Id)
                .Select(e => e.Name)
                .ToList();
            user.Album = albumService
                .GetAll(currentUser.Id)
                .Select(
                e=>mapper.Map<AlbumDomainModel>(e)
                ).ToList();
            return View(user);
        }
        [HttpGet]
        public List<PhotoDomainModel> GetPhotos(int? albumId)
        {
            if (albumId == null)
            {
                return new List<PhotoDomainModel>();
            }
            if (albumService.CanSeeAlbum(albumId.Value))
            {
                return albumService
                    .GetPhotos(albumId.Value)
                    .Select(e => mapper.Map<PhotoDomainModel>(e))
                    .ToList();
            }
            return new List<PhotoDomainModel>();
        }
        [HttpGet]
        public ActionResult MakeProfilePhoto(int? photoId)
        {
            if (photoId == null)
            {
                return NotFoundView();
            }
            if (!photoService.HasThisPhoto(photoId.Value,currentUser.Id))
            {
                return RedirectToAction("Index", "Profile");
            }
            userService.UpdateProfilePhoto(photoId.Value);
            return RedirectToAction("Index", "Profile");

        }

        [HttpGet]
        public ActionResult Album(int? albumId)
        {
            if (albumId == null)
            {
                return NotFoundView();
            }
            if (!albumService.CanSeeAlbum(albumId.Value))
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
                HasThisAlbum = albumService.HasThisAlbum(albumId.Value),
                Id=albumId.Value,
                Name = album.Name
            };

            return View(albumViewerModel);
        }

        [HttpPost]
        public ActionResult AlbumChangeName(AlbumViewerModel model)
        {
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
                    model.Binar.CopyTo(memoryStream);
                    photo.Binary = memoryStream.ToArray();
                    
                }
                photoService.AddPhoto(photo);

                return RedirectToAction("Album", "Profile", new { albumId = model.AlbumId });
            }
            if (albumId == null)
            {
                return NotFoundView();
            }
            AlbumViewerModel albumViewerModel = new AlbumViewerModel()
            {
                PhotoModel = model,
                HasThisAlbum = albumService.HasThisAlbum(albumId.Value),
                Id = albumId.Value,
                Name=albumService.GetAlbum(albumId.Value).Name
            };
            return View("Album", albumViewerModel);
        }

        [HttpGet]
        public ActionResult Edit()
        {

            var user = userService.GetCurrentUser();
            var model = mapper.Map<EditUserModel>(user);

            var counties = countyService.GetAll();

            var interests = interestService.GetAll();

            model.Counties = counties
                .Select(c => mapper.Map<SelectListItem>(c))
                .ToList();
            model.Albume = albumService
                .GetAll(currentUser.Id)
                .Select(e => mapper.Map<AlbumDomainModel>(e))
                .ToList();
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(EditUserModel user)
        {
            if (ModelState.IsValid)
            {
                var raspunsuri = Request.Form.Get("Interests").Split(',');
                interestsUsersService.ChangeInterests(currentUser.Id, raspunsuri.Select(e => int.Parse(e)).ToList());
               

                var updateUser = mapper.Map<Domain.Users>(user);
                userService.Update(updateUser);

                return RedirectToAction("Index");
            }
            user.Albume = albumService
                .GetAll(user.Id)
                .Select(e => mapper.Map<AlbumDomainModel>(e))
                .ToList();
            return View(user);

        }

        [HttpPost]
        public ActionResult AddAlbum(AddAlbumModel model)
        {
            if (ModelState.IsValid)
            {
                int albumId= albumService.AddAlbum(model.Name);
                return RedirectToAction("Album", "Profile",new {albumId });
            }
            var albums = albumService.GetAll(currentUser.Id);
            AlbumsViewModel modelEdit = new AlbumsViewModel()
            {
                CanEdit = true,
                Album = albums.Select(e => mapper.Map<AlbumDomainModel>(e)).ToList(),
                AddAlbumModel = model

            };
            return View("Albums", modelEdit);
        }
        [HttpGet]
        public ActionResult Profile(string userId)
        {
            if(string.IsNullOrWhiteSpace(userId)|| userService.GetUserById(userId) ==null)
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
                ProfileViewerModel user = mapper.Map<ProfileViewerModel>(domainUser);
                user.CanSee = friendService.CanSee(userId);
                user.CanSendRequest = friendService.CanSendRequest(userId);
                user.IsRequested = friendService.IsFriendRequested(userId);
                user.Interests = interestsUsersService.GetAllInterests(domainUser.Id)
                    .Select(e => e.Name)
                    .ToList();
                user.Album = albumService.GetAll(userId).Select(e => mapper.Map<AlbumDomainModel>(e)).ToList();

                return View(user);
            }

        } 

        [HttpGet]
        public ActionResult RemoveAlbum(int? albumId)
        {
            if (albumId == null)
            {
                return NotFoundView();
            }
            if (albumService.CanDeleteAlbum(albumId.Value))
            {
                albumService.RemoveAlbum(albumId.Value);
            }
            return RedirectToAction("Index", "Profile", null);
        }

        [HttpGet]
        public ActionResult FriendRequests()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FriendList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Accept(string id)
        {
            friendService.AcceptFriendRequest(id);
            return RedirectToAction("Profile", "Profile", new { userId = id });
        }
        [HttpGet]
        public ActionResult Refuse(string id)
        {
            friendService.RefuseFriendRequest(id);
            return RedirectToAction("FriendRequests");
        }
        [HttpGet]
        public ActionResult Send(string id)
        {
            friendService.SendFriendRequest(id);
            return RedirectToAction("Profile", "Profile", new {userId=id });
        }
        [Authorize]
        [HttpGet]
        public JsonResult GetFriends(int toSkip)
        {
            var friends = friendService.GetFriends(toSkip, PageSize).Select(e => mapper.Map<FriendJsonModel>(e)).ToList();
            return Json(friends);

        }
        [Authorize]
        [HttpGet]
        public JsonResult GetRequesters(int toSkip)
        {
            var friends = friendService.GetRequesters(toSkip, PageSize).Select(e => mapper.Map<FriendJsonModel>(e)).ToList();
            return Json(friends);

        }

    }
}