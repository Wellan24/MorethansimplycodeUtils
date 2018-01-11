using Cartif.Extensions;
using Cartif.Util;
using GenericForms;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Comun.Cartif.Util;
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
using System.Globalization;
using LAE.Comun.Modelo;

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

        public FlexObservableCollection InnerSource { get; private set; }

        public Object SelectedItem
        {
            get { return InnerSource.Mapper.FromDynamic((Flexpando)GetValue(SelectedItemProperty)); }
            set
            {
                Flexpando flex = InnerSource.ToFlexpando(value);
                SetValue(SelectedItemProperty, flex);
            }
        }

        public int SelectedIndex => dataGrid.SelectedIndex;

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

        public void Build<T>(ITypeGridSettings settings) where T : new()
        {
            InnerBuild<T>(settings);
        }

        private void InnerBuild<T>(ITypeGridSettings settings) where T : new()
        {
            using (var d = Dispatcher.DisableProcessing())
            {
                /* if empty add elemente to build and after remove */
                dataGrid.Columns.Clear();

                /* Configuration DataGrid column */
                ColumnGridSettings innerFields = settings.Columns;

                ITypeGridColumnSettings defaultSettigns = settings.DefaultSettings;
                if (defaultSettigns == null)
                    defaultSettigns = TypeGridColumnSettingsEnum.DefaultColum;

                if (innerFields == null)
                {
                    PropertyInfo[] properties = typeof(T).GetProperties();
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
                    List<ComboBoxConvertParams> convertParams = new List<ComboBoxConvertParams>();
                    /* add header columns */
                    foreach (var item in innerFields)
                    {
                        String propertyName = item.Key;
                        ITypeGridColumnSettings columnSettings = item.Value;

                        propertyName = ProcessComboBoxColumns(propertyName, columnSettings, convertParams);

                        DataGridColumn column = FactoryDataGridColumn.Build(propertyName, columnSettings, settings.DefaultSettings);
                        dataGrid.Columns.Add(column);
                    }

                    InnerSource = new FlexObservableCollection(new ObjectMapper(DynamicMapper<T>.Mapper), convertParams.ToArray());
                    dataGrid.ItemsSource = InnerSource.InnerCollection;
                }

                /* Configuration DataGrid */
                dataGrid.CanUserResizeColumns = settings.CanResizeColumns ?? true;
                dataGrid.CanUserReorderColumns = settings.CanReorderColumns ?? true;
                dataGrid.CanUserSortColumns = settings.CanSortColumns ?? true;

                dataGrid.IsReadOnly = !settings.Editable ?? true;


                ForegroundRow = settings.ForegroundRow;
                BackgroundRow = settings.BackgroundRow;

                AddSelectionChanged(settings.SelectionChanged);
            }
        }

        private string ProcessComboBoxColumns(string propertyName, ITypeGridColumnSettings columnSettings, List<ComboBoxConvertParams> convertParams)
        {
            if (columnSettings.ColumnCombo != null)
            {
                /* Recorrer todos los valores obteniendo la propiedad Id (o Path) para enlazarlos 
                 * y la propiedad que se quiere mostrar (Display o ToString).
                 */
                ComboBoxConvertParams parameters = new ComboBoxConvertParams()
                {
                    PropertyName = propertyName,
                    NewPropertyName = "_" + propertyName
                };

                Object[] innerComboValues = columnSettings.ColumnCombo.InnerValues;
                Type type = innerComboValues.GetType().GetElementType();

                PropertyInfo path = type.GetProperty(columnSettings.ColumnCombo.Path ?? "Id");

                PropertyInfo display = null;
                if (columnSettings.ColumnCombo.DisplayPath != null)
                    display = type.GetProperty(columnSettings.ColumnCombo.DisplayPath);

                /* Uso un dictionary con las claves Path y valores Display */
                parameters.ComboValues = innerComboValues.ToDictionary(v => path.GetValue(v),
                    v => display != null ? display.GetValue(v) : v.ToString());

                propertyName = parameters.NewPropertyName;
                convertParams.Add(parameters);
            }

            return propertyName;
        }

        public void UpdateSelectedItem(Object obj)
        {
            InnerSource.UpdateAt(dataGrid.SelectedIndex, obj);
        }

        public FlexObservableCollection FillDataGrid(IEnumerable<Object> innerValues)
        {
            this.InnerSource.Fill(innerValues);
            return this.InnerSource;
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Object obj = this.InnerSource.Mapper.From((Flexpando)e.Row.DataContext);
            SolidColorBrush brush = ForegroundRow?.Invoke(obj);

            if (brush != null)
                e.Row.Foreground = brush;

            brush = BackgroundRow?.Invoke(obj);
            if (brush != null)
                e.Row.Background = brush;
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

    public class ComboBoxConvertParams
    {
        public String PropertyName;
        public String NewPropertyName;
        public Dictionary<Object, Object> ComboValues;
    }

    public class FlexObservableCollection : IList<Object>
    {
        public ObjectMapper Mapper { get; }
        private ComboBoxConvertParams[] convertParams;

        public ObservableCollection<Flexpando> InnerCollection { get; private set; }

        public Object this[int index]
        {
            get { return Mapper.From(InnerCollection[index]); }
            set { InnerCollection[index] = Mapper.ToFlexpando(value); }
        }

        public int Count => InnerCollection.Count;

        public bool IsReadOnly => false;

        public FlexObservableCollection(ObjectMapper mapper, ComboBoxConvertParams[] convertParams)
        {
            this.Mapper = mapper;
            this.convertParams = convertParams;
            this.InnerCollection = new ObservableCollection<Flexpando>();
        }

        public void Fill(IEnumerable<Object> items)
        {
            this.InnerCollection.Clear();
            ToFlexpando(items).ForEach(i => this.InnerCollection.Add(i));
        }

        public List<T> GetSource<T>()
        {
            return this.InnerCollection.Map(i => (T)Mapper.From(i)).ToList();
        }

        public IEnumerable<T> Enumerate<T>()
        {
            return this.InnerCollection.Map(i => (T)Mapper.From(i));
        }

        private ObservableCollection<Flexpando> ToFlexpando(IEnumerable<Object> innerValues)
        {
            return new ObservableCollection<Flexpando>(innerValues.Map(ToFlexpando));
        }

        public Flexpando ToFlexpando(Object value)
        {
            Flexpando flex = Mapper.ToFlexpando(value);

            Object text;
            foreach (var convertParam in convertParams)
            {
                text = "";
                convertParam.ComboValues.TryGetValue(flex[convertParam.PropertyName], out text);
                flex.Add(convertParam.NewPropertyName, text);
            }

            return flex;
        }

        public void Add(object item)
        {
            InnerCollection.Add(ToFlexpando(item));
        }

        public void AddFirst(object item)
        {
            InnerCollection.Insert(0, ToFlexpando(item));
        }

        public bool Contains(object item)
        {
            Flexpando converted = ToFlexpando(item);
            return ((IList<object>)InnerCollection).Contains(converted);
        }

        public void Insert(int index, object item)
        {
            InnerCollection.Insert(index, ToFlexpando(item));
        }

        public bool Remove(object item)
        {
            return InnerCollection.Remove(ToFlexpando(item));
        }

        public void RemoveAt(int index)
        {
            InnerCollection.RemoveAt(index);
        }

        public void UpdateAt(int index, Object obj)
        {
            InnerCollection[index] = ToFlexpando(obj);
        }

        public void CopyTo(object[] array, int arrayIndex) { }

        public void Clear() => InnerCollection.Clear();

        public IEnumerator<object> GetEnumerator() => InnerCollection.Map(f => Mapper.From(f)).GetEnumerator();

        public int IndexOf(object item) => InnerCollection.IndexOf(Mapper.ToFlexpando(item));

        IEnumerator IEnumerable.GetEnumerator() => InnerCollection.Map(f => Mapper.From(f)).GetEnumerator();
    }
}
