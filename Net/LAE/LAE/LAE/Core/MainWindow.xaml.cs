using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Modelo;
using MahApps.Metro.Controls;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LAE.Core
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && this.WindowState != WindowState.Maximized)
            {
                ReleaseCapture();
                SendMessage(this.CriticalHandle, WM_NCLBUTTONDOWN, new IntPtr(HT_CAPTION), IntPtr.Zero);
            }
        }

        private void mostrarCRM_clicked(object sender, MouseButtonEventArgs e)
        {
            if (crm1.IsVisible)
            {
                crm1.Visibility = Visibility.Collapsed;
                crm2.Visibility = Visibility.Collapsed;
                crm3.Visibility = Visibility.Collapsed;
            }
            else
            {
                crm1.Visibility = Visibility.Visible;
                crm2.Visibility = Visibility.Visible;
                crm3.Visibility = Visibility.Visible;
            }
        }

        private void ScrollViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
