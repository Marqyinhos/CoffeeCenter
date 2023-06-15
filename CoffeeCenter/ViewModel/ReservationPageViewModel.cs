using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class ReservationPageViewModel : BaseNotifyPropertyChanged
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


        int page = 0;
        int pageCount = 5;
        public ICommand PrevPage { get; set; }
        public ICommand NextPage { get; set; }
        public ReservationPageViewModel()
        {
            SetReservations();
            InitCommands();
        }
        void InitCommands()
        {
            PrevPage = new RelayCommand(x =>
            {
                if (page - 1 >= 0)
                {
                    page--;
                    SetReservations();
                }
            });
            NextPage = new RelayCommand(x =>
            {
                if (page + 1 < App.Context.Orders.Count() / pageCount)
                {
                    page++;
                    SetReservations();
                }
            });
        }
        void SetReservations()
        {
            Reservations = new ObservableCollection<ReservationModel>();
            var reservations = App.Context.Reservations
                                          .OrderByDescending(r => r.Id)
                                          .Skip(pageCount * page)
                                          .Take(pageCount)
                                          .ToList();
            foreach (var reservation in reservations)
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
