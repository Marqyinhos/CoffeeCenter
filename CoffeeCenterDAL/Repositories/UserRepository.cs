using CoffeeCenterDAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenterDAL.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        CoffeeCenterContext context;
        public UserRepository(DbContext context) : base(context)
        {
            this.context = (CoffeeCenterContext)context;
        }
        public User Login(string login, string password)
        {
            return context.Users.Where(x => x.Login == login && x.PasswordHash == password).FirstOrDefault();
        }
    }
}
