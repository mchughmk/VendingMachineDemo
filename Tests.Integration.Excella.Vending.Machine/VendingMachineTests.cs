using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using NUnit.Framework;
using System.Transactions;

namespace Tests.Integration.Excella.Vending.Machine
{
    public class VendingMachineTests
    {
        private VendingMachine _efVendingMachine;
        private TransactionScope _transactionScope;

        [OneTimeSetUp]
        public void FixtureSetup() // Leaving this to demonstrate that it's usually called FixtureSerup
        {
        }

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            var adoDao = new ADOPaymentDAO();
            var efDao = new EFPaymentDAO();
            var efPaymentProcessor = new CoinPaymentProcessor(efDao);
            var adoPaymentProcessor = new CoinPaymentProcessor(adoDao);
            _efVendingMachine = new VendingMachine(efPaymentProcessor);
            _efVendingMachine.ReleaseChange();
        }

        [TearDown]
        public void Teardown()
        {
            _transactionScope.Dispose();
        }

        [Test]
        public void InsertCoin_WhenOneCoinInserted_ExpectIncreaseOf25()
        {

            var originalBalance = _efVendingMachine.Balance;

            _efVendingMachine.InsertCoin();

            var currentBalance = _efVendingMachine.Balance;
            Assert.AreEqual(currentBalance, originalBalance + 25);
        }

        [Test]
        public void ReleaseChange_WhenNoMoneyInserted_ExpectZero()
        {
            var change = _efVendingMachine.ReleaseChange();

            Assert.AreEqual(0, change);
        }

        [Test]
        public void ReleaseChange_WhenOneCoinInserted_Expect25()
        {
            _efVendingMachine.InsertCoin();

            var change = _efVendingMachine.ReleaseChange();

            Assert.AreEqual(25, change);
        }

        [Test]
        public void ReleaseChange_WhenThreeCoinsAreInsertedAndAProductIsBought_Expect25()
        {
            _efVendingMachine.InsertCoin();
            _efVendingMachine.InsertCoin();
            _efVendingMachine.InsertCoin();

            _efVendingMachine.BuyProduct();
            var change = _efVendingMachine.ReleaseChange();

            Assert.AreEqual(25, change);
        }
    }
}
