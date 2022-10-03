using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Xml.Linq;
using System.Xml;

namespace RunMigration
{
    public static class Utilities
    {
        public static Assembly ASM_ReturnDLL(string DLL_name)
        {
            string directory = new System.IO.FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            string assemblyToLoad = directory + DLL_name;

            return Assembly.LoadFile(assemblyToLoad);
        }

        public static string TSQL_CreateInsertStatement(DataTable dt)
        {
            StringBuilder sqlInsert = new StringBuilder();
            int rows = dt.Rows.Count;

            if (rows > 0)
            {
                int totalCols = dt.Columns.Count;
                int cols = totalCols;

                sqlInsert.Append($"INSERT INTO [{dt.TableName}] (");
                List<string> lsValues = new List<string> { };
                foreach (DataColumn colname in dt.Columns)
                    lsValues.Add(colname.ColumnName);

                sqlInsert.Append(string.Join(",", lsValues) + ") VALUES(");

                List<string> lsRows = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    lsValues = new List<string>();

                    foreach (DataColumn col in dt.Columns)
                    {
                        var type = col.DataType; //Int32
                        lsValues.Add($"'{row[col.ColumnName]}'");
                    }

                    lsRows.Add(string.Join(",", lsValues));
                }

                sqlInsert.Append($"{string.Join("), (", lsRows)})");

            }

            return sqlInsert.ToString();
        }


        public static DataTable XML_LoadData(string tableNameWithVersion, string tableName)
        {
            var xDoc = XDocument.Load(@$"..\..\..\XML\{tableNameWithVersion}.xml");
            return XML_LoadData(xDoc, tableName);
        }

        public static DataTable XML_LoadData(XDocument xDoc, string tableName)
        {
            DataSet ds = new DataSet();
            var xmlDoc = new XmlDocument();
            try
            {
                using (var xmlReader = xDoc.CreateReader())
                {
                    xmlDoc.Load(xmlReader);
                }
                ds.ReadXml(new XmlNodeReader(xmlDoc));
                ds.Tables[0].TableName = tableName;
                System.Diagnostics.Debug.WriteLine("Loaded Data For Data Load :" + tableName + " Records " + ds.Tables[0].Rows.Count.ToString());
            }
            catch
            {
                return new DataTable(tableName);
            }
            return ds.Tables[0];
        }
    }
}
