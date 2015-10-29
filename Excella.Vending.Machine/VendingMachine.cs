using Excella.Vending.Domain;
using System;

namespace Excella.Vending.Machine
{
    public class VendingMachine : IVendingMachine
    {
        private IPaymentProcessor paymentProcessor;

        public double Balance { get { return paymentProcessor.Payment; } }
        public string Message { get; set; }

        public VendingMachine(IPaymentProcessor paymentProcessor)
        {
            this.paymentProcessor = paymentProcessor;
        }

        public int ReleaseChange()
        {
            return paymentProcessor.Payment;
        }

        public void InsertCoin()
        {
            paymentProcessor.ProcessPayment(25);
        }

        public Product BuyProduct()
        {
            if (paymentProcessor.IsPaymentMade())
            {
                Message = "Enjoy!";
                return new Product();
            }

            Message = "Please insert money";
            throw new InvalidOperationException();
        }
    }
}