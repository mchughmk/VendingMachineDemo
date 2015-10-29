using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Moq;
using NUnit.Framework;

namespace Tests.Unit.Excella.Vending.Domain
{
    [TestFixture]
    public class CoinPaymentProcessorTests
    {
        CoinPaymentProcessor paymentProcessor;
        Mock<IPaymentDAO> paymentDAO;

        [SetUp]
        public void SetUp()
        {
            paymentDAO = new Mock<IPaymentDAO>();
            paymentProcessor = new CoinPaymentProcessor(paymentDAO.Object);
        }

        [Test]
        public void Payment_WhenNoMoney_ExpectBalanceIsZero()
        {
            paymentDAO.Setup(d => d.Retrieve()).Returns(0);
            var balance = paymentProcessor.Payment;

            Assert.AreEqual(0, balance);
        }

        [Test]
        public void Payment_WhenMoney_ExpectBalanceIsNotZero()
        {
            paymentDAO.Setup(d => d.Retrieve()).Returns(25);
            var balance = paymentProcessor.Payment;

            Assert.AreEqual(25, balance);
        }

        [Test]
        public void IsPaymentMade_WhenNoMoney_ExpectFalse()
        {
            paymentDAO.Setup(d => d.Retrieve()).Returns(0);
            var actual = paymentProcessor.IsPaymentMade();

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void IsPaymentMade_WhenMoney_ExpectTrue()
        {
            paymentDAO.Setup(d => d.Retrieve()).Returns(25);
            var actual = paymentProcessor.IsPaymentMade();

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void ProcessPayment_WhenPaymentMade_ExpectSavedToDB()
        {
            paymentDAO.Setup(d => d.Save(It.IsAny<int>())).Verifiable();
            paymentProcessor.ProcessPayment(25);
            paymentDAO.Verify(d => d.Save(25), Times.Once);

        }
    }
}
