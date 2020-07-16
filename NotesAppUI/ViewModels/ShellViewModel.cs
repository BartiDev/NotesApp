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
	public class ShellViewModel : Screen, IShell, IHandle<UserModel>, IHandle<string>
    {

		private IWindowManager _windowManager;
		private ILogin _login;
		private IUserPanel _userPanel;
		private IEventAggregator _eventAggregator;
		private bool _isLoggedIn = false;
		private bool _newNotebookClicked = false;
		private bool _newNoteClicked = false;
		private UserModel _user;
		private BindableCollection<NotebookModel> _notebooks;
		private NotebookModel _selectedNotebook;
		private BindableCollection<NoteModel> _notes;





		public BindableCollection<NotebookModel> Notebooks
		{
			get { return _notebooks; }
			set { _notebooks = value; ; NotifyOfPropertyChange(nameof(Notebooks)); }
		}
		public BindableCollection<NoteModel> Notes
		{
			get { return _notes; }
			set { _notes = value; NotifyOfPropertyChange(nameof(Notes)); }
		}
		public UserModel User
		{
			get { return _user; }
			set { _user = value; NotifyOfPropertyChange(nameof(User)); }
		}
		public NotebookModel SelectedNotebook
		{
			get { return _selectedNotebook; }
			set 
			{ 
				_selectedNotebook = value;
				Notes = new BindableCollection<NoteModel>(DBDataAccessLoad.LoadNotebookNotes(SelectedNotebook.Id));
				NotifyOfPropertyChange(nameof(SelectedNotebook)); 
				NotifyOfPropertyChange(nameof(Notes)); }
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
		public bool NewNoteClicked
		{
			get { return _newNoteClicked; }
			set { _newNoteClicked = value; NotifyOfPropertyChange(nameof(NewNoteClicked)); }
		}

		

		public ShellViewModel(IWindowManager windowManager, ILogin login, IUserPanel userPanel, IEventAggregator eventAggregator)
		{
			_windowManager = windowManager;
			_login = login;
			_userPanel = userPanel;
			_eventAggregator = eventAggregator;

			_eventAggregator.Subscribe(this);
		}

		public void LogIn()
		{
			_windowManager.ShowDialog(_login);
		}
		public void UserPanel()
		{
			_windowManager.ShowWindow(_userPanel);
		}
		public void Handle(UserModel user)
		{
			User = user;
			IsLoggedIn = true;

			Notebooks = new BindableCollection<NotebookModel>(DBDataAccessLoad.LoadNotebooks(user.Id));
			Notes = new BindableCollection<NoteModel>();
		}
		public void Handle(string message)
		{
			if(message=="Sign out")
			{
				User = null;
				IsLoggedIn = false;
				Notes.Clear();
				Notebooks.Clear();
			}
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
					DBDataAccessInsert.InsertNotebook(new NotebookModel()
					{
						UserId = User.Id,
						Name = newNotebookName
					});

					Notebooks.Clear();
					Notebooks = new BindableCollection<NotebookModel>(DBDataAccessLoad.LoadNotebooks(User.Id));

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
		public void CancelNotebook()
		{
			NewNotebookClicked = false;
		}
		public void NewNote()
		{
			NewNoteClicked = true;
		}
		public void ConfirmNewNoteTitle(string newNoteTitle)
		{
			NewNoteClicked = false;

			NoteModel note = Notes.ToList<NoteModel>().Find(n => n.Title == newNoteTitle);
			if(note == null)
			{
				try
				{
					DBDataAccessInsert.InsertNote(new NoteModel()
					{
						NotebookId = SelectedNotebook.Id,
						Title = newNoteTitle,
						CreatedTime = DateTime.Now,
						UpdatedTime = DateTime.Now,
						FileLocation = "c:/dummyAddress"
					});

					Notes.Clear();
					Notes = new BindableCollection<NoteModel>(DBDataAccessLoad.LoadNotebookNotes(SelectedNotebook.Id));

				}
				catch (SQLiteException)
				{
					MessageBox.Show("Invalid Title");
				}
			}
			else
			{
				MessageBox.Show("Note with this title already exists");
			}
		}
		public void CancelNote()
		{
			NewNoteClicked = false;
		}

	}
}
