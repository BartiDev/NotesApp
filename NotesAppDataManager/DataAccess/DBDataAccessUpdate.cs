using NotesAppDataManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppDataManager
{
    public class DBDataAccessUpdate
    {
        public static void UpdateUser(UserModel user)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = $"update users " +
                $"set username = '{user.Username}', " +
                $"password = '{user.Password}', " +
                $"email = '{user.Email}', " +
                $"name = '{user.Name}', " +
                $"lastname = '{user.Lastname}' " +
                $"where id = {user.Id}";
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

        public static void UpdateNotebook(NotebookModel notebook)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = $"update notebooks " +
                $"set name = '{notebook.Name}' " +
                $"where id = {notebook.Id}";
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

        public static void UpdateNote(NoteModel note)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            int index = note.FileLocation.LastIndexOf(@"\");
            string newPath = note.FileLocation.Remove(index);
            newPath = newPath + $@"\{note.Title}.rtf";

            string cmd = $"update notes " +
                $"set title = '{note.Title}', " +
                $"fileLocation = '{newPath}' " +
                $"where id = {note.Id}";
            SQLiteCommand command = new SQLiteCommand(cmd, connection);

            
            
            try
            {
                command.ExecuteNonQuery();
                if(File.Exists(note.FileLocation))
                    File.Move(note.FileLocation, $"{newPath}");
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
