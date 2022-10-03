using FluentMigrator;
using FluentMigrator.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RunMigration.Migrations
{
    [Migration(202001010900)]
    public class _202001010900_Person: Migration
    {
        public override void Up()
        {
            Create.Table("person")
                .WithColumn("Id").AsInt32().Identity(100, 1).PrimaryKey()
                .WithColumn("Name").AsString();
        }

        public override void Down()
        {
            Delete.Table("person");
        }
    }
}
