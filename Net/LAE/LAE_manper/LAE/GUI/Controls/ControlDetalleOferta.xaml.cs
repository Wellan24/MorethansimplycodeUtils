using Cartif.Extensions;
using Cartif.Logs;
using Cartif.Util;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Settings;
using GUI.Windows;
using LAE.DocWord;
using LAE.Modelo;
using Npgsql;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using LAE.Comun.Documentacion;
using LAE.Comun.Modelo;
using GenericForms.Implemented;

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlDetalleOferta.xaml
    /// </summary>
    public partial class ControlDetalleOferta : UserControl
    {

        private Peticion Peticion;

        private Oferta oferta;
        public Oferta Oferta
        {
            get { return oferta; }
            set
            {
                oferta = value;
                Peticion = PersistenceManager.SelectByProperty<Peticion>("IdOferta", Oferta.Id).FirstOrDefault();
                if (Peticion == null)
                {
                    Peticion = new Peticion() { IdOferta = Oferta.Id };
                    bPeticion.Content = "Añadir";
                }
                else
                {
                    bPeticion.Content = "Editar";
                }

                /* limpiar */
                panelPeticiones.ClearGrid();

                /* cargar */
                GenerarPanelPeticion();
                CargarDatosGridRevisiones();
                CargarDatosGridTrabajos();

                CambiarEstadoAnulada();
            }
        }

        private Cliente[] Clientes;
        private Contacto[] Contactos;
        private Tecnico[] Tecnicos;


        private FlexObservableCollection listaRevisiones;
        public List<RevisionOferta> ListaRevisiones
        {
            get { return listaRevisiones.GetSource<RevisionOferta>(); }
            set { listaRevisiones.Fill(value); }
        }

        private List<Trabajo> ListaTrabajos;

        public event RoutedEventHandler BackButtonClick
        {
            add { bBack.Click += value; }
            remove { bBack.Click -= value; }
        }

        public ControlDetalleOferta()
        {
            InitializeComponent();
            //ListaRevisiones = new List<RevisionOferta>();
            //ListaTrabajos = new List<Trabajo>();
            GenerarGridRevisiones();
            GenerarGridTrabajos();
        }

        public void CargarDatosIniciales(Cliente[] c, Contacto[] co, Tecnico[] te)
        {
            Clientes = c;
            Contactos = co;
            Tecnicos = te;
        }

        private void GenerarGridRevisiones()
        {
            gridRevisiones.Build<RevisionOferta>(
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["Num"] = TypeGridColumnSettingsEnum.DefaultColum
                                        .SetWidth(1)
                                        .SetLabel("Núm."),
                        ["Importe"] = TypeGridColumnSettingsEnum.DefaultColum
                                        .SetWidth(2),
                        ["FechaEmision"] = TypeGridColumnSettingsEnum.DefaultColum
                                        .SetWidth(2)
                                        .SetLabel("Fecha emisión")
                                        .SetFormat("dd/MM/yy HH:mm"),
                        ["Enviada"] = new TypeGridColumnSettings
                        {
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[] {
                                    ComboBoxItem<Boolean>.Create("No", false),
                                    ComboBoxItem<Boolean>.Create("Si", true)
                                },
                                Path = "Id"
                            }
                        }.SetWidth(1),
                        ["Aceptada"] = new TypeGridColumnSettings
                        {
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[] {
                                    ComboBoxItem<Boolean>.Create("No", false),
                                    ComboBoxItem<Boolean>.Create("Si", true)
                                },
                                Path = "Id"
                            }
                        }.SetWidth(1),
                        ["Enviar"] = new TypeGridColumnSettings
                        {
                            Label = "Enviar",
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.SendIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    RevisionOferta revisionEnviada = gridRevisiones.SelectedItem as RevisionOferta;
                                    if (!revisionEnviada.Enviada)
                                    {
                                        MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que deseas enviar la revisión? Una vez enviada ya no se podrá modificar", "Enviar revisión", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                                        if (messageBoxResult == MessageBoxResult.Yes)
                                        {
                                            revisionEnviada.Enviada = true;
                                            revisionEnviada.Update();
                                            gridRevisiones.InnerSource.UpdateAt(gridRevisiones.SelectedIndex, revisionEnviada);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Revisión enviada con anterioridad");
                                    }
                                },
                            }
                        }
                        .SetWidth(1),
                        ["Editar"] = new TypeGridColumnSettings
                        {
                            Label = "Editar/Ver",
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.EditIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    //RevisionOferta ro = gridRevisiones.SelectedItem.Clone(typeof(RevisionOferta)) as RevisionOferta;
                                    RevisionOferta ro = gridRevisiones.SelectedItem as RevisionOferta;
                                    Revisiones ventanaRevisiones = new Revisiones(Tecnicos, ro) { Owner = Window.GetWindow(this) };
                                    ventanaRevisiones.ShowDialog();

                                    if (ventanaRevisiones.DialogResult ?? true)
                                        gridRevisiones.UpdateSelectedItem(ventanaRevisiones.UCRevision.Revision);

                                },
                            }
                        }
                        .SetWidth(1),
                        ["Borrar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.RemoveIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    RevisionOferta revisionBorrada = gridRevisiones.SelectedItem as RevisionOferta;
                                    if (!revisionBorrada.Enviada && !revisionBorrada.Aceptada)
                                    {
                                        MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que desea borrar la revisión?. Una vez eliminada, sus datos desaparecerán definitivamente", "Borrar", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                                        if (messageBoxResult == MessageBoxResult.Yes)
                                        {
                                            /*1º quitar de la lista (hacer antes de modificarlo)*/
                                            gridRevisiones.InnerSource.Remove(revisionBorrada);
                                            /*2º eliminar acceso */
                                            revisionBorrada.IdOferta = null;
                                            revisionBorrada.Update();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Imposible eliminar una revisión enviada al cliente o aceptada por este");
                                    }
                                },
                            }
                        }
                        .SetWidth(1),
                        ["Aceptar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.Accept,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    RevisionOferta revisionSelected = gridRevisiones.SelectedItem as RevisionOferta;
                                    if (!revisionSelected.Aceptada)
                                    {
                                        RevisionOferta revisionAceptadaAnterior = ListaRevisiones.Where(r => r.Id != revisionSelected.Id && r.Aceptada == true).FirstOrDefault();
                                        if (revisionAceptadaAnterior != null && ExistenTrabajos(Oferta))
                                            MessageBox.Show("Lo sentimos pero no se puede aceptar la revisión. Ya existe otra revisión aceptada que contiene trabajos");
                                        else
                                            CambiarSolicitudAceptada(revisionSelected, revisionAceptadaAnterior);
                                    }

                                },
                            }
                        }
                        .SetLabel("Acept. solicitud")
                        .SetWidth(1),
                        ["Imprimir"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.Printer,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    RevisionOferta revisionSelected = gridRevisiones.SelectedItem as RevisionOferta;
                                    new Escritor(Escritor.DocumentoOferta, new DocOfertas(revisionSelected));
                                },
                            }
                        }
                        .SetLabel("Imp. oferta")
                        .SetWidth(1),
                    }
                });
        }

        private void GenerarGridTrabajos()
        {
            gridTrabajos.Build<Trabajo>(
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["Codigo"] = TypeGridColumnSettingsEnum.DefaultColum,
                        ["FirmadoCliente"] = new TypeGridColumnSettings
                        {
                            Label = "Firmado por Cliente",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[] {
                                    ComboBoxItem<Boolean>.Create("No", false),
                                    ComboBoxItem<Boolean>.Create("Si", true)
                                },
                                Path = "Id"
                            }
                        },
                        ["FirmadoLae"] = new TypeGridColumnSettings
                        {
                            Label = "Firmado por LAE",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[] {
                                    ComboBoxItem<Boolean>.Create("No", false),
                                    ComboBoxItem<Boolean>.Create("Si", true)
                                },
                                Path = "Id"
                            }
                        },
                        ["FechaFirmaCliente"] = TypeGridColumnSettingsEnum.DefaultColum
                                        .SetLabel("Fecha firma cliente")
                                        .SetFormat("dd/MM/yy HH:mm"),
                        ["FechaFirmaLae"] = TypeGridColumnSettingsEnum.DefaultColum
                                        .SetLabel("Fecha firma LAE")
                                        .SetFormat("dd/MM/yy HH:mm"),
                        ["Editar"] = new TypeGridColumnSettings
                        {
                            Label = "Editar/Ver",
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.EditIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    Trabajo tr = gridTrabajos.SelectedItem as Trabajo;
                                    RevisionOferta revisionAceptada = ListaRevisiones.Where(r => r.Aceptada == true).FirstOrDefault();
                                    Trabajos ventanaTrabajos = new Trabajos(Tecnicos, revisionAceptada, Oferta) { Trabajo = tr };
                                    ventanaTrabajos.ShowDialog();
                                    if (ventanaTrabajos.DialogResult == true)
                                        gridTrabajos.UpdateSelectedItem(ventanaTrabajos.Trabajo);
                                    //CargarDatosGridTrabajos();

                                },
                            }
                        }
                        .SetWidth(1),
                        ["Borrar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.RemoveIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    DeleteTrabajo();
                                },
                            }
                        }
                        .SetWidth(1),
                        ["Trabajos"] = new TypeGridColumnSettings
                        {
                            Label = "Muestras",
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.TestTubeIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    Trabajo trabajo = gridTrabajos.SelectedItem as Trabajo;

                                    GestionMuestras ventanaGestMuestras = new GestionMuestras(Oferta, trabajo);
                                    ventanaGestMuestras.Owner = Window.GetWindow(this);
                                    ventanaGestMuestras.ShowDialog();
                                },
                            }
                        }
                        .SetWidth(1),
                        ["Imprimir"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.Printer,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    Trabajo trabajoSelected = gridTrabajos.SelectedItem as Trabajo;
                                    new Escritor(Escritor.DocumentoSolicitudEnsayo, new DocSolicitud(trabajoSelected));
                                }
                            }
                        }
                        .SetLabel("Imp. solicitud")
                        .SetWidth(1),
                    }
                });
        }

        private void CambiarEstadoAnulada()
        {
            bool anulada = Oferta?.Anulada ?? false;
            /* botones */
            bPeticion.IsEnabled = !anulada;
            bBorrarPeticion.IsEnabled = !anulada;
            bNuevoTrabajo.IsEnabled = !anulada;

            /* panel y grid */
            gridRevisiones.IsEnabled = !anulada;
            gridTrabajos.IsEnabled = !anulada;
        }

        private void GenerarPanelPeticion()
        {
            panelPeticiones.Build(Peticion,
               new TypePanelSettings<Peticion>
               {
                   Fields = new FieldSettings
                   {
                       ["Fecha"] = PropertyControlSettingsEnum.DateTimeDefault,
                       ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Clientes)
                                .SetLabel("Clientes"),
                       ["IdContacto"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Contactos)
                                .SetLabel("Contacto"),
                       ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Tecnicos)
                                .SetLabel("Técnico"),
                   },
                   IsUpdating = true,
                   DefaultSettings = new PropertyControlSettings
                   {
                       ReadOnly = true,
                   }
               });
        }

        private void CargarDatosGridRevisiones()
        {
            ListaRevisiones = PersistenceManager.SelectByProperty<RevisionOferta>("IdOferta", Oferta.Id).OrderByDescending(r => r.Id).ToList();
            gridRevisiones.FillDataGrid(ListaRevisiones);
        }

        private void CambiarSolicitudAceptada(RevisionOferta aceptada, RevisionOferta desaceptada = null)
        {
            using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
            using (NpgsqlTransaction trans = conn.BeginTransaction())
            {

                try
                {
                    aceptada.Aceptada = true;
                    aceptada.Update(conn);
                    if (desaceptada != null)
                    {
                        desaceptada.Aceptada = false;
                        desaceptada.Update(conn);
                    }
                    trans.Commit();

                    CargarDatosGridRevisiones();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();

                    CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al aceptar la solicitud", ex);
                    MessageBox.Show("Se ha producido un error al intentar guardar la aceptación. Por favor, recargue la página o informa a soporte.");

                }
            }
        }

        private bool ExistenTrabajos(Oferta oferta)
        {
            int trabajos = PersistenceManager.SelectByProperty<Trabajo>("IdOferta", oferta.Id).Count();
            return trabajos != 0;
        }

        private void CargarDatosGridTrabajos()
        {
            ListaTrabajos = PersistenceManager.SelectByProperty<Trabajo>("IdOferta", Oferta.Id).OrderByDescending(t => t.Codigo).ToList();
            gridTrabajos.FillDataGrid(ListaTrabajos);
        }

        private void DeleteTrabajo()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que deseas eliminar el trabajo? Una vez eliminado, sus datos desaparecerán definitavamente", "Borrar trabajo", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    Trabajo trabajoBorrar = gridTrabajos.SelectedItem as Trabajo;

                    if (!trabajoBorrar.Delete())
                        MessageBox.Show("No se puede borrar el trabajo, inténtelo de nuevo o informe a soporte");
                    else
                    {
                        gridTrabajos.InnerSource.Remove(trabajoBorrar);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("No se puede borrar la oferta porque tiene 'Toma de Muestra' y/o 'Recepción e Inspección de muestra'");
                }
            }
        }

        private void EditarPeticion_Click(object sender, RoutedEventArgs e)
        {
            Peticiones ventanaPeticiones;
            if (Peticion.Id != 0)
                ventanaPeticiones = new Peticiones(Clientes, Tecnicos, Peticion.Clone(typeof(Peticion)) as Peticion);
            else
                ventanaPeticiones = new Peticiones(Clientes, Tecnicos, Peticion.Clone(typeof(Peticion)) as Peticion, Oferta);

            ventanaPeticiones.Owner = Window.GetWindow(this);
            ventanaPeticiones.ShowDialog();

            if (ventanaPeticiones.DialogResult ?? true)
            {
                panelPeticiones.InnerValue = PersistenceManager.SelectByProperty<Peticion>("IdOferta", Oferta.Id).FirstOrDefault();

            }

        }

        private void BorrarPeticion_Click(object sender, RoutedEventArgs e)
        {
            if (Peticion.Id == 0)
                MessageBox.Show("Imposible eliminar una petición que no existe");
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que deseas borrar la petición? Una vez eliminada, sus datos desaparecerán definitivamente.", "Borrar petición", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Peticion.IdOferta = null;
                    Peticion.Update();

                    Peticion = new Peticion();
                    panelPeticiones.InnerValue = Peticion;
                    MessageBox.Show("Petición borrada con éxito");
                }
            }
        }

        private void NuevaRevision_Click(object sender, RoutedEventArgs e)
        {
            RevisionOferta revisionNueva;
            List<ITipoMuestra> lineasTipoMuestra = new List<ITipoMuestra>();

            RevisionOferta revisionUltima = PersistenceManager.SelectByProperty<RevisionOferta>("IdOferta", Oferta.Id).OrderByDescending(r => r.Id).FirstOrDefault();
            if (revisionUltima != null)
            {
                revisionNueva = CargarRevision(revisionUltima);
                lineasTipoMuestra = CargarTipoMuestra(revisionUltima);
            }
            else if (Peticion.Id != 0)
            {
                revisionNueva = CargarRevision(Peticion, Oferta);
                lineasTipoMuestra = CargarTipoMuestra(Peticion);
            }
            else
                revisionNueva = CargarRevision(Oferta);

            revisionNueva.Num++;
            revisionNueva.FechaEmision = DateTime.Now;

            Revisiones ventanaRevision = new Revisiones(Tecnicos, revisionNueva);
            ventanaRevision.UCRevision.CargarTipoMuestra(lineasTipoMuestra);
            if (revisionUltima != null)
            {
                ventanaRevision.UCRevision.CargarParametros(revisionUltima);
            }
            else if (Peticion.Id != 0)
            {
                ventanaRevision.UCRevision.CargarParametros(Peticion);
            }


            ventanaRevision.Owner = Window.GetWindow(this);
            ventanaRevision.ShowDialog();
            if (ventanaRevision.DialogResult == true)
                gridRevisiones.InnerSource.Add(ventanaRevision.UCRevision.Revision);
        }

        private RevisionOferta CargarRevision(RevisionOferta revisionUltima)
        {
            RevisionOferta r = revisionUltima.Clone(typeof(RevisionOferta)) as RevisionOferta;
            r.Id = 0;
            r.Enviada = false;
            r.Aceptada = false;
            return r;
        }

        private RevisionOferta CargarRevision(Peticion p, Oferta o)
        {
            RevisionOferta r = new RevisionOferta
            {
                FechaEmision = p.Fecha,
                Frecuencia = p.Frecuencia,
                LugarMuestra = p.LugarMuestra,
                NumPuntosMuestreo = p.NumPuntosMuestreo,
                PlazoRealizacion = p.PlazoRealizacion,
                RequiereTomaMuestra = p.RequiereTomaMuestra,
                TrabajoPuntual = p.TrabajoPuntual,

                IdTecnico = o.IdTecnico,
                IdOferta = o.Id
            };
            return r;
        }

        private RevisionOferta CargarRevision(Oferta o)
        {
            RevisionOferta r = new RevisionOferta
            {
                IdTecnico = o.IdTecnico,
                IdOferta = o.Id
            };
            return r;
        }

        private List<ITipoMuestra> CargarTipoMuestra(RevisionOferta revisionUltima)
        {
            var tiposRevisionUltima = PersistenceManager.SelectByProperty<TipoMuestraRevision>("IdRevision", revisionUltima.Id);

            return tiposRevisionUltima.Map(t =>
                new ITipoMuestra { IdRelacion = t.IdRevision, IdTipoMuestra = t.IdTipoMuestra }
            ).ToList();
        }

        private List<ITipoMuestra> CargarTipoMuestra(Peticion peticion)
        {
            var tiposPeticion = PersistenceManager.SelectByProperty<TipoMuestraPeticion>("IdPeticion", peticion.Id);

            return tiposPeticion.Map(t =>
               new ITipoMuestra { IdRelacion = t.IdPeticion, IdTipoMuestra = t.IdTipoMuestra }
            ).ToList();
        }

        private void NuevoTrabajo_Click(object sender, RoutedEventArgs e)
        {
            RevisionOferta revisionAceptada = ListaRevisiones.Where(r => r.Aceptada).FirstOrDefault();
            if (revisionAceptada != default(RevisionOferta))
            {
                Trabajos ventanaTrabajos = new Trabajos(Tecnicos, revisionAceptada, Oferta) { Trabajo = new Trabajo() { Codigo = ListaTrabajos.Max(t => t.Codigo + 1) } };
                ventanaTrabajos.ShowDialog();
                if (ventanaTrabajos.DialogResult == true)
                    gridTrabajos.InnerSource.Add(ventanaTrabajos.Trabajo);
            }
            else
            {
                MessageBox.Show("Para crear un trabajo debe existir alguna revisión aceptada");
            }
        }

        /* Botón eliminado para Imprimir solicitud antes de generar trabajo -> eliminado pues le suponia bastante confusión */

        private void ImprimirSolicitud_Click(object sender, RoutedEventArgs e)
        {
            RevisionOferta revisionAceptada = ListaRevisiones.Where(r => r.Aceptada).FirstOrDefault();
            if (revisionAceptada != default(RevisionOferta))
            {

                int numCodigo = 0;
                if (ListaTrabajos != null && ListaTrabajos.Count > 0)
                    numCodigo = ListaTrabajos.Max(t => t.Codigo);

                Trabajo trabajo = new Trabajo()
                {
                    IdOferta = Oferta.Id,
                    Codigo = numCodigo + 1
                };
                new Escritor(Escritor.DocumentoSolicitudEnsayo, new DocSolicitud(trabajo));
            }
            else
            {
                MessageBox.Show("Para imprimir una solicitud debe existir alguna revisión aceptada");
            }
        }

    }
}
