using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppUI.Models
{
    public class NotebookModel
    {
		private int id;
		private int userId;
		private string name;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}
		public int UserId
		{
			get { return userId; }
			set { userId = value; }
		}
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
	}
}
