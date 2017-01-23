using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace Excella.Vending.DAL
{
    public class ADOPaymentDAO : IPaymentDAO
    {
        public int Retrieve()
        {
            using (var connection = GetConnection())
            {
                int payment = 0;

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
                return payment;
            }
        }

        public void SavePayment(int payment)
        {
            using (var connection = GetConnection())
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

        public void SavePurchase()
        {
            const int PURCHASE_PRICE = 50;
            using (var connection = GetConnection())
            {
                SqlCommand command = new SqlCommand(string.Format("UPDATE Payment SET Value = Value - {0} WHERE ID = 1;", PURCHASE_PRICE), connection);
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
                SqlCommand command = new SqlCommand(string.Format("UPDATE Payment SET Value = 0 WHERE ID = 1;"), connection);
                connection.Open();

                var rowsChanged = command.ExecuteNonQuery();

                // TODO: Remove this; we're covered by tests and don't need it
                if (rowsChanged < 1)
                {
                    Console.WriteLine("No rows found.");
                }
            }
        }

        private SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["VendingMachineContext"]?.ConnectionString;

            return new SqlConnection(connectionString ?? "Server=.;Database=VendingMachine;Trusted_Connection=True;");
        }
    }
}
