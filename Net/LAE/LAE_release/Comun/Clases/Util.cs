using LAE.Comun.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LAE.Comun.Clases
{
    public static class Util
    {
        public static bool ValorUnico<T>(string nombrePropiedad, T valor) where T : PersistenceData, IModelo
        {
            var valorPropiedad = valor.GetType().GetProperty(nombrePropiedad).GetValue(valor);

            return PersistenceManager.SelectByProperty<T>(nombrePropiedad, valorPropiedad)
                .Where(p => p.Id != valor.Id)
                .Count() == 0;
        }

        public static bool ValorUnicoOVacio<T>(string nombrePropiedad, T valor) where T : PersistenceData, IModelo
        {
            var valorPropiedad = valor.GetType().GetProperty(nombrePropiedad).GetValue(valor);

            if (valorPropiedad == null || valorPropiedad.Equals(""))
                return true;

            return PersistenceManager.SelectByProperty<T>(nombrePropiedad, valorPropiedad)
                .Where(p => p.Id != valor.Id)
                .Count() == 0;
        }

        public static void VisualizeWindow(MahApps.Metro.Controls.MetroWindow window)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = window.Width;
            double windowHeight = window.Height;
            if (screenHeight < windowHeight || screenWidth < windowWidth)
                window.WindowState = WindowState.Maximized;
            else
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        /// <summary> Show a message, if accept close the window </summary>
        /// <remarks> manper </remarks>
        /// <param name="ventana">Window to close</param>
        public static void MensajeCancel(this Window ventana)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Deseas salir sin guardar?.", "Salir", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ventana.DialogResult = false;
                ventana.Close();
            }
        }

        public static void Aceptacion(this Label label, bool aceptado, bool CCI = false)
        {
            if (aceptado)
            {
                label.Content = "OK" + ((CCI == true) ? " CCI" : "");
                label.Background = new SolidColorBrush(Color.FromRgb(26, 148, 49));
            }
            else
            {
                label.Content = "Rechazo" + ((CCI == true) ? " CCI" : "");
                label.Background = new SolidColorBrush(Color.FromRgb(193, 46, 34));
            }
            label.Visibility = Visibility.Visible;
        }
    }
}
