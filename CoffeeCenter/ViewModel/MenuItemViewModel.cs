using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using CoffeeCenterDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenter.ViewModel
{
    internal class MenuItemViewModel : BaseNotifyPropertyChanged
    {
        string typeTitle;
        public string TypeTitle
        {
            get => typeTitle;
            set
            {
                typeTitle = "----" + value + "----";
                GetProductsByType(value);
                NotifyPropertyChanged();
            }
        }
        ObservableCollection<ProductModel> products;
        public ObservableCollection<ProductModel> Products
        {
            get => products;
            set
            {
                products = value;
                NotifyPropertyChanged();
            }
        }
        public MenuItemViewModel()
        {
        }
        public void GetProductsByType(string type)
        {
            Products = new ObservableCollection<ProductModel>();
            int typeId = App.Context.ProductTypes.Where(x => x.Name == type).FirstOrDefault().Id;
            var productList = App.Context.Products.Where(x => x.TypeId == typeId).ToList();
            foreach (var product in productList)
                Products.Add(new ProductModel()
                {
                    ProductTitle = "-" + product.Title,
                    ImageSource = product.Photo,
                    Price = "-" + product.Price + "грн"
                });
        }
    }
}
