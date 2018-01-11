using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cartif.Extensions;
using System.ComponentModel;
using GenericForms.Abstract;
using System.Reflection;
using GenericForms.Settings;
using Cartif.Util;

namespace GenericForms.Implemented
{
    /// <summary>
    /// Lógica de interacción para TypePanel.xaml
    /// </summary>
    public partial class TypePanel : UserControl
    {
        public static readonly DependencyProperty InnerValueProperty =
            DependencyProperty.Register("InnerValue", typeof(Object), typeof(TypePanel),
                new PropertyMetadata(new PropertyChangedCallback((o, t) =>
                   ((TypePanel)o).UpdateBindings())));

        public Object InnerValue
        {
            get { return (Object)GetValue(InnerValueProperty); }
            set { SetValue(InnerValueProperty, value); }
        }

        public Type Type { get; set; }

        private Object expectation;
        public Expectation<T> GetExpectation<T>()
        {
            return (Expectation<T>)expectation;
        }

        public T GetValidatedInnerValue<T>()
        {
            // Type == typeof(T)

            /* Loop the propertyControls to check its validations ** They're faster ** */
            foreach (PropertyControl pc in root.Children.OfType<PropertyControl>())
            {
                if (!pc.IsValid)
                    return default(T);
            }

            /* Check for the expectation to be correct before return */
            if (GetExpectation<T>() != null && !this.GetExpectation<T>().Check((T)InnerValue))
                return default(T);

            return (T)InnerValue;

        }

        public TypePanel()
        {
            InitializeComponent();
        }


        public void Build<T>(Object innerValue)
        {
            Build(innerValue, new TypePanelSettings<T>());
        }

        public void Build<T>(Object innerValue, ITypePanelSettings<T> settings)
        {
            if (!innerValue.GetType().Equals(typeof(T)))
                throw new InvalidCastException("Tipos deben coincidir");

            InnerValue = innerValue;
            InnerBuild(settings);
        }

        private void InnerBuild<T>(ITypePanelSettings<T> settings)
        {
            FieldSettings innerFields = settings.Fields;

            IPropertyControlSettings defaultSettings = settings.DefaultSettings;
            if (defaultSettings == null)
                defaultSettings = PropertyControlSettingsEnum.TextBoxDefault;

            /* If there is no innerFieldSettings, fill up with property names and default settings */
            if (innerFields == null)
            {
                PropertyInfo[] properties = InnerValue.GetType().GetProperties();
                innerFields = new FieldSettings(properties.Length);
                properties.Map(p => new
                {
                    Name = p.Name,
                    FieldSettings = defaultSettings
                })
                .ForEach(p => innerFields[p.Name] = defaultSettings);
            }

            if (innerFields != null)
            {
                foreach (var item in innerFields)
                {
                    PropertyControl pc = FactoryPropertyControl.Build(InnerValue, item.Key, item.Value, settings.DefaultSettings);
                    root.Children.Add(pc);
                }
            }

            expectation = settings.PanelValidation;
        }

        private void UpdateBindings() => root.Children.OfType<PropertyControl>().ForEach(pc => pc.SetContentBinding(InnerValue));
    }
}
