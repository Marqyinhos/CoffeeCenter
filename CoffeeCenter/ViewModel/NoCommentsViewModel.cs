using CoffeeCenter.Infrastructure;
using CoffeeCenter.View.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class NoCommentsViewModel : BaseNotifyPropertyChanged
    {
        public ICommand ToLeaveComment { get; set; }
        public NoCommentsViewModel()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            ToLeaveComment = new RelayCommand(x => Switcher.Switch(new LeaveComment()));
        }
    }
}
