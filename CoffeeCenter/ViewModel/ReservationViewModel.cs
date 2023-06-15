using CoffeeCenter.Infrastructure;
using CoffeeCenterDAL.Context;
using CoffeeCenterDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CoffeeCenter.ViewModel
{
    internal class ReservationViewModel : BaseNotifyPropertyChanged
    {
        DateTime startDate = DateTime.Now;
        DateTime currentDate = DateTime.Now.AddMinutes(1);
        public DateTime CurrentDate
        {
            get => currentDate;
            set
            {
                if (value > startDate)
                {
                    var date = value;
                    if (date.Minute <= 30)
                        date = date.AddMinutes(30 - date.Minute);
                    else
                        date = date.AddMinutes(60 - date.Minute);
                    currentDate = date;
                }
                NotifyPropertyChanged();
            }
        }
        public DateTime TodayDate { get; set; } = DateTime.Today;
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
        string errorText;
        public string ErrorText
        {
            get => errorText;
            set
            {
                errorText = value;
                NotifyPropertyChanged();
            }
        }
        Visibility errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get => errorVisibility;
            set
            {
                errorVisibility = value;
                NotifyPropertyChanged();
            }
        }
        Brush messageForeground = Brushes.Red;
        public Brush MessageForeground
        {
            get => messageForeground;
            set
            {
                messageForeground = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand Reservation { get; set; }
        ReservationRepository repository;
        string tableErrorText = "На жаль, дане місце й час зайняті :(";
        string userErrorText = "Потрібно ввести ім'я і телефон";
        string successErrorText = "Резервування пройшло успішно!";
        public ReservationViewModel()
        {
            repository = new ReservationRepository(App.Context);
            if(LoginViewModel.LoginedUser != null)
            {
                UserName = LoginViewModel.LoginedUser.Name;
                PhoneNumber = LoginViewModel.LoginedUser.Phone;
            }
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
        }
        void InitCommands()
        {
            Reservation = new RelayCommand(x =>
            {
                User currentUser = IsUserCreated();
                int tableId = TableCheck();
                if (currentUser != null && tableId != -1)
                {
                    repository.AddOrUpdate(new Reservation()
                    {
                        UserId = currentUser.Id,
                        Date = CurrentDate,
                        TableId = tableId,
                        IsActive = true
                    });
                    ErrorVisibility = Visibility.Visible;
                    ErrorText = successErrorText;
                    MessageForeground = Brushes.Green;
                }
                else if(tableId == -1)
                {
                    ErrorVisibility = Visibility.Visible;
                    ErrorText = tableErrorText;
                    MessageForeground = Brushes.Red;
                }
                else if(currentUser == null)
                {
                    if(UserName == null || PhoneNumber == null)
                    {
                        ErrorVisibility = Visibility.Visible;
                        ErrorText = userErrorText;
                        MessageForeground = Brushes.Red;
                        return;
                    }
                    App.Context.Users.Add(new User() { Name = UserName, Phone = PhoneNumber, RoleId = 2 });
                    App.Context.SaveChanges();
                    currentUser = App.Context.Users
                                        .Where(us => us.Name == UserName && us.Phone == PhoneNumber)
                                        .FirstOrDefault();
                    repository.AddOrUpdate(new Reservation()
                    {
                        UserId = currentUser.Id,
                        Date = CurrentDate,
                        TableId = tableId,
                        IsActive = true
                    });
                    ErrorVisibility = Visibility.Visible;
                    ErrorText = successErrorText;
                    MessageForeground = Brushes.Green;
                }
            });
        }
        User IsUserCreated() =>
            App.Context.Users
                .Include("Reservations")
                .Where(us => us.Phone == PhoneNumber && us.Name == UserName)
                .FirstOrDefault();
        int TableCheck()
        {
            DateTime firstDate = CurrentDate.AddMinutes(-1);
            DateTime secondDate = CurrentDate.AddMinutes(1);
            var reservations = App.Context.Reservations
                .Where(r => firstDate < r.Date && r.Date < secondDate)
                .ToList();
            if (reservations.Count() > 0)
            {
                bool isReserved = false;
                var tables = App.Context.Tables.Where(t => t.SeatsNumber == SelectedSeat).ToList();
                foreach (var table in tables)
                {
                    isReserved = false;
                    foreach (var reservation in reservations)
                        if (reservation.IsActive && reservation.TableId == table.Id)
                            isReserved = true;
                    if (!isReserved)
                        return table.Id;
                }
                return -1;
            }
            return App.Context.Tables.Where(t => t.SeatsNumber == SelectedSeat).FirstOrDefault().Id;
        }
    }
}
