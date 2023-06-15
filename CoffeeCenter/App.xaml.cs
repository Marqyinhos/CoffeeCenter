using CoffeeCenterDAL.Context;
using CoffeeCenterDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeeCenter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CoffeeCenterContext Context { get; set; }
        public App()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("uk-UA");
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            Context = new CoffeeCenterContext();
            if (Context.Roles.Count() == 0)
            {
                Context.Roles.Add(new Role() { Name = "User" });
                Context.Roles.Add(new Role() { Name = "PhoneTemp" });
                Context.Roles.Add(new Role() { Name = "Admin" });
                Context.SaveChanges();

                Context.OrderStatuses.Add(new OrderStatus() { StatusName = "Обробка" });
                Context.OrderStatuses.Add(new OrderStatus() { StatusName = "Завершений" });
                Context.OrderStatuses.Add(new OrderStatus() { StatusName = "Доставка" });
                Context.OrderStatuses.Add(new OrderStatus() { StatusName = "Відмінено" });
                Context.SaveChanges();               
            }
        }
    }
}
