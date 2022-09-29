using System;
using System.Collections.Generic;

namespace DictionaryToObject
{
    public class fillDictionary
    {
        public Dictionary<String, Object> GetDictionary { get; private set; }

        public fillDictionary(params string[] fieldsToCheck)
        {
            GetDictionary = new Dictionary<String, Object>();

            foreach (var item in fieldsToCheck)
                GetDictionary.Add(item, string.Empty);
        }
    }

    
}
