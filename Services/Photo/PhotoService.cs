using System.Collections.Generic;
using DataAccess;
using System.Linq;
using System.Data.Entity;

namespace Services.Photo
{
    public class PhotoService : Base.BaseService
    {

        public PhotoService(SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool CanSeePhoto(int photoId, CurrentUser currentUser)
        {
            if (currentUser.IsAdmin)
            {
                return true;
            }

            var photo = unitOfWork.Photos.Query
                .Include(e=>e.Post)
                .Include("Album.User")
                .FirstOrDefault(e => e.Id == photoId);
            if (photo == null)
            {
                return false;
            }
            if (photo.Post != null)
            {
                if (photo.Post.UserId == currentUser.Id) return true;
                if (photo.Post.Confidentiality == Confidentiality.Public) return true;
                else if (photo.Post.Confidentiality == Confidentiality.Private) return false;
                else if (unitOfWork.Friendships.Query.Any(e => e.IdReceiver == currentUser.Id && e.IdSender == photo.Post.UserId && (e.Accepted ?? false)))
                {
                    return true;
                }
            }
            else if (photo.Album != null)
            {
                var user = photo.Album.User;
                if (user.PhotoId == photoId) return true;
                if (currentUser.Id == user.Id) return true;
                if (user.Confidentiality == Confidentiality.Public) return true;
                else if (user.Confidentiality == Confidentiality.Private) return false;
                else if (unitOfWork.Friendships.Query.Any(e => e.IdReceiver == currentUser.Id && e.IdSender == user.Id && (e.Accepted ?? false)))
                {
                    return true;
                }
            }
                return false;
            
        }
        public void ChangeDescription(int photoId,string description)
        {
            var image = unitOfWork.Photos.Query
                .FirstOrDefault(e => e.Id == photoId);
            image.Description = description;
            unitOfWork.Photos.Update(image);
            unitOfWork.SaveChanges();

        }

        public bool HasThisPhoto(int photoId,string userId)
        {
            var photo = unitOfWork.Photos.Query
                .FirstOrDefault(e => e.Id == photoId);
            var albums = unitOfWork.Albums.Query
                .Where(e => e.UserId == userId);
            return albums.Select(e => e.Id).Contains(photo.AlbumId.Value);

        }
        public void AddPhoto(Domain.Photo photo, CurrentUser currentUser)
        {
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false;
            if (isBanned) return;

            unitOfWork.Photos.Add(photo);
            if (photo.PostId.HasValue)
            {
                var post = unitOfWork.Posts.Query
                    .Include(e=>e.Photo)
                    .FirstOrDefault(e => e.Id == photo.PostId);
                
                post.Photo = photo;
                unitOfWork.Posts.Update(post);
            }
            else
            {
                photo.Position = unitOfWork.Photos.Query
                    .Where(e => e.AlbumId == photo.AlbumId)
                    .Count() + 1;
            }
            unitOfWork.SaveChanges();
        }

        public bool RemovePhoto(int photoId)
        {
            var photo = unitOfWork.Photos.Query
                .AsNoTracking()
                .First(e => e.Id == photoId);
            if (photo == null) return false;
            int? postId = photo.PostId;
            int? albumId= photo.AlbumId;
            IQueryable<Domain.Photo> listaPoze;
            if (postId != null)
            {
                listaPoze = unitOfWork.Photos.Query
                    .Where(e => e.PostId == postId);
            }
            else
            {
                listaPoze = unitOfWork.Photos.Query
                    .Where(e => e.AlbumId == albumId);
            }
            var poze = listaPoze
                .OrderBy(e => e.Position)
                .ToList();

           

            if(albumId!= null)
            {
                var user = unitOfWork.Albums.Query.Where(e => e.Id == albumId).Select(e => e.User).FirstOrDefault();
                if (user.PhotoId == photo.Id)
                {
                    user.PhotoId = null;
                    unitOfWork.Users.Update(user);
                }
            }

            foreach (var poza in poze)
            {
                if (poza.Id == photoId)
                {
                    unitOfWork.Photos
                        .Remove(poza);
                }
                else if (poza.Position > photo.Position)
                {
                    poza.Position -= 1;
                    unitOfWork.Photos.Update(poza);
                }
            }

            return unitOfWork.SaveChanges() != 0;
        }

        public Domain.Photo GetPhoto(int id)
        {
            return unitOfWork.Photos.Query
                .FirstOrDefault(e => e.Id == id);
        }

        public List<Domain.Photo> GetPhotos(int toSkip,int howMany,int albumId)
        {
            return unitOfWork.Photos.Query
                .Where(e => e.AlbumId == albumId)
                .OrderBy(e => e.Position)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
        }

    }
}
