using GenericForms.Settings;
using LAE.Modelo;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GenericForms.Abstract
{
    class FactoryDataGridColumn
    {
        public static DataGridColumn Build(String propertyName, ITypeGridColumnSettings settings, ITypeGridColumnSettings defaultSettings)
        {

            if (defaultSettings == null)
                defaultSettings = TypeGridColumnSettingsEnum.DefaultColum;

            DataGridColumn column;
            if (settings.ColumnCombo != null)
                column = new DataGridComboBoxColumn();
            else if (settings.ColumnButton != null)
                column = new DataGridTemplateColumn();
            else
                column = new DataGridTextColumn();

            column.Width = new DataGridLength(settings.Width ?? defaultSettings.Width ?? 1, (settings.LengthUnitType ?? defaultSettings.LengthUnitType ?? ColumnLengthUnitType.Star).LengthUnitType);
            column.Header = settings.Label ?? propertyName;


            if (settings.ColumnCombo!=null)
            {
                TipoMuestra[] tipos = PersistenceManager<TipoMuestra>.SelectAll().OrderBy(t => t.Nombre).ToArray();
                ((DataGridComboBoxColumn)column).ItemsSource = settings.ColumnCombo.InnerValues;
                ((DataGridComboBoxColumn)column).SelectedValueBinding = new Binding(propertyName);
                ((DataGridComboBoxColumn)column).DisplayMemberPath = settings.ColumnCombo.DisplayPath;
                ((DataGridComboBoxColumn)column).SelectedValuePath = settings.ColumnCombo.Path ?? "Id";
            }
            else if (settings.ColumnButton != null)
            {
                DataTemplate dtm = new DataTemplate();
                FrameworkElementFactory btn = new FrameworkElementFactory(typeof(Button));
                btn.SetValue(Button.WidthProperty, settings.ColumnButton.Size);
                btn.SetValue(Button.HeightProperty, settings.ColumnButton.Size);
                btn.SetValue(Button.MarginProperty, new Thickness(settings.ColumnButton.Margin));
                btn.SetValue(Button.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                if (settings.ColumnButton.Click != null)
                    btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(settings.ColumnButton.Click));

                FrameworkElementFactory path = new FrameworkElementFactory(typeof(Path));
                path.SetValue(Path.DataProperty, Geometry.Parse(settings.ColumnButton.DesingPath));
                path.SetValue(Path.StretchProperty, Stretch.Fill);
                path.SetValue(Path.FillProperty, new SolidColorBrush(settings.ColumnButton.Color));
                path.SetValue(Path.MarginProperty, new Thickness(settings.ColumnButton.Margin));

                btn.AppendChild(path);

                dtm.VisualTree = btn;
                ((DataGridTemplateColumn)column).CellTemplate = dtm;
            }
            else
                ((DataGridTextColumn)column).Binding = new Binding(propertyName) { Mode = BindingMode.TwoWay, FallbackValue = "" };



            return column;
        }

    }
}
