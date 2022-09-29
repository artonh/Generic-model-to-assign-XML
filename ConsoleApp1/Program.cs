using DictionaryToObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            /* The scenario to read XML data values for running migrations to the Database Server
             
            Suppose that:
             *1  you have more columns on that file than you actually need --- reffer to: arrProperty2Check
             *2  you are itterating in that file, each rows then each columns and you have to assign values to the dictionary --- arrPropertyCommingFromAnotherSource

             *** In this way you the way around to itterate in XML rows and columns, 
                assigning values each of strings above and finally writing the hardcoded object 
                as key pair to pass at the Insert statment of any Database wrapper you're using! ***
             */


            var arrProperty2Check = new string[] { "Prop1", "Prop2", "Prop3" };
            var dictColumnValues = new fillDictionary(arrProperty2Check).GetDictionary;


            /*for testing purposes we will make the same  props.
            And I'll show you only the first row algorithm, the others should be the repetitive up to the insert statement!*/
            var arrPropertyCommingFromAnotherSource = new dummyColumns(arrProperty2Check).rowCols;
            foreach (var column in arrPropertyCommingFromAnotherSource)
            {
                if (dictColumnValues.ContainsKey(column.ColumnName))
                    dictColumnValues[column.ColumnName] = column.ColumnValue;
            }

            var objectConvertedFromProps = Dictionary.ToObject<object>(dictColumnValues);
            //DBwrapper.Insert(objectConvertedFromProps);

            Console.ReadLine();

        }
    }

    public class dummyColumns
    {
        public List<Column> rowCols { get; set; } = new List<Column>();

        public dummyColumns(string[] dummyArrays)
        {
            rowCols.AddRange(
                dummyArrays.Select(item => new Column(item, item.Reverse().ToArray()[0].ToString()))
            );
        }
    }

    public class Column
    {
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }

        public Column(string n, string v)
        {
            ColumnName = n;
            ColumnValue = v;
        }
    }
}
