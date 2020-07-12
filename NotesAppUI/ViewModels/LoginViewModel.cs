using Caliburn.Micro;
using NotesAppDataManager;
using NotesAppDataManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        private string _logUsername;
        private string _logPassword;
        private string _regName;
        private string _regLastName;
        private string _regEmail;
        private string _regUsername;
        private string _regPassword;


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

        public string LogUsername
        {
            get { return _logUsername; }
            set { _logUsername = value; NotifyOfPropertyChange(nameof(LogUsername)); }
        }
        public string LogPassword
        {
            get { return _logPassword; }
            set { _logPassword = value; NotifyOfPropertyChange(nameof(LogPassword)); }
        }
        public string RegName
        {
            get { return _regName; }
            set { _regName = value; NotifyOfPropertyChange(nameof(RegName)); }
        }
        public string RegLastName
        {
            get { return _regLastName; }
            set { _regLastName = value; NotifyOfPropertyChange(nameof(RegLastName)); }
        }
        public string RegEmail
        {
            get { return _regEmail; }
            set { _regEmail = value; NotifyOfPropertyChange(nameof(RegEmail)); }
        }
        public string RegUsername
        {
            get { return _regUsername; }
            set { _regUsername = value; NotifyOfPropertyChange(nameof(RegUsername)); }
        }
        public string RegPassword
        {
            get { return _regPassword; }
            set { _regPassword = value; NotifyOfPropertyChange(nameof(RegPassword)); }
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
        public void Login()
        {
            List<UserModel> users = DBDataAccess.LoadUsers();
            UserModel user = users.Find(u => (u.Username == LogUsername && u.Password == LogPassword));
            if (user == null)
            {
                MessageBox.Show("Invalid Username or Password");
            }
            else
            {
                MessageBox.Show("Login successful");
                _eventAggregator.PublishOnUIThread(user);
                TryClose();
            }
        }
        public void Register()
        {
            try
            {
                DBDataAccess.InsertUser(new UserModel()
                {
                    Username = RegUsername,
                    Password = RegPassword,
                    Email = RegEmail,
                    Name = RegName,
                    Lastname = RegLastName
                });
                MessageBox.Show("Registration Successful");
                SignIn();
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Email or Username is already registered");
            }
        }
    }
}
