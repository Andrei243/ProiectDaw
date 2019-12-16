using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Services;
using Services.User;

namespace Proiect_DAW.Code.Base
{
    public class BaseController : Controller
    {
        protected readonly CurrentUser currentUser;
        public BaseController()
        {
            var context = this.HttpContext;
            if(context == null)
            {
                currentUser= currentUser = new CurrentUser(isAuthenticated: false);
                return;
            }
            var mail = ((ClaimsIdentity)context.User).Claims.FirstOrDefault(C => C.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            var userService = new UserAccountService(new DataAccess.SocializRUnitOfWork(new DataAccess.SocializRContext())); 
            var user = userService.Get(mail);
            if (user != null)
                currentUser= new CurrentUser(isAuthenticated: true)
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    IsAdmin = user.Role.Name == "admin",
                    IsBanned = user.IsBanned,
                    BirthDay = user.BirthDay,
                    LocalityId = user.LocalityId,
                    ProfilePhoto = user.PhotoId,
                    SexualIdentity = user.SexualIdentity,
                    Surname = user.Surname,
                    Vizibility = user.Confidentiality,
                    Locality = user.Locality,
                    //Password = user.Password


                };
            else
            {
                currentUser= new CurrentUser(isAuthenticated: false);
            }
        }

        public ActionResult InternalServerErrorView()
        {
            return View("InternalServerError");
        }

        public ActionResult NotFoundView()
        {
            return View("NotFound");
        }

        public ActionResult ForbidView()
        {
            return View("Forbid");
        }

        public int PageSize = 50;
    }
}
