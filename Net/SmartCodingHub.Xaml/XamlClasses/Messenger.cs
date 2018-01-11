using Cartif.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;

namespace Cartif.XamlClasses
{
    public class Messenger
    {
        public static Messenger Default { get; } = new Messenger();

        private SmartCodingHubDictionary<String, Action<Object>[]> registered = new SmartCodingHubDictionary<String, Action<Object>[]>(4);

        public void Register(String key, Action<Object> callback)
        {
            Action<Object>[] actions = registered[key];

            if (actions == null)
            {
                registered[key] = new Action<Object>[] { callback };
            }
            else
            {
                foreach (var item in actions)
                {
                    if (item.Method.ToString() == callback.Method.ToString())
                        return;
                }

                registered[key].Insert(callback);
            }
        }

        public void Unregister(string key, Action<object> callback)
        {
            int index = -1;
            registered[key].ForEachWithIndex((i, item) => { if (item.Equals(callback)) index = (int)i; });

            if (index >= 0)
                registered[key].Remove(index);
        }

        public void RaiseNotification(string key, object args) => registered[key]?.ForEach(callback => callback(args));
    }
}
