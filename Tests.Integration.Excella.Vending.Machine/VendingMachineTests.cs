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
        private VendingMachine _vendingMachine;
        private TransactionScope _transactionScope;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
        }

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();

            var paymentDAO = new ADOPaymentDAO();
            var paymentProcessor = new CoinPaymentProcessor(paymentDAO);
            _vendingMachine = new VendingMachine(paymentProcessor);
        }

        [TearDown]
        public void Teardown()
        {
            _transactionScope.Dispose();
        }

        [Test]
        public void InsertCoin_WhenOneCoinInserted_ExpectIncreaseOf25()
        {
            var originalBalance = GetCurrentDBBalance();

            _vendingMachine.InsertCoin();

            var currentBalance = GetCurrentDBBalance();
            Assert.AreEqual(currentBalance, originalBalance + 25);
        }

        [Test]
        public void ReleaseChange_WhenNoMoneyInserted_ExpectZero()
        {
            var change = _vendingMachine.ReleaseChange();

            Assert.AreEqual(0, change);
        }

        [Test]
        public void ReleaseChange_WhenOneCoinInserted_Expect25()
        {
            _vendingMachine.InsertCoin();

            var change = _vendingMachine.ReleaseChange();

            Assert.AreEqual(25, change);
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
