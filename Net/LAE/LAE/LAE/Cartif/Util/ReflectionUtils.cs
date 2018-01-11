using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Util
{
    public static class ReflectionUtils
    {
        public static TReturn GetValueOfStaticPropertyOf<TReceive, TReturn>(String name) where TReceive : class
                                                                                         where TReturn : class
        {
            // Use the PropertyInfo to retrieve the value from the type by not passing in an instance
            return typeof(TReceive)?.GetProperty(name, BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as TReturn;
        }

        public static Object GetValueOfStaticPropertyOf(this Type type, String name)
        {
            // Use the PropertyInfo to retrieve the value from the type by not passing in an instance
            return type?.GetProperty(name, BindingFlags.Public | BindingFlags.Static)?.GetValue(null);
        }

        public static TReturn GetValueOfStaticFieldOf<TReceive, TReturn>(String name) where TReceive : class
                                                                                      where TReturn : class
        {
            // Use the PropertyInfo to retrieve the value from the type by not passing in an instance
            return typeof(TReceive)?.GetField(name, BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as TReturn;
        }

        public static Object GetValueOfStaticFieldOf(this Type type, String name)
        {
            // Use the PropertyInfo to retrieve the value from the type by not passing in an instance
            return type?.GetField(name, BindingFlags.Public | BindingFlags.Static)?.GetValue(null);
        }

        public static TReturn GetValueOfFieldOf<TReturn>(String name, Object obj) where TReturn : class
        {
            // Use the PropertyInfo to retrieve the value from the type by not passing in an instance
            return typeof(TReturn)?.GetField(name, BindingFlags.Public | BindingFlags.Instance)?.GetValue(obj) as TReturn;
        }

        public static Object GetValueOfFieldOf(this Type type, String name, Object obj)
        {
            // Use the PropertyInfo to retrieve the value from the type by not passing in an instance
            return type?.GetField(name, BindingFlags.Public | BindingFlags.Instance)?.GetValue(obj);
        }

        public static TReturn GetValueOfPropertyOf<TReturn>(String name, Object obj) where TReturn : class
        {
            // Use the PropertyInfo to retrieve the value from the type by not passing in an instance
            return typeof(TReturn)?.GetProperty(name, BindingFlags.Public | BindingFlags.Instance)?.GetValue(obj) as TReturn;
        }

        public static Object GetValueOfPropertyOf(this Type type, String name, Object obj)
        {
            // Use the PropertyInfo to retrieve the value from the type by not passing in an instance
            return type?.GetProperty(name, BindingFlags.Public | BindingFlags.Instance)?.GetValue(obj);
        }
    }
}
