using CoffeeCenterDAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenterDAL.Repositories
{
    public class ProductTypeRepository : GenericRepository<ProductType>
    {
        public ProductTypeRepository(DbContext context) : base(context)
        {
        }
    }
}
