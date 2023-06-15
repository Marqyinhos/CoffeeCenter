using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using CoffeeCenterDAL.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class OrdersPageViewModel : BaseNotifyPropertyChanged
    {
        ObservableCollection<OrderModel> orders;
        public ObservableCollection<OrderModel> Orders
        {
            get => orders;
            set
            {
                orders = value;
                NotifyPropertyChanged();
            }
        }

        int page = 0;
        int pageCount = 5;
        public ICommand PrevPage { get; set; }
        public ICommand NextPage { get; set; }
        public OrdersPageViewModel()
        {
            SetOrders();
            InitCommands();
        }
        void InitCommands()
        {
            PrevPage = new RelayCommand(x =>
            {
                if (page - 1 >= 0)
                {
                    page--;
                    SetOrders();
                }
            });
            NextPage = new RelayCommand(x =>
            {
                if (page + 1 < App.Context.Orders.Count() / pageCount)
                {
                    page++;
                    SetOrders();
                }
            });
        }
        void SetOrders()
        {
            Orders = new ObservableCollection<OrderModel>();
            var orders = App.Context.Orders
                                    .OrderByDescending(or => or.Id)
                                    .Skip(pageCount * page)
                                    .Take(pageCount)
                                    .ToList();
            foreach (var order in orders)
            {
                var orderModel = new OrderModel();
                orderModel.FillModel(order);
                Orders.Add(orderModel);
            }
        }
    }
}
