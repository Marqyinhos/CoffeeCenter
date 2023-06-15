using CoffeeCenter.Infrastructure;
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
    internal class ReservationModel : BaseNotifyPropertyChanged
    {
        int reservationNumber;
        public string ReservationNumber
        {
            get => "Резервування №" + reservationNumber;
            set
            {
                reservationNumber = Convert.ToInt32(value);
                NotifyPropertyChanged();
            }
        }
        int tableNumber;
        public string TableNumber
        {
            get => "Столик № " + tableNumber;
            set
            {
                tableNumber = Convert.ToInt32(value); ;
                NotifyPropertyChanged();
            }
        }
        DateTime date;
        public string Date
        {
            get => "Дата та час: " + date;
            set
            {
                date = Convert.ToDateTime(value);
                NotifyPropertyChanged();
            }
        }
        bool isActive;
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                CancelVisibility = isActive ? Visibility.Visible : Visibility.Collapsed;
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
        public ICommand CancelReservation { get; set; }
        public ReservationModel()
        {
            CancelReservation = new RelayCommand(x =>
            {
                var reservation = App.Context.Reservations.Where(r => r.Id == reservationNumber).FirstOrDefault();
                reservation.IsActive = false;
                App.Context.SaveChanges();
                IsActive = false;
            });
        }
    }
}
