using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenterDAL.Context
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public decimal Price { get; set; }
        public int TypeId { get; set; }
        public virtual ProductType Type { get; set; }
        public virtual List<OrderProducts> OrderProducts { get; set; }
    }
}
