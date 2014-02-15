using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VApp.Common;
using VApp.VAWebsite;
using Windows.UI.Xaml.Controls;

namespace VApp.ViewModels
{
    public class LoginPageViewModel : DefaultNotifyPropertyChanged
    {
        private string username;

        private bool loginInProgress;

        private bool errorOccurred;

        private DelegateCommand loginCommand;

        public LoginPageViewModel()
        {
            this.ErrorOccurred = false;
            this.LoginInProgress = false;
        }

        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                if (value != null && !value.Equals(username))
                {
                    this.username = value;

                    this.NotifyPropertyChanged("Username");

                    if (this.loginCommand != null)
                    {
                        this.loginCommand.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        public bool LoginInProgress
        {
            get
            {
                return this.loginInProgress;
            }

            set
            {
                if (!value.Equals(this.loginInProgress))
                {
                    this.loginInProgress = value;
                    this.NotifyPropertyChanged("LoginInProgress");
                }
            }
        }

        public bool ErrorOccurred
        {
            get
            {
                return this.errorOccurred;
            }

            set
            {
                if (!value.Equals(this.errorOccurred))
                {
                    this.errorOccurred = value;
                    this.NotifyPropertyChanged("ErrorOccurred");
                }
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                if (this.loginCommand == null)
                {
                    this.loginCommand = new DelegateCommand(o => this.Login(o), o => this.CanLogin(o));
                }

                return this.loginCommand;
            }
        }

        private async void Login(object passwordObject)
        {
            PasswordBox passwordBox = passwordObject as PasswordBox;

            this.ErrorOccurred = false;
            this.LoginInProgress = true;

            if (await VACommunicator.Authenticate(this.Username, passwordBox.Password))
            {
                App.RootFrame.Navigate(typeof(TestPage), "AllGroups");
            }
            else
            {
                this.LoginInProgress = false;
                this.ErrorOccurred = true;
            }
        }

        private bool CanLogin(object passwordBox)
        {
            return !string.IsNullOrWhiteSpace(this.Username);
        }
    }
}
