using Excella.Vending.Domain;
using Excella.Vending.Machine;
using Moq;
using NUnit.Framework;

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

            Assert.That(change, Is.EqualTo(0));
        }

        [Test]
        public void ReleaseChange_WhenOneCoinInserted_Expect25()
        {
            _paymentProcessor.Setup(p => p.Payment).Returns(25);

            var change = _vendingMachine.ReleaseChange();

            Assert.That(change, Is.EqualTo(25));
        }

        [Test]
        public void BuyProduct_WhenNoMoneyInserted_ExpectNull()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(false);

            var product = _vendingMachine.BuyProduct();

            Assert.That(product, Is.Null);
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

            _vendingMachine.BuyProduct();

            _paymentProcessor.Verify(x => x.ProcessPurchase(), Times.Never);
        }

        [Test]
        public void BuyProduct_WhenMoneyInserted_ExpectProduct()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(true);

            var product = _vendingMachine.BuyProduct();

            Assert.That(product, Is.Not.Null);
        }

        [Test]
        public void GetMessage_WhenNoMoneyInserted_ExpectMoneyPrompt()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(false);

            _vendingMachine.BuyProduct();

            Assert.That(_vendingMachine.Message, Is.EqualTo("Please insert money"));
        }

        [Test]
        public void GetMessage_WhenMoneyInserted_ExpectEnjoyPrompt()
        {
            _paymentProcessor.Setup(p => p.IsPaymentMade()).Returns(true);

            _vendingMachine.BuyProduct();

            var message = _vendingMachine.Message;

            Assert.That(message, Is.EqualTo("Enjoy!"));
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
