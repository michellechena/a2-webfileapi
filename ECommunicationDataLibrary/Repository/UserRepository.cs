using ECommunicationDataLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommunicationDataLibrary.Repository
{
    public sealed class UserRepository
    {
        private static ECommunicationEntities dbContext;

        private static readonly Lazy<UserRepository> Repository = new Lazy<UserRepository>(() => new UserRepository());
        private UserRepository()
        {
            dbContext = DBContextRepository.Instance;
        }

        public static UserRepository Instance
        {
            get
            {
                return Repository.Value;
            }
        }
        public IQueryable<User> GetAll()
        {
            return dbContext.Users.AsQueryable();
        }

        public User GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.UserId == id);
        }

        public User Create(User user)
        {
            var emp = dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return emp;
        }

        public User Update(User user)
        {
            if (user == null)
            {
                return null;
            }
            else
            {
                var existing = dbContext.Users.FirstOrDefault(x => x.UserId == user.UserId);
                existing.FirstName = user.FirstName;
                existing.LastName = user.LastName;
                dbContext.SaveChanges();

                return user;
            }
        }

        public User Delete(int id)
        {
            var userToRemove = GetById(id);
            var usr = dbContext.Users.Remove(userToRemove);
            dbContext.SaveChanges();
            return usr;
        }
    }
}
