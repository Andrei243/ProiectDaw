using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Services.InterestsUsers
{
    public class InterestsUsersService : Base.BaseService
    {

        public InterestsUsersService( SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<Domain.Interest> GetAllInterests(string idUser)
        {
            return unitOfWork.InterestsUserss.Query.AsNoTracking().Where(e => e.UserId == idUser).Select(e=>e.Interest).ToList();

        }

        public bool ChangeInterests(string userId,List<int> interestId)
        {
            unitOfWork.InterestsUserss.RemoveRange(unitOfWork.InterestsUserss.Query.Where(e => e.UserId == userId));
            unitOfWork.InterestsUserss.AddRange(interestId.Select(e => new Domain.InterestsUsers()
            {
                UserId = userId,
                InterestId = e
            }));
            return unitOfWork.SaveChanges()!=0;
        }

    }
}
