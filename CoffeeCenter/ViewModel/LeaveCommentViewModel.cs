using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using CoffeeCenter.View;
using CoffeeCenterDAL.Context;
using CoffeeCenterDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class LeaveCommentViewModel : BaseNotifyPropertyChanged
    {
        bool isCounting = false;
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
        string comment;
        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand SaveComment { get; set; }
        int starNumber = 5;
        public int StarNumber
        {
            get => starNumber;
            set
            {
                starNumber = value;
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
        int selectedStarIndex;
        public int SelectedStarIndex
        {
            get => selectedStarIndex;
            set
            {
                if(value != -1)
                    selectedStarIndex = value;
                if (!isCounting)
                    StarCounting(value);
                NotifyPropertyChanged();
            }
        }
        bool isLogedIn = LoginViewModel.LoginedUser != null;
        public bool IsLogedIn
        {
            get => isLogedIn;
            set
            {
                isLogedIn = value;
                NotifyPropertyChanged();
            }
        }

        CommentRepository repository;
        public LeaveCommentViewModel()
        {            
            repository = new CommentRepository(App.Context);
            StarList = new ObservableCollection<ImageModel>();
            for(int i = 0; i < 5; i++)
                StarList.Add(new ImageModel() { ImageSource = "../../Resources/star.png" });
            if(LoginViewModel.LoginedUser == null)
                errorVisibility = Visibility.Visible;
            InitCommands();
        }

        private void InitCommands()
        {
            SaveComment = new RelayCommand(x => 
            {
                repository.
                AddOrUpdate(new Comment()
                {
                    Content = Comment,
                    StarNumber = SelectedStarIndex + 1,
                    UserId = LoginViewModel.LoginedUser.Id

                });
                Switcher.Switch(new MainPage());
            });
        }

        private void StarCounting(int index)
        {
            isCounting = true;
            StarList = new ObservableCollection<ImageModel>();
            for(int i = 0; i < 5; i++)
            {
                if (i <= index)
                    StarList.Add(new ImageModel() { ImageSource = "../../Resources/star.png" });
                else
                    StarList.Add(new ImageModel() { ImageSource = "../../Resources/emptyStar.png" });
            }
            isCounting = false;
        }
    }
}
