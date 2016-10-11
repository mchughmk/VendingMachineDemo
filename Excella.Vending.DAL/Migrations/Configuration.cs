using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Transactions;
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
            // Using SQL text here because EF's AddOrUpdate() couldn't make use of IDENTITY_INSERT Without being in a distinct
            // Transaction scope, which appeared to require MSDTC, which is overkill. 
            const string SQL_TO_ADD_OR_UPDATE_ID_ROW = @"
                BEGIN
	                IF EXISTS (select * from Payment where id = 1)
	                  BEGIN
		                UPDATE PAYMENT SET Value = 0 WHERE Id = 1
	                  END
	                ELSE
		                BEGIN
		                    SET IDENTITY_INSERT [dbo].[Payment] ON
		                    INSERT INTO Payment (Id, Value) VALUES (1,0)
		                    SET IDENTITY_INSERT [dbo].[Payment] OFF
		                END
	                END";

            context.Database.ExecuteSqlCommand(SQL_TO_ADD_OR_UPDATE_ID_ROW);
        }
    }
}
