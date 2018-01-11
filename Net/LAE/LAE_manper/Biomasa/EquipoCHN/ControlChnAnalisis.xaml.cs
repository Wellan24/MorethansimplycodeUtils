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
    /// Lógica de interacción para ControlChnAnalisis.xaml
    /// </summary>
    public partial class ControlChnAnalisis : UserControl
    {

        private Chn chn;
        public Chn Chn
        {
            get { return chn; }
            set
            {
                chn = value;
                PageChn.ParamCHN = Chn;
            }
        }

        public event RoutedEventHandler BackButtonClick
        {
            add { bBack.Click += value; }
            remove { bBack.Click -= value; }
        }

        public ControlChnAnalisis()
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
                    GuardarCHN(conn);
                    trans.Commit();
                    MessageBox.Show("Datos guardados con éxito");
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar los datos de los análisis de CHN", ex);
                    MessageBox.Show("Error al guardar los datos de los análisis de CHN. Por favor, informa a soporte.");
                }
            }
        }

        private void GuardarCHN(NpgsqlConnection conn)
        {
            PersistenceDataManipulation.Guardar(conn, Chn);
            PersistenceDataManipulation.GuardarElement1N(conn, Chn, Chn.Replicas, c => c.Id, "IdCHN");
            PersistenceDataManipulation.Borrar1N(conn, Chn.Replicas, Chn.Id, "IdCHN");
        }
    }
}
