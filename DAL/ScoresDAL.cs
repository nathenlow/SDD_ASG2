using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SDD_ASG2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using SDD_ASG2.ViewModels;
using Org.BouncyCastle.Utilities.Collections;
using static System.Formats.Asn1.AsnWriter;

namespace SDD_ASG2.DAL
{
    public class ScoresDAL
    {
        private MySqlConnection conn;
        private IConfiguration Configuration { get; }

        public ScoresDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("Default");
            conn = new MySqlConnection(strConn);
        }

        //get top 10 scores
        public List<Scores> GetHighscores()
        {
            List<Scores> scoreList = new List<Scores>();

            MySqlCommand cmd = new MySqlCommand($"SELECT * FROM highscore h;", conn);

            //open and execute cmd
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            //read
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Scores highscore = new Scores
                    {
                        Username = reader.GetString(0),
                        Score = reader.GetInt32(1),
                    };

                    scoreList.Add(highscore);
                }
            }
            conn.Close();
            return scoreList;
        }

        // Insert game score into scores table
        public void InsertScore(int userid, int score)
        {

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT scores(score, userid) VALUES(@score, @userid);";
            cmd.Parameters.AddWithValue("@score", score);
            cmd.Parameters.AddWithValue("@userid", userid);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public int CheckInLeaderboard(int userid)
        {

            MySqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandText = @"SELECT s.id FROM  scores s ORDER BY s.score DESC, s.timestamp ASC LIMIT 0,10;";

            MySqlCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = @"SELECT s.id FROM  scores s WHERE s.userid = @userid ORDER BY s.timestamp DESC LIMIT 0,1;";
            cmd2.Parameters.AddWithValue("@userid", userid);

            conn.Open();
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            if (reader2.HasRows)
            {
                reader2.Read();
                int scoreid = reader2.GetInt32(0);
                reader2.Close();

                MySqlDataReader reader1 = cmd1.ExecuteReader();
                if (reader1.HasRows)
                {
                    int count = 0;
                    while (reader1.Read())
                    {
                        count++;
                        reader1.GetInt32(0);

                        if (reader1.GetInt32(0) == scoreid)
                        {
                            return count;
                        }

                    }

                }
                reader1.Close();
            }

            conn.Close();

            return 0;
        }


        // Get user highest score by id
        public int GetUserHighscore(int userid)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT s.score FROM scores s WHERE s.userid = @userid ORDER BY s.score DESC LIMIT 1;";
            cmd.Parameters.AddWithValue("@userid", userid);
            int highscore = 0;

            conn.Open();
            MySqlDataReader reader =cmd.ExecuteReader();

            //read
            if (reader.HasRows)
            {
                reader.Read();
                highscore= reader.GetInt32(0);
            }
            conn.Close();
            return highscore;
        }

    }

}
