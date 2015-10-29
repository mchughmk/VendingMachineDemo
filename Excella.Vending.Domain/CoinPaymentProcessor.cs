using Excella.Vending.DAL;

namespace Excella.Vending.Domain
{
    public class CoinPaymentProcessor : IPaymentProcessor
    {
        public int Payment
        {
            get
            {
                return paymentDAO.Retrieve();
            }
        }

        private IPaymentDAO paymentDAO;

        public CoinPaymentProcessor(IPaymentDAO dao)
        {
            paymentDAO = dao;
        }

        public void ProcessPayment(int amount)
        {
            paymentDAO.Save(amount);
        }

        public bool IsPaymentMade()
        {
            return Payment > 0;
        }
    }
}
