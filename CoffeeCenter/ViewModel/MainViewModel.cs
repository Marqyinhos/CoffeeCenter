using CoffeeCenter.Infrastructure;
using CoffeeCenter.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    public class MainViewModel : BaseNotifyPropertyChanged, INavigate
    {
        string userName = "";
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                NotifyPropertyChanged();
            }
        }
        private UserControl control;
        public UserControl Control
        {
            get => control;
            set
            {
                control = value;
                NotifyPropertyChanged();   
            }
        }
        private Visibility isCustomerVisibility = Visibility.Visible;
        public Visibility IsCustomerVisibility
        {
            get => isCustomerVisibility;
            set
            {
                isCustomerVisibility = value;
                NotifyPropertyChanged();   
            }
        }
        private Visibility isAdminVisibility = Visibility.Collapsed;
        public Visibility IsAdminVisibility
        {
            get => isAdminVisibility;
            set
            {
                isAdminVisibility = value;
                NotifyPropertyChanged();   
            }
        }
        private Visibility isWhyAdminVisibility = Visibility.Visible;
        public Visibility IsWhyAdminVisibility
        {
            get => isWhyAdminVisibility;
            set
            {
                isWhyAdminVisibility = value;
                NotifyPropertyChanged();   
            }
        }
        bool isAdmin = false;
        bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                IsCustomerVisibility = isAdmin ? Visibility.Collapsed : Visibility.Visible;
                IsAdminVisibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
                IsWhyAdminVisibility = isAdmin ? Visibility.Hidden : Visibility.Visible;
            }
        }
        public ICommand Close { get; set; }
        public ICommand Login { get; set; }
        public ICommand BackToMain { get; set; }
        public ICommand ToMenu { get; set; }
        public ICommand ToReservation { get; set; }
        public ICommand ToOrder { get; set; }
        public ICommand ToMap { get; set; }
        public ICommand ToOrdersPage { get; set; }
        public ICommand ToReservationsPage { get; set; }
        public MainViewModel()
        {
            Control = new MainPage();
            Switcher.Content = this;            
            InitCommands();
        }
        void InitCommands()
        {
            Close = new RelayCommand(x => ((MainView)x).Close());
            Login = new RelayCommand(x =>
            {
                if (LoginViewModel.LoginedUser == null)
                    Switcher.Switch(new LoginView());
                else
                    Switcher.Switch(new ProfileView());
            });
            BackToMain = new RelayCommand(x => Switcher.Switch(new MainPage()));
            ToMenu = new RelayCommand(x => Switcher.Switch(new MenuView()));
            ToReservation = new RelayCommand(x => Switcher.Switch(new ReservationView()));
            ToOrder = new RelayCommand(x => Switcher.Switch(new OrderView()));
            ToMap = new RelayCommand(x => Switcher.Switch(new MapControl()));
            ToOrdersPage = new RelayCommand(x => Switcher.Switch(new OrdersPageView()));
            ToReservationsPage = new RelayCommand(x => Switcher.Switch(new ReservationPageView()));
        }

        public void Navigate(UserControl control)
        {
            this.Control = control;
            if (LoginViewModel.LoginedUser != null && !UserName.Equals(LoginViewModel.LoginedUser.Login))
            {
                UserName = LoginViewModel.LoginedUser.Name;
                IsAdmin = LoginViewModel.LoginedUser.RoleId == App.Context.Roles.FirstOrDefault(x => x.Name == "Admin").Id;
            }
            if (LoginViewModel.LoginedUser == null)
            {
                UserName = "";
                IsAdmin = false;
            }
        }
    }
}
