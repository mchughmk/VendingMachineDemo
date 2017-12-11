using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Moq;
using NUnit.Framework;

namespace Tests.Unit.Excella.Vending.Domain
{
    [TestFixture]
    public class CoinPaymentProcessorTests
    {
        CoinPaymentProcessor _paymentProcessor;
        Mock<IPaymentDAO> _paymentDAO;

        [SetUp]
        public void SetUp()
        {
            _paymentDAO = new Mock<IPaymentDAO>();
            _paymentProcessor = new CoinPaymentProcessor(_paymentDAO.Object);
        }

        [Test]
        public void Payment_WhenNoMoney_ExpectBalanceIsZero()
        {
            _paymentDAO.Setup(d => d.Retrieve()).Returns(0);

            var balance = _paymentProcessor.Payment;

            Assert.AreEqual(0, balance);
        }

        [Test]
        public void Payment_WhenMoney_ExpectBalanceIsNotZero()
        {
            _paymentDAO.Setup(d => d.Retrieve()).Returns(25);

            var balance = _paymentProcessor.Payment;

            Assert.AreEqual(25, balance);
        }

        [Test]
        public void IsPaymentMade_WhenNoMoney_ExpectFalse()
        {
            _paymentDAO.Setup(d => d.Retrieve()).Returns(0);

            var actual = _paymentProcessor.IsPaymentMade();

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void IsPaymentMade_WhenLessThan50Cents_ExpectFalse()
        {
            _paymentDAO.Setup(d => d.Retrieve()).Returns(25);

            var actual = _paymentProcessor.IsPaymentMade();

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void IsPaymentMade_When50Cents_ExpectTrue()
        {
            _paymentDAO.Setup(d => d.Retrieve()).Returns(50);

            var actual = _paymentProcessor.IsPaymentMade();

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void IsPaymentMade_WhenGreaterThan50Cents_ExpectTrue()
        {
            _paymentDAO.Setup(d => d.Retrieve()).Returns(75);

            var actual = _paymentProcessor.IsPaymentMade();

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void ProcessPayment_WhenPaymentMade_ExpectSavedToDB()
        {
            _paymentDAO.Setup(d => d.SavePayment(It.IsAny<int>())).Verifiable();

            _paymentProcessor.ProcessPayment(25);

            _paymentDAO.Verify(d => d.SavePayment(25), Times.Once);
        }

        [Test]
        public void ProcessPurchase_WhenPurchaseMade_ExpectSavedToDB()
        {
            _paymentProcessor.ProcessPurchase();

            _paymentDAO.Verify(d => d.SavePurchase(), Times.Once);
        }

        [Test]
        public void ClearPayment_WhenPaymentHasBeenMade_TellsDaoToClearPayment()
        {
            _paymentDAO.Setup(x => x.Retrieve()).Returns(25);

            _paymentProcessor.ClearPayments();
            
            _paymentDAO.Verify(x=>x.ClearPayments(), Times.Once);
        }

        [Test]
        public void ClearPayment_WhenPaymentHasNotBeenMade_DoesNotTellDaoToClearPayment()
        {
            _paymentDAO.Setup(x => x.Retrieve()).Returns(0);

            _paymentProcessor.ClearPayments();

            _paymentDAO.Verify(x => x.ClearPayments(), Times.Never);
        }
    }
}
