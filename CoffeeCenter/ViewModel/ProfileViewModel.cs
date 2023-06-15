using CoffeeCenter.Infrastructure;
using CoffeeCenter.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class ProfileViewModel : BaseNotifyPropertyChanged
    {
        UserControl profileControl;
        public UserControl ProfileControl
        {
            get => profileControl;
            set
            {
                profileControl = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand ToProfile { get; set; }
        public ICommand ToProfileReservations { get; set; }
        public ICommand ToProfileOrders { get; set; }
        public ProfileViewModel()
        {
            ProfileControl = new RegistrationView();
            InitCommands();
        }
        void InitCommands()
        {
            ToProfile = new RelayCommand(x => ProfileControl = new RegistrationView());
            ToProfileReservations = new RelayCommand(x => ProfileControl = new ProfileReservationsView());
            ToProfileOrders = new RelayCommand(x => ProfileControl = new ProfileOrderView());
        }
    }
}
