using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NotesAppUI.ViewModels
{
    public class LoginViewModel : Screen, ILogin
    {
        private IEventAggregator _eventAggregator;
        private Visibility _loginMode = Visibility.Visible;
        private Visibility _registerMode = Visibility.Collapsed;

        public Visibility LoginMode
        {
            get { return _loginMode; }
            set { _loginMode = value; NotifyOfPropertyChange(nameof(LoginMode)); }
        }
        public Visibility RegisterMode
        {
            get { return _registerMode; }
            set { _registerMode = value; NotifyOfPropertyChange(nameof(RegisterMode)); }
        }


        public LoginViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            eventAggregator.Subscribe(this);
        }

        public void SignIn()
        {
            LoginMode = Visibility.Visible;
            RegisterMode = Visibility.Collapsed;
        }
        public void SignUp()
        {
            RegisterMode = Visibility.Visible;
            LoginMode = Visibility.Collapsed;
        }

        public bool Validate(string userName, string password)
        {
            return true;
        }
    }
}
