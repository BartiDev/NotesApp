using NotesAppDataManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppDataManager
{
    public class DBDataAccessLoad
    {
        public static List<UserModel> LoadUsers()
        {
            List<UserModel> users = new List<UserModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = "select * from users";
            SQLiteCommand command = new SQLiteCommand(cmd, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new UserModel()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Username = (string)reader["username"],
                    Password = (string)reader["password"],
                    Email = (string)reader["email"],
                    Name = (string)reader["name"],
                    Lastname = (string)reader["lastname"]
                });
            }

            connection.Close();
            return users;
        }



        public static List<NotebookModel> LoadNotebooks(int userId)
        {
            List<NotebookModel> notebooks = new List<NotebookModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = $"select * from notebooks where userId = {userId}";
            SQLiteCommand command = new SQLiteCommand(cmd, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                notebooks.Add(new NotebookModel()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = (string)reader["name"],
                    UserId = Convert.ToInt32(reader["userId"])
                });
            }

            connection.Close();
            return notebooks;
        }


        public static List<NoteModel> LoadUserNotes(int userId)
        {
            List<NoteModel> notes = new List<NoteModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = $"select notes.id, notebookId, title, createdTime, updatedTime, fileLocation from notes " +
                $"left join notebooks on notes.notebookId = notebooks.id " +
                $"where notebooks.userId = {userId}";
            SQLiteCommand command = new SQLiteCommand(cmd, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                notes.Add(new NoteModel()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    NotebookId = Convert.ToInt32(reader["notebookId"]),
                    Title = (string)reader["title"],
                    CreatedTime = Convert.ToDateTime((string)reader["createdTime"]),
                    UpdatedTime = Convert.ToDateTime((string)reader["updatedTime"]),
                    FileLocation = (string)reader["fileLocation"]

                });
            }

            connection.Close();
            return notes;
        }


        public static List<NoteModel> LoadNotebookNotes(int notebookId)
        {
            List<NoteModel> notes = new List<NoteModel>();
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = $"select * from notes where notebookId = {notebookId}";
            SQLiteCommand command = new SQLiteCommand(cmd, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                notes.Add(new NoteModel()
                {
                    Id = Convert.ToInt32(reader["id"]),
                    NotebookId = Convert.ToInt32(reader["notebookId"]),
                    Title = (string)reader["title"],
                    CreatedTime = Convert.ToDateTime((string)reader["createdTime"]),
                    UpdatedTime = Convert.ToDateTime((string)reader["updatedTime"]),
                    FileLocation = (string)reader["fileLocation"]

                });
            }

            connection.Close();
            return notes;
        }


    }
}
