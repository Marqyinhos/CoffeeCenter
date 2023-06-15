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
    internal class ProfileReservationsViewModel : BaseNotifyPropertyChanged
    {
        ObservableCollection<ReservationModel> reservations;
        public ObservableCollection<ReservationModel> Reservations
        {
            get => reservations;
            set
            {
                reservations = value;
                NotifyPropertyChanged();
            }
        }
        public ProfileReservationsViewModel()
        {
            Reservations = new ObservableCollection<ReservationModel>();
            int userId = LoginViewModel.LoginedUser.Id;
            var reservations = App.Context.Reservations.Where(x => x.UserId == userId).ToList();
            foreach(var reservation in reservations)
                Reservations.Add(new ReservationModel()
                {
                    IsActive = reservation.IsActive,
                    TableNumber = reservation.TableId.ToString(),
                    Date = reservation.Date.ToString(),
                    ReservationNumber = reservation.Id.ToString()
                });
        }
    }
}
