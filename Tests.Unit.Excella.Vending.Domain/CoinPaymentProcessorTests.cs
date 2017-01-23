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
        public void IsPaymentMade_WhenLessThan50Cents_ExpectFalse()
        {
            paymentDAO.Setup(d => d.Retrieve()).Returns(25);
            var actual = paymentProcessor.IsPaymentMade();

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void IsPaymentMade_When50Cents_ExpectTrue()
        {
            paymentDAO.Setup(d => d.Retrieve()).Returns(50);
            var actual = paymentProcessor.IsPaymentMade();

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void IsPaymentMade_WhenGreaterThan50Cents_ExpectTrue()
        {
            paymentDAO.Setup(d => d.Retrieve()).Returns(75);
            var actual = paymentProcessor.IsPaymentMade();

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void ProcessPayment_WhenPaymentMade_ExpectSavedToDB()
        {
            paymentDAO.Setup(d => d.SavePayment(It.IsAny<int>())).Verifiable();
            paymentProcessor.ProcessPayment(25);
            paymentDAO.Verify(d => d.SavePayment(25), Times.Once);
        }

        [Test]
        public void ProcessPurchase_WhenPurchaseMade_ExpectSavedToDB()
        {
            paymentProcessor.ProcessPurchase();
            paymentDAO.Verify(d => d.SavePurchase(), Times.Once);
        }

        [Test]
        public void ClearPayment_WhenPaymentHasBeenMade_TellsDaoToClearPayment()
        {
            paymentDAO.Setup(x => x.Retrieve()).Returns(25);
            paymentProcessor.ClearPayments();
            
            paymentDAO.Verify(x=>x.ClearPayments(), Times.Once);
        }

        [Test]
        public void ClearPayment_WhenPaymentHasNotBeenMade_DoesNotTellDaoToClearPayment()
        {
            paymentDAO.Setup(x => x.Retrieve()).Returns(0);
            paymentProcessor.ClearPayments();

            paymentDAO.Verify(x => x.ClearPayments(), Times.Never);
        }
    }
}
