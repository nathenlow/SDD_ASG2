using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SDD_ASG2.Models;
using System;
using System.Collections.Generic;
using System.IO;

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

         public void Register(User user)
        {
            //Create a SqlCommand object from connection object
            MySqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO user (userid, email, password, username, savedgamedata)
                                VALUES(@userid, @email, @password, @username, @savedgamedata)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.

            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@userid", 6);
            cmd.Parameters.AddWithValue("@savedgamedata", "");

            //A connection to database must be opened before any operations made.
            conn.Open();

            //SqlDataReader reader = cmd.ExecuteReader();
            //reader.Close();
            //A connection should be closed after operations.
            conn.Close();
        }

    }

}
