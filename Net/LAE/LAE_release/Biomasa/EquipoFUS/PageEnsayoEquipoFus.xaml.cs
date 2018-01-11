using Cartif.Logs;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
using Npgsql;
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

namespace LAE.Biomasa.Pages
{
    /// <summary>
    /// Lógica de interacción para PageEnsayoEquipoFus.xaml
    /// </summary>
    public partial class PageEnsayoEquipoFus : UserControl
    {
        private EnsayoPNT ensayo;
        public EnsayoPNT Ensayo
        {
            get { return ensayo; }
            set
            {
                ensayo = value;
                UCEnsayo.Ensayo = Ensayo;
                UCEquipos.GenerarPanel(ensayo);
            }
        }

        private RoutedEventHandler AnalisisGuardado;
        public event RoutedEventHandler VisibilidadAnalisis
        {
            add { AnalisisGuardado += value; }
            remove { AnalisisGuardado -= value; }
        }

        public PageEnsayoEquipoFus()
        {
            InitializeComponent();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
            using (NpgsqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    /*Guardar Ensayo*/
                    PersistenceDataManipulation.Guardar(conn, Ensayo);

                    GuardarEquipos(conn);

                    trans.Commit();
                    MessageBox.Show("Datos guardados con éxito");
                    AnalisisGuardado(sender, e);
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar los datos de Fusibilidad", ex);
                    MessageBox.Show("Error al guardar los datos de Fuisibilidad. Por favor, informa a soporte.");
                }
            }
        }

        private void GuardarEquipos(NpgsqlConnection conn)
        {
            List<EquipoEnsayo> equiposFus = UCEquipos.GetEquipos();
            PersistenceDataManipulation.Guardar(conn, equiposFus, Ensayo.Id, "IdEnsayo");
        }
    }
}
