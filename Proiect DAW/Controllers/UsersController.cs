using DataAccess;
using Domain;
using Proiect_DAW.Code.Base;
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
    [Authorize(Roles = "Admin")]
    public class UsersController : BaseController
    {
        private readonly Services.User.UserService userService;
        private readonly Services.CurrentUser currentUser;
        private readonly Services.FriendShip.FriendshipService friendshipService;
        private readonly Services.InterestsUsers.InterestsUsersService interestsUsersService;
        private readonly Services.County.CountyService countyService;
        private readonly Services.Interest.InterestService interestService;
        private readonly Services.Album.AlbumService albumService;
        private readonly Services.Photo.PhotoService photoService;


        public UsersController()
            : base(mapper)
        {
            userService = new UserService(currentUser,new SocializRUnitOfWork(new SocializRContext()));
            friendshipService = new FriendshipService(currentUser, new SocializRUnitOfWork(new SocializRContext()));
            interestsUsersService = new InterestsUsersService(new SocializRUnitOfWork(new SocializRContext()));
            countyService = new CountyService(new SocializRUnitOfWork(new SocializRContext()));
            interestService = new InterestService(new SocializRUnitOfWork(new SocializRContext()));
            albumService = new AlbumService(currentUser, new SocializRUnitOfWork(new SocializRContext()));
            photoService = new PhotoService(new SocializRUnitOfWork(new SocializRContext()), currentUser);
            PageSize = 50;
        }
        [AllowAnonymous]
        [HttpGet]
        public List<UserDropdownModel> GetUsersByName(string name)
        {
            var el = userService
                .GetUsersByName(name)
                .Select(e => new UserDropdownModel() { Id = e.Id, ProfilePhotoId = e.PhotoId, Name = e.Name + " " + e.Surname })
                .OrderBy(e => e.Name)
                .Take(125)
                .ToList();
            return el;
        }

        [HttpGet]
        public JsonResult GetInterests(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Json(new List<InterestSelectJsonModel>());
            }
            var indexes = interestsUsersService.GetAllInterests(userId)
                .Select(e => e.Id)
                .ToList();
            var interests = interestService.GetAll().Select(e =>
            {
                var item = mapper.Map<InterestSelectJsonModel>(e);
                item.Selected = indexes.Contains(e.Id);
                return item;

            }).ToList();
            return Json(interests);
        }

        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpGet]
        public ActionResult Details(string? userId)
        {
            if (string.IsNullOrWhiteSpace (userId) || userService.GetUserById(userId) == null)
            {
                return NotFoundView();
            }

            if (userId == currentUser.Id)
            {
                return RedirectToAction("Index", "Profile", null);
            }

            var domainUser = userService.GetUserById(userId);
            ProfileViewerModel user = mapper.Map<ProfileViewerModel>(domainUser);
            user.CanSee = friendshipService.CanSee(userId);
            user.CanSendRequest = friendshipService.CanSendRequest(userId);
            user.IsRequested = friendshipService.IsFriendRequested(userId);
            user.Interests = interestsUsersService.GetAllInterests(domainUser.Id)
                .Select(e => e.Name)
                .ToList();
            user.Album = albumService.GetAll(userId).Select(e => mapper.Map<Models.DomainModels.AlbumDomainModel>(e)).ToList();
            
            return View(user);

        }



        [HttpGet]
        public ActionResult Edit(string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return NotFoundView();
            }
            var user = userService.GetUserById(userId);
            if (user == null)
            {
                return NotFoundView();
            }
            var model = mapper.Map<EditUserModel>(user);

            var counties = countyService.GetAll();


            model.Counties = counties.Select(c => mapper.Map<SelectListItem>(c)).ToList();
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(EditUserModel user)
        {
            if (ModelState.IsValid)
            {
                //Request.Form.TryGetValue("Interests",
                //    out var raspunsuri);
                var raspunsuri = Request.Form.Get("Interests").Split(',');
                interestsUsersService.ChangeInterests(user.Id, raspunsuri.Select(e => int.Parse(e)).ToList());


                var updateUser = mapper.Map<Users>(user);
                userService.Update(updateUser);

                return RedirectToAction("Index");
            }
            return View(user);
        }
        [HttpGet]
        public ActionResult DeleteAlbum(int? albumId)
        {
            if (albumId == null)
            {
                return NotFoundView();
            }
            string userId = albumService.RemoveAlbum(albumId.Value);


            return RedirectToAction("Details", new { userId });
        }
        [HttpGet]
        public ActionResult Album(int? albumId)
        {
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
            if (userId == null) { return NotFoundView(); }

            userService.BanUser(userId);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Unban(string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) { return NotFoundView(); }

            userService.UnbanUser(userId);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult MakeAdmin(string? userId)
        {
            if (userId == null)
            {
                return NotFoundView();
            }
            userService.MakeAdmin(userId);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult RevokeAdmin(string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return NotFoundView();
            }
            userService.RevokeAdmin(userId);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public JsonResult GetUsers(int toSkip)
        {
            var users = userService.GetUsers(toSkip, PageSize).Select(mapper.Map<UserJsonModel>).ToList();
            return Json(users);

        }

    }
}
