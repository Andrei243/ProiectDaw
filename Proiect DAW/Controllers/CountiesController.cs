using System.Linq;
using Proiect_DAW.Models.DomainModels;
using Proiect_DAW.Models.AdminModels;
using Proiect_DAW.Code.Base;
using AutoMapper;
using Proiect_DAW.Models.JsonModels;
using System.Web.Mvc;
using Proiect_DAW.AdminModels;
using Services.County;
using DataAccess;
using Services.Locality;

namespace Proiect_DAW.Controllers
{

    [Authorize(Roles =  "Admin")]

    public class CountiesController : BaseController
    {
        private readonly Services.County.CountyService countyService;
        private readonly Services.Locality.LocalityService localityService;
        

        public CountiesController ():
            base(mapper)
        {
            countyService = new CountyService(new SocializRUnitOfWork(new SocializRContext()));
            localityService = new LocalityService(new SocializRUnitOfWork(new SocializRContext()));
        }

        [HttpGet]
        public ActionResult Index()
        {
            var counties = countyService.GetAll().Select(e => mapper.Map<CountyDomainModel>(e));
            return View(counties);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFoundView();
            }

            var county = countyService.GetCountyById(id);
            if (county == null)
            {
                return NotFoundView();
            }
           
            var model = mapper.Map<DetailsCountyModel>(county);
            model.Localities = localityService.GetAll(county.Id).Select(e => e.Name).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Create(CountyDomainModel model)
        {
            if (ModelState.IsValid)
            {
                countyService.Add(model.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public  ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFoundView();
            }

            var county = countyService.GetCountyById(id);
            
            var model = mapper.Map<EditCountyModel>(county);
            if (county == null)
            {
                return NotFoundView();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditCountyModel model)
        {
            

            if (ModelState.IsValid)
            {
                countyService.Update(model.Id, model.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public bool CanDelete(int? countyId)
        {
            if (countyId == null)
            {
                return true;
            }
            var canBeDeleted = countyService.CanBeDeleted(countyId.Value);
            if (canBeDeleted)
            {
                countyService.Remove(countyId.Value);
            }
            return canBeDeleted;
        }

       
        [HttpGet]
        public JsonResult GetCounties(int toSkip)
        {
            var counties = countyService.GetCounties(toSkip, PageSize).Select(e => mapper.Map<CountyJsonModel>(e)).ToList();

            return Json(counties);

        }

    }
}
