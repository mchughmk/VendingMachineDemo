﻿using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using NUnit.Framework;
using System.Transactions;

namespace Tests.Integration.Excella.Vending.Machine
{
    public class VendingMachineTestsADO
    {
        private VendingMachine _vendingMachine;
        private ADOPaymentDAO _paymentDao = new ADOPaymentDAO();
        private TransactionScope _transactionScope;

        [OneTimeSetUp]
        public void FixtureSetup() 
        {
            _paymentDao.ClearPayments();
        }

        [SetUp]
        public void Setup()
        {
            _paymentDao = new ADOPaymentDAO();
            _transactionScope = new TransactionScope();
            var paymentProcessor = new CoinPaymentProcessor(_paymentDao);
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

            Assert.That(currentBalance, Is.EqualTo(originalBalance + 25));
        }

        [Test]
        public void ReleaseChange_WhenNoMoneyInserted_ExpectZero()
        {
            var change = _vendingMachine.ReleaseChange();

            Assert.That(change, Is.EqualTo(0));
        }

        [Test]
        public void ReleaseChange_WhenOneCoinInserted_Expect25()
        {
            _vendingMachine.InsertCoin();

            var change = _vendingMachine.ReleaseChange();

            Assert.That(change, Is.EqualTo(25));
        }

        [Test]
        public void ReleaseChange_WhenThreeCoinsAreInsertedAndAProductIsBought_Expect25()
        {
            _vendingMachine.InsertCoin();
            _vendingMachine.InsertCoin();
            _vendingMachine.InsertCoin();
            _vendingMachine.BuyProduct();

            var change = _vendingMachine.ReleaseChange();

            Assert.That(change, Is.EqualTo(25));
        }
    }
}