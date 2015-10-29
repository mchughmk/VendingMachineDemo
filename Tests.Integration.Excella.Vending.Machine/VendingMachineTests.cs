using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Transactions;

namespace Tests.Integration.Excella.Vending.Machine
{
    public class VendingMachineTests
    {
        private VendingMachine vendingMachine;
        private TransactionScope transactionScope;

        [SetUp]
        public void Setup()
        {
            transactionScope = new TransactionScope();

            var paymentDAO = new ADOPaymentDAO();
            var paymentProcessor = new CoinPaymentProcessor(paymentDAO);
            vendingMachine = new VendingMachine(paymentProcessor);
        }

        [TearDown]
        public void Teardown()
        {
            transactionScope.Dispose();
        }

        [Test]
        public void ReleaseChange_WhenNoMoneyInserted_ExpectZero()
        {
            var change = vendingMachine.ReleaseChange();

            Assert.AreEqual(25, change);
        }

        [Test]
        public void ReleaseChange_WhenOneCoinInserted_Expect25()
        {
            vendingMachine.InsertCoin();

            var change = vendingMachine.ReleaseChange();

            Assert.AreEqual(25, change);
        }

        [Test]
        public void InsertCoin_WhenOneCoinInserted_Expect25()
        {
            var currentBalance = GetCurrentDBBalance();
            Assert.AreEqual(0, currentBalance);

            vendingMachine.InsertCoin();

            currentBalance = GetCurrentDBBalance();
            Assert.AreEqual(25, currentBalance);
        }

        private SqlConnection GetConnection()
        {
            var connectionString = "Server=.;Database=VendingMachine;Trusted_Connection=True;";

            return new SqlConnection(connectionString);
        }

        private int GetCurrentDBBalance()
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
    }
}
