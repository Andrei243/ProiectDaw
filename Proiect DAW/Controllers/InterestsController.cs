using System.Linq;
using Proiect_DAW.Models.DomainModels;
using Proiect_DAW.Models.JsonModels;
using System.Web.Mvc;
using Proiect_DAW.Code.Base;
using Services.Interest;
using DataAccess;

namespace Proiect_DAW.Controllers
{
    [Authorize(Roles = "admin")]
    public class InterestsController : BaseController
    {

        private readonly Services.Interest.InterestService interestService;
        public InterestsController():
            base()
        {
           interestService = new InterestService(new SocializRUnitOfWork(new SocializRContext()));
        }

        [HttpGet]
        public ActionResult Index()
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;
            var interese = interestService.GetAll().Select(e =>new InterestDomainModel() { 
            Id=e.Id,
            Name=e.Name
            });

            return View(interese);
        }


        [HttpGet]
        public ActionResult Create()
        {
            MakeCurrentUser();
            return View();
        }

        
        [HttpPost]
        public ActionResult Create( InterestDomainModel interest)
        {
            MakeCurrentUser();
            if (ModelState.IsValid)
            {
                interestService.AddInterest(interest.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(interest);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            MakeCurrentUser();
            if (id == null)
            {
                return NotFoundView();
            }

            var interest = interestService.GetInterest(id.Value);
            if (interest == null)
            {
                return NotFoundView();
            }

            var model = new InterestDomainModel()
            {
                Id = interest.Id,
                Name = interest.Name
            };
            return View(model);
        }

        
        [HttpPost]
        public ActionResult Edit( InterestDomainModel interest)
        {
            MakeCurrentUser();
            if (ModelState.IsValid)
            {

                interestService.EditInterest(interest.Id, interest.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(interest);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            MakeCurrentUser();
            if (id == null)
            {
                return NotFoundView();
            }

            interestService.RemoveInterest(id.Value);

            return RedirectToAction("Index", "Interests");
        }

        [HttpGet]
       public JsonResult GetInterests(int toSkip)
        {
            var interests = interestService.GetInterests(toSkip, PageSize).Select(e => 
            new InterestJsonModel() { Id=e.Id,Name=e.Name}
            ).ToList();
            return Json(interests);
        }

    }
}
