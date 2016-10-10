using System.Diagnostics.Contracts;
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
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Payment] ON");
            var seedPayment = new Payment
            {
                Id = 1,
                Value = 0
            };
            context.Payments.AddOrUpdate(p => p.Id, seedPayment);
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Payment] OFF");
        }
    }
}
