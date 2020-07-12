using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppDataManager.Models
{
    public class UserModel
    {
		private int id;
		private string username;
		private string password;
		private string email;
		private string name;
		private string lastname;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}
		public string Username
		{
			get { return username; }
			set { username = value; }
		}
		public string Password
		{
			get { return password; }
			set { password = value; }
		}
		public string Email
		{
			get { return email; }
			set { email = value; }
		}
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		public string Lastname
		{
			get { return lastname; }
			set { lastname = value; }
		}
	}
}
