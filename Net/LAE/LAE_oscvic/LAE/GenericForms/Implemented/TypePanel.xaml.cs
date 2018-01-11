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
using System.Runtime.CompilerServices;

namespace GenericForms.Implemented
{
    /// <summary>
    /// Lógica de interacción para TypePanel.xaml
    /// </summary>
    public partial class TypePanel : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Boolean IsUpdating;

        public PropertyControl this[String key]
        {
            get { return root.Children.OfType<PropertyControl>().FirstOrDefault(pc => pc.Name.Equals(key)); }
        }

        public bool Enabled
        {
            set { PropertyControls.ForEach(p => p.Enabled = value); }
        }

        public IEnumerable<PropertyControl> PropertyControls
        {
            get { return root.Children.OfType<PropertyControl>(); }
        }

        public Object innerValue;
        public Object InnerValue
        {
            get { return innerValue; }
            set
            {
                if (IsUpdating)
                    SetField(ref innerValue, value);
                else
                    SetField(ref innerValue, value?.Clone(value?.GetType()));

                this.UpdateBindings();
            }
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

            IsUpdating = settings.IsUpdating;
            InnerValue = innerValue;
            InnerBuild(settings);
        }

        private void InnerBuild<T>(ITypePanelSettings<T> settings)
        {
            FieldSettings innerFields = settings.Fields;

            IPropertyControlSettings defaultSettings = settings.DefaultSettings;
            if (defaultSettings == null)
                defaultSettings = PropertyControlSettingsEnum.TextBoxDefault;
            else if (defaultSettings.Type == null)
                defaultSettings.Type = typeof(PropertyControlTextBox);

            int[] columnWidth = settings.ColumnWidths;
            if (columnWidth == null)
                columnWidth = new int[] { 1, 1, 1 };

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
                int r = 0;
                int c = 0;

                int addElementsByColumnSpan = 0;
                foreach (var item in innerFields)
                {
                    if (item.Value.ColumnSpan != 0)
                        addElementsByColumnSpan += (item.Value.ColumnSpan - 1);
                }

                BuildGrid(columnWidth, innerFields.Count + addElementsByColumnSpan);
                foreach (var item in innerFields)
                {
                    PropertyControl pc = FactoryPropertyControl.Build(InnerValue, item.Key, item.Value, defaultSettings);
                    Grid.SetRow(pc, r);
                    Grid.SetColumn(pc, c);
                    if (item.Value.ColumnSpan != 0)
                    {
                        Grid.SetColumnSpan(pc, item.Value.ColumnSpan);
                        c += (item.Value.ColumnSpan - 1);
                    }

                    root.Children.Add(pc);
                    c++;
                    if (c == columnWidth.Length)
                    {
                        r++;
                        c = 0;
                    }
                }
            }
            expectation = settings.PanelValidation;
        }

        private void BuildGrid(int[] columnWidth, int numElements)
        {
            int numColumnas = columnWidth.Length;
            ColumnDefinition c;
            for (int i = 0; i < numColumnas; i++)
            {
                c = new ColumnDefinition();
                c.Width = new GridLength(columnWidth[i], GridUnitType.Star);
                root.ColumnDefinitions.Add(c);
            }
            /* http://stackoverflow.com/questions/17944/how-to-round-up-the-result-of-integer-division */
            int numFilas = (numElements + numColumnas - 1) / numColumnas;
            RowDefinition r;
            for (int i = 0; i < numFilas; i++)
            {
                r = new RowDefinition();
                root.RowDefinitions.Add(r);
            }
        }

        public void ClearGrid()
        {
            root.Children.Clear();
            root.RowDefinitions.Clear();
            root.ColumnDefinitions.Clear();
        }

        private void UpdateBindings()
        {
            root.Children.OfType<PropertyControl>().ForEach(pc => pc.SetContentBinding(InnerValue));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            //if (EqualityComparer<T>.Default.Equals(field, value))
            //    return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
