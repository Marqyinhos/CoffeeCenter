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
using System.Windows.Interop;

namespace CoffeeCenter.ViewModel
{
    internal class LoginViewModel : BaseNotifyPropertyChanged
    {
        public static User LoginedUser { get; set; }
        UserRepository repository;
        const string phoneModeText = "Зайти з телефоном";
        const string loginModeText = "Зайти з логіном";
        const string phoneEnterText1 = "Відправити код";
        const string phoneEnterText2 = "Підтвердити";
        const string loginEnterText = "Увійти";
        const string phoneCodeText1 = "Номер телефону";
        const string phoneCodeText2 = "Введіть код";
        string loginMode = phoneModeText;
        public string LoginMode
        {
            get => loginMode;
            set
            {
                loginMode = value;
                NotifyPropertyChanged();
            }
        }
        string enterMode = loginEnterText;
        public string EnterMode
        {
            get => enterMode;
            set
            {
                enterMode = value;
                NotifyPropertyChanged();
            }
        }
        string phoneCode = phoneCodeText1;
        public string PhoneCode
        {
            get => phoneCode;
            set
            {
                phoneCode = value;
                NotifyPropertyChanged();
            }
        }
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
        string userPhoneNumber;
        string phoneNumber;
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {                
                phoneNumber = value;
                NotifyPropertyChanged();
            }
        }

        string errorText;
        public string ErrorText
        {
            get => errorText;
            set
            {
                errorText = value;
                NotifyPropertyChanged();
            }
        }
        Visibility isPhoneLogin = Visibility.Collapsed;
        public Visibility IsPhoneLogin
        {
            get => isPhoneLogin;
            set
            {
                isPhoneLogin = value;
                NotifyPropertyChanged();
            }
        }
        Visibility isLoginLogin = Visibility.Visible;
        public Visibility IsLoginLogin
        {
            get => isLoginLogin;
            set
            {
                isLoginLogin = value;
                NotifyPropertyChanged();
            }
        }
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
        public ICommand ChangeInput { get; set; }
        public ICommand Enter { get; set; }
        public ICommand ChangeCode { get; set; }
        public ICommand ToRegistration { get; set; }

        string errorLoginText = "Невірний логін або пароль";
        string errorCodeText = "Необхідно ввести код";
        string errorPhoneText = "Невірний номер телефону";

        public LoginViewModel()
        {
            repository = new UserRepository(App.Context);
            InitCommands();
        }
        void InitCommands()
        {
            ChangeInput = new RelayCommand(x => {
                if (LoginMode.Equals(phoneModeText))
                {
                    LoginMode = loginModeText;
                    EnterMode = phoneEnterText1;
                    IsPhoneLogin = Visibility.Visible;
                    IsLoginLogin = Visibility.Collapsed;
                }
                else
                {
                    LoginMode = phoneModeText;
                    EnterMode = loginEnterText;
                    IsPhoneLogin = Visibility.Collapsed;
                    IsLoginLogin = Visibility.Visible;
                }
            });
            Enter = new RelayCommand(x => {
                if (LoginMode.Equals(loginModeText))
                {
                    if (PhoneCode != phoneCodeText2)
                    {
                        if (!App.Context.Users.Any(us => us.Phone == PhoneNumber))
                        {
                            ErrorText = errorPhoneText;
                            ErrorVisibility = Visibility.Visible;
                        }
                        else
                        {
                            userPhoneNumber = PhoneNumber;
                            PhoneNumber = "";
                            EnterMode = phoneEnterText2;
                            PhoneCode = phoneCodeText2;
                        }
                    }
                    else
                    {
                        if (PhoneNumber.Length == 0)
                        {
                            ErrorText = errorCodeText;
                            ErrorVisibility = Visibility.Visible;
                        }
                        else
                        {
                            LoginedUser = App.Context.Users
                                                .Where(us => us.Phone == userPhoneNumber)
                                                .FirstOrDefault();
                            Switcher.Switch(new MainPage());
                        }
                    }
                }
                else
                {
                    var passwordBox = x as PasswordBox;
                    var password = passwordBox.Password;
                    var user = repository.Login(Login, password.GetHashCode().ToString());
                    if (user != null)
                    {
                        LoginedUser = user;
                        if (user.RoleId == App.Context.Roles.FirstOrDefault(r => r.Name == "Admin").Id)
                            Switcher.Switch(new OrdersPageView());
                        else
                            Switcher.Switch(new MainPage());
                    }
                    else
                    {
                        ErrorText = errorLoginText;
                        ErrorVisibility = Visibility.Visible;
                    }
                }
            });
            ToRegistration = new RelayCommand(x => Switcher.Switch(new RegistrationView()));
        }
    }
}
