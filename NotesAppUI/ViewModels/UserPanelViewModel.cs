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
    public class UserPanelViewModel : Screen, IUserPanel, IHandle<UserModel>
    {
		private IEventAggregator _eventAggregator;
		private UserModel _user;
		private int _notebooksCount;
		private int _notesCount;
		private bool _editProfileMode = false;
		private bool _newPasswordMode = false;
		private bool _deleteProfileMode = false;
		private string _newUsername;
		private string _newName;
		private string _newLastName;
		private string _newEmail;
		private string _oldPassword;
		private string _newPassword;
		private string _confirmedNewPassword;



		public string NewUsername
		{
			get { return _newUsername; }
			set { _newUsername = value; NotifyOfPropertyChange(nameof(NewUsername)); }
		}
		public string NewName
		{
			get { return _newName; }
			set { _newName = value; NotifyOfPropertyChange(nameof(NewName)); }
		}
		public string NewLastName
		{
			get { return _newLastName; }
			set { _newLastName = value; NotifyOfPropertyChange(nameof(NewLastName)); }
		}
		public string NewEmail
		{
			get { return _newEmail; }
			set { _newEmail = value; NotifyOfPropertyChange(nameof(NewEmail)); }
		}
		public string OldPassword
		{
			get { return _oldPassword; }
			set { _oldPassword = value; NotifyOfPropertyChange(nameof(OldPassword)); }
		}
		public string NewPassword
		{
			get { return _newPassword; }
			set { _newPassword = value; NotifyOfPropertyChange(nameof(NewPassword)); }
		}
		public string ConfirmedNewPassword
		{
			get { return _confirmedNewPassword; }
			set { _confirmedNewPassword = value; NotifyOfPropertyChange(nameof(ConfirmedNewPassword)); }
		}
		public bool EditProfileMode
		{
			get { return _editProfileMode; }
			set { _editProfileMode = value; NotifyOfPropertyChange(nameof(EditProfileMode)); }
		}
		public bool NewPasswordMode
		{
			get { return _newPasswordMode; }
			set { _newPasswordMode = value; NotifyOfPropertyChange(nameof(NewPasswordMode)); }
		}
		public bool DeleteProfileMode
		{
			get { return _deleteProfileMode; }
			set { _deleteProfileMode = value; NotifyOfPropertyChange(nameof(DeleteProfileMode)); }
		}
		public int NotebooksCount
		{
			get { return _notebooksCount; }
			set { _notebooksCount = value; NotifyOfPropertyChange(nameof(NotebooksCount)); }
		}
		public int NotesCount
		{
			get { return _notesCount; }
			set { _notesCount = value; NotifyOfPropertyChange(nameof(NotesCount)); }
		}


		public UserModel User
		{
			get { return _user; }
			set { _user = value; NotifyOfPropertyChange(nameof(User)); }
		}



		public UserPanelViewModel(IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
			_eventAggregator.Subscribe(this);
		}


		public void SignOut()
		{
			User = null;
			_eventAggregator.PublishOnUIThread("Sign out");
			TryClose();
		}
		public void EditProfile()
		{
			EditProfileMode = true;
		}
		public void CancelEdit()
		{
			CancelNewPassword();
			EditProfileMode = false;
			NewUsername = null;
			NewName = null;
			NewLastName = null;
			NewEmail = null;
		}
		public void UpdatePassword()
		{
			NewPasswordMode = true;
		}
		public void CancelNewPassword()
		{
			NewPasswordMode = false;
			OldPassword = null;
			NewPassword = null;
			ConfirmedNewPassword = null;
		}
		public void ConfirmNewPassword()
		{
			if (OldPassword == User.Password)
			{
				if ((NewPassword == ConfirmedNewPassword) && !string.IsNullOrEmpty(NewPassword))
				{
					User.Password = NewPassword;
					CancelNewPassword();
				}
				else
					MessageBox.Show("Password couldn't be confirmed");
			}
			else
				MessageBox.Show("Incorrect Old Password");
		}
		public void ConfirmEdit()
		{
			if (!string.IsNullOrEmpty(NewUsername))
				User.Username = NewUsername;
			if (!string.IsNullOrEmpty(NewName))
				User.Name = NewName;
			if (!string.IsNullOrEmpty(NewLastName))
				User.Lastname = NewLastName;
			if (!string.IsNullOrEmpty(NewEmail))
				User.Email = NewEmail;

			try
			{
				DBDataAccessUpdate.UpdateUser(User);
				_eventAggregator.PublishOnUIThread(User);
				MessageBox.Show("Updating Successful");
				EditProfileMode = false;
			}
			catch (SQLiteException)
			{
				MessageBox.Show("Could not updated");
			}
		}
		public void DeleteProfile()
		{
			DeleteProfileMode = true;
			EditProfileMode = false;
		}
		public void ConfirmDeleteProfile()
		{
			int userId = User.Id;
			SignOut();
			DBDataAccessDelete.DeleteUser(userId);
		}
		public void CancelDeleteProfile()
		{
			DeleteProfileMode = false;
		}


		public void Handle(UserModel user)
		{
			User = user;
		}
		protected override void OnActivate()
		{
			NotebooksCount = DBDataAccessLoad.LoadNotebooks(User.Id).Count;
			NotesCount = DBDataAccessLoad.LoadUserNotes(User.Id).Count;
		}
	}
}
