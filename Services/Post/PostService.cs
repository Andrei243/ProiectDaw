using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataAccess;

namespace Services.Post
{
   
    public class PostService : Base.BaseService
    {
        

        public PostService(SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        
        public bool AddPost(Domain.Post post, CurrentUser currentUser)
        {
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false;
            if (isBanned) return false;

            post.AddingMoment = DateTime.Now;
            post.UserId = currentUser.Id;
            post.Confidentiality = post.Confidentiality ?? Confidentiality.FriendsOnly;
            unitOfWork.Posts.Add(post);
            return unitOfWork.SaveChanges()!=0;

        }

      

        public List<Domain.Post> GetPersonPost(int toSkip,int howMany,string userId)
        {
            return GetFeed()
                .Where(e => e.UserId == userId)
                .OrderByDescending(e=>e.AddingMoment)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
        }

        public bool CanDetelePost(int postId, CurrentUser currentUser)
        {
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false;
            if (isBanned) return false;

            var post = unitOfWork.Posts.Query.FirstOrDefault(e => e.Id == postId);
            if (post == null) return false;
            return post.UserId == currentUser.Id || currentUser.IsAdmin;
        }

        private IQueryable<Domain.Post> GetFeed()
        {
            return unitOfWork
                            .Posts
                            .Query
                            .Include(e => e.User)
                            .Include(e => e.Photo)
                            .Include(e => e.Reaction)
                            .Where(e=>!e.User.IsBanned)
                            ;
        }

        public List<Domain.Post> GetPublicNewsfeed(int toSkip,int howMany)
        {
            return GetFeed()
                .Where(e=>e.Confidentiality == Confidentiality.Public)
                .OrderByDescending(e => e.AddingMoment)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
        }

        public List<Domain.Post> GetNewsfeed(int toSkip,int howMany, CurrentUser currentUser)
        {

            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false;
            if (isBanned) return GetPublicNewsfeed(toSkip,howMany);
            var friends = unitOfWork.Friendships.Query
                .Where(e => e.IdReceiver == currentUser.Id && (e.Accepted??false))
                .Select(e=>e.IdSender)
                .ToList();

            
            var posts = GetFeed()
                .Where(e =>
                (e.Confidentiality == Confidentiality.Public) ||
                (e.UserId == currentUser.Id) ||
                (friends.Contains(e.UserId) && e.Confidentiality == Confidentiality.FriendsOnly)
                )
                .OrderByDescending(e => e.AddingMoment)
                .Skip(toSkip)
                .Take(howMany)
                .ToList();
            return posts;
        }
        

        public bool RemovePost(int postId)
        {
            var post = unitOfWork.Posts.Query
                .FirstOrDefault(e => e.Id == postId);
            if (post == null) return false;

            var reactions = unitOfWork.Reactions.Query.Where(e => e.PostId == postId);
            var comments = unitOfWork.Comments.Query.Where(e => e.PostId == postId);
            var photos = unitOfWork.Photos.Query.Where(e => e.PostId == postId);
            unitOfWork.Reactions.RemoveRange(reactions);
            unitOfWork.Comments.RemoveRange(comments);
            unitOfWork.Photos.RemoveRange(photos);
            unitOfWork.Posts.Remove(post);
            return unitOfWork.SaveChanges() != 0;
        }

        public bool CanSeePost(int postId, CurrentUser currentUser)
        {
            if (currentUser.IsAdmin) return true;
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false;
            if (isBanned) return false;

            var post = unitOfWork.Posts.Query.Include(e=>e.User).FirstOrDefault(e => e.Id == postId);
            if (post == null || post.User.IsBanned) return false;
            if (post.UserId == currentUser.Id || post.Confidentiality == Confidentiality.Public) return true;
            var suntPrieteni = unitOfWork.Friendships.Query
                .Any(e => e.IdReceiver == post.UserId && e.IdSender == currentUser.Id && (e.Accepted ?? false));
            if (suntPrieteni) return post.Confidentiality == Confidentiality.FriendsOnly;
            else return false;
        }
    }
}
