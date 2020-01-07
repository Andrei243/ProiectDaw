using System;
using System.Collections.Generic;
using DataAccess;
using System.Linq;
using System.Data.Entity;

namespace Services.Comment
{
    public class CommentService : Base.BaseService
    {
        public CommentService(SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public int AddComment(string text,int PostId,CurrentUser currentUser)
        {
            var user = unitOfWork.Users.Query.First(e => e.Id == currentUser.Id);
            if (user.IsBanned) return -1;

            var comment = new Domain.Comment()
            {
                Content = text,
                PostId = PostId,
                UserId = currentUser.Id,
                AddingMoment=DateTime.Now
            };
            unitOfWork.Comments.Add(comment);
             unitOfWork.SaveChanges();
            return comment.Id;
        }

        public bool RemoveComment(int commentId)
        {
            var comment = unitOfWork.Comments.Query
                .FirstOrDefault(e => e.Id == commentId);

            if (comment == null) return false;
            unitOfWork.Comments.Remove(comment);
            return unitOfWork.SaveChanges() != 0;
        }

        public bool CanDeleteComment(int commentId,CurrentUser currentUser)
        {
            if (currentUser.IsAdmin) return true;
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false;
            if (isBanned) return false;
            var comment = unitOfWork.Comments.Query
                .First(e => e.Id == commentId);
            if (comment == null) return false;
            var bool1 = comment.UserId == currentUser.Id;
            if (bool1) return true;
            var postId = comment.PostId;
            return unitOfWork.Posts.Query.First(e => e.Id == postId).UserId == currentUser.Id;
        }

        public List<Domain.Comment> GetComments(int toSkip,int howMany,int postId)
        {
            return unitOfWork.Comments.Query.OrderByDescending(e => e.AddingMoment)
                .Where(e=>e.PostId==postId&&!e.User.IsBanned)
                .OrderByDescending(e=>e.AddingMoment)
                .Skip(toSkip)
                .Take(howMany)
                .Include(e => e.User)
                .AsNoTracking()
                .ToList();
        }
    }
}
