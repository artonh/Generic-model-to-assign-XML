using FluentMigrator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace RunMigration.Migrations
{
    /// <summary>
    /// Load data XML files from another assembly
    /// 
    ///  XMLMigrationFiles.Firstnamespace.202001011000_person.xml
    /// </summary>
    [Migration(202001011000)] //yyyyMMddhhmm
    public class _202001011000_Person : BaseMigration
    {

    }

    [Migration(202001011005)]
    public class _202001011005_Person : BaseMigration
    {

    }


    [Migration(202001011006)]
    public class _202001011006_Person : SpecialMigration
    {
        //to replace the placeholders in the XML files

        public _202001011006_Person() : base(
                new Dictionary<string, string>() { {"###{date}###", DateTime.Now.AddDays(-2).ToString() }
            })
        {

        }
    }
}
