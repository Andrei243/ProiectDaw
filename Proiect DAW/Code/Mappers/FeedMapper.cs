using Proiect_DAW.Models.AdminModels;
using Proiect_DAW.Models.DomainModels;
using Proiect_DAW.Models.FeedModels;
using Proiect_DAW.Models.ProfileModels;
using AutoMapper;
using System.Linq;
using System.Web.Mvc;
using Proiect_DAW.AdminModels;

namespace Proiect_DAW.Code.Mappers
{
    public class FeedMapper : Profile
    {
        public FeedMapper()
        {
            CreateMap<Domain.County, CountyDomainModel>();
            CreateMap<CountyDomainModel, Domain.County>();

            CreateMap<DetailsCountyModel, Domain.County>();
            CreateMap<Domain.County, DetailsCountyModel>();

            CreateMap<EditCountyModel, Domain.County>();
            CreateMap<Domain.County, EditCountyModel>();

            CreateMap<Domain.Users, PostUserModel>()
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.PhotoId))
                .ForMember(dest => dest.Name, s => s.MapFrom(src => src.Name + " " + src.Surname));
            
            CreateMap<PostAddModel, Domain.Post>()
                .ForMember(dest => dest.Confidentiality, s => s.MapFrom(src => src.Visibility));

            CreateMap<Domain.Interest, InterestDomainModel>();

            CreateMap<CountyDomainModel, SelectListItem>()
                .ForMember(e => e.Value, s => s.MapFrom(dest => dest.Id.ToString()))
                .ForMember(e => e.Text, s => s.MapFrom(dest => dest.Name));

            CreateMap<Domain.Locality, EditLocalityModel>();

            CreateMap<Domain.Album, AlbumDomainModel>()
                .ForMember(dest => dest.Count, s => s.MapFrom(src => src.Photo.Count))
                .ForMember(dest => dest.CoverPhoto, s => s.MapFrom(
                       src => src.Photo.Count > 0 ?
                         src.Photo.OrderBy(f => f.Position).FirstOrDefault(f => f.AlbumId == src.Id).Id :
                   -1))
                .ForMember(dest => dest.Id, s => s.MapFrom(src => src.Id));

            CreateMap<PhotoModel, Domain.Photo>()
                .ForMember(dest => dest.MIMEType, s => s.MapFrom(src => src.Binar.ContentType));

            CreateMap<EditUserModel, Domain.Users>()
                .ForMember(dest => dest.Confidentiality, s => s.MapFrom(src => src.Visibility));

            CreateMap<Domain.Users, EditUserModel>()
                .ForMember(dest => dest.Visibility, s => s.MapFrom(src => src.Confidentiality))
                .ForMember(dest => dest.Albume, s => s.MapFrom(src => src.Album.Select(e => new AlbumDomainModel
                {
                    Count = e.Photo.Count,
                    CoverPhoto = e.Photo.Count > 0 ? e.Photo.First().Id : -1,
                    Id = e.Id,
                    Name = e.Name
                })));

            CreateMap<Domain.Users, UserIndex>()
                .ForMember(dest => dest.FullName, s => s.MapFrom(src => src.Name + " " + src.Surname))
                .ForMember(dest => dest.ProfilePhoto, s => s.MapFrom(src => src.PhotoId));

            CreateMap<Domain.Comment, CommentModel>()
                .ForMember(dest => dest.Text, s => s.MapFrom(src => src.Content))
                .ForMember(dest => dest.User, s => s.MapFrom(
                       src => new PostUserModel
                       {
                           Id = src.User.Id,
                           Name = src.User.Name + " " + src.User.Surname,
                           ProfilePhoto = src.User.PhotoId
                       }
                       ));

            CreateMap<Domain.Locality, LocalityDomainModel>()
                .ForMember(dest => dest.County, s => s.MapFrom(src =>
                     new CountyDomainModel()
                     {
                         Id = src.County.Id,
                         Name = src.County.Name
                     }
                   ));

            CreateMap<Domain.Album, AlbumEditModel>()
                .ForMember(dest => dest.Photos, s => s.MapFrom(src => src.Photo.Select(e => e.Id)));

            CreateMap<PhotoModel, Domain.Photo>()
                .ForMember(dest => dest.AlbumId, s => s.MapFrom(src => src.AlbumId))
                .ForMember(dest => dest.PostId, s => s.MapFrom(src => src.PostId))
                .ForMember(dest => dest.Position, s => s.MapFrom(src => src.Position))
                .ForMember(dest => dest.MIMEType, s => s.MapFrom(src => src.Binar.ContentType));
            CreateMap<Domain.Photo, PhotoDomainModel>();
            CreateMap<PhotoDomainModel, Domain.Photo>();
           
        }
    }
}
