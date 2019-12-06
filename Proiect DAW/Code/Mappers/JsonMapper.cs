using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proiect_DAW.Models.JsonModels;

namespace Proiect_DAW.Code.Mappers
{
    public class JsonMapper : Profile
    {
        public JsonMapper()
        {
            CreateMap<Domain.County, CountyJsonModel>();
            CreateMap<CountyJsonModel, Domain.County>();

            CreateMap<Domain.Locality, LocalityJsonModel>()
                .ForMember(dest => dest.County, s => s.MapFrom(src => src.County.Name));
            CreateMap<LocalityJsonModel, Domain.Locality>();

            CreateMap<Domain.Interest, InterestJsonModel>();
            CreateMap<InterestJsonModel, Domain.Interest>();

            CreateMap<Domain.Comment, CommentJsonModel>()
                .ForMember(dest => dest.UserName, s => s.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.UserId, s => s.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.User.PhotoId))
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Content));
            CreateMap<CommentJsonModel, Domain.Comment>();

            CreateMap<Domain.Post, PostJsonModel>()
                .ForMember(dest => dest.UserId, s => s.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, s => s.MapFrom(src => src.User.Name + " " + src.User.Surname))
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.User.PhotoId))
                .ForMember(dest => dest.NoReactions, s => s.MapFrom(src => src.Reaction.Count))
                .ForMember(dest => dest.PhotoId, s => s.MapFrom(src => new List<int>() { src.Photo.Id }));

            CreateMap<Domain.Photo, ImageJsonModel>();

            CreateMap<Domain.Users, FriendJsonModel>()
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.PhotoId))
                .ForMember(dest => dest.Name, s => s.MapFrom(src => src.Name + " " + src.Surname));

            CreateMap<Domain.Users, UserJsonModel>()
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.PhotoId))
                .ForMember(dest => dest.Name, s => s.MapFrom(src => src.Name + " " + src.Surname))
                .ForMember(dest => dest.IsAdmin, s => s.MapFrom(src => src.RoleId == 2))
                .ForMember(dest => dest.IsBanned, s => s.MapFrom(src => src.IsBanned));

            CreateMap<Domain.Interest, InterestSelectJsonModel>()
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, s => s.MapFrom(src => src.Id));
                

        }
    }
}
