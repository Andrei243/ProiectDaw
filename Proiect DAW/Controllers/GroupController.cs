using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using Domain;
using Proiect_DAW.Code.Base;
using Services.Group;

namespace Proiect_DAW.Controllers
{
    [Authorize]
    public class GroupController : BaseController
    {
        protected GroupService GroupService;
        public GroupController()
        {
            GroupService = new GroupService(new SocializRUnitOfWork(new SocializRContext()));
        }

        [HttpGet]
        public ActionResult UserGroups()
        {
            MakeCurrentUser();

            var groups = GroupService.GetGroups(currentUser.Id);
            return View(groups);
        }

        [HttpGet]
        public ActionResult FindGroups()
        {
            MakeCurrentUser();
            var groups = GroupService.GetGroupsNotIncluded(currentUser.Id);
            return View(groups);
        }


        [HttpGet]
        public ActionResult Details(int? id)
        {
            MakeCurrentUser();
            if (id == null)
            {
                return NotFoundView();
            }

            var group = GroupService.GetGroup(id.Value);
            return View(group);

        }

        [HttpGet] 
        public ActionResult JoinGroup(int? id)
        {
            MakeCurrentUser();
            if(id == null)
            {
                return NotFoundView();
            }
            GroupService.JoinGroup(id.Value, currentUser);
            return RedirectToAction("Details", new { id = id });
            
        }
        

        [HttpGet]
        public ActionResult ExitGroup(int? id)
        {
            MakeCurrentUser();
            if (id == null)
            {
                return NotFoundView();
            }
            GroupService.ExitGroup(id.Value, currentUser);
            return RedirectToAction("UserGroups");
        }

       

        // GET: Group/Create
        public ActionResult Create()
        {
            MakeCurrentUser();

            return View();
        }

        // POST: Group/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")] Group group)
        {
            MakeCurrentUser();
            if (ModelState.IsValid)
            {
                GroupService.CreateGroup(group.Name, currentUser);
                return RedirectToAction("UserGroups");
            }

            return View(group);
        }

    }
}
