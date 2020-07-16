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
    public class DBDataAccessDelete
    {
        public static void DeleteNote(int noteId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmd = $"delete from notes where id = {noteId}"; 
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

        public static void DeleteNotebook(int notebookId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmdNote = $"delete from notes where notebookId = {notebookId}";
            SQLiteCommand commandNote = new SQLiteCommand(cmdNote, connection);

            string cmdNotebook = $"delete from notebooks where id = {notebookId}";
            SQLiteCommand commandNotebook = new SQLiteCommand(cmdNotebook, connection);

            try
            {
                commandNote.ExecuteNonQuery();
                commandNotebook.ExecuteNonQuery();
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


        public static void DeleteUser(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmdNote = $"delete from notes where notebookId in (select notebookId from notes left join notebooks on (notebooks.id = notebookId) where userId = {userId})";
            SQLiteCommand commandNote = new SQLiteCommand(cmdNote, connection);

            string cmdNotebook = $"delete from notebooks where userId = {userId}";
            SQLiteCommand commandNotebook = new SQLiteCommand(cmdNotebook, connection);

            string cmdUser = $"delete from users where id = {userId}";
            SQLiteCommand commandUser = new SQLiteCommand(cmdUser, connection);

            try
            {
                commandNote.ExecuteNonQuery();
                commandNotebook.ExecuteNonQuery();
                commandUser.ExecuteNonQuery();
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
