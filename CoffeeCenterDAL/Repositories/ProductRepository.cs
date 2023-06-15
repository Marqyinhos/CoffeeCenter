using CoffeeCenterDAL.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenterDAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }
    }
}
