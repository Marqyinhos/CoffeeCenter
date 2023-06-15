using CoffeeCenter.Infrastructure;
using CoffeeCenter.View;
using CoffeeCenterDAL.Context;
using CoffeeCenterDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeCenter.ViewModel
{
    internal class RegistrationViewModel : BaseNotifyPropertyChanged
    {
        string login;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                NotifyPropertyChanged();
            }
        }
        string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }
        string address;
        public string Address
        {
            get => address;
            set
            {
                address = value;
                NotifyPropertyChanged();
            }
        }
        string phone;
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                NotifyPropertyChanged();
            }
        }
        string pageModeText;
        public string PageModeText
        {
            get => pageModeText;
            set
            {
                pageModeText = value;
                NotifyPropertyChanged();
            }
        }
        Visibility pageModeVisibility;
        public Visibility PageModeVisibility
        {
            get => pageModeVisibility;
            set
            {
                pageModeVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand Registration { get; set; }
        public ICommand Exit { get; set; }
        UserRepository repository;
        string RegistrationModeText = "Зареєструватися";
        string EditModeText = "Зберегти";
        public RegistrationViewModel()
        {
            repository = new UserRepository(App.Context);
            PageModeText = LoginViewModel.LoginedUser == null ? RegistrationModeText : EditModeText;
            PageModeVisibility = LoginViewModel.LoginedUser == null ? Visibility.Collapsed : Visibility.Visible;
            if(LoginViewModel.LoginedUser != null)
            {
                var user = App.Context.Users
                                      .Where(x => x.Phone == LoginViewModel.LoginedUser.Phone 
                                                  && x.Name == LoginViewModel.LoginedUser.Name)
                                      .FirstOrDefault();
                Phone = user.Phone;
                Name = user.Name;
                Address = user.Address;
                Login = user.Login;
            }
            InitCommands();
        }
        void InitCommands()
        {
            Registration = new RelayCommand(x =>
            {
                var passwordBox = x as PasswordBox;
                var password = passwordBox.Password;
                if (LoginViewModel.LoginedUser == null)
                {                    
                    repository.AddOrUpdate(new User()
                    {
                        Login = login,
                        PasswordHash = password.GetHashCode().ToString(),
                        Name = Name,
                        Address = address,
                        Phone = phone,
                        RoleId = 1
                    });
                    Switcher.Switch(new LoginView());
                }
                else
                {
                    var user = App.Context.Users
                                      .Where(us => us.Phone == LoginViewModel.LoginedUser.Phone
                                                   && us.Name == LoginViewModel.LoginedUser.Name)
                                      .FirstOrDefault();
                    user.Phone = Phone;
                    user.Name = Name;
                    user.Address = Address;
                    user.Login = Login;
                    user.PasswordHash = password.GetHashCode().ToString();
                    App.Context.SaveChanges();
                }
            });
            Exit = new RelayCommand(x => 
            {
                LoginViewModel.LoginedUser = null;
                Switcher.Switch(new MainPage());
            });
        }
    }
}
