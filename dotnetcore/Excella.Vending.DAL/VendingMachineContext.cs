using Excella.Vending.DAL.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Excella.Vending.DAL
{
    public class VendingMachineContext : DbContext
    {
        public VendingMachineContext() : base("VendingMachineContext")
        {
        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
