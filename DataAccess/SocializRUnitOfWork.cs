using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Domain;
using DataAccess.Base;

namespace DataAccess
{
    public class SocializRUnitOfWork : Base.BaseUnitOfWork
    {


        public SocializRUnitOfWork(SocializRContext context) : base(context)
        {
        }

        private IRepository<Post> posts;
        public IRepository<Post> Posts => posts ?? (posts = new BaseRepository<Post>(DbContext));

        private IRepository<ApplicationUser> users;
        public IRepository<ApplicationUser> Users => users ?? (users = new BaseRepository<ApplicationUser>(DbContext));

        private IRepository<Album> albums;
        public IRepository<Album> Albums => albums ?? (albums = new BaseRepository<Album>(DbContext));

        private IRepository<Comment> comments;
        public IRepository<Comment> Comments => comments ?? (comments = new BaseRepository<Comment>(DbContext));

        private IRepository<County> counties;
        public IRepository<County> Counties => counties ?? (counties = new BaseRepository<County>(DbContext));

        private IRepository<Friendship> friendships;
        public IRepository<Friendship> Friendships => friendships ?? (friendships = new BaseRepository<Friendship>(DbContext));

        private IRepository<Interest> interests;
        public IRepository<Interest> Interests => interests ?? (interests = new BaseRepository<Interest>(DbContext));

        private IRepository<InterestsUsers> interestsUserss;
        public IRepository<InterestsUsers> InterestsUserss => interestsUserss ?? (interestsUserss = new BaseRepository<InterestsUsers>(DbContext));
        private IRepository<Locality> localities;
        public IRepository<Locality> Localities => localities ?? (localities = new BaseRepository<Locality>(DbContext));

        private IRepository<Message> messages;
        public IRepository<Message> Messages => messages ?? (messages = new BaseRepository<Message>(DbContext));
        private IRepository<Group> groups;
        public IRepository<Group> Groups => groups ?? (groups = new BaseRepository<Group>(DbContext));
        public IRepository<ApplicationUserGroup> applicationUserGroups;
        public IRepository<ApplicationUserGroup> ApplicationUserGroups => applicationUserGroups ?? (applicationUserGroups = new BaseRepository<ApplicationUserGroup>(DbContext));

        private IRepository<Photo> photos;
        public IRepository<Photo> Photos => photos ?? (photos = new BaseRepository<Photo>(DbContext));
        private IRepository<Reaction> reactions;
        public IRepository<Reaction> Reactions => reactions ?? (reactions = new BaseRepository<Reaction>(DbContext));
        private IRepository<Role> roles;
        public IRepository<Role> Roles => roles ?? (roles = new BaseRepository<Role>(DbContext));

    }
}
