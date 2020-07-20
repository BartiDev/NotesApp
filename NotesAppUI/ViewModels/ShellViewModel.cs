using Caliburn.Micro;
using NotesAppDataManager;
using NotesAppDataManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NotesAppUI.ViewModels
{
	public partial class ShellViewModel : Screen, IShell, IHandle<UserModel>, IHandle<string>
    {

		private IWindowManager _windowManager;
		private ILogin _login;
		private IUserPanel _userPanel;
		private IEventAggregator _eventAggregator;
		private bool _isLoggedIn = false;
		private bool _newNotebookClicked = false;
		private bool _newNoteClicked = false;
		private UserModel _user;
		private NotebookModel _selectedNotebook;
		private BindableCollection<NotebookModel> _notebooks;
		private BindableCollection<NoteModel> _notes;
		private NoteModel _selectedNote;
		private bool _renameNoteClicked;
		private bool _renameNotebookClicked;





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
				if(value != null)
					Notes = new BindableCollection<NoteModel>(DBDataAccessLoad.LoadNotebookNotes(SelectedNotebook.Id));
				NotifyOfPropertyChange(nameof(SelectedNotebook)); 
				NotifyOfPropertyChange(nameof(Notes)); 
			}
		}
		public NoteModel SelectedNote
		{
			get { return _selectedNote; }
			set { _selectedNote = value; NotifyOfPropertyChange(nameof(SelectedNote)); }
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
		public bool RenameNoteClicked
		{
			get { return _renameNoteClicked; }
			set { _renameNoteClicked = value; NotifyOfPropertyChange(nameof(RenameNoteClicked)); }
		}
		public bool RenameNotebookClicked
		{
			get { return _renameNotebookClicked; }
			set { _renameNotebookClicked = value; NotifyOfPropertyChange(nameof(RenameNotebookClicked)); }
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
				SelectedNotebook = null;
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

			string cd = Environment.CurrentDirectory;
			string filePath = cd + $@"\data\{User.Id}" + $@"\{SelectedNotebook.Id}" + $@"\{newNoteTitle}.rtf";

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
						FileLocation = filePath
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
		public void SaveNote(object o)
		{
			RichTextBox rt = (RichTextBox)o;
			TextRange t = new TextRange(rt.Document.ContentStart, rt.Document.ContentEnd);

			Directory.CreateDirectory(Path.GetDirectoryName(SelectedNote.FileLocation));
			using (FileStream fs = new FileStream(SelectedNote.FileLocation, FileMode.Create))
			{
				t.Save(fs, System.Windows.DataFormats.Rtf);
			}

		}
		public void OpenNote(object o)
		{

			if (File.Exists(SelectedNote.FileLocation))
			{
				TextRange t = new TextRange(((RichTextBox)o).Document.ContentStart, ((RichTextBox)o).Document.ContentEnd);

				using(FileStream fs = new FileStream(SelectedNote.FileLocation, FileMode.Open))
				{
					t.Load(fs, System.Windows.DataFormats.Rtf);
				}
			}
			else
				((RichTextBox)o).Document.Blocks.Clear();
		}
		public void DeleteNotebook()
		{
			int notebookId = SelectedNotebook.Id;
			SelectedNotebook = null;
			SelectedNote = null;
			DBDataAccessDelete.DeleteNotebook(notebookId);
			Notes = null;
			Notebooks = new BindableCollection<NotebookModel>(DBDataAccessLoad.LoadNotebooks(User.Id));
		}
		public void DeleteNote()
		{
			int noteId = SelectedNote.Id;
			SelectedNote = null;
			DBDataAccessDelete.DeleteNote(noteId);
			if (SelectedNotebook != null)
				Notes = new BindableCollection<NoteModel>(DBDataAccessLoad.LoadNotebookNotes(SelectedNotebook.Id));
		}
		public void EditNoteTitle()
		{
			RenameNoteClicked = true;
		}
		public void EditNotebookName()
		{
			RenameNotebookClicked = true;
		}
		public void ConfirmNotebookNameRename(string newName)
		{
			RenameNotebookClicked = false;
			SelectedNotebook.Name = newName;

			try
			{
				DBDataAccessUpdate.UpdateNotebook(SelectedNotebook);
				Notebooks = new BindableCollection<NotebookModel>(DBDataAccessLoad.LoadNotebooks(User.Id));
				SelectedNotebook = null;
			}
			catch (SQLiteException)
			{
				MessageBox.Show("Notebook name already in use");
			}
		}
		public void CancelNotebookRename()
		{
			RenameNotebookClicked = false;
		}
		public void ConfirmNoteTitleRename(string newTitle)
		{
			RenameNoteClicked = false;
			SelectedNote.Title = newTitle;

			try
			{
				DBDataAccessUpdate.UpdateNote(SelectedNote);
				Notes = new BindableCollection<NoteModel>(DBDataAccessLoad.LoadNotebookNotes(SelectedNotebook.Id));
			}
			catch (SQLiteException)
			{
				MessageBox.Show("Note title already in use");
			}
		}
		public void CancelNoteRename()
		{
			RenameNoteClicked = false;
		}
	}
}
