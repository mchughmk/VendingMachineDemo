using System.Linq;

namespace Excella.Vending.DAL
{
    public class EFPaymentDAO : IPaymentDAO
    {
        private VendingMachineContext context = new VendingMachineContext();

        public int Retrieve()
        {
            var payment = context.Payments.Where(p => p.ID == 1).FirstOrDefault();

            if (payment != null)
            {
                return payment.Value;
            }
            else
            {
                return 0;
            }
        }

        public void Save(int amount)
        {
            var payment = context.Payments.Where(p => p.ID == 1).FirstOrDefault();

            if (payment != null)
            {
                payment.Value += amount;
                context.SaveChanges();
            }
        }
    }
}
