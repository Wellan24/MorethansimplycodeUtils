using GUI.TreeListView.Tree;
using LAE.Modelo;
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

namespace GUI.TreeListView.Tabs
{
    /// <summary>
    /// Lógica de interacción para AlicuotasRecepcionAgua.xaml
    /// </summary>
    public partial class AlicuotasRecepcionAgua : UserControl
    {
        public static readonly DependencyProperty SelectedAlicuotaProperty =
            DependencyProperty.Register("SelectedItem", typeof(AlicuotaRecepcionAgua), typeof(AlicuotasRecepcionAgua),
                new PropertyMetadata(null));

        public AlicuotaRecepcionAgua SelectedItem
        {
            get { return GetValue(SelectedAlicuotaProperty) as AlicuotaRecepcionAgua; }
            set { SetValue(SelectedAlicuotaProperty, value); }
        }

        public AlicuotaItem SelectedItemTree { get; set; }

        Point startPoint;

        public AlicuotasRecepcionAgua()
        {
            SelectedItem = new AlicuotaRecepcionAgua();
            SelectedItemTree = new AlicuotaItem() { };
            InitializeComponent();
        }

        public void Build(MuestraRecepcionAgua muestra)
        {
            AlicuotaRecepcionAguaModel model = AlicuotaRecepcionAguaModel.CreateAlicuotaModel(muestra.Alicuotas.ToList(), muestra.Parametros.ToList());
            tree.Model = model;

            foreach (var node in tree.Nodes)
                if (node.IsExpandable)
                    node.IsExpanded = true;
        }

        public void AddAlicuota(AlicuotaRecepcionAgua alicuota)
        {
            AlicuotaRecepcionAguaModel modelo = tree.Model as AlicuotaRecepcionAguaModel;
            AlicuotaItem ali = new AlicuotaItem(alicuota);
            modelo.Root.Items.Insert(0, ali);

            Refresh(modelo);
            SelectedItem = new AlicuotaRecepcionAgua();
        }

        public void UpdateAlicuota(AlicuotaRecepcionAgua alicuota)
        {
            AlicuotaRecepcionAguaModel modelo = tree.Model as AlicuotaRecepcionAguaModel;

            if (SelectedItem != null)
            {
                SelectedItem.NumeroAlicuotas = alicuota.NumeroAlicuotas;
                SelectedItem.Cantidad = alicuota.Cantidad;
                SelectedItem.IdUdsCantidad = alicuota.IdUdsCantidad;

                Refresh(modelo);
                SelectedItem = new AlicuotaRecepcionAgua();
            }
        }

        public void RemoveAlicuota()
        {

            AlicuotaRecepcionAguaModel modelo = tree.Model as AlicuotaRecepcionAguaModel;
            if (SelectedItemTree != null)
            {
                for (int i = SelectedItemTree.Items.Count - 1; i >= 0; i--)
                {
                    modelo.Root.Items.Add(SelectedItemTree.Items[i]);
                    SelectedItemTree.Items.RemoveAt(i);
                }
                modelo.Root.Items.Remove(SelectedItemTree);

                Refresh(modelo);
                SelectedItem = new AlicuotaRecepcionAgua();
            }
        }

        private void tree_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            startPoint = e.GetPosition(null);
        }

        private void tree_MouseMove(object sender, MouseEventArgs e)
        {

            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                TreeListItem listViewItem = FindAnchestor<TreeListItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null)
                {
                    TreeNode node = (TreeNode)listViewItem.DataContext;
                    Item item = node.Tag as Item;
                    DataObject dragData = new DataObject("item", item);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move | DragDropEffects.None);
                }
            }
        }

        // Helper to search up the VisualTree
        private static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void tree_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void tree_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("item"))
            {
                Item itemDrag = e.Data.GetData("item") as Item;
                Item itemDrop = (e.OriginalSource as FrameworkElement).DataContext as Item;

                if (itemDrag is ParametroItem && itemDrop is AlicuotaItem)
                    e.Effects = DragDropEffects.Move;
                else
                    e.Effects = DragDropEffects.None;

                e.Handled = true;
            }
        }

        private void tree_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent("item"))
            {
                Item itemDrag = e.Data.GetData("item") as Item;
                Item itemDrop = (e.OriginalSource as FrameworkElement).DataContext as Item;

                if (itemDrag is ParametroItem && itemDrop is AlicuotaItem)
                {
                    ParametroItem param = itemDrag as ParametroItem;
                    AlicuotaItem alic = itemDrop as AlicuotaItem;

                    AlicuotaRecepcionAguaModel modelo = tree.Model as AlicuotaRecepcionAguaModel;
                    /* Remove from parent */
                    modelo.Root.Items.ForEach(r => r.Items?.Remove(param));
                    modelo.Root.Items?.Remove(param);

                    /* Add*/
                    alic.Items.Add(param);


                    Refresh(modelo);
                }

            }
        }

        private void Refresh(AlicuotaRecepcionAguaModel modelo)
        {

            tree.Model = null;
            tree.Model = modelo;

            foreach (var node in tree.Nodes)
                if (node.IsExpandable)
                    node.IsExpanded = true;
        }

        private void tree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                TreeNode node = (TreeNode)e.AddedItems[0];
                AlicuotaItem alicuota = node.Tag as AlicuotaItem;
                if (alicuota != null)
                    SelectedItem = alicuota.Alicuota;
                else
                    SelectedItem = new AlicuotaRecepcionAgua();

                SelectedItemTree = alicuota;
            }
        }

        private void treeDeleteAlicuota_Click(object sender, RoutedEventArgs e)
        {
            TreeListItem listViewItem = FindAnchestor<TreeListItem>((DependencyObject)e.OriginalSource);
            if (listViewItem != null)
            {
                AlicuotaRecepcionAguaModel modelo = tree.Model as AlicuotaRecepcionAguaModel;

                TreeNode node = (TreeNode)listViewItem.DataContext;
                AlicuotaItem alic = node.Tag as AlicuotaItem;

                for (int i = alic.Items.Count - 1; i >= 0; i--)
                {
                    modelo.Root.Items.Add(alic.Items[i]);
                    alic.Items.RemoveAt(i);
                }

                modelo.Root.Items.Remove(alic);

                Refresh(modelo);
            }
        }

        private void tree_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
    }
}
