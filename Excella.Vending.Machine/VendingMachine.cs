using Excella.Vending.Domain;

namespace Excella.Vending.Machine
{
    public class VendingMachine : IVendingMachine
    {
        private readonly IPaymentProcessor _paymentProcessor;

        public double Balance { get { return _paymentProcessor.Payment; } }
        public string Message { get; set; }

        public VendingMachine(IPaymentProcessor paymentProcessor)
        {
            _paymentProcessor = paymentProcessor;
        }

        public int ReleaseChange()
        {
            var payment = _paymentProcessor.Payment;

            if (payment > 0)
            {
                _paymentProcessor.ClearPayments();
            }

            return payment;
        }

        public void InsertCoin()
        {
            _paymentProcessor.ProcessPayment(25);
        }

        public Product BuyProduct()
        {
            if (_paymentProcessor.IsPaymentMade())
            {
                _paymentProcessor.ProcessPurchase();
                Message = "Enjoy!";
                return new Product();
            }

            Message = "Please insert money";
            return null;
        }
    }
}