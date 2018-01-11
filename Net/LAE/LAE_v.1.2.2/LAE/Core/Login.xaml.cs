using LAE.CartifService;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;
using Cartif.Extensions;
using LAE.AccesoDatos;

namespace LAE.Core
{
    public partial class Login : MetroWindow
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void buttonLogin_Click(Object sender, RoutedEventArgs e)
        {
            Boolean ok = await RealizarLogin();

            if (ok)
            {
                Mensaje("Correcto. Iniciando aplicación.", Brushes.Green);
                // TODO register the user RealizarLogin deberá devolver un Task<User>
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
        }

        private Task<Boolean> RealizarLogin()
        {
            return new Task<bool>(() =>
            {
                String user = textLogin.Text;
                String pass = textPassword.Password;

                if (user.IsNullOrWhitespace() || pass.IsNullOrWhitespace())
                {
                    Mensaje("Usuario o Contraseña vacío", Brushes.Red);
                    return false;
                }

                Mensaje("Autenticando", Brushes.Black);

                CartifWebService cs = CartifServices.Service;
                if (cs.LoginLDAPZimbra(user, pass))
                {
                    Mensaje("Correcto. Iniciando aplicación.", Brushes.Green);
                }
                else
                {
                    Mensaje("Usuario o Contraseña incorrectos", Brushes.Red);
                }

                return cs.LoginLDAPZimbra(user, pass);
            });
        }

        private void Mensaje(String mensaje, Brush brush)
        {
            labelMensaje.Content = mensaje;
            labelMensaje.Foreground = brush;
            labelMensaje.Visibility = Visibility.Visible;
        }
    }
}
