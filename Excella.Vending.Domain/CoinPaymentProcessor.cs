using System.Runtime.InteropServices;
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
            paymentDAO.SavePayment(amount);
        }

        public bool IsPaymentMade()
        {
            return Payment >= 50;
        }

        public void ProcessPurchase()
        {
            paymentDAO.SavePurchase();
        }

        public void ClearPayments()
        {
            if (Payment > 0)
            {
                paymentDAO.ClearPayments();
            }
        }
    }
}
