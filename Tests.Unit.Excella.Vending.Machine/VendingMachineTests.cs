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
        private VendingMachine vendingMachine;
        private Mock<IPaymentProcessor> paymentProcessor;

        [SetUp]
        public void Setup()
        {
            paymentProcessor = new Mock<IPaymentProcessor>();
            vendingMachine = new VendingMachine(paymentProcessor.Object);
        }

        [Test]
        public void ReleaseChange_WhenNoMoneyInserted_ExpectZero()
        {
            paymentProcessor.Setup(p => p.Payment).Returns(0);
            var change = vendingMachine.ReleaseChange();

            Assert.AreEqual(0, change);
        }

        [Test]
        public void ReleaseChange_WhenOneCoinInserted_Expect25()
        {
            paymentProcessor.Setup(p => p.Payment).Returns(25);

            var change = vendingMachine.ReleaseChange();

            Assert.AreEqual(25, change);
        }

        [Test]
        public void BuyProduct_WhenNoMoneyInserted_ExpectException()
        {
            paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(false);

            Assert.That(()=> vendingMachine.BuyProduct(), Throws.InvalidOperationException);
        }

        [Test]
        public void BuyProduct_WhenPaymentMade_CallsPaymentProcessorToProcessPurchase()
        {
            paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(true);

            vendingMachine.BuyProduct();

            paymentProcessor.Verify(x=>x.ProcessPurchase(), Times.Once);
        }

        [Test]
        public void BuyProduct_WhenPaymentNotMade_CallsPaymentProcessorToProcessPurchase_()
        {
            paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(false);

            try
            {
                vendingMachine.BuyProduct();
            }
            catch (Exception)
            {
                // Ignored because we are testing the verification, not the exception
            }

            paymentProcessor.Verify(x => x.ProcessPurchase(), Times.Never);
        }


        [Test]
        public void BuyProduct_WhenMoneyInserted_ExpectProduct()
        {
            paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(true);

            var product = vendingMachine.BuyProduct();

            Assert.IsNotNull(product);
        }

        [Test]
        public void GetMessage_WhenNoMoneyInserted_ExpectMoneyPrompt()
        {
            paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(false);

            try
            {
                var product = vendingMachine.BuyProduct();
                Assert.Fail("BuyProduct with no money did not throw InvalidOperationException");
            }
            catch (InvalidOperationException)
            {
                var message = vendingMachine.Message;
                Assert.AreEqual("Please insert money", message);
            }
        }

        [Test]
        public void GetMessage_WhenMoneyInserted_ExpectEnjoyPrompt()
        {
            paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(true);
            var product = vendingMachine.BuyProduct();

            var message = vendingMachine.Message;

            Assert.AreEqual("Enjoy!", message);
        }

        [Test]
        public void ReleaseChange_WhenCoinInserted_CallsPaymentProcessorToResetPayment()
        {
            paymentProcessor.Setup(x => x.Payment).Returns(25);
            vendingMachine.ReleaseChange();

            paymentProcessor.Verify(x=>x.ClearPayments(), Times.Once);
        }

        [Test]
        public void ReleaseChange_WhenNoCoinInserted_CallsPaymentProcessorToResetPayment()
        {
            vendingMachine.ReleaseChange();

            paymentProcessor.Verify(x => x.ClearPayments(), Times.Never);
        }
    }
}
