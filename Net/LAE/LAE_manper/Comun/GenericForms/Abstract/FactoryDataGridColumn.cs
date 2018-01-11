using Cartif.Util;
using Cartif.XamlResources;
using GenericForms.Settings;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            //DataGridColumnResult result = new DataGridColumnResult();

            DataGridColumn column;
            if (settings.ColumnButton != null)
                column = new DataGridTemplateColumn();
            else
                column = new DataGridTextColumn();

            column.Width = new DataGridLength(settings.Width ?? defaultSettings.Width ?? 1, (settings.LengthUnitType ?? defaultSettings.LengthUnitType ?? ColumnLengthUnitType.Star).LengthUnitType);
            column.Header = settings.Label ?? propertyName;

            if (settings.ColumnButton != null)
            {
                DataTemplate dtm = new DataTemplate();
                FrameworkElementFactory btn = new FrameworkElementFactory(typeof(Button));
                btn.SetValue(Button.WidthProperty, settings.ColumnButton.Size);
                btn.SetValue(Button.HeightProperty, settings.ColumnButton.Size);
                btn.SetValue(Button.MarginProperty, new Thickness(settings.ColumnButton.Margin));
                btn.SetValue(Button.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                if (settings.ColumnButton.Click != null)
                    btn.AddHandler(Button.ClickEvent, new RoutedEventHandler(settings.ColumnButton.Click));

                btn.SetValue(Button.IsEnabledProperty, settings.ColumnButton.Enabled ?? true);

                FrameworkElementFactory path = new FrameworkElementFactory(typeof(Path));

                path.SetValue(Path.DataProperty, Geometry.Parse(settings.ColumnButton.DesingPath));
                path.SetValue(Path.StretchProperty, Stretch.Fill);
                path.SetValue(Path.FillProperty, new SolidColorBrush(settings.ColumnButton.Color));
                path.SetValue(Path.MarginProperty, new Thickness(settings.ColumnButton.Margin));

                btn.AppendChild(path);

                /* hidde with property of type boolean */
                //Binding binding = new Binding(propertyName);
                //binding.Converter = new BooleanToVisibilityConverter();
                //btn.SetBinding(Button.VisibilityProperty, binding);

                dtm.VisualTree = btn;
                ((DataGridTemplateColumn)column).CellTemplate = dtm;
            }
            else
            {
                ((DataGridTextColumn)column).Binding = new Binding(propertyName) { Mode = BindingMode.TwoWay, FallbackValue = "", StringFormat = settings.Format };
            }

            return column;
        }

    }
}
