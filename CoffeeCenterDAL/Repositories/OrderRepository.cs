using CoffeeCenterDAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenterDAL.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(DbContext context) : base(context)
        {
        }
    }
}
