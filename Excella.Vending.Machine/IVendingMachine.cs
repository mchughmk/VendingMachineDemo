namespace Excella.Vending.Machine
{
    public interface IVendingMachine
    {
        double Balance { get; }
        string Message { get; set; }
        Product BuyProduct();
        void InsertCoin();
        int ReleaseChange();
    }
}