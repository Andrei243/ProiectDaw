using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Proiect_DAW.Models;
using Services;
using Proiect_DAW.Models.ProfileModels;
using Proiect_DAW.Models.FeedModels;
using Proiect_DAW.Models.GeneralModels;
using Proiect_DAW.Models.DomainModels;
using System.Web.Mvc;

namespace Proiect_DAW.Code.Mappers
{
    public class WebMapper:Profile
    {

        public WebMapper()
        {
            CreateMap<Users, LoginModel>();
            CreateMap<LoginModel, Users>();

            CreateMap<Users, UserDropdownModel>();
            CreateMap<UserDropdownModel, Users>();

            CreateMap<Users, RegisterModel>();
            CreateMap<RegisterModel, Users>();

            CreateMap<County, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));

            CreateMap<Interest, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));

            CreateMap<Locality, SelectListItem>()
                .ForMember(dest => dest.Value, s => s.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name));

            CreateMap<Users, ProfileViewerModel>()
                .ForMember(dest => dest.Album, s => s.MapFrom(src => new List<AlbumDomainModel>(src.Album.Select(e => new AlbumDomainModel()
                {
                    Id = e.Id,
                    Count = e.Photo.Count,
                    Name = e.Name,
                }))))
                .ForMember(dest => dest.Interests, s => s.MapFrom(src => new List<string>(src.InterestsUsers.Select(e => e.Interest.Name).ToList())))
                .ForMember(dest=>dest.Locality,s=>s.MapFrom(src=>src.Locality.Name))
                .ForMember(dest=>dest.County,s=>s.MapFrom(src=>src.Locality.County.Name))
                .ForMember(dest=>dest.Birthday,s=>s.MapFrom(src=>src.BirthDay.Year+"."+src.BirthDay.Month+"."+src.BirthDay.Day))
                .ForMember(dest=>dest.PhotoId,s=>s.MapFrom(src=>src.PhotoId))
                .ForMember(dest=>dest.IsAdmin,s=>s.MapFrom(src=>src.Role.Id==1))
                .ForMember(dest=>dest.IsBanned,s=>s.MapFrom(src=>src.IsBanned))
                ;
            
        }

    }
}
