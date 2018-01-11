using Cartif.Extensions;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Modelo;
using GUI.Wizards;
using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using GUI.Windows;
using Cartif.Util;
using Npgsql;
using Dapper;
using Cartif.Logs;
using LAE.DocWord;
using GenericForms.Implemented;

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para Oferta.xaml
    /// </summary>
    public partial class Ofertas : UserControl, INotifyPropertyChanged
    {
        private bool cargar = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public Oferta[] ListaOfertas;
        public Oferta SelectedOferta;
        public Peticion SelectedPeticion;
        public RevisionOferta[] SelectedRevisiones;

        public Cliente[] Clientes;
        public Contacto[] Contactos;
        public Tecnico[] Tecnicos;

        public Object selectedValue;
        public Object SelectedValue
        {
            get { return selectedValue; }
            set
            {
                selectedValue = value;
                if (SelectedValue != null)
                {
                    /* fill oferta */
                    SelectedOferta = SelectedValue as Oferta;
                    panelOfertas.InnerValue = SelectedOferta;

                    /* fill peticion */
                    SelectedPeticion = PersistenceManager.SelectByProperty<Peticion>("IdOferta", SelectedOferta.Id).FirstOrDefault();
                    if (SelectedPeticion == null)
                    {
                        SelectedPeticion = new Peticion() { IdOferta = SelectedOferta.Id };
                        bPeticion.Content = "Añadir";
                    }
                    else
                        bPeticion.Content = "Editar";
                    panelPeticiones.InnerValue = SelectedPeticion;

                    /* fill revision */
                    SelectedRevisiones = PersistenceManager.SelectByProperty<RevisionOferta>("IdOferta", SelectedOferta.Id).OrderByDescending(r => r.Id).ToArray();
                    gridRevisiones.FillDataGrid(SelectedRevisiones);

                    CambiarEstadoAnulada();

                }
                OnPropertyChanged("SelectedValue");
            }
        }

        private void CambiarEstadoAnulada()
        {
            bool anulada = (SelectedValue as Oferta)?.Anulada ?? false;
            /* botones */
            bAnularOferta.IsEnabled = !anulada;
            bGuardarOferta.IsEnabled = !anulada;
            bPeticion.IsEnabled = !anulada;
            bBorrarPeticion.IsEnabled = !anulada;
            bNuevaRevision.IsEnabled = !anulada;

            /* panel y grid */
            panelOfertas.IsEnabled = !anulada;
            gridRevisiones.IsEnabled = !anulada;

            /* aviso */
            if (anulada)
                AvisoOfertaAnulada.Visibility = Visibility.Visible;
            else
                AvisoOfertaAnulada.Visibility = Visibility.Hidden;
        }

        public Ofertas()
        {
            InitializeComponent();
        }

        private void pageOfertas_Loaded(object sender, RoutedEventArgs e)
        {
            if (cargar)
            {

                SelectedPeticion = new Peticion();
                SelectedOferta = new Oferta();
                SelectedRevisiones = new RevisionOferta[] { new RevisionOferta() };
                ListaOfertas = new Oferta[] { new Oferta() };

                /* limpiar */
                panelOfertas.ClearGrid();
                panelPeticiones.ClearGrid();


                /* cargar */
                CargarDatos();
                CargarGridOfertas();
                GenerarPaneles();
                GenerarGridRevisiones();

                CambiarEstadoAnulada();
            }
            else
                cargar = true;
        }

        private void CargarDatos()
        {
            Clientes = PersistenceManager.SelectAll<Cliente>()
                .OrderByDescending(c => c.Nombre).ToArray();
            Contactos = PersistenceManager.SelectAll<Contacto>()
                .OrderByDescending(c => c.Nombre).ThenBy(c => c.Apellidos).ToArray();
            Tecnicos = PersistenceManager.SelectAll<Tecnico>()
                .OrderByDescending(c => c.Nombre).ThenBy(c => c.PrimerApellido).ThenBy(c => c.SegundoApellido).ToArray();
        }

        private void CargarGridOfertas()
        {
            gridOfertas.Build(ListaOfertas, new TypeGridSettings()
            {
                Columns = new ColumnGridSettings
                {
                    ["Codigo"] = TypeGridColumnSettingsEnum.DefaultColum
                        .SetLabel("Código oferta"),
                    ["AnnoOferta"] = TypeGridColumnSettingsEnum.DefaultColum
                        .SetLabel("Año oferta"),
                    ["IdCliente"] = new TypeGridColumnSettings
                    {
                        Label = "Empresa",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = Clientes,
                            Path = "Id"
                        }
                    },
                    ["IdContacto"] = new TypeGridColumnSettings
                    {
                        Label = "Contacto",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = Contactos,
                            Path = "Id"
                        }
                    },
                    ["IdTecnico"] = new TypeGridColumnSettings
                    {
                        Label = "Técnico",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = Tecnicos,
                            Path = "Id"
                        }
                    },
                },
                LoadGrid = (o) =>
                {
                    Oferta oferta = o as Oferta;
                    if (oferta != null)
                        return oferta.Anulada;
                    return false;
                }

            });


            ListaOfertas = PersistenceManager.SelectAll<Oferta>()
                .OrderByDescending(c => c.AnnoOferta)
                .ThenByDescending(c => c.NumCodigoOferta)
                .ToArray();
            gridOfertas.FillDataGrid(ListaOfertas);
        }

        private void GenerarPaneles()
        {
            panelOfertas.Build<Oferta>(SelectedOferta,
                new TypePanelSettings<Oferta>
                {
                    Fields = new FieldSettings
                    {
                        ["Codigo"] = PropertyControlSettingsEnum.TextBoxDefault
                                .SetLabel("Código oferta")
                                .SetEnabled(false),
                        ["AnnoOferta"] = PropertyControlSettingsEnum.TextBoxDefault
                                .SetLabel("* Año oferta")
                                .SetEnabled(false),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(Tecnicos)
                                .SetLabel("* Técnico"),
                        ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(Clientes)
                                .SetLabel("* Empresa")
                                .AddSelectionChanged(RefreshIdContacto),
                        ["IdContacto"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(RecuperarContactos())
                                .SetLabel("* Contacto"),
                    },
                });

            panelPeticiones.Build<Peticion>(SelectedPeticion,
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

        private void GenerarGridRevisiones()
        {
            gridRevisiones.Build(SelectedRevisiones,
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
                                Size = 30,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    RevisionOferta revisionEnviada = ((FrameworkElement)sender).DataContext as RevisionOferta;
                                    if (!revisionEnviada.Enviada)
                                    {
                                        MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que deseas enviar la revisión? Una vez enviada ya no se podrá modificar", "Enviar revisión", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                                        if (messageBoxResult == MessageBoxResult.Yes)
                                        {
                                            revisionEnviada.Enviada = true;
                                            revisionEnviada.Update();
                                            this.SelectedValue = SelectedValue;
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
                                Size = 30,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    RevisionOferta ro = gridRevisiones.SelectedItem.Clone(typeof(RevisionOferta)) as RevisionOferta;
                                    Revisiones ventanaRevisiones = new Revisiones(Tecnicos, ro) { Owner = Window.GetWindow(this) };
                                    ventanaRevisiones.ShowDialog();

                                    if (ventanaRevisiones.DialogResult ?? true)
                                        this.SelectedValue = SelectedValue;
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
                                Size = 30,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    RevisionOferta revisionBorrada = ((FrameworkElement)sender).DataContext as RevisionOferta;
                                    if (!revisionBorrada.Enviada)
                                    {
                                        MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que desea borrar la revisión?. Una vez eliminada, sus datos desaparecerán definitivamente", "Borrar", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                                        if (messageBoxResult == MessageBoxResult.Yes)
                                        {

                                            gridRevisiones.GetItemSource().Remove(revisionBorrada);

                                            /* borrar del todo */
                                            //BorrarLineasOferta(revisionBorrada.Id);
                                            //revisionBorrada.Delete();

                                            /* eliminar acceso */
                                            revisionBorrada.IdOferta = null;
                                            revisionBorrada.Update();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Imposible eliminar una revisión enviada al cliente");
                                    }
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
                                Size = 30,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    /* Elegir carpeta */
                                    System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                                    if (result == System.Windows.Forms.DialogResult.OK)
                                    {
                                        String ruta = dialog.SelectedPath;
                                        RevisionOferta revisionSelected = ((FrameworkElement)sender).DataContext as RevisionOferta;
                                        new Escritor(Escritor.DocumentoSolicitudEnsayo, ruta, new DocSolicitud(revisionSelected));
                                    }


                                },
                            }
                        }
                        .SetLabel("Imp. solicitud")
                        .SetWidth(1),
                        ["Aceptar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.Accept,
                                Color = Colors.White,
                                Size = 30,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    RevisionOferta revisionSelected = ((FrameworkElement)sender).DataContext as RevisionOferta;

                                    SolicitudesAceptacion ventanaSolicitud = new SolicitudesAceptacion(Tecnicos, revisionSelected) { Owner = Window.GetWindow(this) };
                                    ventanaSolicitud.ShowDialog();
                                    if (ventanaSolicitud.DialogResult == true)
                                        this.SelectedValue = SelectedValue;

                                },
                            }
                        }
                        .SetLabel("Acept. solicitud")
                        .SetWidth(1),
                        ["Imprimir2"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.Printer,
                                Color = Colors.White,
                                Size = 30,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    /* Elegir carpeta */
                                    System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                                    if (result == System.Windows.Forms.DialogResult.OK)
                                    {
                                        String ruta = dialog.SelectedPath;
                                        RevisionOferta revisionSelected = ((FrameworkElement)sender).DataContext as RevisionOferta;
                                        new Escritor(Escritor.DocumentoOferta, ruta, new DocOfertas(revisionSelected));
                                    }


                                },
                            }
                        }
                        .SetLabel("Imp. oferta")
                        .SetWidth(1),
                    }
                });
            gridRevisiones.FillDataGrid(new RevisionOferta[0]);
        }

        private void GuardarOferta_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                if (panelOfertas.GetValidatedInnerValue<Oferta>() != default(Oferta))
                {
                    /* update oferta */
                    Oferta ofertaActualizada = panelOfertas.InnerValue as Oferta;
                    ofertaActualizada.Update();

                    /* update grid */
                    int indice = Array.FindIndex(ListaOfertas, o => o.Id == ofertaActualizada.Id);
                    ListaOfertas[indice] = ofertaActualizada?.Clone(typeof(Oferta)) as Oferta;
                    gridOfertas.FillDataGrid(ListaOfertas);
                    gridOfertas.dataGrid.SelectedIndex = indice;

                    MessageBox.Show("Oferta actualizada con éxito");

                }
                else
                    MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
            else
                MessageBox.Show("Seleccione una oferta");
        }

        private void AnularOferta_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                if (!RevisionEnviada())
                {

                    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que deseas anular la oferta? Una vez anulada ya no se podrá editar", "Anular oferta", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        SelectedOferta.Anulada = true;
                        SelectedOferta.Update();
                        ReloadData();
                        MessageBox.Show("Oferta anulada con éxito");
                    }
                }
                else
                {
                    MessageBox.Show("Imposible anular una oferta que contiene revisiones enviadas al cliente");
                }
            }
            else
                MessageBox.Show("Seleccione la oferta que deseas eliminar");
        }

        private bool RevisionEnviada()
        {
            StringBuilder consulta = new StringBuilder(@"SELECT EXISTS(
                                                            SELECT id_revisionoferta IdRevision 
                                                            FROM revisiones_oferta
                                                            INNER JOIN ofertas ON id_oferta=idoferta_revisionoferta
                                                            WHERE enviada_revisionoferta=true");
            consulta.AppendFormat(" AND id_oferta={0})", SelectedOferta.Id);
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                    return conn.Query<bool>(consulta.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "La query: " + consulta, ex);
                MessageBox.Show("Error al comprobar si existen revisiones enviadas");
                return true;
            }

        }

        private void BorrarPeticion_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                if (SelectedPeticion.Id == 0)
                    MessageBox.Show("Imposible eliminar una petición que no existe");
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que deseas borrar la petición? Una vez eliminada, sus datos desaparecerán definitivamente.", "Borrar petición", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        SelectedPeticion.IdOferta = null;
                        SelectedPeticion.Update();

                        SelectedPeticion = new Peticion();
                        panelPeticiones.InnerValue = SelectedPeticion;
                        MessageBox.Show("Petición borrada con éxito");
                    }
                }
            }
            else
                MessageBox.Show("Para eliminar una petición debes seleccionar su oferta asociada");
        }

        private Contacto[] RecuperarContactos()
        {
            Oferta o = panelOfertas.InnerValue as Oferta;
            if (o != null)
                return PersistenceManager.SelectByProperty<Contacto>("IdCliente", o.IdCliente).ToArray();

            return new Contacto[0];
        }

        private void RefreshIdContacto(object sender, SelectionChangedEventArgs e)
        {
            panelOfertas["IdContacto"].InnerValues = RecuperarContactos();
        }

        private void ReloadData()
        {
            SelectedPeticion = new Peticion();
            panelPeticiones.InnerValue = SelectedPeticion;
            SelectedOferta = new Oferta();
            panelOfertas.InnerValue = new Oferta();
            ListaOfertas = PersistenceManager.SelectAll<Oferta>().OrderByDescending(c => c.AnnoOferta).ThenByDescending(c => c.NumCodigoOferta).ToArray();
            gridOfertas.FillDataGrid(ListaOfertas);

            SelectedRevisiones = new RevisionOferta[0];
            gridRevisiones.FillDataGrid(SelectedRevisiones);


        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private void EditarPeticion_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                Peticiones ventanaPeticiones;
                if (SelectedPeticion.Id != 0)
                    ventanaPeticiones = new Peticiones(Clientes, Tecnicos, SelectedPeticion.Clone(typeof(Peticion)) as Peticion);
                else
                    ventanaPeticiones = new Peticiones(Clientes, Tecnicos, SelectedPeticion.Clone(typeof(Peticion)) as Peticion, SelectedOferta);

                ventanaPeticiones.Owner = Window.GetWindow(this);
                ventanaPeticiones.ShowDialog();

                if (ventanaPeticiones.DialogResult ?? true)
                    this.SelectedValue = SelectedValue;

            }
            else
            {
                MessageBox.Show("Selecciona la oferta en la que desea editar o añadir una petición");
            }
        }

        private void NuevaRevision_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                RevisionOferta revisionNueva;
                ObservableCollection<ITipoMuestra> lineasTipoMuestra = new ObservableCollection<ITipoMuestra>();
                ObservableCollection<ILineasParametros> lineasParametros = new ObservableCollection<ILineasParametros>();

                RevisionOferta revisionUltima = PersistenceManager.SelectByProperty<RevisionOferta>("IdOferta", SelectedOferta.Id).OrderByDescending(r => r.Id).FirstOrDefault();
                if (revisionUltima != null)
                {
                    revisionNueva = CargarRevision(revisionUltima);
                    lineasTipoMuestra = CargarTipoMuestra(revisionUltima);
                    lineasParametros = CargarParametro(revisionUltima);
                }
                else if (SelectedPeticion != null)
                {
                    revisionNueva = CargarRevision(SelectedPeticion, SelectedOferta);
                    lineasTipoMuestra = CargarTipoMuestra(SelectedPeticion);
                    lineasParametros = CargarParametro(SelectedPeticion);
                }
                else
                    revisionNueva = CargarRevision(SelectedOferta);

                revisionNueva.Num++;
                revisionNueva.FechaEmision = DateTime.Now;

                Revisiones ventanaRevision = new Revisiones(Tecnicos, revisionNueva);
                ventanaRevision.UCRevision.CargarTipoMuestra(lineasTipoMuestra);
                ventanaRevision.UCRevision.CargarParametro(lineasParametros);

                ventanaRevision.Owner = Window.GetWindow(this);
                ventanaRevision.ShowDialog();
                if (ventanaRevision.DialogResult == true)
                    this.SelectedValue = SelectedValue;

            }
            else
            {
                MessageBox.Show("Selecciona la oferta en la que desea añadir una nueva revisión");
            }
        }

        private RevisionOferta CargarRevision(RevisionOferta revisionUltima)
        {
            RevisionOferta r = revisionUltima.Clone(typeof(RevisionOferta)) as RevisionOferta;
            r.Id = 0;
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

        private ObservableCollection<ITipoMuestra> CargarTipoMuestra(RevisionOferta revisionUltima)
        {
            var tiposRevisionUltima = PersistenceManager.SelectByProperty<TipoMuestraRevision>("IdRevision", revisionUltima.Id);
            ObservableCollection<ITipoMuestra> lista = new ObservableCollection<ITipoMuestra>();
            ITipoMuestra tm;
            tiposRevisionUltima.ForEach(t =>
            {
                tm = new ITipoMuestra
                {
                    IdRelacion = t.IdRevision,
                    IdTipoMuestra = t.IdTipoMuestra
                };
                lista.Add(tm);
            });
            return lista;
        }

        private ObservableCollection<ILineasParametros> CargarParametro(RevisionOferta revisionUltima)
        {
            var parametrosRevisionUltima = PersistenceManager.SelectByProperty<LineasRevisionOferta>("IdRevisionOferta", revisionUltima.Id);
            ObservableCollection<ILineasParametros> lista = new ObservableCollection<ILineasParametros>();
            ILineasParametros lp;
            parametrosRevisionUltima.ForEach(p =>
            {
                lp = new ILineasParametros
                {
                    IdRelacion = p.IdRevisionOferta,
                    Cantidad = p.Cantidad,
                    IdParametro = p.IdParametro
                };
                lista.Add(lp);
            });
            return lista;
        }

        private ObservableCollection<ITipoMuestra> CargarTipoMuestra(Peticion peticion)
        {
            var tiposPeticion = PersistenceManager.SelectByProperty<TipoMuestraPeticion>("IdPeticion", peticion.Id);
            ObservableCollection<ITipoMuestra> lista = new ObservableCollection<ITipoMuestra>();
            ITipoMuestra tm;
            tiposPeticion.ForEach(t =>
            {
                tm = new ITipoMuestra
                {
                    IdRelacion = t.IdPeticion,
                    IdTipoMuestra = t.IdTipoMuestra
                };
                lista.Add(tm);
            });
            return lista;
        }

        private ObservableCollection<ILineasParametros> CargarParametro(Peticion peticion)
        {
            var parametrosPeticion = PersistenceManager.SelectByProperty<LineasPeticion>("IdPeticion", peticion.Id);
            ObservableCollection<ILineasParametros> lista = new ObservableCollection<ILineasParametros>();
            ILineasParametros lp;
            parametrosPeticion.ForEach(p =>
            {
                lp = new ILineasParametros
                {
                    IdRelacion = p.IdPeticion,
                    Cantidad = p.Cantidad,
                    IdParametro = p.IdParametro
                };
                lista.Add(lp);
            });
            return lista;
        }

        private void VentanaNuevaOferta_Click(object sender, RoutedEventArgs e)
        {
            NuevaOferta np = new NuevaOferta();
            np.Owner = Window.GetWindow(this);
            np.ShowDialog();

            if (np.DialogResult ?? true)
                ReloadData();
        }

        private void ImprimirPeticion_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                if (SelectedPeticion.Id == 0)
                    MessageBox.Show("Imposible imprimir, no existe petición");
                else
                {
                    /* Elegir carpeta */
                    System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        String ruta = dialog.SelectedPath;
                        
                        new Escritor(Escritor.DocumentoPeticion, ruta, new DocPeticion(SelectedPeticion));
                    }
                }
            }
            else
                MessageBox.Show("Para imprimir una petición debes seleccionar su oferta asociada");
        }
    }
}
