using System.Linq;
using Proiect_DAW.Models.DomainModels;
using Proiect_DAW.Models.AdminModels;
using Proiect_DAW.Code.Base;
using AutoMapper;
using Proiect_DAW.Models.JsonModels;
using System.Web.Mvc;
using Services.Locality;
using Services.County;
using DataAccess;

namespace Proiect_DAW.Controllers
{
    [Authorize(Roles ="Admin")]
    public class LocalitiesController : BaseController
    {
        private readonly Services.Locality.LocalityService localityService;
        private readonly Services.County.CountyService countyService;
       
        public LocalitiesController():
            base(mapper)
        {
            localityService = new LocalityService(new SocializRUnitOfWork(new SocializRContext()));
            countyService = new CountyService(new SocializRUnitOfWork(new SocializRContext()));
        }

        [HttpGet]
        public ActionResult Index()
        {
            var localities = localityService.GetAll().Select(e =>mapper.Map<LocalityDomainModel>(e));
            return View(localities);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new AddLocalityModel()
            {
                CountyIds = countyService.GetAll().Select(e => mapper.Map<SelectListItem>(e)).ToList()
            };
            return View(model);
        }

        
        [HttpPost]
        public ActionResult Create(AddLocalityModel model)
        {
            if (localityService.CityAlreadyExistsInCounty(model.Name, model.CountyId))
            {
                ModelState.AddModelError(nameof(model.Name), "Locality already exists");
            }
            if (ModelState.IsValid)
            {
                localityService.AddLocality(model.Name, model.CountyId);
                return RedirectToAction("Index","Localities");
            }

            model.CountyIds = countyService.GetAll().Select(e => mapper.Map<SelectListItem>(e)).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFoundView();
            }

            var locality = localityService.GetLocality(id.Value);
            if (locality == null)
            {
                return NotFoundView();
            }
            
            var model = mapper.Map<EditLocalityModel>(locality);
            model.CountyIds = countyService.GetAll().Select(e => 
            { var item = mapper.Map<SelectListItem>(e);
                item.Selected = e.Id == locality.Id;
                return item; })
                .ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditLocalityModel model)
        {
            
            if (ModelState.IsValid)
            {
                localityService.EditLocality(model.Id, model.Name, model.CountyId);
                return RedirectToAction(nameof(Index));
            }
            model.CountyIds = countyService.GetAll().Select(e => {
                var item = mapper.Map<SelectListItem>(e);
                item.Selected = e.Id == model.Id;
                return item; })
                .ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFoundView();
            }

            localityService.RemoveLocality(id.Value);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetLocalities(int toSkip)
        {
            var result= localityService
                .GetLocalities(toSkip, PageSize)
                .Select(e => mapper.Map<LocalityJsonModel>(e))
                .ToList();
            return Json(result);
        }

    }
}
