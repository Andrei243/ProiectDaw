using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Proiect_DAW.Models.DomainModels;
using Proiect_DAW.Models.JsonModels;
using System.Web.Mvc;
using Proiect_DAW.Code.Base;
using Services.Interest;
using DataAccess;

namespace Proiect_DAW.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InterestsController : BaseController
    {

        private readonly Services.Interest.InterestService interestService;
        public InterestsController():
            base(mapper)
        {
           interestService = new InterestService(new SocializRUnitOfWork(new SocializRContext()));
        }

        [HttpGet]
        public ActionResult Index()
        {

            var interese = interestService.GetAll().Select(e =>mapper.Map<InterestDomainModel>(e));

            return View(interese);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create( InterestDomainModel interest)
        {
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
            if (id == null)
            {
                return NotFoundView();
            }

            var interest = interestService.GetInterest(id.Value);
            if (interest == null)
            {
                return NotFoundView();
            }
           
            var model = mapper.Map<InterestDomainModel>(interest);
            return View(model);
        }

        
        [HttpPost]
        public ActionResult Edit( InterestDomainModel interest)
        {
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
            var interests = interestService.GetInterests(toSkip, PageSize).Select(e => mapper.Map<InterestJsonModel>(e)).ToList();
            return Json(interests);
        }

    }
}
