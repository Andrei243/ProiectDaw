using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;

namespace Services.Reaction
{
    public class ReactionService : Base.BaseService
    {

        public ReactionService(SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public bool IsLiked(int postId, CurrentUser currentUser)
        {
            return unitOfWork.Reactions.Query
                .Any(e => e.PostId == postId && e.UserId == currentUser.Id);
        }

        public bool ChangeReaction(int postId, CurrentUser currentUser)
        {
            var isBanned = unitOfWork.Users.Query.FirstOrDefault(e => e.Id == currentUser.Id)?.IsBanned ?? false;
            if (isBanned) return unitOfWork.Reactions.Query.Any(e=>e.PostId==postId&&e.UserId==currentUser.Id);
            var reaction = unitOfWork.Reactions.Query.FirstOrDefault(e => e.PostId == postId && e.UserId == currentUser.Id);
            if (reaction == null)
            {
                var reaction2 = new Domain.Reaction()
                {
                    PostId = postId,
                    UserId = currentUser.Id
                };
                unitOfWork.Reactions.Add(reaction2);
                unitOfWork.SaveChanges();
                return true;
            }
            else
            {
                unitOfWork.Reactions.Remove(reaction);
                unitOfWork.SaveChanges();
                return false;
            }

            
        }

       
    }
}
