using CoffeeCenter.Infrastructure;
using CoffeeCenter.View;
using CoffeeCenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeCenter.Models
{
    internal class ProductTypeModel
    {
        public string Title { get; set; }
        public ICommand ToProducts { get; set; } = new RelayCommand(x =>
        {
            var menuItemView = new MenuItemView();
            MenuItemViewModel vm = menuItemView.DataContext as MenuItemViewModel;
            vm.TypeTitle = x.ToString();
            Switcher.Switch(menuItemView);
        });
    }
}
