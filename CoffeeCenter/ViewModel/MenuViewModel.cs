using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using CoffeeCenter.View;
using CoffeeCenterDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class MenuViewModel : BaseNotifyPropertyChanged
    {
        ObservableCollection<ProductTypeModel> productTypes;
        public ObservableCollection<ProductTypeModel> ProductTypes
        {
            get => productTypes;
            set
            {
                productTypes = value;
                NotifyPropertyChanged();
            }
        }
        

        ProductTypeRepository repository;
        public MenuViewModel()
        {
            repository = new ProductTypeRepository(App.Context);
            ProductTypes = new ObservableCollection<ProductTypeModel>();
            foreach(var type in repository.GetAll())
                ProductTypes.Add(new ProductTypeModel() { Title = type.Name });
        }
    }
}
