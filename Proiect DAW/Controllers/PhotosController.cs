﻿using Proiect_DAW.Code.Base;
using System.Web.Mvc;
using Services.Photo;
using DataAccess;

namespace Proiect_DAW.Controllers
{
    public class PhotosController : BaseController
    {
        private readonly Services.Photo.PhotoService photoService;

        public PhotosController():
            base()
        {
            photoService = new PhotoService(new SocializRUnitOfWork(new SocializRContext()));
        }

        
        [HttpGet]
        public ActionResult Download(int? id)
        {
            MakeCurrentUser();

            if (id == null) return NotFoundView();
            if (!photoService.CanSeePhoto(id.Value,currentUser)) return ForbidView();
            var photo = photoService.GetPhoto(id.Value);

            if (photo == null) return NotFoundView();

            return File(photo.Binary,photo.MIMEType);

        }

        
    }
}
