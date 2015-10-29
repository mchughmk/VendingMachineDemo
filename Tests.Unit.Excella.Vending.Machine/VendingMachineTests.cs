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
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuyProduct_WhenNoMoneyInserted_ExpectException()
        {
            paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(false);
            var product = vendingMachine.BuyProduct();
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
    }
}
