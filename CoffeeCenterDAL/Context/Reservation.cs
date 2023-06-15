using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenterDAL.Context
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public int TableId { get; set; }
        public virtual Table Table { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
