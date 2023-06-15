using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenter.ViewModel
{
    internal class ProfileOrderViewModel : BaseNotifyPropertyChanged
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
        public ProfileOrderViewModel()
        {
            Orders = new ObservableCollection<OrderModel>();
            var orders = App.Context.Orders.Where(x => x.UserId == LoginViewModel.LoginedUser.Id).ToList();
            foreach (var order in orders)
            {
                var orderModel = new OrderModel();
                orderModel.FillModel(order);
                Orders.Add(orderModel);
            }
        }
    }
}
