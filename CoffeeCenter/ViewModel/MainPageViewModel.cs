using CoffeeCenter.Infrastructure;
using CoffeeCenter.View;
using CoffeeCenter.View.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class MainPageViewModel : BaseNotifyPropertyChanged
    {
        UserControl commentControl;
        public UserControl CommentControl
        {
            get => commentControl;
            set
            {
                commentControl = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand ToReservation { get; set; }
        public ICommand ToOrder { get; set; }
        public MainPageViewModel()
        {
            if (App.Context.Comments.Count() == 0)
                CommentControl = new NoCommentsView();
            else
                CommentControl = new ShortCommentsView();
            InitialCommands();
        }
        void InitialCommands()
        {
            ToReservation = new RelayCommand(x => Switcher.Switch(new ReservationView()));
            ToOrder = new RelayCommand(x => Switcher.Switch(new OrderView()));
        }
    }
}
