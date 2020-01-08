using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Proiect_DAW.Models.GeneralModels;
using Services;
using Services.User;

namespace Proiect_DAW.Code.Base
{
    public class BaseController : Controller
    {
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;
        protected ApplicationRoleManager _roleManager;
        protected CurrentUser currentUser;
        protected List<MessageBoxViewer> messageBoxes;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public BaseController()
        {

        }

        public void MakeCurrentUser()
        {

            var mail = ((ClaimsIdentity)SignInManager.AuthenticationManager.User?.Identity)?.Claims.FirstOrDefault(C => C.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            var userService = new UserAccountService(new DataAccess.SocializRUnitOfWork(new DataAccess.SocializRContext()));
            if (string.IsNullOrEmpty(mail))
            {
                currentUser = new CurrentUser(isAuthenticated: false);
                messageBoxes = new List<MessageBoxViewer>();
                ViewBag.CurrentUser = currentUser;
                return;
            }
            SocializRContext context = new SocializRContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = userService.Get(mail);
            if (user != null)
            {
                currentUser = new CurrentUser(isAuthenticated: true)
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    IsBanned = user.IsBanned,
                    BirthDay = user.BirthDay,
                    LocalityId = user.LocalityId,
                    ProfilePhoto = user.PhotoId,
                    SexualIdentity = user.SexualIdentity,
                    Surname = user.Surname,
                    Vizibility = user.Confidentiality,
                    Locality = user.Locality,
                };
                currentUser.IsAdmin = userManager.GetRoles(user.Id).Contains("admin");
                ViewBag.CurrentUser = currentUser;
            }
            else
            {
                currentUser = new CurrentUser(isAuthenticated: false);
                ViewBag.CurrentUser = currentUser;
            }
        }

        public ActionResult InternalServerErrorView()
        {
            MakeCurrentUser();
            return View("InternalServerError");
        }

        public ActionResult NotFoundView()
        {
            MakeCurrentUser();
            return View("NotFound");
        }

        public ActionResult ForbidView()
        {
            MakeCurrentUser();
            return View("Forbid");
        }

        public int PageSize = 50;
    }
}
