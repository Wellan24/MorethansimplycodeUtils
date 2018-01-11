using Cartif.Extensions;
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
using System.Reflection;
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

        public DataGridColumn this[String key]
        {
            get { return dataGrid.Columns.FirstOrDefault(c => c.Header.Equals(key)); }
        }

        public Func<Object, bool> LoadGrid { get; set; }

        public TypeGrid()
        {
            InitializeComponent();
        }


        public void Build(Object[] innerValues)
        {
            InnerBuild(new ObservableCollection<Object>(innerValues), new TypeGridSettings());
        }

        public void Build<T>(ObservableCollection<T> innerValues)
        {
            InnerBuild(innerValues, new TypeGridSettings());
        }

        public void Build(Object[] innerValues, ITypeGridSettings settings)
        {
            InnerBuild(new ObservableCollection<Object>(innerValues), settings);
        }

        public void Build<T>(ObservableCollection<T> innerValues, ITypeGridSettings settings)
        {
            InnerBuild(innerValues, settings);
        }


        private void InnerBuild<T>(ObservableCollection<T> innerValues, ITypeGridSettings settings)
        {
            dataGrid.Columns.Clear();
            if (innerValues.Count != 0)
            {
                FillDataGrid(innerValues);

                /* Configuration DataGrid column */
                ColumnGridSettings innerFields = settings.Columns;

                ITypeGridColumnSettings defaultSettigns = settings.DefaultSettings;
                if (defaultSettigns == null)
                    defaultSettigns = TypeGridColumnSettingsEnum.DefaultColum;

                if (innerFields == null)
                {
                    PropertyInfo[] properties = innerValues[0].GetType().GetProperties();
                    innerFields = new ColumnGridSettings(properties.Length);
                    properties.Map(p => new
                    {
                        Name = p.Name,
                        ColumnGridSettings = defaultSettigns
                    })
                    .ForEach(p => innerFields[p.Name] = defaultSettigns);

                }
                if (innerFields != null)
                {
                    /* add header columns */
                    foreach (var item in innerFields)
                    {
                        DataGridColumn column = FactoryDataGridColumn.Build(item.Key, item.Value, settings.DefaultSettings);
                        dataGrid.Columns.Add(column);
                    }
                }

                /* Configuration DataGrid */
                dataGrid.CanUserResizeColumns = settings.CanResizeColumns ?? true;
                dataGrid.CanUserReorderColumns = settings.CanReorderColumns ?? true;

                LoadGrid = settings.LoadGrid;
            }
        }

        public void FillDataGrid(Object[] innerValues)
        {
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = new ObservableCollection<Object>(innerValues);
        }

        public void FillDataGrid<T>(ObservableCollection<T> innerValues)
        {
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = innerValues;
        }

        
        public ObservableCollection<Object> GetItemSource()
        {
            return dataGrid.ItemsSource as ObservableCollection<Object>;
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (LoadGrid != null && LoadGrid(e.Row.DataContext))
            {
                e.Row.Foreground = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                e.Row.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
