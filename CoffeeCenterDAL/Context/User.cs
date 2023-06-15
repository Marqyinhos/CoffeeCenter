using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenterDAL.Context
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
    }
}
