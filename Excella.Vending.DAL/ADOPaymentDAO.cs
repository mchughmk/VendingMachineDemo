using System;
using System.Data.SqlClient;

namespace Excella.Vending.DAL
{
    public class ADOPaymentDAO : IPaymentDAO
    {
        public int Retrieve()
        {
            var connection = GetConnection();
            int payment = 0;

            using (connection)
            {
                SqlCommand command = new SqlCommand("SELECT Value FROM Payment WHERE ID = 1;", connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        payment = reader.GetInt32(0);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }

            return payment;
        }

        public void Save(int payment)
        {
            var connection = GetConnection();
            int id = 0;

            using (connection)
            {
                SqlCommand command = new SqlCommand(string.Format("UPDATE Payment SET Value = Value + {0} WHERE ID = 1;", payment), connection);
                connection.Open();

                var rowsChanged = command.ExecuteNonQuery();

                if (rowsChanged < 1)
                {
                    Console.WriteLine("No rows found.");
                }
            }
        }

        private SqlConnection GetConnection()
        {
            var connectionString = "Server=.;Database=VendingMachine;Trusted_Connection=True;";

            return new SqlConnection(connectionString);
        }
    }
}
