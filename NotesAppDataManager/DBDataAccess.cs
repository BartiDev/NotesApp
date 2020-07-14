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
    public class DBDataAccess
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

        public static void InsertUser(UserModel user)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = "insert into users (username, password, email, name, lastname)" +
                $"values ('{user.Username}', '{user.Password}', '{user.Email}', '{user.Name}', '{user.Lastname}')";
            SQLiteCommand command = new SQLiteCommand(cmd, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (SQLiteException)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

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

        public static void InsertNotebook(NotebookModel notebook)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = "insert into notebooks (userId, name)" +
                $"values ({notebook.UserId}, '{notebook.Name}')";
            SQLiteCommand command = new SQLiteCommand(cmd, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (SQLiteException)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

        }

        public static List<NoteModel> LoadNotes(int notebookId)
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

        public static void InsertNote(NoteModel note)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = "insert into notes (notebookId, title, createdTime, updatedTime, fileLocation)" +
                $"values ({note.NotebookId}, '{note.Title}', '{note.CreatedTime.ToString()}', '{note.UpdatedTime.ToString()}', '{note.FileLocation}')";
            SQLiteCommand command = new SQLiteCommand(cmd, connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (SQLiteException)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
