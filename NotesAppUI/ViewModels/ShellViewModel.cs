using Caliburn.Micro;
using NotesAppDataManager;
using NotesAppDataManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NotesAppUI.ViewModels
{
	public class ShellViewModel : Screen, IShell, IHandle<UserModel>
    {

		private IWindowManager _windowManager;
		private ILogin _login;
		private IEventAggregator _eventAggregator;
		private bool _isLoggedIn = false;
		private bool _newNotebookClicked = false;
		private UserModel _user;
		private BindableCollection<NotebookModel> _notebooks;


		public BindableCollection<NotebookModel> Notebooks
		{
			get { return _notebooks; }
			set { _notebooks = value; ; NotifyOfPropertyChange(nameof(Notebooks)); }
		}
		public UserModel User
		{
			get { return _user; }
			set { _user = value; NotifyOfPropertyChange(nameof(User)); }
		}
		public bool IsLoggedIn
		{
			get { return _isLoggedIn; }
			set { _isLoggedIn = value; NotifyOfPropertyChange(nameof(IsLoggedIn)); }
		}
		public bool NewNotebookClicked
		{
			get { return _newNotebookClicked; }
			set { _newNotebookClicked = value; NotifyOfPropertyChange(nameof(NewNotebookClicked)); }
		}

		
		public ShellViewModel(IWindowManager windowManager, ILogin login, IEventAggregator eventAggregator)
		{
			_windowManager = windowManager;
			_login = login;
			_eventAggregator = eventAggregator;

			_eventAggregator.Subscribe(this);
		}

		public void LogIn()
		{
			_windowManager.ShowDialog(_login);
		}
		public void Handle(UserModel user)
		{
			User = user;
			IsLoggedIn = true;

			Notebooks = new BindableCollection<NotebookModel>(DBDataAccess.LoadNotebooks(user.Id));
		}
		public void NewNotebook()
		{
			if (IsLoggedIn == false)
			{
				MessageBox.Show("Please Sign in first");
			}
			else
			{
				NewNotebookClicked = true;
			}
		}
		public void ConfirmNewNotebookName(string newNotebookName)
		{
			NewNotebookClicked = false;

			NotebookModel notebook = Notebooks.ToList<NotebookModel>().Find(n => n.Name == newNotebookName);
			if (notebook == null)
			{
				try
				{
					DBDataAccess.InsertNotebook(new NotebookModel()
					{
						UserId = User.Id,
						Name = newNotebookName
					});

					Notebooks.Clear();
					Notebooks = new BindableCollection<NotebookModel>(DBDataAccess.LoadNotebooks(User.Id));

				}
				catch (SQLiteException)
				{
					MessageBox.Show("Invalid Name");
				}
			}
			else
			{
				MessageBox.Show("Notebook with this name already exists");
			}
		}
	}
}
