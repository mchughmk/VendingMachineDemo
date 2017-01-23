using System.ComponentModel;

namespace Excella.Vending.DAL
{
    public class FakePaymentDAO : IPaymentDAO
    {
        private int balance;

        public void SavePayment(int amount)
        {
            balance += amount;
        }

        public void SavePurchase()
        {
            const int PURCHASE_COST = 50;
            balance -= PURCHASE_COST;
        }

        public void ClearPayments()
        {
            balance = 0;
        }

        public int Retrieve()
        {
            return balance;
        }
    }
}
