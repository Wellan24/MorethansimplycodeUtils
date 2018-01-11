using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Cartif.Collections
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A dictionary that handles null. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    /// <typeparam name="K"> Generic type parameter. </typeparam>
    /// <typeparam name="V"> Generic type parameter. </typeparam>
    ///------------------------------------------------------------------------------------------------------
    public class ConcurrentCartifDictionary<K, V> : ConcurrentDictionary<K, V>
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Indexer to get or set items within this collection using array index syntax. </summary>
        /// <param name="key"> The key. </param>
        /// <returns> The indexed item. </returns>
        ///--------------------------------------------------------------------------------------------------
        public new V this[K key]
        {
            get
            {
                if (key != null && this.ContainsKey(key))
                    return base[key];
                else
                    return default(V);
            }
            set
            {
                if (key != null)
                {
                    if (base.ContainsKey(key))
                        base[key] = value;
                    else
                        base.TryAdd(key, value);
                }
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public ConcurrentCartifDictionary() { }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="capacity"> The capacity. </param>
        ///--------------------------------------------------------------------------------------------------
        public ConcurrentCartifDictionary(int capacity) : base(1, capacity) { }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="initialColection"> The initial colection. </param>
        ///--------------------------------------------------------------------------------------------------
        public ConcurrentCartifDictionary(IDictionary<K, V> initialColection) : base(initialColection) { }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds key. </summary>
        /// <remarks> Oscvic, 2016-01-05. </remarks>
        /// <param name="key">   The key. </param>
        /// <param name="value"> The value. </param>
        ///--------------------------------------------------------------------------------------------------
        public new void Add(K key, V value)
        {
            if (base.ContainsKey(key))
                base[key] = value;
            else
                base.TryAdd(key, value);
        }
    }
}