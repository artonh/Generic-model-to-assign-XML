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

        public static string[] TSQL_CreateInsertStatement(DataTable dt, int maxRow2Seperate = 200)
        {

            int rows = dt.Rows.Count;
            string[] result = new string[System.Math.Abs(rows / maxRow2Seperate) + 2];

            if (rows > 0)
            {
                StringBuilder sqlInsert = new StringBuilder();

                int totalCols = dt.Columns.Count;
                int cols = totalCols;

                sqlInsert.Append($"INSERT INTO [{dt.TableName}] (");
                foreach (DataColumn colname in dt.Columns)
                {
                    sqlInsert.Append(colname.ColumnName);
                    cols--;
                    if (cols > 0)
                        sqlInsert.Append(",");
                    else
                        sqlInsert.AppendLine(")");
                }
                //---header values

                //split in more insertion
                maxRow2Seperate--;

                int insertedRows = 0, executedSlots = 0;
                StringBuilder sqlInsertSepartion = new StringBuilder();
                sqlInsertSepartion.Append(sqlInsert);

                foreach (DataRow row in dt.Rows)
                {
                    cols = totalCols;
                    bool isEven = insertedRows % maxRow2Seperate == 0;
                    bool executeMid = insertedRows != 0 && isEven,
                         executeEnd = executedSlots != 0 && isEven;
                    insertedRows++;

                    sqlInsertSepartion.Append("SELECT ");

                    foreach (DataColumn col in dt.Columns)
                    {
                        sqlInsertSepartion.Append($"'{row[col.ColumnName].ToString().Replace("'", "''")}'");
                        cols--;
                        if (cols > 0)
                            sqlInsertSepartion.Append(",");
                    }
                    rows--;
                    if (rows > 0 && !executeMid && !executeEnd)
                        sqlInsertSepartion.AppendLine(" UNION ALL");
                    else
                    {
                        if (rows == 0)
                            executeEnd = true;
                    }

                    if (executeMid)
                    {
                        result[executedSlots] = sqlInsertSepartion.ToString();

                        executedSlots++;

                        sqlInsertSepartion = new StringBuilder();
                        sqlInsertSepartion.Append(sqlInsert);//header values
                    }
                    else if (executeEnd)
                    {
                        result[executedSlots] = sqlInsertSepartion.ToString();
                    }

                }

            }

            return result;
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
