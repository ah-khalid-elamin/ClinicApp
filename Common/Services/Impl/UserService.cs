using Common.Contexts;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly ClinicAppDbContext _clinicAppDbContext;
        public UserService(ClinicAppDbContext clinicAppDbContext)
        {
            _clinicAppDbContext = clinicAppDbContext;
        }
        public void Delete(string Id)
        {
            User user = GetUser(Id);
            _clinicAppDbContext.Users.Remove(user);
            _clinicAppDbContext.SaveChanges();
        }

        public User GetUser(string Id)
        {
            return _clinicAppDbContext.Users.AsEnumerable().FirstOrDefault(c => c.Id == Id);
        }

        public List<User> GetUsers()
        {
            return _clinicAppDbContext.Users.ToList();
        }

        public User Save(User user)
        {
            var _existingUser = GetUser(user.Id);
            if (_existingUser == null)
            {
                _clinicAppDbContext.Users.Add(user);
                _clinicAppDbContext.SaveChanges();
            }
           
            
            return user;
        }

        public User Update(string Id, User user)
        {
            User _existingUser = GetUser(Id);
            if (_existingUser != null)
            {
                _clinicAppDbContext.Users.Update(user);
                _clinicAppDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No Such User Exist");
            }

            return user;
        }
    }
}
