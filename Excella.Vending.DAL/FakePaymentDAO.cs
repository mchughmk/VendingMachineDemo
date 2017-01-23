namespace Excella.Vending.DAL
{
    public class FakePaymentDAO : IPaymentDAO
    {
        private int _balance;

        public void SavePayment(int amount)
        {
            _balance += amount;
        }

        public void SavePurchase()
        {
            const int PURCHASE_COST = 50;
            _balance -= PURCHASE_COST;
        }

        public void ClearPayments()
        {
            _balance = 0;
        }

        public int Retrieve()
        {
            return _balance;
        }
    }
}
