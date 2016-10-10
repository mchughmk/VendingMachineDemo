using Excella.Vending.DAL.Models;

namespace Excella.Vending.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<VendingMachineContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(VendingMachineContext context)
        {
            context.Payments.AddOrUpdate(p => new Payment
            {
                ID = 1,
                Value = 0
            });
        }
    }
}
