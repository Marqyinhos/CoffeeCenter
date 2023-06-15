using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using CoffeeCenterDAL.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeCenter.ViewModel
{
    internal class CommentViewModel : BaseNotifyPropertyChanged
    {
        string commentText;
        public string CommentText
        {
            get => commentText;
            set
            {
                commentText = value;
                NotifyPropertyChanged();
            }
        }
        string commentAuthor;
        public string CommentAuthor
        {
            get => commentAuthor;
            set
            {
                commentAuthor = value;
                NotifyPropertyChanged();
            }
        }
        ObservableCollection<ImageModel> starList;
        public ObservableCollection<ImageModel> StarList
        {
            get => starList;
            set
            {
                starList = value;
                NotifyPropertyChanged();
            }
        }
        public CommentViewModel()
        {
            
        }
    }
}
