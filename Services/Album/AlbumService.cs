using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Domain;
using System.Linq;
using System.Data.Entity;

namespace Services.Album
{
    public class AlbumService : Base.BaseService
    {

        public AlbumService( SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool CanSeeAlbum(int albumId,CurrentUser currentUser)
        {
            if (currentUser.IsAdmin) return true;
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false;
            if (isBanned) return false;
            var album = unitOfWork.Albums.Query.Include(e=>e.User)
                .AsNoTracking().First(e => e.Id == albumId);
            if (album.User.IsBanned) return false;
            if (album.UserId == currentUser.Id) return true;
            if (album.User.Confidentiality == Confidentiality.Public) return true;
            if (album.User.Confidentiality == Confidentiality.Private) return false;
            return unitOfWork.Friendships.Query
                .Any(e => e.IdReceiver == currentUser.Id && e.IdSender == album.UserId);
        }

        public List<Domain.Album> GetAll(string idUser,CurrentUser currentUser)
        {
            var user = unitOfWork.Users.Query.First(e => e.Id == idUser);
            if (user.IsBanned && !currentUser.IsAdmin) return new List<Domain.Album>();
            
            return unitOfWork.Albums.Query
                .Where(e => e.UserId == idUser).AsNoTracking()
                .Include(e => e.Photo).AsNoTracking()
                .OrderBy(e => e.Id)
                .ToList();
        }

        public List<Domain.Photo> GetPhotos(int albumId,CurrentUser currentUser)
        {
            var album = unitOfWork.Albums.Query.Include(e=>e.User).First(e => e.Id == albumId);
            if (album.User.IsBanned && !currentUser.IsAdmin) return new List<Domain.Photo>();
            return unitOfWork.Photos.Query
                .Where(e => e.AlbumId == albumId)
                .ToList();
        }

        public int AddAlbum(string denumire,CurrentUser currentUser)
        {
            if (currentUser.IsBanned) return -1;

            Domain.Album album = new Domain.Album { Name = denumire, UserId = currentUser.Id };
            unitOfWork.Albums.Add(album);
            unitOfWork.SaveChanges();
            return album.Id;
        }
        public bool CanDeleteAlbum(int albumId,CurrentUser currentUser)
        {
            if (currentUser.IsAdmin) return true;
            if (currentUser.IsBanned) return false;
            return unitOfWork.Albums.Query
                .Any(e => e.Id == albumId && e.UserId == currentUser.Id);
        }

        public string RemoveAlbum(int albumId,CurrentUser currentUser)
        {
            if (currentUser.IsBanned) return "-1";
            var album = unitOfWork.Albums.Query.FirstOrDefault(e => e.Id == albumId);
            if (album == null) return "-1";

            var profilePhoto = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == album.UserId).PhotoId;
            if (profilePhoto != null)
            {
                var photo = unitOfWork.Photos.Query.FirstOrDefault(e => e.Id == profilePhoto);
                if (photo.AlbumId == albumId)
                {
                    var user = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == album.UserId);
                    user.PhotoId = null;
                    unitOfWork.Users.Update(user);
                }
            }
            unitOfWork.Photos.RemoveRange(unitOfWork.Photos.Query.Where(e => e.AlbumId == albumId));
            unitOfWork.Albums.Remove(album);
            unitOfWork.SaveChanges();
            return album.UserId;

        }

        public bool HasThisAlbum(int albumId,CurrentUser currentUser)
        {
            return unitOfWork.Albums.Query
                .Any(e => e.Id == albumId && e.UserId == currentUser.Id);
        }

        public Domain.Album GetAlbum(int albumId)
        {
            return unitOfWork.Albums.Query
                //.Include(e => e.Photo)
                .FirstOrDefault(e => e.Id == albumId);
        }

        public bool ChangeName(int albumId,string name)
        {
            var album = unitOfWork.Albums.Query.FirstOrDefault(e => e.Id == albumId);
            if (album == null) return false;
            album.Name = name;
            unitOfWork.Albums.Update(album);
            return unitOfWork.SaveChanges()!=0;
        }
    }
}
