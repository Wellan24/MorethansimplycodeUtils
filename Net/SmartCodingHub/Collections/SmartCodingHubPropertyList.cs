using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SmartCodingHub.Collections
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> List of cartif properties. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    /// <typeparam name="K"> Generic type parameter. </typeparam>
    /// <typeparam name="V"> Generic type parameter. </typeparam>
    ///------------------------------------------------------------------------------------------------------
    public class SmartCodingHubPropertyList<K, V> : SmartCodingHubDictionary<K, V>
    {
        private readonly Func<V, K> keySelector;    /* The key selector */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="keySelector"> The key selector. </param>
        ///--------------------------------------------------------------------------------------------------
        public SmartCodingHubPropertyList(Func<V, K> keySelector) { this.keySelector = keySelector; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="capacity">    The capacity. </param>
        /// <param name="keySelector"> The key selector. </param>
        ///--------------------------------------------------------------------------------------------------
        public SmartCodingHubPropertyList(int capacity, Func<V, K> keySelector) : base(capacity) { this.keySelector = keySelector; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="initialColection"> The initial colection. </param>
        /// <param name="keySelector">      The key selector. </param>
        ///--------------------------------------------------------------------------------------------------
        public SmartCodingHubPropertyList(List<V> initialColection, Func<V, K> keySelector) : base(initialColection.ToDictionary(keySelector)) { this.keySelector = keySelector; }

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
                /* If the dictionary contains the value set it, if not insert if the key is the same as given by keySelector */
                if (base.ContainsKey(key))
                    base[key] = value;
                else if (key.Equals(keySelector(value)))
                    base.Add(key, value);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds key. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="key">   The key. </param>
        /// <param name="value"> The value. </param>
        ///--------------------------------------------------------------------------------------------------
        public new void Add(K key, V value) { this.Add(value); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds value. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="value"> The value to add. </param>
        ///--------------------------------------------------------------------------------------------------
        public void Add(V value) { base.Add(keySelector(value), value); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds value. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="values"> The value to add. </param>
        ///--------------------------------------------------------------------------------------------------
        public void AddRange(V[] values)
        {
            foreach (var value in values)
                Add(value);
        }
    }
}
