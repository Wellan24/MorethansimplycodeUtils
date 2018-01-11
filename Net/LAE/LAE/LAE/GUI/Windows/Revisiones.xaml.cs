using LAE.Modelo;
using MahApps.Metro.Controls;
using Persistence;
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

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para Revisiones.xaml
    /// </summary>
    public partial class Revisiones : MetroWindow
    {
        public Revisiones()
        {
            InitializeComponent();
        }

        public Revisiones(Tecnico[] tecnicos, RevisionOferta revision)
        {
            InitializeComponent();
            UCRevision.Tecnicos = tecnicos;
            UCRevision.Revision = revision;
        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (UCRevision.ValidarRevision())
            {
                RevisionOferta rev = UCRevision.Revision;
                GuardarRevision(rev);
                GuardarTipoMuestra(rev);
                GuardarParametros(rev);
                MessageBox.Show("Datos guardados con exito");
                DialogResult = true;
                this.Close();
            }
        }

        private void GuardarRevision(RevisionOferta rev)
        {
            if (rev.Id == 0)
            {
                int idRevision = rev.Insert();
                rev.Id = idRevision;
            }
            else
                rev.Update();
        }

        private void GuardarTipoMuestra(RevisionOferta rev)
        {
            List<TipoMuestraRevision> lineas = PersistenceManager<TipoMuestraRevision>.SelectByProperty("IdRevision", rev.Id).ToList();

            foreach (ITipoMuestra item in UCRevision.lineasTipoMuestra)
            {
                TipoMuestraRevision tmr = new TipoMuestraRevision();
                tmr.IdTipoMuestra = item.IdTipoMuestra;

                if (item.Id == 0)
                {
                    /* inserto nuevos */
                    tmr.IdRevision = rev.Id;
                    tmr.Insert();
                }
                else
                {
                    /* actualizo existentes */
                    tmr.Id = item.Id;
                    tmr.IdRevision = item.IdRelacion;
                    //tmp.Update();

                    lineas.Remove(tmr);
                }
            }
            foreach (TipoMuestraRevision item in lineas)
            {
                /* elimino borrados */
                item.Delete();
            }
        }

        private void GuardarParametros(RevisionOferta rev)
        {
            List<LineasRevisionOferta> lineas = PersistenceManager<LineasRevisionOferta>.SelectByProperty("IdRevisionOferta", rev.Id).ToList();

            foreach (ILineasParametros item in UCRevision.lineasParametros)
            {
                LineasRevisionOferta lr = new LineasRevisionOferta();
                lr.Cantidad = item.Cantidad;
                lr.Metodo = item.Metodo;
                lr.IdParametro = item.IdParametro;

                if (item.Id == 0)
                {
                    /* inserto nuevos */
                    lr.IdRevisionOferta = rev.Id;
                    lr.Insert();
                }
                else
                {
                    /* actualizo existentes */
                    lr.IdRevisionOferta = rev.Id;
                    lr.Id = item.Id;
                    lr.Update();

                    lineas.Remove(lr);
                }
            }
            foreach (LineasRevisionOferta item in lineas)
            {
                /* elimino borrados */
                item.Delete();
            }
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
