using DataAccess;
using System.Linq;
using Domain;
using System.Data.Entity;

namespace Services.User
{
    public class UserAccountService : Base.BaseService
    {
        public static readonly int IDROLPUBLIC = 2;
        public UserAccountService(SocializRUnitOfWork unitOfWork):
            base(unitOfWork)
        { }

        public Users Get(string email)
        {

            return unitOfWork.Users.Query.AsNoTracking().Include(e=>e.Role).AsNoTracking()
                .Include(e=>e.Locality).AsNoTracking()
                .Include("Locality.County").AsNoTracking()
                .FirstOrDefault(e => e.Email == email);
        }

        public Users Login(string email, string password)
        {
            return unitOfWork.Users.Query.Include(e=>e.Role)
                .FirstOrDefault(e => e.Email == email && e.Password == password);
        }

        public bool Register(Users user)
        {
            user.Confidentiality = Confidentiality.FriendsOnly;
            user.RoleId = IDROLPUBLIC;
            unitOfWork.Users.Add(user);
            return unitOfWork.SaveChanges() != 0;
        }

        public bool EmailExists(string email)
        {
            return unitOfWork.Users.Query.AsNoTracking().Any(e => e.Email == email);
        }

    }
}
