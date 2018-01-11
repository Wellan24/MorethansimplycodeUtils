using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Data;
using Cartif.Extensions;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;

namespace Cartif.Util
{
    public static class BindUtils
    {
        public static void Bind(Object element, String path, DependencyObject target, DependencyProperty property)
        {
            Binding bind = new Binding(path);
            bind.Source = element;
            bind.Mode = BindingMode.TwoWay;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(target, property, bind);
        }

        public static void Bind(Object element, String path, DependencyObject target, DependencyProperty property, ValidationRule rule)
        {
            Binding bind = new Binding(path);
            bind.Source = element;
            bind.Mode = BindingMode.TwoWay;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            bind.ValidationRules.Add(rule);
            BindingOperations.SetBinding(target, property, bind);
        }

        public static void Bind(Object element, String path, DependencyObject target, DependencyProperty property, BindingMode mode)
        {
            Binding bind = new Binding(path);
            bind.Source = element;
            bind.Mode = mode;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(target, property, bind);
        }


        public static void MultiBind(DependencyObject target, DependencyProperty property, IMultiValueConverter converter, params Binding[] binds)
        {
            MultiBinding bind = new MultiBinding();

            binds.ForEach((b) => bind.Bindings.Add(b));
            bind.Converter = converter;

            BindingOperations.SetBinding(target, property, bind);
        }

        public static Binding CreateBinding(Object element, String path)
        {
            Binding bind = new Binding(path);
            bind.Source = element;
            bind.Mode = BindingMode.TwoWay;
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            return bind;
        }
    }
}
