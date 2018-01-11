using Cartif.Extensions;
using Cartif.Util;
using GenericForms;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Comun.Persistence;
using System;
using System.Collections;
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
using System.Windows.Markup;
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

        public DataGrid DataGrid => this.dataGrid;

        public Func<Object, SolidColorBrush> ForegroundRow { get; set; }

        public Func<Object, SolidColorBrush> BackgroundRow { get; set; }

        public SelectionChangedEventHandler SelectionChanged { get; set; }

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
            /* if empty add elemente to build and after remove */
            bool vacio = false;
            if (innerValues.Count == 0)
            {
                vacio = true;
                innerValues.Add(default(T));
            }


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
                dataGrid.CanUserSortColumns = settings.CanSortColumns ?? true;

                dataGrid.IsReadOnly = !settings.Editable ?? true;

                dataGrid.Sorting += DataGrid_Sorting;

                ForegroundRow = settings.ForegroundRow;
                BackgroundRow = settings.BackgroundRow;

                AddSelectionChanged(settings.SelectionChanged);
            }

            if (vacio)
                innerValues.Clear();
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            DataGridComboBoxColumn column = e.Column as DataGridComboBoxColumn;
            var comboBox = (column == null) ? Enumerable.Empty<ComboBoxItem<Boolean>>() : column.ItemsSource.OfType<ComboBoxItem<Boolean>>();

            // do event only if the grid contain datagridcomboboxcolumn 
            // and itemssource is not a array of ComboBoxItem<Boolean> because if is ComboBoxItem I dont need sort
            if (column != null && comboBox.IsEmpty())
            {

                //i do some custom checking based on column to get the right comparer
                //i have different comparers for different columns. I also handle the sort direction
                //in my comparer

                // prevent the built-in sort from sorting
                e.Handled = true;

                ListSortDirection direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

                //set the sort order on the column
                column.SortDirection = direction;

                //use a ListCollectionView to do the sort.
                ListCollectionView lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);

                //this is my custom sorter it just derives from IComparer and has a few properties
                //you could just apply the comparer but i needed to do a few extra bits and pieces
                lcv.CustomSort = new ComboBoxColumnComparer(direction, column);
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
            if (ForegroundRow != null && ForegroundRow(e.Row.DataContext) != null)
                e.Row.Foreground = ForegroundRow(e.Row.DataContext);

            if (BackgroundRow != null && BackgroundRow(e.Row.DataContext) != null)
                e.Row.Background = BackgroundRow(e.Row.DataContext);

        }

        private void dataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        public void AddSelectionChanged(SelectionChangedEventHandler handler)
        {
            if (handler != null)
                dataGrid.SelectionChanged += handler;
        }
    }

    public class ComboBoxColumnComparer : IComparer
    {
        private ListSortDirection direction;
        private DataGridComboBoxColumn column;
        private String selectedValuePath;
        private PropertyInfo propertySelectedMemeber;
        private Dictionary<int, String> mappedValues;

        public ComboBoxColumnComparer(ListSortDirection direction, DataGridComboBoxColumn column)
        {
            this.direction = direction;
            this.column = column;
            this.selectedValuePath = ((Binding)(column.SelectedValueBinding)).Path.Path;
            this.LoadValues();
        }

        private void LoadValues()
        {
            var collection = column.ItemsSource.OfType<Object>();
            var item = collection.FirstOrDefault();
            if (item != null)
            {
                /* Obtengo las property info para luego extraer los valores y los display de los diferentes objetos en el ComboBox */
                PropertyInfo propDisplay = null;
                PropertyInfo propValue = item.GetType().GetProperty(column.SelectedValuePath, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

                /* Compruebo si hay path, si no lo hay uso el ToString() */
                if (column.DisplayMemberPath != null)
                    propDisplay = item.GetType().GetProperty(column.DisplayMemberPath, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

                /* Mapeo las propiedades, si la de display es null uso el método ToString() */
                mappedValues = collection.ToDictionary(i => (int)propValue.GetValue(i),
                    i => (String)(propDisplay != null ? propDisplay.GetValue(i) : i.ToString()));
            }
        }

        public int Compare(Object left, Object right)
        {
            if (left.IsNull() && right.IsNotNull())
                return 1 * (1 - 2 * (int)direction);
            else if (left.IsNotNull() && right.IsNull())
                return -1 * (1 - 2 * (int)direction);
            else
            {
                /* Obtengo el id, usando la propiedad que hay en el SelectedValueBinding.Path => selectedValuePath */
                var leftValue = EvalBinding(left);
                var rightValue = EvalBinding(right);

                /* Comparo los valores, uso (- 2 * direction) para cambiar el sentido al resultado y cambiar la dirección de ordenación */
                return String.Compare(mappedValues[leftValue], mappedValues[rightValue], true) * (1 - 2 * (int)direction);
            }
        }

        public int EvalBinding(Object source)
        {
            /* Solo obtengo el ProperyInfo la primera vez, para evitar ralentizar el proceso */
            if (propertySelectedMemeber == null)
                propertySelectedMemeber = source.GetType().GetProperty(selectedValuePath, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            return (int)propertySelectedMemeber.GetValue(source); ;
        }
    }
}
