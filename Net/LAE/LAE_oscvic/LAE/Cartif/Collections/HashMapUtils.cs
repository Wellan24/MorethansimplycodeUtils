using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Collections
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A hash map utilities. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class HashMapUtils
    {
        #region Static Config Fields

        public static readonly int DEFAULT_INITIAL_CAPACITY = 1 << 4;   /* The default initial capacity */
        public static readonly int MAXIMUM_CAPACITY = 1 << 30;  /* The maximum capacity */
        public static readonly float DEFAULT_LOAD_FACTOR = 0.75f;   /* The default load factor */
        public static readonly int TREEIFY_THRESHOLD = 8;   /* The treeify threshold */
        public static readonly int UNTREEIFY_THRESHOLD = 6; /* The untreeify threshold */
        public static readonly int MIN_TREEIFY_CAPACITY = 64;   /* The minimum treeify capacity */

        #endregion

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Hashes the given key. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="key"> The key. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int Hash(Object key)
        {
            int h;
            return (key == null) ? 0 : (h = key.GetHashCode()) ^ (int)((uint)h >> 16);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Comparable type for. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="x"> The Object to process. </param>
        /// <returns> A Type. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Type ComparableTypeFor(Object x)
        {

            if (x is IComparable)
            {
                Type comparableToReturn;

                Type[] implementedInterfaces, genericsOfIComparable;

                if ((comparableToReturn = x.GetType()) == typeof(String)) // bypass checks
                    return comparableToReturn;

                /* Check if x is IComparable or IComparable<typeof(x)> */
                if ((implementedInterfaces = comparableToReturn.GetInterfaces()) != null)
                {
                    for (int i = 0; i < implementedInterfaces.Length; ++i)
                    {
                        if ((implementedInterfaces[1] is IComparable)
                            || ((genericsOfIComparable = implementedInterfaces[1].GetGenericArguments()) != null
                            && genericsOfIComparable.Length == 1 && genericsOfIComparable[0] == comparableToReturn))
                        {
                            return comparableToReturn;
                        }
                    }
                }
            }

            return null;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Compare comparables. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="kc"> The kc. </param>
        /// <param name="k">  The Object to process. </param>
        /// <param name="x">  The Object to process. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int CompareComparables(Type kc, Object k, Object x)
        {
            return (x == null || x.GetType() != kc ? 0 : ((IComparable)k).CompareTo(x));
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Table size for. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="cap"> The capability. </param>
        /// <returns> An int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int TableSizeFor(int cap)
        {
            int n = cap - 1;
            n |= (int)((uint)n >> 1);
            n |= (int)((uint)n >> 2);
            n |= (int)((uint)n >> 4);
            n |= (int)((uint)n >> 8);
            n |= (int)((uint)n >> 16);
            return (n < 0) ? 1 : (n >= MAXIMUM_CAPACITY) ? MAXIMUM_CAPACITY : n + 1;
        }
    }
}
