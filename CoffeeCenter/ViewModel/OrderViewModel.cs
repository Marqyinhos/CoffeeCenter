using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using CoffeeCenter.View;
using CoffeeCenterDAL.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CoffeeCenter.ViewModel
{
    internal class OrderViewModel : BaseNotifyPropertyChanged
    {
        int orderId = 0;
        public delegate void SetOrderProducts();
        public static event SetOrderProducts Notify;
        static List<OrderProducts> orderProducts = new List<OrderProducts>();
        public static List<OrderProducts> OrderProducts
        {
            get => orderProducts;
            set
            {
                orderProducts = value;
            }
        }
        DateTime startDate = DateTime.Now;
        DateTime deliveryDate = DateTime.Now.AddMinutes(1);
        public DateTime DeliveryDate
        {
            get => deliveryDate;
            set
            {
                if (value > startDate)
                {
                    var date = value;
                    if (date.Minute <= 30)
                        date = date.AddMinutes(30 - date.Minute);
                    else
                        date = date.AddMinutes(60 - date.Minute);
                    deliveryDate = date;
                }
                NotifyPropertyChanged();
            }
        }
        public DateTime TodayDate { get; set; } = DateTime.Today;
        string phoneNumber;
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                NotifyPropertyChanged();
            }
        }
        string userName;
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                NotifyPropertyChanged();
            }
        }
        string deliveryAddress;
        public string DeliveryAddress
        {
            get => deliveryAddress;
            set
            {
                deliveryAddress = value;
                NotifyPropertyChanged();
            }
        }
        string totalPriceText;
        public string TotalPriceText
        {
            get => totalPriceText;
            set
            {
                totalPriceText = value;
                NotifyPropertyChanged();
            }
        }
        Visibility isOrderEmpty;
        public Visibility IsOrderEmpty
        {
            get => isOrderEmpty;
            set
            {
                isOrderEmpty = value;
                NotifyPropertyChanged();
            }
        }
        Visibility listViewVisibility;
        public Visibility ListViewVisibility
        {
            get => listViewVisibility;
            set
            {
                listViewVisibility = value;
                NotifyPropertyChanged();
            }
        }
        Visibility deliveryVisibility = Visibility.Collapsed;
        public Visibility DeliveryVisibility
        {
            get => deliveryVisibility;
            set
            {
                deliveryVisibility = value;
                NotifyPropertyChanged();
            }
        }
        Visibility seatsVisibility;
        public Visibility SeatsVisibility
        {
            get => seatsVisibility;
            set
            {
                seatsVisibility = value;
                NotifyPropertyChanged();
            }
        }
        bool isDelivery;
        public bool IsDelivery
        {
            get => isDelivery;
            set
            {
                isDelivery = value;
                DeliveryVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                SeatsVisibility = value ? Visibility.Collapsed : Visibility.Visible;
                NotifyPropertyChanged();
            }
        }
        ObservableCollection<ProductModel> products;
        public ObservableCollection<ProductModel> Products
        {
            get => products;
            set
            {
                products = value;
                NotifyPropertyChanged();
            }
        }
        ObservableCollection<int> numberOfSeats;
        public ObservableCollection<int> NumberOfSeats
        {
            get => numberOfSeats;
            set
            {
                numberOfSeats = value;
                NotifyPropertyChanged();
            }
        }
        ObservableCollection<string> statuses;
        public ObservableCollection<string> Statuses
        {
            get => statuses;
            set
            {
                statuses = value;
                NotifyPropertyChanged();
            }
        }
        int selectedSeat;
        public int SelectedSeat
        {
            get => selectedSeat;
            set
            {
                selectedSeat = value;
                NotifyPropertyChanged();
            }
        }
        string messageText;
        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                NotifyPropertyChanged();
            }
        }
        string finalButtonText;
        public string FinalButtonText
        {
            get => finalButtonText;
            set
            {
                finalButtonText = value;
                NotifyPropertyChanged();
            }
        }
        string selectedStatus;
        public string SelectedStatus
        {
            get => selectedStatus;
            set
            {
                selectedStatus = value;
                NotifyPropertyChanged();
            }
        }
        Visibility messageVisibility = Visibility.Collapsed;
        public Visibility MessageVisibility
        {
            get => messageVisibility;
            set
            {
                messageVisibility = value;
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
        public Visibility statusVisibility = Visibility.Collapsed;
        public Visibility StatusVisibility
        {
            get => statusVisibility;
            set
            {
                statusVisibility = value;
                NotifyPropertyChanged();
            }
        }
        Brush messageBrush = Brushes.Red;
        public Brush MessageBrush
        {
            get => messageBrush;
            set
            {
                messageBrush = value;
                NotifyPropertyChanged();
            }
        }
        bool isAdmin = false;
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                IsEnableWhyAdmin = !value;
                NotifyPropertyChanged();
            }
        }
        bool isEnableWhyAdmin = true;
        public bool IsEnableWhyAdmin
        {
            get => isEnableWhyAdmin;
            set
            {
                isEnableWhyAdmin = value;
                NotifyPropertyChanged();
            }
        }
        string tableErrorText = "На жаль, дане місце й час зайняті :(";
        string userErrorText = "Потрібно ввести ім'я і телефон";
        string newUserErrorText = "Проблеми з вашою реєстрацією";
        string addressErrorText = "Потрібно ввести адресу";
        string orderErrorText = "Для замовлення необхідно вибрати товари!";
        string successMessageText = "Ваше замовлення прийняте!";
        string orderUserText = "Замовити";
        string orderAdminText = "Зберегти";
        decimal totalPrice = 0;
        public ICommand ToMenu { get; set; }
        public ICommand Order { get; set; }
        public OrderViewModel()
        {
            Notify += LoadProducts;
            if(LoginViewModel.LoginedUser != null)
            {
                IsAdmin = LoginViewModel.LoginedUser.RoleId == App.Context.Roles.FirstOrDefault(x => x.Name == "Admin").Id;
                WhyAdminVisibility = IsAdmin ? Visibility.Collapsed : Visibility.Visible;
                StatusVisibility = IsAdmin ? Visibility.Visible : Visibility.Collapsed;
                if (!IsAdmin)
                {
                    UserName = LoginViewModel.LoginedUser.Name;
                    PhoneNumber = LoginViewModel.LoginedUser.Phone;
                    DeliveryAddress = LoginViewModel.LoginedUser.Address;
                }
            }
            FinalButtonText = IsAdmin ? orderAdminText : orderUserText;
            NumberOfSeats = new ObservableCollection<int>();
            List<Table> list = App.Context.Tables
                                    .GroupBy(x => x.SeatsNumber)
                                    .Select(x => x.FirstOrDefault())
                                    .ToList();
            var tab = list.FirstOrDefault();
            if (tab != null)
            {
                SelectedSeat = tab.SeatsNumber;
                foreach (var table in list)
                    NumberOfSeats.Add(table.SeatsNumber);
            }
            InitCommands();            
            LoadProducts();
        }
        void InitCommands()
        {
            ToMenu = new RelayCommand(x => Switcher.Switch(new MenuView()));
            Order = new RelayCommand(x =>
            {
                if(IsAdmin)
                {
                    SetStatus();
                    return;
                }
                if(OrderProducts.Count() == 0)
                {
                    MessageText = orderErrorText;
                    MessageBrush = Brushes.Red;
                    MessageVisibility = Visibility.Visible;
                    return;
                }
                if (DeliveryAddress == null && isDelivery)
                {
                    MessageText = addressErrorText;
                    MessageBrush = Brushes.Red;
                    MessageVisibility = Visibility.Visible;
                    return;
                }
                User currentUser = LoginViewModel.LoginedUser;
                if(UserName == null || PhoneNumber == null)
                {
                    MessageText = userErrorText;
                    MessageBrush = Brushes.Red;
                    MessageVisibility = Visibility.Visible;
                    return;
                }
                if (LoginViewModel.LoginedUser == null)
                {
                    currentUser = App.Context.Users.Add(new User()
                    {
                        RoleId = 2,
                        Name = UserName,
                        Phone = PhoneNumber,
                        Address = DeliveryAddress
                    });
                    App.Context.SaveChanges();
                    if(currentUser == null)
                    {
                        MessageText = newUserErrorText;
                        MessageBrush = Brushes.Red;
                        MessageVisibility = Visibility.Visible;
                        return;
                    }

                }
                int tableId = 1;
                if (!IsDelivery)
                    tableId = TableCheck();
                if(tableId == -1)
                {
                    MessageText = tableErrorText;
                    MessageBrush = Brushes.Red;
                    MessageVisibility = Visibility.Visible;
                    return;
                }
                var order = App.Context.Orders.Add(new Order()
                {
                    OrderStatusId = App.Context.OrderStatuses.FirstOrDefault(s => s.StatusName == "Обробка").Id,
                    TableId = tableId,
                    TotalPrice = totalPrice,
                    Date = DeliveryDate,
                    UserId = currentUser.Id
                });
                App.Context.SaveChanges();
                foreach (var orProduct in OrderProducts)
                App.Context.OrderProducts.Add(new OrderProducts()
                {
                    OrderId = order.Id,
                    Order = order,
                    Product = App.Context.Products.FirstOrDefault(p => p.Id == orProduct.Id),
                    ProductCount = orProduct.ProductCount,
                    ProductId = orProduct.ProductId
                });
                App.Context.SaveChanges();

                if(IsDelivery)
                {
                    App.Context.Deliveries.Add(new Delivery()
                    {
                        Address = DeliveryAddress,
                        OrderId = order.Id
                    });
                    App.Context.SaveChanges();
                }
                MessageText = successMessageText;
                MessageBrush = Brushes.Green;
                MessageVisibility = Visibility.Visible;
            });
        }
        public void LoadProducts()
        {
            Products = new ObservableCollection<ProductModel>();
            var product = new Product();
            decimal price = 0;
            foreach (var op in OrderProducts)
            {
                product = App.Context.Products.FirstOrDefault(x => x.Id == op.ProductId);
                Products.Add(new ProductModel()
                {
                    ProductTitle = "-" + product.Title,
                    ImageSource = product.Photo,
                    Price = "-" + product.Price + "грн",
                    OrderCount = op.ProductCount
                });
                price += product.Price * op.ProductCount;
            }
            totalPrice = price;
            TotalPriceText = Products.Count() == 0 ? "" : "Загальна сума: " + price + "грн";
            ListViewVisibility = Products.Count() == 0 ? Visibility.Collapsed : Visibility.Visible;
            IsOrderEmpty = Products.Count() == 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        int TableCheck()
        {
            DateTime firstDate = DeliveryDate.AddMinutes(-1);
            DateTime secondDate = DeliveryDate.AddMinutes(1);
            Reservation reservation = App.Context.Reservations
                .Where(r => firstDate < r.Date && r.Date < secondDate)
                .FirstOrDefault();
            if (reservation != null && reservation.IsActive)
            {
                var tables = App.Context.Tables.Where(t => t.SeatsNumber == SelectedSeat).ToList();
                foreach (var table in tables)
                    if (reservation.TableId != table.Id)
                        return table.Id;
                return -1;
            }
            return App.Context.Tables.Where(t => t.SeatsNumber == SelectedSeat).FirstOrDefault().Id;
        }
        public static void LoadInvoke()
        {
            Notify?.Invoke();
        }
        public void SetOrder(int id)
        {
            Order order = App.Context.Orders.FirstOrDefault(or => or.Id == id);
            orderId = order.Id;
            Delivery delivery = App.Context.Deliveries.FirstOrDefault(d => d.OrderId == id);
            User user = App.Context.Users.FirstOrDefault(us => order.UserId == us.Id);
            if (delivery != null)
            {
                DeliveryAddress = delivery.Address;
                IsDelivery = true;
            }
            DeliveryDate = order.Date;
            UserName = user.Name;
            PhoneNumber = user.Phone;
            SelectedSeat = App.Context.Tables.FirstOrDefault(t => t.Id == order.TableId).SeatsNumber;
            OrderProducts = App.Context.OrderProducts.Where(op => op.OrderId == order.Id).ToList();
            var statuses = App.Context.OrderStatuses.ToList();
            Statuses = new ObservableCollection<string>();
            foreach (var status in statuses)
                Statuses.Add(status.StatusName);
            SelectedStatus = statuses.FirstOrDefault(s => s.Id == order.OrderStatusId).StatusName;
            LoadProducts();
        }
        void SetStatus()
        {
            Order order = App.Context.Orders.FirstOrDefault(or => or.Id == orderId);
            order.OrderStatusId = App.Context.OrderStatuses.FirstOrDefault(s => s.StatusName == SelectedStatus).Id;
            App.Context.SaveChanges();
        }
    }
}
