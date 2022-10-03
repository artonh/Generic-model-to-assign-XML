using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunMigration.Migrations
{
    [Migration(202001011200)]
    public class _202001011200_Person : BaseMigration
    {
        public override void Up()
        {
            var dataTable = Utilities.XML_LoadData(ClassName, this.FileName);
            bool IsIdentity = true;

            Execute.Sql($"DELETE FROM {FileName}");
            if (IsIdentity)
                Execute.Sql($"SET IDENTITY_INSERT {FileName} ON");

            var sqlinsert = Utilities.TSQL_CreateInsertStatement(dataTable);
            Execute.Sql(sqlinsert);

            if (IsIdentity)
                Execute.Sql($"SET IDENTITY_INSERT {FileName} OFF");

            Console.WriteLine($"Executed rows at the {FileName}!");

        }

        public override void Down()
        {
        }
    }
}
