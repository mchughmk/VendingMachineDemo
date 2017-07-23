using System.Collections.Generic;
using Excella.Vending.DAL;

namespace Tests.Integration.Excella.Vending.Machine
{
    public class PaymentDaoTestCases
    {
        public static IEnumerable<object> TestCases
        {
            get
            {
                yield return new EFPaymentDAO();
                yield return new ADOPaymentDAO();
            }
        }
    }
}