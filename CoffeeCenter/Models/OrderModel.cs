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
using System.Windows.Media;

namespace CoffeeCenter.Models
{
    internal class OrderModel : BaseNotifyPropertyChanged
    {
        int orderId = 0;
        int orderNumber;
        public string OrderNumber
        {
            get => "Замовлення № " + orderNumber.ToString();
            set
            {
                orderNumber = Convert.ToInt32(value);
                NotifyPropertyChanged();
            }
        }
        int tableNumber;
        public string TableNumber
        {
            get => "Столик № " + tableNumber.ToString();
            set
            {
                tableNumber = Convert.ToInt32(value);
                NotifyPropertyChanged();
            }
        }
        string status;
        public string Status
        {
            get => "Статус замовлення: " + status.ToString();
            set
            {
                status = value;
                NotifyPropertyChanged();
            }
        }
        decimal totalPrice;
        public string TotalPrice
        {
            get => "Загальна сума: " + totalPrice.ToString() + "грн";
            set
            {
                totalPrice = Convert.ToDecimal(value);
                NotifyPropertyChanged();
            }
        }
        DateTime date;
        public string Date
        {
            get => "Дата замовлення: " + date.ToString();
            set
            {
                date = Convert.ToDateTime(value);
                NotifyPropertyChanged();
            }
        }
        Visibility addressVisibility = Visibility.Collapsed;
        public Visibility AddressVisibility
        {
            get => addressVisibility;
            set
            {
                addressVisibility = value;
                NotifyPropertyChanged();
            }
        }
        Visibility tableVisibility;
        public Visibility TableVisibility
        {
            get => tableVisibility;
            set
            {
                tableVisibility = value;
                NotifyPropertyChanged();
            }
        }
        Visibility cancelVisibility;
        public Visibility CancelVisibility
        {
            get => cancelVisibility;
            set
            {
                cancelVisibility = value;
                NotifyPropertyChanged();
            }
        }
        string address;
        public string Address
        {
            get => "Адреса доставки: " + address?.ToString();
            set
            {
                address = value;
                NotifyPropertyChanged();
            }
        }
        string productList;
        public string ProductList
        {
            get => "Список товарів: " + productList.ToString();
            set
            {
                productList = value;
                NotifyPropertyChanged();
            }
        }
        Brush statusBrush;
        public Brush StatusBrush
        {
            get => statusBrush;
            set
            {
                statusBrush = value;
                NotifyPropertyChanged();
            }
        }
        Brush totalPriceBrush;
        public Brush TotalPriceBrush
        {
            get => totalPriceBrush;
            set
            {
                totalPriceBrush = value;
                NotifyPropertyChanged();
            }
        }
        Brush dateBrush;
        public Brush DateBrush
        {
            get => dateBrush;
            set
            {
                dateBrush = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand CancelOrder { get; set; }
        public ICommand EditCommand { get; set; }
        public OrderModel()
        {
            CancelOrder = new RelayCommand(x =>
            {
                var order = App.Context.Orders.FirstOrDefault(or => or.Id == orderId);
                order.OrderStatusId = App.Context.OrderStatuses
                                                 .FirstOrDefault(os => os.StatusName == "Відмінено").Id;
                App.Context.SaveChanges();
                Status = "Відмінено";
                CancelVisibility = Visibility.Collapsed;
            });
            EditCommand = new RelayCommand(x =>
            {
                var view = new OrderView();
                var vm = view.DataContext as OrderViewModel;
                vm.SetOrder(orderId);
                Switcher.Switch(view);
            });
        }
        public void FillModel(Order order)
        {
            orderId = order.Id;
            OrderNumber = order.Id.ToString();
            TableNumber = order.TableId.ToString();
            TotalPrice = order.TotalPrice.ToString();
            Date = order.Date.ToString();

            var productIds = App.Context.OrderProducts
                                        .Where(x => x.OrderId == order.Id)
                                        .Select(x => x.ProductId)
                                        .ToList();
            string list = "";
            bool isFirst = true;
            foreach(var productId in productIds)            
                if (isFirst)
                {
                    list += App.Context.Products.FirstOrDefault(x => x.Id == productId).Title;
                    isFirst = false;
                }
                else
                    list += ", " + App.Context.Products.FirstOrDefault(x => x.Id == productId).Title;
            ProductList = list;

            Status = App.Context.OrderStatuses.FirstOrDefault(x => x.Id == order.OrderStatusId).StatusName;
            if (status == App.Context.OrderStatuses
                                     .FirstOrDefault(x => x.StatusName == "Завершений")
                                     .StatusName
                || status == App.Context.OrderStatuses
                                     .FirstOrDefault(x => x.StatusName == "Відмінено")
                                     .StatusName)
                CancelVisibility = Visibility.Collapsed;
            var delivery = App.Context.Deliveries.FirstOrDefault(x => x.OrderId == order.Id);
            StatusBrush = delivery == null ? Brushes.Black : new SolidColorBrush(Color.FromRgb(97, 97, 97));
            TotalPriceBrush = delivery == null ? new SolidColorBrush(Color.FromRgb(97, 97, 97)) : Brushes.Black;
            DateBrush = delivery == null ? Brushes.Black : new SolidColorBrush(Color.FromRgb(97, 97, 97));            
            if (delivery != null)
            {
                Address = delivery?.Address;
                AddressVisibility = Visibility.Visible;
                TableVisibility = Visibility.Collapsed;
            }
        }
    }
}
