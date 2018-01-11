using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Util
{
    public class ComboBoxItem<T>
    {
        public String Text { get; set; }
        public T Id { get; set; }

        public ComboBoxItem() { }

        public ComboBoxItem(String text, T value)
        {
            Text = text;
            Id = value;
        }

        public override string ToString(){
            return Text;
        }

        public override bool Equals(object obj)
        {
            ComboBoxItem<T> item = obj as ComboBoxItem<T>;

            if (item != null)
                return item.Id.Equals(Id);

            return false;
        }

        public static ComboBoxItem<T> Create(String text, T value)
        {
            return new ComboBoxItem<T> { Text = text, Id = value };
        }
    }
}
