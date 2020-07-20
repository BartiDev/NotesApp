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
    public class DBDataAccessDelete
    {
        public static void DeleteNote(int noteId)
        {
            string cd = Environment.CurrentDirectory;

            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            /* Search for note's notebookId */
            string searchNote = $"select notebookId, title from notes where id = {noteId}";
            SQLiteCommand commandSearchNote = new SQLiteCommand(searchNote, connection);
            int notebookId;
            string noteTitle;

            try
            {
                var reader = commandSearchNote.ExecuteReader();
                reader.Read();
                notebookId = reader.GetInt32(0);
                noteTitle = reader.GetString(1);
            }
            catch
            {
                throw;
            }

            /* Search for note's notebook's userId and delete note's .rtf file */
            string searchNotebook = $"select userId from notebooks where id = {notebookId}";
            SQLiteCommand commandSearchNotebook = new SQLiteCommand(searchNotebook, connection);
            int userId;

            try
            {
                var reader = commandSearchNotebook.ExecuteReader();
                reader.Read();
                userId = reader.GetInt32(0);
                if (File.Exists(cd + $@"\data\{userId}\{notebookId}\{noteTitle}.rtf"))
                    File.Delete(cd + $@"\data\{userId}\{notebookId}\{noteTitle}.rtf");
            }
            catch
            {
                throw;
            }

            /* Delete note from database */
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
            string cd = Environment.CurrentDirectory;

            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string searchNotebook = $"select userId from notebooks where id = {notebookId}";
            SQLiteCommand commandSearch = new SQLiteCommand(searchNotebook, connection);

            string cmdNote = $"delete from notes where notebookId = {notebookId}";
            SQLiteCommand commandNote = new SQLiteCommand(cmdNote, connection);

            string cmdNotebook = $"delete from notebooks where id = {notebookId}";
            SQLiteCommand commandNotebook = new SQLiteCommand(cmdNotebook, connection);

            /* Delete notebook directory, including all notebook's notes */
            try
            {
                var reader = commandSearch.ExecuteReader();
                reader.Read();
                int userId = reader.GetInt32(0);
                if (Directory.Exists(cd + $@"\data\{userId}\{notebookId}"))
                    Directory.Delete(cd + $@"\data\{userId}\{notebookId}", true);
            }
            catch
            {
                throw;
            }

            /* Delete notebook and notebook's notes from database */
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
            /* Delete user directory, with it's notebooks and notes */
            string cd = Environment.CurrentDirectory;
            Directory.Delete(cd + $@"\data\{userId}", true);
          

            string connectionString = ConfigurationManager.ConnectionStrings["NotesAppDB"].ConnectionString;
            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string cmdNote = $"delete from notes where notebookId in (select notebookId from notes left join notebooks on (notebooks.id = notebookId) where userId = {userId})";
            SQLiteCommand commandNote = new SQLiteCommand(cmdNote, connection);

            string cmdNotebook = $"delete from notebooks where userId = {userId}";
            SQLiteCommand commandNotebook = new SQLiteCommand(cmdNotebook, connection);

            string cmdUser = $"delete from users where id = {userId}";
            SQLiteCommand commandUser = new SQLiteCommand(cmdUser, connection);

            /* Delete user, user's notebooks and notes from database */
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
