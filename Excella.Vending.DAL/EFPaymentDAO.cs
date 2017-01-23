using System.Linq;

namespace Excella.Vending.DAL
{
    public class EFPaymentDAO : IPaymentDAO
    {
        private VendingMachineContext context = new VendingMachineContext();

        public int Retrieve()
        {
            var payment = context.Payments.Where(p => p.Id == 1).FirstOrDefault();

            if (payment != null)
            {
                return payment.Value;
            }
            else
            {
                return 0;
            }
        }

        public void SavePayment(int amount)
        {
            var payment = context.Payments.Where(p => p.Id == 1).FirstOrDefault();

            if (payment != null)
            {
                payment.Value += amount;
                context.SaveChanges();
            }
        }

        public void SavePurchase()
        {
            const int PURCHASE_COST = 50;
            var payment = context.Payments.Where(p => p.Id == 1).FirstOrDefault();

            if (payment != null)
            {
                payment.Value -= PURCHASE_COST;
                context.SaveChanges();
            }
        }

        public void ClearPayments()
        {
            const int PURCHASE_COST = 50;
            var payment = context.Payments.Where(p => p.Id == 1).FirstOrDefault();

            if (payment != null)
            {
                payment.Value = 0;
                context.SaveChanges();
            }
        }
    }
}
