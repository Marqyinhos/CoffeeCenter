using CoffeeCenter.Infrastructure;
using CoffeeCenter.Models;
using CoffeeCenter.View.Comments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class MoreCommentsViewModel : BaseNotifyPropertyChanged
    {
        int pageCount = 10;
        int page = 0;
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
        public ICommand PrevPage { get; set; }
        public ICommand NextPage { get; set; }
        public MoreCommentsViewModel()
        {
            GetComments();
            InitCommands();
        }

        private void InitCommands()
        {
            PrevPage = new RelayCommand(x => 
            {
                if (page - 1 >= 0)
                {
                    page--;
                    GetComments();
                }
            });
            NextPage = new RelayCommand(x =>
            {
                if (page + 1 < (App.Context.Comments.Count() / pageCount) + 1)
                {
                    page++;
                    GetComments();
                }
            });
        }

        private void GetComments()
        {
            Comments = new ObservableCollection<CommentModel>();
            var list = App
                        .Context
                        .Comments
                        .OrderByDescending(x => x.Id)
                        .Skip(page * pageCount)
                        .Take(pageCount)
                        .ToList();
            for (int i = 0; i < list.Count; i++)
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
        }
    }
}
