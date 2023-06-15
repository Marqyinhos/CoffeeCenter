using CoffeeCenter.Infrastructure;
using CoffeeCenter.View;
using CoffeeCenter.ViewModel;
using CoffeeCenterDAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoffeeCenter.Models
{
    internal class ProductModel :  BaseNotifyPropertyChanged
    {
        public string ProductTitle { get; set; }
        public string ImageSource { get; set; }
        public string Price { get; set; }
        public int orderCount = 1;
        public int OrderCount
        {
            get => orderCount;
            set
            {
                if (value > 0)
                {
                    orderCount = value;
                }
                NotifyPropertyChanged();
            }
        }
        public Visibility whyAdminVisibility = Visibility.Visible;
        public Visibility WhyAdminVisibility
        {
            get => whyAdminVisibility;
            set
            {
                whyAdminVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand LessCount { get; set; }
        public ICommand PlusCount { get; set; }
        public ICommand AddToOrder { get; set; }
        public ICommand RemoveFromOrder { get; set; }
        public ProductModel()
        {
            if(LoginViewModel.LoginedUser != null)
                WhyAdminVisibility = App.Context.Roles
                                                .FirstOrDefault(r => r.Name == "Admin")
                                                .Id 
                                                == 
                                                LoginViewModel.LoginedUser.RoleId 
                                                ? 
                                                Visibility.Collapsed : Visibility.Visible;
            LessCount = new RelayCommand(x => 
            {
                OrderCount--;
                if (Convert.ToBoolean(x))
                {
                    OrderViewModel.OrderProducts
                              .FirstOrDefault(op =>
                                             op.ProductId == App.Context.Products
                                            .Where(p => ImageSource == p.Photo)
                                            .FirstOrDefault()
                                            .Id)
                              .ProductCount = OrderCount;
                    OrderViewModel.LoadInvoke();
                }
            });
            PlusCount = new RelayCommand(x => 
            {
                OrderCount++;
                if (Convert.ToBoolean(x))
                {
                    OrderViewModel.OrderProducts
                              .FirstOrDefault(op =>
                                             op.ProductId == App.Context.Products
                                            .Where(p => ImageSource == p.Photo)
                                            .FirstOrDefault()
                                            .Id)
                              .ProductCount = OrderCount;
                    OrderViewModel.LoadInvoke();
                }
            });
            AddToOrder = new RelayCommand(x =>
            {
                if (OrderViewModel.OrderProducts == null)
                    OrderViewModel.OrderProducts = new List<OrderProducts>();

                var orderProduct = App.Context.Products
                                    .Where(p => ImageSource == p.Photo)
                                    .FirstOrDefault();
                int index = -1;
                if (orderProduct != null)
                    index = OrderViewModel.OrderProducts.FindIndex(op => op.ProductId == orderProduct.Id);

                if (index != -1)
                {
                    OrderViewModel.OrderProducts[index].ProductCount += OrderCount;
                    return;
                }

                OrderViewModel.OrderProducts.Add(new OrderProducts()
                {
                     ProductId = App.Context.Products
                                   .Where(p => ImageSource == p.Photo)
                                   .FirstOrDefault()
                                   .Id,
                     ProductCount = OrderCount
                });
            });
            RemoveFromOrder = new RelayCommand(x =>
            {
                int opIndex = OrderViewModel.OrderProducts
                              .FindIndex(op =>
                                         op.ProductId == App.Context.Products
                                        .Where(p => ImageSource == p.Photo)
                                        .FirstOrDefault()
                                        .Id);
                OrderViewModel.OrderProducts.Remove(OrderViewModel.OrderProducts[opIndex]);
                OrderViewModel.LoadInvoke();
            });
        }
    }
}
