using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.Comment
{
    public class CommentService : Base.BaseService
    {
        private readonly CurrentUser CurrentUser;
        public CommentService(CurrentUser currentUser,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            CurrentUser = currentUser;
        }


        public int AddComment(string text,int PostId)
        {
            var user = unitOfWork.Users.Query.First(e => e.Id == CurrentUser.Id);
            if (user.IsBanned) return -1;

            var comment = new Domain.Comment()
            {
                Content = text,
                PostId = PostId,
                UserId = CurrentUser.Id,
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

        public bool CanDeleteComment(int commentId)
        {
            if (CurrentUser.IsAdmin) return true;
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == CurrentUser.Id)?.IsBanned ?? false;
            if (isBanned) return false;
            var comment = unitOfWork.Comments.Query
                .First(e => e.Id == commentId);
            if (comment == null) return false;
            var bool1 = comment.UserId == CurrentUser.Id;
            if (bool1) return true;
            var postId = comment.PostId;
            return unitOfWork.Posts.Query.First(e => e.Id == postId).UserId == CurrentUser.Id;
        }

        public List<Domain.Comment> GetComments(int toSkip,int howMany,int postId)
        {
            return unitOfWork.Comments.Query.OrderByDescending(e => e.AddingMoment)
                .Where(e=>e.PostId==postId&&!e.User.IsBanned)
                .Skip(toSkip)
                .Take(howMany)
                .Include(e => e.User)
                .AsNoTracking()
                .ToList();
        }
    }
}
