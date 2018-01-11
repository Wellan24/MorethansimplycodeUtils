using Cartif.Util;
using GenericForms;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Modelo;
using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace GenericForms.Implemented
{
    /// <summary>
    /// Lógica de interacción para TypeGrid.xaml
    /// </summary>
    public partial class TypeGrid : UserControl
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(Object), typeof(TypeGrid),
                new PropertyMetadata(null));


        public Object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public TypeGrid()
        {
            InitializeComponent();
        }

        public void Build(Object[] innerValues)
        {
            InnerBuild(innerValues, new TypeGridSettings());
        }

        public void Build(Object[] innerValues, ITypeGridSettings settings)
        {

            InnerBuild(innerValues, settings);
        }

        private void InnerBuild(Object[] innerValues, ITypeGridSettings settings)
        {
            dataGrid.ItemsSource = new ObservableCollection<Object>(innerValues);

            if (settings != null)
            {
                ColumnGridSettings innerFields = settings.Columns;
                if (innerFields == null)
                {
                    dataGrid.AutoGenerateColumns = true;
                }
                if (innerFields != null)
                {
                    /* add header columns */
                    foreach (var item in innerFields)
                    {
                        DataGridTextColumn column = new DataGridTextColumn();
                        //column.Width = (item.Value.Width != null) ? new DataGridLength(item.Value.Width ?? 0, DataGridLengthUnitType.Star) : DataGridLength.Auto;
                        column.Width = (item.Value.Columns.Width != null) ? new DataGridLength(item.Value.Width ?? 0, DataGridLengthUnitType.Star) : new DataGridLength(1, DataGridLengthUnitType.Star);
                        column.Header = (item.Value.Label != null) ? item.Value.Label : item.Key;
                        column.Binding = new Binding(item.Key) { Mode = BindingMode.TwoWay, FallbackValue = "" };
                        dataGrid.Columns.Add(column);
                    }
                }


                dataGrid.ItemsSource = new ObservableCollection<Object>(innerValues);

                /* add header columns */
                foreach (var item in settings)
                {
                    DataGridTextColumn column = new DataGridTextColumn();
                    //column.Width = (item.Value.Width != null) ? new DataGridLength(item.Value.Width ?? 0, DataGridLengthUnitType.Star) : DataGridLength.Auto;
                    column.Width = (item.Value.Width != null) ? new DataGridLength(item.Value.Width ?? 0, DataGridLengthUnitType.Star) : new DataGridLength(1, DataGridLengthUnitType.Star);
                    column.Header = (item.Value.Label != null) ? item.Value.Label : item.Key;
                    column.Binding = new Binding(item.Key) { Mode = BindingMode.TwoWay, FallbackValue = "" };
                    dataGrid.Columns.Add(column);
                }
            }

        }

        //private void InnerBuild(Object[] innerValues)
        //{
        //    dataGrid.AutoGenerateColumns = true;
        //    dataGrid.ItemsSource = new ObservableCollection<Object>(innerValues);
        //}
    }
}
