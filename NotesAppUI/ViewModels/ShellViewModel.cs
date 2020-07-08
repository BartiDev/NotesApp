﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppUI.ViewModels
{
	public class ShellViewModel : PropertyChangedBase, IShell
    {

		private IWindowManager _windowManager;
		private ILogin _login;
		private IEventAggregator _eventAggregator;
		private bool _isLoggedIn = false;

		public bool IsLoggedIn
		{
			get { return _isLoggedIn; }
			set { _isLoggedIn = value; NotifyOfPropertyChange(nameof(IsLoggedIn)); }
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
	}
}