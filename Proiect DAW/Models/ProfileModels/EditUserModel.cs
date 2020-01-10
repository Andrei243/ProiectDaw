using Proiect_DAW.Models.DomainModels;
using Proiect_DAW.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Proiect_DAW.Models.ProfileModels
{
    public class EditUserModel
    {
        public EditUserModel()
        {
            Counties = new List<SelectListItem>();
            Interests = new List<int>();
            Albume = new List<AlbumDomainModel>();
        }

        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime BirthDay { get; set; }
        public int? LocalityId { get; set; }
        [Required]
        public string SexualIdentity { get; set; }
        [Required]
        public string Visibility { get; set; }
        public int? PhotoId { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public List<int> Interests { get; set; }
        public List<AlbumDomainModel> Albume { get; set; }
        public AddAlbumModel AddAlbumModel { get; set; }
    }

}
