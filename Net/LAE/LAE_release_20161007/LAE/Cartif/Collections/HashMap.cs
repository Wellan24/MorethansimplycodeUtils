//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using Cartif.Extensions;

//namespace Cartif.Collections
//{
//    [Serializable]
//    public class HashMap<K, V>
//    {
//        #region Fields

//        private Node<K, V>[] table;
//        private HashSet<Node<K, V>> entrySet;
//        int size;
//        int modCount;
//        int threshold;
//        float loadFactor;

//        #endregion

//        #region Constructors

//        /// <summary>
//        /// Constructs an empty <tt>HashMap</tt> with the specified initial capacity and load factor.
//        /// </summary>
//        /// <param name="initialCapacity">the initial capacity</param>
//        /// <param name="loadFactor">the load factor</param>
//        /// <exception cref="ArgumentException">ArgumentException if the initial capacity is negative or the load factor is nonpositive</exception>
//        public HashMap(int initialCapacity, float loadFactor)
//        {
//            if (initialCapacity < 0)
//                throw new ArgumentException("Illegal initial capacity: " + initialCapacity);

//            if (initialCapacity > HashMapUtils.MAXIMUM_CAPACITY)
//                initialCapacity = HashMapUtils.MAXIMUM_CAPACITY;

//            if (loadFactor <= 0 || loadFactor.IsNaNSafe())
//                throw new ArgumentException("Illegal load factor: " + loadFactor);

//            this.loadFactor = loadFactor;
//            this.threshold = HashMapUtils.TableSizeFor(initialCapacity);
//        }

//        // TODO Comment this using Atomineers
//        public HashMap(int initialCapacity) : this(initialCapacity, HashMapUtils.DEFAULT_LOAD_FACTOR) { }

//        public HashMap() { this.loadFactor = HashMapUtils.DEFAULT_LOAD_FACTOR; }

//        /// <summary>
//        /// This is a workaround for ? extends K, ? extends V, cause no where clause can be used in constructors in C#
//        /// </summary>
//        /// <param name="m">the map</param>
//        /// <param name="evict">false when initially constructing this map, else true (relayed to method afterNodeInsertion).</param>
//        /// <returns></returns>
//        public static HashMap<K, V> CreateFromDictionary(Dictionary<K, V> dictionary)
//        {
//            HashMap<K, V> toReturn = new HashMap<K, V>();
//            //toReturn.putMapEntries(dictionary, false);
//            return toReturn;
//        }
//        // TODO Entry set
//        //    private void putMapEntries(HashMap< K,  V> map, Boolean evict) {
//        //    int s = map.size;
//        //    if (s > 0) {
//        //        if (table == null) { // pre-size
//        //            float ft = ((float)s / loadFactor) + 1.0F;
//        //            int t = ((ft < (float)HashMapUtils.MAXIMUM_CAPACITY) ?
//        //                     (int)ft : HashMapUtils.MAXIMUM_CAPACITY);
//        //            if (t > threshold)
//        //                threshold = HashMapUtils.TableSizeFor(t);
//        //        }
//        //        else if (s > threshold)
//        //            resize();
//        //        foreach (Node<K,V> e in map.entrySet()) {
//        //            K key = e.getKey();
//        //            V value = e.getValue();
//        //            putVal(hash(key), key, value, false, evict);
//        //        }
//        //    }
//        //}

//        #endregion

//        #region Public Properties

//        public int Size { get { return size; } }
//        public Boolean IsEmpty { get { return size == 0; } }

//        #endregion

//        #region Private Methods

//        Node<K, V> getNode(int hash, Object key)
//        {
//            Node<K, V>[] tab; Node<K, V> first, e; int n; K k;

//            if ((tab = table) != null && (n = tab.Length) > 0 && (first = tab[(n - 1) & hash]) != null)
//            {
//                if (first.hash == hash && ((k = first.key).Equals(key) || (key != null && key.Equals(k)))) // always check first node
//                    return first;

//                if ((e = first.next) != null)
//                {
//                    if (first is TreeNode)
//                        return ((TreeNode<K, V>)first).getTreeNode(hash, key);
//                    do
//                    {
//                        if (e.hash == hash &&
//                            ((k = e.key) == key || (key != null && key.equals(k))))
//                            return e;
//                    } while ((e = e.next) != null);
//                }
//            }
//            return null;
//        }

//        #endregion

//        #region EntrySet
//        //class EntrySet : HashSet<Node<K, V>>
//        //{
//        //    //public int size() { return size; }
//        //    //public void clear() { HashMap<K, V>.clear(); }
//        //    public Boolean contains(Object o)
//        //    {
//        //        Node<K, V> e = o as Node<K, V>;
//        //        if (e == null)
//        //            return false;

//        //        Object key = e.Key;
//        //        Node<K, V> candidate = getNode(hash(key), key);
//        //        return candidate != null && candidate.Equals(e);
//        //    }
//        //}
//        #endregion
//    }


//}
