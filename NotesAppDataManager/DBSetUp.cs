using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppDataManager
{
    public class DBSetUp
    {
        public static void SetUpOnStartUp()
        {
            string currDir = Environment.CurrentDirectory;
            string dbName = "NotesAppDB.db3";
            string dbPath = currDir + "//" + dbName;

            if (!Directory.Exists(dbPath))
                SetUp();
        }

        public static void SetUp()
        {
            string currDir = Environment.CurrentDirectory;
            string dbName = "NotesAppDB.db3";
            string dbPath = currDir + "//" + dbName;

            SQLiteConnection.CreateFile(dbPath);
            SQLiteConnection connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();

            string cmd = "PRAGMA foreign_keys = ON";
            SQLiteCommand command = new SQLiteCommand(cmd, connection);
            command.ExecuteNonQuery();


            cmd = "CREATE TABLE users " +
                "(" +
                    "id integer primary key autoincrement, " +
                    "username varchar(35) not null unique, " +
                    "password varchar(45) not null, " +
                    "email varchar(50) not null unique, " +
                    "name varchar(30) not null," +
                    "lastname varchar(35) not null" +
                ")";
            command = new SQLiteCommand(cmd, connection);
            command.ExecuteNonQuery();

            cmd = "CREATE TABLE notebook " +
                "(" +
                    "id integer primary key autoincrement, " +
                    "userId integer," +
                    "name varchar(30) not null," +
                    "foreign key (userId) references users (id)" +
                ")";
            command = new SQLiteCommand(cmd, connection);
            command.ExecuteNonQuery();

            cmd = "CREATE TABLE note " +
                "(" +
                    "id integer primary key autoincrement, " +
                    "notebookId integer, " +
                    "title varchar(30) not null, " +
                    "createdTime text not null, " +
                    "updatedTime text not null, " +
                    "fileLocation text not null, " +
                    "foreign key (notebookId) references notebook (id)" +
                ")";
            command = new SQLiteCommand(cmd, connection);
            command.ExecuteNonQuery();
        }
    }
}
