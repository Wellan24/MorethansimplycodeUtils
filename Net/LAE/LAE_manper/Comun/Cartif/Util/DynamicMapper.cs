using Cartif.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Cartif.Util
{
    public class DynamicMapper
    {
        protected Dictionary<String, PropertyInfo> propertyMap;

        public Dictionary<String, PropertyInfo> GetPropertyMap()
        {
            return new Dictionary<String, PropertyInfo>(propertyMap);
        }
    }

    public class ObjectMapper : DynamicMapper<Object>
    {
        private Type type;

        public ObjectMapper(Type type)
        {
            this.type = type;
            this.propertyMap = type.GetProperties().ToDictionary(p => p.Name, p => p);
        }

        public ObjectMapper(DynamicMapper mapper)
        {
            this.type = mapper.GetType().GetGenericArguments()[0];
            this.propertyMap = mapper.GetPropertyMap();
        }

        public Object From(DynamicObject source)
        {
            var dict = (IDictionary<string, object>)source;

            if (dict.ContainsKey("_sourceReference"))
                return dict["_sourceReference"];

            Object destination = Activator.CreateInstance(type);

            foreach (var prop in propertyMap)
            {
                PropertyInfo p = prop.Value;
                p.SetValue(destination, dict[prop.Key]);
            }

            return destination;
        }
    }

    public class DynamicMapper<T> : DynamicMapper where T : new()
    {
        public static DynamicMapper<T> Mapper = new DynamicMapper<T>();

        protected DynamicMapper()
        {
            propertyMap = typeof(T).GetProperties().ToDictionary(p => p.Name, p => p);
        }

        public Flexpando ToFlexpando(T source, Boolean keepReference = true)
        {
            Flexpando destination = new Flexpando();

            foreach (var prop in propertyMap)
            {
                PropertyInfo p = prop.Value;
                destination.Add(prop.Key, p.GetValue(source));
            }

            if (keepReference)
                destination.Add("_sourceReference", source);

            return destination;
        }

        public DynamicObject ToDynamic(T source, Boolean keepReference = false)
        {
            Flexpando destination = new Flexpando();

            foreach (var prop in propertyMap)
            {
                PropertyInfo p = prop.Value;
                destination.Add(prop.Key, p.GetValue(source));
            }

            if (keepReference)
                destination.Add("_sourceReference", source);

            return destination;
        }

        public T FromDynamic(DynamicObject source, [CallerMemberName] String name = "")
        {
            var dict = (IDictionary<string, object>)source;

            if (dict.ContainsKey("_sourceReference"))
                return (T)dict["_sourceReference"];

            T destination = new T();

            foreach (var prop in propertyMap)
            {
                PropertyInfo p = prop.Value;
                p.SetValue(destination, dict[prop.Key]);
            }

            return destination;
        }
    }

    public class Flexpando : DynamicObject, IDictionary<String, Object>
    {
        private Dictionary<String, Object> dictionary = new Dictionary<String, Object>(StringComparer.OrdinalIgnoreCase);

        public ICollection<String> Keys => dictionary.Keys;

        public ICollection<Object> Values => dictionary.Values;

        public int Count => dictionary.Count;

        public bool IsReadOnly => ((IDictionary<String, Object>)dictionary).IsReadOnly;

        public object this[string key]
        {
            get { return dictionary[key]; }
            set { dictionary[key] = value; }
        }

        public Flexpando() { }

        public Flexpando(Dictionary<string, object> dictionary) { this.dictionary = dictionary; }

        public void Add(String key, Object value) { dictionary.Add(key, value); }

        public override bool Equals(Object obj)
        {
            if (this == obj) return true;

            Flexpando other = obj as Flexpando;

            if (other == null || this.Count != other.Count) return false;

            var valueComparer = EqualityComparer<Object>.Default;

            foreach (var kvp in this.dictionary)
            {
                Object value2;
                if (!other.TryGetValue(kvp.Key, out value2)) return false;
                if (!valueComparer.Equals(kvp.Value, value2)) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return dictionary.GetHashCode();
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dictionary[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return dictionary.TryGetValue(binder.Name, out result);
        }

        public bool ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            ((IDictionary<String, Object>)dictionary).Add(item);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((IDictionary<String, Object>)dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return ((IDictionary<String, Object>)dictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
    }
}
