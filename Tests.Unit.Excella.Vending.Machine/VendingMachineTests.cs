using Excella.Vending.Domain;
using Excella.Vending.Machine;
using Moq;
using NUnit.Framework;
using System;

namespace Tests.Unit.Excella.Vending.Machine
{
    [TestFixture]
    public class VendingMachineTests
    {
        private VendingMachine _vendingMachine;
        private Mock<IPaymentProcessor> _paymentProcessor;

        [SetUp]
        public void Setup()
        {
            _paymentProcessor = new Mock<IPaymentProcessor>();
            _vendingMachine = new VendingMachine(_paymentProcessor.Object);
        }

        [Test]
        public void ReleaseChange_WhenNoMoneyInserted_ExpectZero()
        {
            _paymentProcessor.Setup(p => p.Payment).Returns(0);
            var change = _vendingMachine.ReleaseChange();

            Assert.AreEqual(0, change);
        }

        [Test]
        public void ReleaseChange_WhenOneCoinInserted_Expect25()
        {
            _paymentProcessor.Setup(p => p.Payment).Returns(25);

            var change = _vendingMachine.ReleaseChange();

            Assert.AreEqual(25, change);
        }

        [Test]
        public void BuyProduct_WhenNoMoneyInserted_ExpectException()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(false);

            Assert.That(()=> _vendingMachine.BuyProduct(), Throws.InvalidOperationException);
        }

        [Test]
        public void BuyProduct_WhenPaymentMade_CallsPaymentProcessorToProcessPurchase()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(true);

            _vendingMachine.BuyProduct();

            _paymentProcessor.Verify(x=>x.ProcessPurchase(), Times.Once);
        }

        [Test]
        public void BuyProduct_WhenPaymentNotMade_DoesNotCallPaymentProcessorToProcessPurchase()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(false);

            try
            {
                _vendingMachine.BuyProduct();
            }
            catch (Exception)
            {
                // Ignored because we are testing the verification, not the exception
            }

            _paymentProcessor.Verify(x => x.ProcessPurchase(), Times.Never);
        }


        [Test]
        public void BuyProduct_WhenMoneyInserted_ExpectProduct()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(true);

            var product = _vendingMachine.BuyProduct();

            Assert.IsNotNull(product);
        }

        [Test]
        public void GetMessage_WhenNoMoneyInserted_ExpectMoneyPrompt()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(false);

            try
            {
                var product = _vendingMachine.BuyProduct();
                Assert.Fail("BuyProduct with no money did not throw InvalidOperationException");
            }
            catch (InvalidOperationException)
            {
                var message = _vendingMachine.Message;
                Assert.AreEqual("Please insert money", message);
            }
        }

        [Test]
        public void GetMessage_WhenMoneyInserted_ExpectEnjoyPrompt()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(true);
            var product = _vendingMachine.BuyProduct();

            var message = _vendingMachine.Message;

            Assert.AreEqual("Enjoy!", message);
        }

        [Test]
        public void ReleaseChange_WhenCoinInserted_CallsPaymentProcessorToResetPayment()
        {
            _paymentProcessor.Setup(x => x.Payment).Returns(25);
            _vendingMachine.ReleaseChange();

            _paymentProcessor.Verify(x=>x.ClearPayments(), Times.Once);
        }

        [Test]
        public void ReleaseChange_WhenNoCoinInserted_CallsPaymentProcessorToResetPayment()
        {
            _vendingMachine.ReleaseChange();

            _paymentProcessor.Verify(x => x.ClearPayments(), Times.Never);
        }
    }
}
