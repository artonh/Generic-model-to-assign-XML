using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RunMigration.Migrations
{
    /// <summary>
    /// before migration to rebuild the XML data
    /// </summary>
    public abstract class SpecialMigration : BaseMigration
    {
        protected Dictionary<string, string> patternReplace { get; private set; }

        /// <param name="pattern">new Dictionary<string, string>() { {"###{date}###", DateTime.Now.ToString() } }</param>
        public SpecialMigration(Dictionary<string, string> pattern)
        {
            patternReplace = pattern;
        }

        /// <summary>
        /// XML modification with placeholder replacement with the keypairs object above
        /// </summary>
        /// <param name="xmlStream"></param>
        /// <returns></returns>
        protected override DataTable XML2Datatable(Stream xmlStream)
        {
            var xDocument = XDocument.Parse(new StreamReader(xmlStream).ReadToEnd().Trim());

            if (patternReplace != null)
                foreach (var item in patternReplace)
                {
                    var elementsToUpdate = xDocument.Descendants().Where(c => c.Value == item.Key && !c.HasElements);
                    foreach (XElement element in elementsToUpdate)
                    {
                        element.Value = item.Value;
                    }
                }

            return Utilities.XML_LoadData(xDocument, FileName);
        }
    }
}
