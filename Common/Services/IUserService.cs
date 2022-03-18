using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IUserService
    {
        public List<User> GetUsers();
        public User GetUser(string Id);
        public User Save(User user);
        public User Update(string Id, User user);
        public void Delete(string Id);
    }
}
