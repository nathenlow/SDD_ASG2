﻿using Microsoft.Extensions.Configuration;
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

        //get user through email
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


    }

}
