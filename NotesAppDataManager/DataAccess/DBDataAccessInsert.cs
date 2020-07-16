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
    public class DBDataAccessInsert
    {
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
