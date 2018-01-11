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
        public static bool GuardarDatos<T>(TypePanel panel, TypeGrid grid, ObservableCollection<T> Lista, String tipo) where T : PersistenceData, IModelo
        {
            if (panel.InnerValue == null) {
                MessageBox.Show("Elija el " + tipo + " que desea actualizar o la opción 'Nuevo' para crearlo");
            }
            else {
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
                        //Lista.Add(objetoSeleccionado);
                        Lista.Insert(0,objetoSeleccionado);

                        grid.dataGrid.SelectedIndex = 0;
                        MessageBox.Show(tipo + " guardado");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Datos erróneos. Por favor, revisa la información");
                }
            }
            return false;
        }

        public static bool BorrarDato<T>(TypePanel panel, TypeGrid grid, ObservableCollection<T> Lista, String tipo) where T : PersistenceData, IModelo
        {
            T objetoSeleccionado = panel.InnerValue as T;
            if (objetoSeleccionado != null && objetoSeleccionado?.Id != 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que deseas eliminar el " + tipo + "? Una vez eliminado, sus datos desaparecerán definitavamente", "Borrar " + tipo, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (objetoSeleccionado.Delete())
                    {
                        Lista.Remove(objetoSeleccionado);
                        grid.dataGrid.SelectedIndex = 0;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Imposible eliminar un " + tipo + " que contiene ofertas y/o revisiones");
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione el " + tipo + " que deseas eliminar");
            }
            return false;
        }
    }
}
