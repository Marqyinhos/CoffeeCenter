using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using CoffeeCenter.View.Comments;
using CoffeeCenterDAL.Context;
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
    internal class ShortCommentsViewModel : BaseNotifyPropertyChanged
    {
        ObservableCollection<CommentModel> comments;
        public ObservableCollection<CommentModel> Comments
        {
            get => comments;
            set
            {
                comments = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand ToLeaveComment { get; set; }
        public ICommand ToMoreComments { get; set; }
        public ShortCommentsViewModel()
        {
            Comments = new ObservableCollection<CommentModel>();
            var list = App
                        .Context
                        .Comments
                        .OrderByDescending(x => x.Id)
                        .Take(10)
                        .ToList();
            for(int i = 0; i < list.Count; i++)
            {
                CommentView commentView = new CommentView();
                var vm = commentView.DataContext as CommentViewModel;
                vm.CommentText = list[i].Content;
                var myId = list[i].UserId;
                vm.CommentAuthor = App
                                    .Context
                                    .Users
                                    .Where(x => x.Id == myId)
                                    .FirstOrDefault()
                                    .Name;
                vm.StarList = new ObservableCollection<ImageModel>();
                for (int j = 0; j < 5; j++)
                {
                    if (j < list[i].StarNumber)
                        vm.StarList.Add(new ImageModel() { ImageSource = "../../../Resources/star.png" });
                    else
                        vm.StarList.Add(new ImageModel() { ImageSource = "../../../Resources/emptyStar.png" });
                }
                Comments.Add(new CommentModel() { CommentView = commentView });
            }
            InitCommands();
        }

        private void InitCommands()
        {
            ToLeaveComment = new RelayCommand(x => Switcher.Switch(new LeaveComment()));
            ToMoreComments = new RelayCommand(x => Switcher.Switch(new MoreCommentsView()));
        }
    }
}
