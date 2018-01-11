using GenericForms.Implemented;
using LAE.Modelo;
using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.Pages
{
    class FormBasicFunctions
    {
        public static void GuardarDatos<T>(TypePanel panel, TypeGrid grid, ObservableCollection<T> Lista, String tipo) where T : PersistenceData, IModelo
        {
            if (panel.GetValidatedInnerValue<T>() != default(T))
            {
                T objetoSeleccionado = panel.InnerValue as T;
                if (objetoSeleccionado.Id != 0)
                {
                    /* update cliente */
                    objetoSeleccionado.Update();

                    /* update grid */
                    int indice = Lista.IndexOf(objetoSeleccionado);
                    Lista[indice] = objetoSeleccionado;
                    grid.FillDataGrid(Lista);

                    grid.dataGrid.SelectedIndex = indice;
                    MessageBox.Show(tipo + " actualizado");
                }
                else {
                    /* insert cliente */
                    int idCliente = objetoSeleccionado.Insert();
                    objetoSeleccionado.Id = idCliente;
                    /* update grid */
                    Lista.Add(objetoSeleccionado);

                    grid.dataGrid.SelectedIndex = grid.dataGrid.Items.Count - 1;
                    MessageBox.Show(tipo + " guardado");
                }
            }
            else
            {
                MessageBox.Show("Algún dato es erroneo");
            }
        }

        public static void BorrarDato<T>(TypePanel panel, TypeGrid grid, ObservableCollection<T> Lista, String tipo) where T : PersistenceData, IModelo
        {
            T objetoSeleccionado = panel.InnerValue as T;
            if (objetoSeleccionado != null && objetoSeleccionado?.Id != 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro de borrar el " + tipo + "?", "Borrar " + tipo, MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (objetoSeleccionado.Delete())
                    {
                        Lista.Remove(objetoSeleccionado);
                        grid.dataGrid.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("No se puede borrar el " + tipo + ". Es referenciado en alguna tabla");
                    }
                }
            }
        }
    }
}
