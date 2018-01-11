
using Cartif.XamlClasses;
using LAE.Modelo;
using LAE.Comun.Persistence;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
using Cartif.Extensions;
using Cartif.Util;
using GenericForms;
using GenericForms.Abstract;
using GenericForms.Settings;
using GenericForms.Implemented;
using MahApps.Metro.Controls;
using Npgsql;
using LAE.Comun.Modelo;

namespace LAE.Test
{
    /// <summary>
    /// Lógica de interacción para test.xaml
    /// </summary>
    public partial class test : MetroWindow
    {
        public test()
        {
            InitializeComponent();

            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
            {
                NpgsqlTransaction trans = null;
                try
                {
                    trans = conn.BeginTransaction();
                    var a = PersistenceManager.SelectAll<Tecnico>(conn).ToArray();

                    var b = new Tecnico() { Id = 20, Nombre = "Paco" };
                    b.Insert(conn, false);

                    b.PrimerApellido = "1";
                    b.Update(conn);

                    var c = new Tecnico() { Id = 20 };
                    c.Load(conn);

                    throw new ArgumentException("");

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                }
            }
        }

        private void button_Click(Object sender, RoutedEventArgs e)
        {

        }
    }

}
