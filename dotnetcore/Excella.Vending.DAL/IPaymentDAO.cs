namespace Excella.Vending.DAL
{
    public interface IPaymentDAO
    {
        int Retrieve();
        void SavePayment(int amount);
        void SavePurchase();
        void ClearPayments();
    }
}