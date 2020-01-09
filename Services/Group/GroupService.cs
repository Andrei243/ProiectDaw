using DataAccess;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services.Group
{
     public class GroupService : BaseService
    {
        public GroupService(SocializRUnitOfWork unitOfWork):base(unitOfWork)
        {
            
        }


        public int CreateGroup(string name,CurrentUser currentUser)
        {
            var group = new Domain.Group()
            {
                AdminId = currentUser.Id,
                Name = name
            };
            unitOfWork.Groups.Add(group);

            unitOfWork.Messages.Add(new Domain.Message()
            {
                Content = "Welcome to the " + name + " group",
                GroupId = group.Id,
                SenderId = currentUser.Id,
                SendingMoment = DateTime.Now
            });

            unitOfWork.ApplicationUserGroups.Add(new Domain.ApplicationUserGroup()
            {
                GroupId = group.Id,
                UserId = currentUser.Id
            });

            return unitOfWork.SaveChanges();
            

        }

        public List<Domain.Group> GetGroups(string userId)
        {
            return unitOfWork.ApplicationUserGroups.Query
                .Where(e => e.UserId == userId)
                .Select(e => e.Group).ToList();

        }

        public List<Domain.Group> GetGroupsNotIncluded(string userId)
        {
            var groups = unitOfWork.ApplicationUserGroups.Query
                .Where(e => e.UserId == userId)
                .Select(e => e.GroupId);

            var result = unitOfWork.Groups.Query.Where(e => !groups.Contains(e.Id)).ToList();
            return result;
        }

        public Domain.Group GetGroup(int groupId)
        {
            return unitOfWork.Groups.Query.Where(e => e.Id == groupId)
                .Include("Users.User").FirstOrDefault();

        }

        public void JoinGroup(int id,CurrentUser currentUser)
        {
            var isAlready = unitOfWork.ApplicationUserGroups.Query.Any(e => e.GroupId == id && e.UserId == currentUser.Id);
            if (!isAlready)
            {
                unitOfWork.ApplicationUserGroups.Add(new Domain.ApplicationUserGroup()
                {
                    GroupId = id,
                    UserId = currentUser.Id
                });
                unitOfWork.SaveChanges();
            }
        }

        public void ExitGroup(int id,CurrentUser currentUser)
        {
            var group = unitOfWork.ApplicationUserGroups.Query.FirstOrDefault(e => e.GroupId == id && e.UserId == currentUser.Id);
            if (group != null)
            {
                unitOfWork.ApplicationUserGroups.Remove(group);
                unitOfWork.SaveChanges();
            }

        }

    }
}
