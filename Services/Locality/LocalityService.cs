using System.Collections.Generic;
using DataAccess;
using System.Linq;
using System.Data.Entity;

namespace Services.Locality
{
    public class LocalityService : Base.BaseService
    {

        public LocalityService(SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public bool CityAlreadyExistsInCounty(string locality,int countyId)
        {
            return unitOfWork.Localities.Query
                .Any(e => e.Name == locality && e.CountyId == countyId);
        }
        public Domain.Locality GetLocality(int id)
        {
            return unitOfWork.Localities.Query
                .FirstOrDefault(e => e.Id == id);
        }

        public bool AddLocality(string denumire,int countyId)
        {
            if (unitOfWork.Localities.Query.Any(e =>e.Name==denumire && e.CountyId==countyId)) return false;
            var locality = new Domain.Locality()
            {
                CountyId = countyId,
                Name = denumire
            };
            unitOfWork.Localities.Add(locality);
            return unitOfWork.SaveChanges() != 0;
        }

        public void EditLocality(int id,string name,int countyId)
        {
            var locality = unitOfWork.Localities.Query
                .FirstOrDefault(e => e.Id == id);
            locality.Name = name;
            locality.CountyId = countyId;
            unitOfWork.Localities.Update(locality);
            unitOfWork.SaveChanges();
        }

        public bool RemoveLocality(int id)
        {
            var locality = unitOfWork.Localities.Query
                .FirstOrDefault(e => e.Id == id);
            if (locality==null) return false;
            unitOfWork.Localities.Remove(locality);
            var users = unitOfWork.Users.Query
                .Where(e => e.LocalityId == id);
            foreach(var user in users)
            {
                user.LocalityId = null;
                unitOfWork.Users.Update(user);
            }
            
            return unitOfWork.SaveChanges() != 0;

        }

        public List<Domain.Locality> GetAll(int countyId)
        {
            return unitOfWork.Localities.Query
                .AsNoTracking().Where(e=>e.CountyId==countyId)
                .ToList();
        }

        public List<Domain.Locality> GetAll()
        {
            return unitOfWork.Localities.Query
                .Include(e=>e.County).AsNoTracking()
                .ToList();
        }

        public List<Domain.Locality> GetLocalities(int toSkip,int howMany)
        {
            return unitOfWork.Localities.Query
                .OrderBy(e => e.Id)
                .Skip(toSkip)
                .Take(howMany)
                .Include(e=>e.County)
                .AsNoTracking()
                .ToList();
        }

    }
}
