using System;
using System.Collections.Generic;
using System.Text;

namespace DictionaryToObject
{
    public class Dictionary
    {
        public static dynamic ToObject(IDictionary<String, Object> dictionary)
        {
            var expandoObj = new System.Dynamic.ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<String, Object>>)expandoObj;

            foreach (var keyValuePair in dictionary)
            {
                expandoObjCollection.Add(keyValuePair);
            }
            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }

        public static T ToObject<T>(IDictionary<String, Object> dictionary) where T : class
        {
            return ToObject(dictionary) as T;
        }
    }
}
