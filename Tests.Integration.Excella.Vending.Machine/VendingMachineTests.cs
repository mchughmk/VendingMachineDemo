using System.Collections;
using System.Collections.Generic;
using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using NUnit.Framework;
using System.Transactions;

namespace Tests.Integration.Excella.Vending.Machine
{
    [TestFixtureSource(typeof(PaymentDaoTestCases), "TestCases")]
    public class VendingMachineTests
    {
        private VendingMachine _vendingMachine;
        private readonly IPaymentDAO _injectedPaymentDao;
        private TransactionScope _transactionScope;

        public VendingMachineTests(IPaymentDAO paymentDao)
        {
            _injectedPaymentDao = paymentDao;
        }

        [OneTimeSetUp]
        public void FixtureSetup() // Leaving this to demonstrate that it's usually called FixtureSerup
        {
        }

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            var paymentProcessor = new CoinPaymentProcessor(_injectedPaymentDao);
            _vendingMachine = new VendingMachine(paymentProcessor);

            _vendingMachine.ReleaseChange();
        }

        [TearDown]
        public void Teardown()
        {
            _transactionScope.Dispose();
        }

        [Test]
        public void InsertCoin_WhenOneCoinInserted_ExpectIncreaseOf25()
        {

            var originalBalance = _vendingMachine.Balance;

            _vendingMachine.InsertCoin();

            var currentBalance = _vendingMachine.Balance;
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

        [Test]
        public void ReleaseChange_WhenThreeCoinsAreInsertedAndAProductIsBought_Expect25()
        {
            _vendingMachine.InsertCoin();
            _vendingMachine.InsertCoin();
            _vendingMachine.InsertCoin();

            _vendingMachine.BuyProduct();
            var change = _vendingMachine.ReleaseChange();

            Assert.AreEqual(25, change);
        }

        public class PaymentDaoTestCases
        {
            public static IEnumerable<object> TestCases
            {
                get
                {
                    yield return new EFPaymentDAO();
                    yield return new ADOPaymentDAO();
                }
            }
        }
    }
}
