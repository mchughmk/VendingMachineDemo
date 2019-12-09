using System.Data.Common;
using System.Linq;

namespace Excella.Vending.DAL
{
    public class EFPaymentDAO : IPaymentDAO
    {
        private readonly VendingMachineContext _context = new VendingMachineContext();

        public int Retrieve()
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == 1);

            if (payment != null)
            {
                return payment.Value;
            }

            return 0;
        }

        public void SavePayment(int amount)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == 1);

            if (payment != null)
            {
                payment.Value += amount;
                _context.SaveChanges();
            }
        }

        public void SavePurchase()
        {
            const int PURCHASE_COST = 50;
            var payment = _context.Payments.FirstOrDefault(p => p.Id == 1);

            if (payment != null)
            {
                payment.Value -= PURCHASE_COST;
                _context.SaveChanges();
            }
        }

        public void ClearPayments()
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == 1);

            if (payment != null)
            {
                payment.Value = 0;
                _context.SaveChanges();
            }
        }
    }
}
