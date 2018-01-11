using Cartif.Logs;
using LAE.Biomasa.Modelo;
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

namespace LAE.Biomasa.Controles
{
    /// <summary>
    /// Lógica de interacción para ControlFusAnalisis.xaml
    /// </summary>
    public partial class ControlFusAnalisis : UserControl
    {
        private Fusibilidad fusibilidad;
        public Fusibilidad Fusibilidad
        {
            get { return fusibilidad; }
            set
            {
                fusibilidad = value;
                PageFusibilidad.Fusibilidad = Fusibilidad;
            }
        }

        public event RoutedEventHandler BackButtonClick
        {
            add { bBack.Click += value; }
            remove { bBack.Click -= value; }
        }

        public ControlFusAnalisis()
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
                    GuardarFusibilidad(conn);
                    trans.Commit();
                    MessageBox.Show("Datos guardados con éxito");
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar los datos de los análisis de Fusibilidad", ex);
                    MessageBox.Show("Error al guardar los datos de los análisis de Fusibilidad. Por favor, informa a soporte.");
                }
            }
        }

        private void GuardarFusibilidad(NpgsqlConnection conn)
        {
            PersistenceDataManipulation.Guardar(conn, Fusibilidad);
            PersistenceDataManipulation.GuardarElement1N(conn, Fusibilidad, Fusibilidad.Replicas, c => c.Id, "IdFusibilidad");
            PersistenceDataManipulation.Borrar1N(conn, Fusibilidad.Replicas, Fusibilidad.Id, "IdFusibilidad");
        }
    }
}
