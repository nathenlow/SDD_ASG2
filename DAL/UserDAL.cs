using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SDD_ASG2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using SDD_ASG2.ViewModels;

namespace SDD_ASG2.DAL
{
    public class UserDAL
    {
        private MySqlConnection conn;
        private IConfiguration Configuration { get; }

        public UserDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("Default");
            conn = new MySqlConnection(strConn);
        }

        //get user through email
        public User getUser(string email)
        {

            MySqlCommand cmd = new MySqlCommand($"SELECT * FROM user u WHERE u.email = '{email}';", conn);

            //open and execute cmd
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            //read
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    User user = new User
                    {
                        UserId = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Password = reader.GetString(2),
                        Username = reader.GetString(3),
                        SavedGameData = reader.GetString(4)
                    };
                    conn.Close();
                    return user;
                }
            }
            conn.Close();
            return null;
        }

        public void Register(UserRegister user)
        {
            //Create a SqlCommand object from connection object
            MySqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO user (email, password, username) VALUES(@email, @password, @username)";


            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@username", user.Username);

            //A connection to database must be opened before any operations made.
            conn.Open();
            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();
        }

        public bool IsEmailExist(string email, Int32 userid =0)
        {
            bool emailFound = false;
            //to get a user record with the email to be validated
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT userid FROM user
								WHERE email=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);
            //Open a database connection and execute the SQL statement
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != userid)
                        //The email address is used by another user
                        emailFound = true;
                }
            }
            else
            { //No record
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();
            return emailFound;
        }

        public bool IsUsernameExist(string username, Int32 userid = 0)
        {
            bool usernameFound = false;
            //to get a user record with the username to be validated
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT userid FROM user
								WHERE username=@username";
            cmd.Parameters.AddWithValue("@username", username);
            //Open a database connection and execute the SQL statement
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != userid)
                        //The username is used by another user
                        usernameFound = true;
                }
            }
            else
            { //No record
                usernameFound = false; // The username given does not exist
            }
            reader.Close();
            conn.Close();
            return usernameFound;
        }

        public bool CheckPassword(string email, string password)
        {
            //Create a MySqlCommand object from connection object
            MySqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT password FROM user
				WHERE email = @email";

            cmd.Parameters.AddWithValue("@email", email);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            MySqlDataReader reader = cmd.ExecuteReader();


            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    if (password == reader.GetString(0))
                    {
                        //Close data reader
                        reader.Close();
                        //Close database connection
                        conn.Close();
                        return true;
                    }
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return false;
        }

    }

}
