using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Excella.Vending.DAL
{
    public class ADOPaymentDAO : IPaymentDAO
    {
        public int Retrieve()
        {
            using (var connection = GetConnection())
            {
                var payment = 0;

                var command = new SqlCommand("SELECT Value FROM Payment WHERE ID = 1;", connection);
                connection.Open();

                var reader = command.ExecuteReader();

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
                return payment;
            }
        }

        public void SavePayment(int payment)
        {
            using (var connection = GetConnection())
            {
                var sqlCommandString = string.Format("UPDATE Payment SET Value = Value + {0} WHERE ID = 1;", payment);
                var command = new SqlCommand(sqlCommandString, connection);
                connection.Open();

                var rowsChanged = command.ExecuteNonQuery();

                if (rowsChanged < 1)
                {
                    Console.WriteLine("No rows found.");
                }
            }
        }

        public void SavePurchase()
        {
            const int PURCHASE_PRICE = 50;
            using (var connection = GetConnection())
            {
                var commandText = string.Format("UPDATE Payment SET Value = Value - {0} WHERE ID = 1;", PURCHASE_PRICE);
                var command = new SqlCommand(commandText, connection);
                connection.Open();

                var rowsChanged = command.ExecuteNonQuery();

                if (rowsChanged < 1)
                {
                    Console.WriteLine("No rows found.");
                }
            }
        }

        public void ClearPayments()
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand("UPDATE Payment SET Value = 0 WHERE ID = 1;", connection);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        private SqlConnection GetConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["VendingMachineContext"].ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}
