﻿using DataAccess;
using Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using Proiect_DAW.Code.Base;
using Proiect_DAW.Models.DomainModels;
using Proiect_DAW.Models.GeneralModels;
using Proiect_DAW.Models.JsonModels;
using Proiect_DAW.Models.ProfileModels;
using Services.Album;
using Services.County;
using Services.FriendShip;
using Services.Interest;
using Services.InterestsUsers;
using Services.Photo;
using Services.User;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Proiect_DAW.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : BaseController
    {
        private readonly Services.User.UserService userService;
        private readonly Services.FriendShip.FriendshipService friendshipService;
        private readonly Services.InterestsUsers.InterestsUsersService interestsUsersService;
        private readonly Services.County.CountyService countyService;
        private readonly Services.Interest.InterestService interestService;
        private readonly Services.Album.AlbumService albumService;
        private readonly Services.Photo.PhotoService photoService;


        public UsersController()
            : base()
        {
            userService = new UserService(new SocializRUnitOfWork(new SocializRContext()));
            friendshipService = new FriendshipService( new SocializRUnitOfWork(new SocializRContext()));
            interestsUsersService = new InterestsUsersService(new SocializRUnitOfWork(new SocializRContext()));
            countyService = new CountyService(new SocializRUnitOfWork(new SocializRContext()));
            interestService = new InterestService(new SocializRUnitOfWork(new SocializRContext()));
            albumService = new AlbumService( new SocializRUnitOfWork(new SocializRContext()));
            photoService = new PhotoService(new SocializRUnitOfWork(new SocializRContext()));
            PageSize = 50;
        }
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetUsersByName(string name)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;
            var el = userService
                .GetUsersByName(name)
                .Select(e => new UserDropdownModel() { Id = e.Id, ProfilePhotoId = e.PhotoId, Name = e.Name + " " + e.Surname })
                .OrderBy(e => e.Name)
                .Take(125)
                .ToList();
            return Json(el,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInterests(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Json(new List<InterestSelectJsonModel>(), JsonRequestBehavior.AllowGet);
            }
            var indexes = interestsUsersService.GetAllInterests(userId)
                .Select(e => e.Id)
                .ToList();
            var interests = interestService.GetAll().Select(e =>
           
            new InterestSelectJsonModel() {
            Id=e.Id,
            Selected=indexes.Contains(e.Id),
            Text=e.Name
            }
            ).ToList();
            return Json(interests,JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public ActionResult Index()
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            return View();
        }

        
        [HttpGet]
        public ActionResult Details(string? userId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (string.IsNullOrWhiteSpace (userId) || userService.GetUserById(userId) == null)
            {
                return NotFoundView();
            }

            if (userId == currentUser.Id)
            {
                return RedirectToAction("Index", "Profile", null);
            }

            var domainUser = userService.GetUserById(userId);
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
            user.CanSee = friendshipService.CanSee(userId,currentUser);
            user.CanSendRequest = friendshipService.CanSendRequest(userId, currentUser);
            user.IsRequested = friendshipService.IsFriendRequested(userId, currentUser);
            user.Interests = interestsUsersService.GetAllInterests(domainUser.Id)
                .Select(e => e.Name)
                .ToList();
            user.Album = albumService.GetAll(userId, currentUser).Select(e => new AlbumDomainModel()
            {
                Count = e.Photo.Count,
                CoverPhoto = e.Photo.Count > 0 ? e.Photo.OrderBy(e => e.Position).First().Id : -1,
                Id = e.Id,
                Name = e.Name
            }).ToList();
            
            return View(user);

        }



        [HttpGet]
        public ActionResult Edit(string? userId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return NotFoundView();
            }
            var user = userService.GetUserById(userId);
            if (user == null)
            {
                return NotFoundView();
            }
            var model = new EditUserModel()
            {
                Visibility = user.Confidentiality,
                BirthDay = user.BirthDay,
                Id = user.Id,
                LocalityId = user.LocalityId,
                Name = user.Name,
                PhotoId = user.PhotoId,
                SexualIdentity = user.SexualIdentity,
                Surname = user.Surname

            };

            var counties = countyService.GetAll();


            model.Counties = counties.Select(c => 
            new SelectListItem() { Text=c.Name,Value=c.Id.ToString()}
            ).ToList();
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(EditUserModel user)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (ModelState.IsValid)
            {
                //Request.Form.TryGetValue("Interests",
                //    out var raspunsuri);
                var raspunsuri = Request.Form.Get("Interests")?.Split(',').ToList();
                if (raspunsuri == null)
                {
                    raspunsuri = new List<string>();
                }
                interestsUsersService.ChangeInterests(user.Id, raspunsuri.Select(e => int.Parse(e)).ToList());


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
            return View(user);
        }
        [HttpGet]
        public ActionResult DeleteAlbum(int? albumId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (albumId == null)
            {
                return NotFoundView();
            }
            string userId = albumService.RemoveAlbum(albumId.Value, currentUser);


            return RedirectToAction("Details", new { userId });
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

            var album = albumService.GetAlbum(albumId.Value);
            if (album == null)
            {
                return NotFoundView();
            }

            AlbumViewerModel model = new AlbumViewerModel()
            {
               
                Id = albumId.Value,
                Name = album.Name
            };

            return View(model);
        }
        [HttpGet]
        public ActionResult DeletePhoto(int? photoId, int? albumId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (photoId == null || albumId == null)
            {
                return NotFoundView();
            }
            photoService.RemovePhoto(photoId.Value);
            return RedirectToAction("Album", new { albumId });

        }

        [HttpGet]
        public ActionResult Delete(string? userId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (userId == null)
            {
                return NotFoundView();
            }

            userService.RemoveUser(userId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Ban(string? userId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (userId == null) { return NotFoundView(); }

            userService.BanUser(userId);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Unban(string? userId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (string.IsNullOrWhiteSpace(userId)) { return NotFoundView(); }

            userService.UnbanUser(userId);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult MakeAdmin(string? userId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (userId == null)
            {
                return NotFoundView();
            }
            UserManager.AddToRoleAsync(userId, "admin");
            userService.MakeAdmin(userId);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult RevokeAdmin(string? userId)
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return NotFoundView();
            }
            userService.RevokeAdmin(userId);
            UserManager.RemoveFromRoleAsync(userId, "admin");
            return RedirectToAction("Index");

        }
        [HttpGet]
        public JsonResult GetUsers(int toSkip)
        {
            MakeCurrentUser();
            var users = userService.GetUsers(toSkip, PageSize, currentUser).Select(e=>new UserJsonModel() {
            
            Id=e.Id,
            IsAdmin=e.RoleId==1,
            IsBanned=e.IsBanned,
            Name=e.Name+" "+e.Surname,
            ProfilePhoto=e.PhotoId
            }).ToList();
            return Json(users, JsonRequestBehavior.AllowGet);

        }

    }
}
