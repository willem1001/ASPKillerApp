using System.Data.SqlClient;

namespace AspKillerapp.Models
{
    public class Database
    {
        static SqlConnection Connection;
        public static SqlConnection Connect()
        {
            string Connectionstring = "Data Source=WILLEM_LAPTOP;Initial Catalog=KillerappGame;Integrated Security=True";
            
            try
            {
                Connection = new SqlConnection(Connectionstring);
                Connection.Open();
                return Connection;
            }
            catch (SqlException)
            {
                return null;
            }
        }

        public static void CloseConnection()
        {
            Connection.Close();
        }
    }
}