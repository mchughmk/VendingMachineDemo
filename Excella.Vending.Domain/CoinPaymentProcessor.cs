using Excella.Vending.DAL;

namespace Excella.Vending.Domain
{
    public class CoinPaymentProcessor : IPaymentProcessor
    {
        public int Payment
        {
            get { return _paymentDAO.Retrieve(); }
        }

        private IPaymentDAO _paymentDAO;

        public CoinPaymentProcessor(IPaymentDAO dao)
        {
            _paymentDAO = dao;
        }

        public void ProcessPayment(int amount)
        {
            _paymentDAO.SavePayment(amount);
        }

        public bool IsPaymentMade()
        {
            return Payment >= 50;
        }

        public void ProcessPurchase()
        {
            _paymentDAO.SavePurchase();
        }

        public void ClearPayments()
        {
            if (Payment > 0)
            {
                _paymentDAO.ClearPayments();
            }
        }
    }
}
