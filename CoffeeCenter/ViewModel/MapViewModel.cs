using CoffeeCenter.Infrastructure;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class MapViewModel : BaseNotifyPropertyChanged
    {
        string coordinates;
        public string Coordinates
        {
            get => "Вибрані координати: " + coordinates;
            set
            {
                coordinates = value;
                NotifyPropertyChanged();
            }
        }
        Visibility copyTextVisibility = Visibility.Collapsed;
        public Visibility CopyTextVisibility
        {
            get => copyTextVisibility;
            set
            {
                copyTextVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand SetCoordinates { get; set; }
        public MapViewModel()
        {
            SetCoordinates = new RelayCommand(x => 
            {
                Coordinates = x.ToString();
                Location location = x as Location;
                Clipboard.SetText(location.Latitude.ToString() + " " + location.Longitude);
                CopyTextVisibility = Visibility.Visible;
            });
        }
    }
}
