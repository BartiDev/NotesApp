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
    }
}
