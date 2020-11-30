using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.General.Generic
{
    /// <summary>
    /// Remove all items in a Dictionnary base on a provided value item
    /// </summary>
    public static class DictionaryHelpers
    {
        public static void RemoveByValue<TKey, TValue>(this Dictionary<TKey, TValue> Dictionary, TValue Item) 
        {
            var keysToRemove = Dictionary.Where(KVP => KVP.Value.Equals(Item))
                                        .Select(KVP => KVP.Key)
                                        .ToArray();
            foreach(var key in keysToRemove) {
                Dictionary.Remove(key);
            }
        }
    }
}
