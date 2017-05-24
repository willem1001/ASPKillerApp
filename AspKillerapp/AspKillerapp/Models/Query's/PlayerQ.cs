using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ClassLibrary;

namespace AspKillerapp.Models.Query_s
{
    public class PlayerQ : IPlayer
    {
        public void NewPlayer(string name, string classes)
        {
            string query = "USE [KillerappGame]" +
                           "DECLARE @return_value int" +
                           "EXEC    @return_value = [dbo].[NewCaracter]" +
                           "@Naam = @Name," +
                           "@Klasse = NULL," +
                           "@Weapon = NULL," +
                           "@Shield = NULL," +
                           "@Armor = NULL";

            SqlCommand cmd = new SqlCommand(query, Database.Connect());
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Classes", classes);
            cmd.ExecuteNonQuery();
        }
    }
}