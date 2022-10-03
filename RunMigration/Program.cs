using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;
using FluentMigrator.SqlServer;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace RunMigration
{
    class Program
    {//more to work with namespaces
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            /*
             using (IAnnouncer announcer = new TextWriterAnnouncer(Console.Out))
            {
                IRunnerContext migrationContext = new RunnerContext(announcer) 
                { 
                  Connection = "Data Source=test.db;Version=3", 
                  Database = "sqlite", 
                  Target = "migrations" 
                };
                TaskExecutor executor = new TaskExecutor(migrationContext);
                executor.Execute();
            }*/
                       
            string connectionString = @"server=.\SQLEXPRESS;database=test;Trusted_Connection=true";//uid=**;pwd=**
            Announcer announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            announcer.ShowSql = true;

            Assembly assembly = Assembly.GetExecutingAssembly();
            IRunnerContext migrationContext = new RunnerContext(announcer);

            var options = new ProcessorOptions
            {
                PreviewOnly = false,  // set to true to see the SQL
                //Timeout = 60
            };
            var factory = new SqlServer2008ProcessorFactory();
            using (IMigrationProcessor processor = factory.Create(connectionString, announcer, options))
            {
                var runner = new MigrationRunner(assembly, migrationContext, processor);
                runner.MigrateUp(true);

                // Or go back down
                //runner.MigrateDown(0);
            }

        }
    }

}
