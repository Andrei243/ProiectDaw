using DataAccess;
using System.Collections.Generic;
using Domain;
using System.Linq;
using System.Data.Entity;

namespace Services.User
{
    public class UserService : Base.BaseService
    {
        private readonly CurrentUser currentUser;
        public UserService(CurrentUser currentUser, SocializRUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this.currentUser = currentUser;
        }

        public bool BanUser(string userId)
        {
            var user = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == userId);
            if (user.IsBanned) return false;
            user.IsBanned = true;
            unitOfWork.Users.Update(user);
            return unitOfWork.SaveChanges() != 0;
        }

        public bool UnbanUser(string userId)
        {
            var user = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == userId);
            if (!user.IsBanned) return false;
            user.IsBanned = false;
            unitOfWork.Users.Update(user);
            return unitOfWork.SaveChanges() != 0;
        }


        public Users GetCurrentUser()
        {
            return GetUserById(currentUser.Id);
        }
        public Users GetUserById(string id)
        {
            return unitOfWork
                .Users
                .Query
                .AsNoTracking()
                .Include("Locality.County")
                .AsNoTracking()
                .Include(e => e.Role)
                .AsNoTracking()
                .Include(e => e.InterestsUsers)
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id);
        }


        public IQueryable<Users> GetUsersByName(string name)
        {
            return unitOfWork
                .Users
                .Query
                .AsNoTracking()
                .Where(e => (e.Name + e.Surname).Contains(name));
        }


        public void Update(Users user)
        {
            Users oldUser = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == user.Id);
            oldUser.LocalityId = user.LocalityId ?? oldUser.LocalityId;
            oldUser.Confidentiality = user.Confidentiality ?? oldUser.Confidentiality;
            oldUser.BirthDay = user.BirthDay;
            oldUser.Name = user.Name;
            oldUser.Surname = user.Surname;
            oldUser.SexualIdentity = user.SexualIdentity;

            unitOfWork.Users.Update(oldUser);
            unitOfWork.SaveChanges();
        }


        public void UpdateProfilePhoto(int photoId)
        {
            var user = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id);
            user.PhotoId = photoId;
            unitOfWork.Users.Update(user);
            unitOfWork.SaveChanges();
        }

        public void MakeAdmin(string userId)
        {
            var user = unitOfWork.Users.Query.First(e => e.Id == userId);
            user.RoleId = 1;
            unitOfWork.Users.Update(user);
            unitOfWork.SaveChanges();

        }

        public void RevokeAdmin(string userId)
        {
            var user = unitOfWork.Users.Query.First(e => e.Id == userId);
            user.RoleId = 2;
            unitOfWork.Users.Update(user);
            unitOfWork.SaveChanges();

        }

        public bool RemoveUser(string userId)
        {
            var user = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == userId);
            if (user == null) return false;

            var albums = unitOfWork.Albums.Query.Where(e => e.UserId == userId);
            var posts = unitOfWork.Posts.Query.Where(e => e.UserId == userId);
            var comments = unitOfWork.Comments.Query.Where(e => e.UserId == userId || posts.Select(f => f.Id).Contains(e.PostId));
            var reactions = unitOfWork.Reactions.Query.Where(e => e.UserId == userId || posts.Select(f => f.Id).Contains(e.PostId));
            var friendships = unitOfWork.Friendships.Query.Where(e => e.IdReceiver == userId || e.IdSender == userId);
            var interestsUsers = unitOfWork.InterestsUserss.Query.Where(e => e.UserId == userId);
            var photos = unitOfWork.Photos.Query.Where(e => posts.Select(f => f.Id).Contains(e.PostId.Value) || albums.Select(f => f.Id).Contains(e.AlbumId.Value));
            unitOfWork.Photos.RemoveRange(photos);
            unitOfWork.InterestsUserss.RemoveRange(interestsUsers);
            unitOfWork.Friendships.RemoveRange(friendships);
            unitOfWork.Reactions.RemoveRange(reactions);
            unitOfWork.Comments.RemoveRange(comments);
            unitOfWork.Posts.RemoveRange(posts);
            unitOfWork.Albums.RemoveRange(albums);
            unitOfWork.Users.Remove(user);
            return unitOfWork.SaveChanges() != 0;
        }

        public List<Users> GetUsers(int toSkip, int toTake)
        {
            return unitOfWork.Users.Query
                .Where(e => e.Id != currentUser.Id)
                .OrderBy(e => e.BirthDay)
                .Skip(toSkip)
                .Take(toTake)
                .ToList();
        }
    }
}
