using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


namespace SmartCodingHub.Collections
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A node. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    /// <typeparam name="K"> Generic type parameter. </typeparam>
    /// <typeparam name="V"> Generic type parameter. </typeparam>
    ///------------------------------------------------------------------------------------------------------
    public class Node<K, V>
    {
        public readonly int hash;   /* The hash */
        public readonly K key;  /* The key */
        public V value; /* The value */
        public Node<K, V> next; /* The next */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="hash">  The hash. </param>
        /// <param name="key">   The key. </param>
        /// <param name="value"> The value. </param>
        /// <param name="next">  The next. </param>
        ///--------------------------------------------------------------------------------------------------
        public Node(int hash, K key, V value, Node<K, V> next)
        {
            this.hash = hash;
            this.key = key;
            this.value = value;
            this.next = next;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the key. </summary>
        /// <value> The key. </value>
        ///--------------------------------------------------------------------------------------------------
        public K Key { get { return key; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the value. </summary>
        /// <value> The value. </value>
        ///--------------------------------------------------------------------------------------------------
        public V Value { get { return this.value; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Convert this Node&lt;K, V&gt; into a string representation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <returns> A String that represents this Node&lt;K, V&gt; </returns>
        ///--------------------------------------------------------------------------------------------------
        public String toString() { return key + "=" + value; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Actúa como función hash para un tipo concreto. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <returns> Código hash para la clase <see cref="T:System.Object" /> actual. </returns>
        ///--------------------------------------------------------------------------------------------------
        public override int GetHashCode() { return RuntimeHelpers.GetHashCode(key) ^ RuntimeHelpers.GetHashCode(value); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Sets a value. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="newValue"> The new value. </param>
        /// <returns> A V. </returns>
        ///--------------------------------------------------------------------------------------------------
        public V SetValue(V newValue)
        {
            V oldValue = value;
            value = newValue;
            return oldValue;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Determina si el objeto <see cref="T:System.Object" /> especificado es igual al objeto
        ///           <see cref="T:System.Object" /> actual. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="o"> The object to compare to this Node&lt;K, V&gt; </param>
        /// <returns> true si el objeto especificado es igual al objeto actual; de lo contrario, false. </returns>
        ///--------------------------------------------------------------------------------------------------
        public override Boolean Equals(Object o)
        {
            if (Object.ReferenceEquals(o, this))
                return true;

            if (o is KeyValuePair<K, V>)
            {
                KeyValuePair<K, V> e = (KeyValuePair<K, V>)o;
                if (Object.Equals(key, e.Key) && Object.Equals(value, e.Value))
                    return true;
            }

            return false;
        }
    }
}
