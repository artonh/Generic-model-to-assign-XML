using FluentMigrator;
using FluentMigrator.SqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace RunMigration.Migrations
{
    /*
     [Migration(202201011201)] //yyyyMMddhhmm
      public class _202201011201_CreateTables : Migration
   */
    public abstract class BaseMigration : Migration
    {
        protected static Assembly asm { get; private set; } = Utilities.ASM_ReturnDLL(@"\XMLMigrationFiles.dll");
        private static string[] GetManifestResourceNames { get; set; } = asm.GetManifestResourceNames();
        protected string resourceName { get; private set; }
        protected string BaseFileNamespace { get; private set; } = "XMLMigrationFiles.Firstnamespace";

        protected long Version { get; private set; } = 0;
        protected string FileName { get; private set; } = string.Empty;
        protected string ClassName { get; private set; }

        public BaseMigration()
        {
            ClassName = this.GetType().Name;

            if (ClassName.Count(x => x == '_') == 2)
            {
                Version = Convert.ToInt64(Regex.Match(ClassName.Substring(0, 14), @"\d+").Value);

                ClassName = ClassName.Substring(1, ClassName.Length - 1);

                FileName = ClassName.Replace($"{Version}_", string.Empty);
            }
            else
                FileName = ClassName;

            resourceName = GetManifestResourceNames.FirstOrDefault(x => x.Equals($"{BaseFileNamespace}.{ClassName}.xml", StringComparison.OrdinalIgnoreCase));
        }

        protected virtual void OnBeforemethodUp() {}

        public override void Up()
        {
            if (resourceName != null)
            {
                OnBeforemethodUp();

                using (Stream xmlStream = asm.GetManifestResourceStream(resourceName))
                {
                    DataTable dataTable = XML2Datatable(xmlStream);

                    Execute.Sql(string.Format("IF (SELECT OBJECTPROPERTY(OBJECT_ID('{0}'), 'TableHasIdentity'))=1 BEGIN SET IDENTITY_INSERT {0} ON; END ELSE BEGIN PRINT 1; END", FileName));
                    foreach (var item in Utilities.TSQL_CreateInsertStatement(dataTable))
                    {
                        Execute.Sql(item);
                    } 
                    Execute.Sql(string.Format("IF (SELECT OBJECTPROPERTY(OBJECT_ID('{0}'), 'TableHasIdentity'))=1 BEGIN SET IDENTITY_INSERT {0} OFF; END ELSE BEGIN PRINT 1; END", FileName));

                    Console.WriteLine($"Executed rows at the {FileName}!");
                }
            }
            else
                Console.WriteLine($"Resource '{ClassName}' cannot be found!");
        }

        protected virtual DataTable XML2Datatable(Stream xmlStream)
        { 
            return Utilities.XML_LoadData(XDocument.Parse(new StreamReader(xmlStream).ReadToEnd().Trim()), FileName);
        }

        public override void Down()
        {
            Execute.Sql($"DELETE FROM {FileName};");
        }
    }







}
