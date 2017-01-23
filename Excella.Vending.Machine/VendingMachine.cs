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
            var payment = paymentProcessor.Payment;

            if (payment > 0)
            {
                paymentProcessor.ClearPayments();
            }

            return payment;
        }

        public void InsertCoin()
        {
            paymentProcessor.ProcessPayment(25);
        }

        public Product BuyProduct()
        {
            if (paymentProcessor.IsPaymentMade())
            {
                paymentProcessor.ProcessPurchase();
                Message = "Enjoy!";
                return new Product();
            }

            Message = "Please insert money";
            throw new InvalidOperationException();
        }
    }
}