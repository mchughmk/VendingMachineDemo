using Excella.Vending.Domain;
using System;

namespace Excella.Vending.Machine
{
    public class VendingMachine : IVendingMachine
    {
        private IPaymentProcessor _paymentProcessor;

        public double Balance { get { return _paymentProcessor.Payment; } }
        public string Message { get; set; }

        public VendingMachine(IPaymentProcessor paymentProcessor)
        {
            this._paymentProcessor = paymentProcessor;
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
            throw new InvalidOperationException();
        }
    }
}