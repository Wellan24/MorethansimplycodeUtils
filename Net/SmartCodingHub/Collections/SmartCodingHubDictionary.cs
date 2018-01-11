using System.Collections.Generic;

namespace SmartCodingHub.Collections
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> -----------------------------------------------------------------
    ///             Namespace:      Cartif.Util Class:          DictionaryCache Description:
    ///             Represents a cache using key value pairs to add/remove its entries Author:
    ///             Oscar - Cartif       Date: 14-10-2015 Notes: Revision History: Name:           Date:
    ///             Description:
    ///           -----------------------------------------------------------------. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    /// <typeparam name="K"> Generic type parameter. </typeparam>
    /// <typeparam name="V"> Generic type parameter. </typeparam>
    ///------------------------------------------------------------------------------------------------------
    public class SmartCodingHubDictionary<K, V> : Dictionary<K, V>
    {

        //Dictionary<K, V> internalCache;

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The indexed item. </returns>
        ///--------------------------------------------------------------------------------------------------
        public new V this[K key]
        {
            get
            {
                if (this.ContainsKey(key))
                    return base[key];
                else
                    return default(V);
            }
            set
            {
                if (base.ContainsKey(key))
                    base[key] = value;

                else
                    base.Add(key, value);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public SmartCodingHubDictionary() { }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="capacity"> The capacity. </param>
        ///--------------------------------------------------------------------------------------------------
        public SmartCodingHubDictionary(int capacity) : base(capacity) { }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="initialColection"> The initial colection. </param>
        ///--------------------------------------------------------------------------------------------------
        public SmartCodingHubDictionary(IDictionary<K, V> initialColection) : base(initialColection) { }
    }
}