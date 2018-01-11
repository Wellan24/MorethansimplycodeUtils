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

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para Oferta.xaml
    /// </summary>
    public partial class Ofertas : UserControl, INotifyPropertyChanged
    {
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
                    SelectedPeticion = PersistenceManager<Peticion>.SelectByProperty("IdOferta", SelectedOferta.Id).FirstOrDefault();
                    if (SelectedPeticion == null)
                    {
                        SelectedPeticion = new Peticion() { IdOferta = SelectedOferta.Id };
                        buttonPeticion.Content = "Añadir";
                    }
                    else
                        buttonPeticion.Content = "Editar";
                    panelPeticiones.InnerValue = SelectedPeticion;

                    /* fill revision */
                    SelectedRevisiones = PersistenceManager<RevisionOferta>.SelectByProperty("IdOferta", SelectedOferta.Id).OrderByDescending(r=>r.Id).ToArray();
                    gridRevisiones.FillDataGrid(SelectedRevisiones);
                }
                OnPropertyChanged("SelectedValue");
            }
        }

        public Ofertas()
        {
            InitializeComponent();
            SelectedPeticion = new Peticion();
            SelectedOferta = new Oferta();
            SelectedRevisiones = new RevisionOferta[] { new RevisionOferta() };

            CargarDatos();

            CargarGridOfertas();
            GenerarPaneles();
            GenerarGrid();
        }

        private void CargarDatos()
        {
            Clientes = PersistenceManager<Cliente>.SelectAll()
                .OrderByDescending(c => c.Nombre).ToArray();
            Contactos = PersistenceManager<Contacto>.SelectAll()
                .OrderByDescending(c => c.Nombre).ThenBy(c => c.Apellidos).ToArray();
            Tecnicos = PersistenceManager<Tecnico>.SelectAll()
                .OrderByDescending(c => c.Nombre).ThenBy(c => c.PrimerApellido).ThenBy(c => c.SegundoApellido).ToArray();
        }

        private void CargarGridOfertas()
        {
            ListaOfertas = PersistenceManager<Oferta>.SelectAll().OrderByDescending(c => c.Id).ToArray();
            gridOfertas.Build(ListaOfertas, new TypeGridSettings()
            {
                Columns = new ColumnGridSettings
                {
                    ["Id"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["CodigoOferta"] = TypeGridColumnSettingsEnum.DefaultColum,
                    ["AnnoOferta"] = TypeGridColumnSettingsEnum.DefaultColum
                                        .SetLabel("Año oferta"),
                    ["IdCliente"] = new TypeGridColumnSettings
                    {
                        Label = "Cliente",
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
                }
            });
        }

        private void GenerarPaneles()
        {
            panelOfertas.Build<Oferta>(SelectedOferta,
                new TypePanelSettings<Oferta>
                {
                    Fields = new FieldSettings
                    {
                        ["Id"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["CodigoOferta"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["AnnoOferta"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(Clientes)
                                .SetLabel("Cliente")
                                .AddSelectionChanged(RefreshIdContacto),
                        ["IdContacto"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(RecuperarContactos())
                                .SetLabel("Contacto"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(Tecnicos)
                                .SetLabel("Técnico"),
                    },
                });


            panelPeticiones.Build<Peticion>(SelectedPeticion,
               new TypePanelSettings<Peticion>
               {
                   Fields = new FieldSettings
                   {
                       ["Id"] = PropertyControlSettingsEnum.TextBoxDefault
                                    .SetEnabled(true),
                       ["Fecha"] = PropertyControlSettingsEnum.DateTimeDefault,
                       ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Clientes)
                                .SetLabel("Clientes"),
                       ["IdContacto"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Contactos)
                                .SetLabel("Contacto"),
                       ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Tecnicos)
                                .SetLabel("Técnico")
                   },
                   IsUpdating = true,
                   DefaultSettings = new PropertyControlSettings
                   {
                       ReadOnly = true,
                   }
               });


        }

        private void GenerarGrid()
        {
            gridRevisiones.Build(SelectedRevisiones,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["Id"] = TypeGridColumnSettingsEnum.DefaultColum
                                        .SetWidth(2),
                        ["Num"] = TypeGridColumnSettingsEnum.DefaultColum
                                        .SetWidth(2),
                        ["Importe"] = TypeGridColumnSettingsEnum.DefaultColum
                                        .SetWidth(2),
                        ["Editar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.EditIcon,
                                Color = Colors.White,
                                Size = 30,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    RevisionOferta ro = gridRevisiones.SelectedItem.Clone(typeof(RevisionOferta)) as RevisionOferta;
                                    Revisiones r = new Revisiones(Tecnicos, ro);
                                    r.ShowDialog();
                                    if (r.DialogResult ?? true)
                                        this.SelectedValue = SelectedValue;
                                }
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
                                    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que desea borrar la revisión y sus lineas?", "Borrar", MessageBoxButton.YesNo);
                                    if (messageBoxResult == MessageBoxResult.Yes)
                                    {
                                        RevisionOferta revisionBorrada = ((FrameworkElement)sender).DataContext as RevisionOferta;
                                        gridRevisiones.GetItemSource().Remove(revisionBorrada);

                                        /* borrar del todo */
                                        //BorrarLineasOferta(revisionBorrada.Id);
                                        //revisionBorrada.Delete();

                                        /* eliminar acceso */
                                        revisionBorrada.IdOferta = null;
                                        revisionBorrada.Update();
                                        

                                        
                                    }
                                }
                            }
                        }
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

                    MessageBox.Show("Oferta actualizada");
                }
                else
                    MessageBox.Show("Algún dato es erroneo");
            }
            else
                MessageBox.Show("Seleccione una oferta");
        }

        private void BorrarOferta_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro de borrar la oferta, y todas sus referencias?", "Borrar oferta", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    BorrarRevisiones();
                    BorrarPeticion();
                    SelectedOferta.Delete();
                    ReloadData();
                    MessageBox.Show("Oferta borrada con exito");
                }
            }
            else
                MessageBox.Show("Seleccione la oferta que desea borrar");
        }

        private void BorrarPeticion_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                if (SelectedPeticion.Id == 0)
                    MessageBox.Show("La oferta no tiene petición asociada");
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro de borrar la petición?", "Borrar petición", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        SelectedPeticion.IdOferta = null;
                        SelectedPeticion.Update();

                        SelectedPeticion = new Peticion();
                        panelPeticiones.InnerValue = SelectedPeticion;
                        MessageBox.Show("Petición borrada con exito");
                    }
                }
            }
            else
                MessageBox.Show("Seleccione oferta que contiene petición a borrar");
        }

        private void BorrarRevisiones()
        {
            IEnumerable<RevisionOferta> revisiones = PersistenceManager<RevisionOferta>.SelectByProperty("IdOferta", SelectedOferta.Id);
            revisiones.ForEach(r =>
            {
                BorrarLineasOferta(r.Id);
                BorrarTipoMuestraOferta(r.Id);
                r.Delete();
            });
        }

        private void BorrarLineasOferta(int idRevision)
        {
            IEnumerable<LineasRevisionOferta> lineasOferta = PersistenceManager<LineasRevisionOferta>.SelectByProperty("IdRevisionOferta", idRevision);
            lineasOferta.ForEach(l => l.Delete());
        }

        private void BorrarTipoMuestraOferta(int idRevision)
        {
            IEnumerable<TipoMuestraRevision> tipoMuestraRevision = PersistenceManager<TipoMuestraRevision>.SelectByProperty("IdRevision", idRevision);
            tipoMuestraRevision.ForEach(t => t.Delete());
        }

        private void BorrarPeticion()
        {
            BorrarLineasPeticion(SelectedPeticion.Id);
            BorrarTipoMuestraPeticion(SelectedPeticion.Id);
            SelectedPeticion.Delete();
        }

        private void BorrarLineasPeticion(int idPeticion)
        {
            IEnumerable<LineasPeticion> lineasOferta = PersistenceManager<LineasPeticion>.SelectByProperty("IdPeticion", idPeticion);
            lineasOferta.ForEach(l => l.Delete());
        }

        private void BorrarTipoMuestraPeticion(int idPeticion)
        {
            IEnumerable<TipoMuestraPeticion> tipoMuestraPeticion = PersistenceManager<TipoMuestraPeticion>.SelectByProperty("IdPeticion", idPeticion);
            tipoMuestraPeticion.ForEach(t => t.Delete());
        }

        private Contacto[] RecuperarContactos()
        {
            Oferta o = panelOfertas.InnerValue as Oferta;
            if (o != null)
                return PersistenceManager<Contacto>.SelectByProperty("IdCliente", o.IdCliente).ToArray();

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
            ListaOfertas = PersistenceManager<Oferta>.SelectAll().OrderByDescending(c => c.Id).ToArray();
            gridOfertas.FillDataGrid(ListaOfertas);

            SelectedRevisiones = new RevisionOferta[0];
            gridRevisiones.FillDataGrid(SelectedRevisiones);


        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        private void ButtonGuardarPeticion_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                Peticiones p = new Peticiones(Clientes, Tecnicos, SelectedPeticion.Clone(typeof(Peticion)) as Peticion);
                p.ShowDialog();
                if (p.DialogResult ?? true)
                    this.SelectedValue = SelectedValue;

            }
            else
            {
                MessageBox.Show("Seleccione oferta para editar o añadir la petición relacionada");
            }
        }
        private void NuevaOferta_Click(object sender, RoutedEventArgs e)
        {
            //pruebas
            Console.WriteLine(ListaOfertas);
        }

        private void NuevaRevision_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                RevisionOferta revisionNueva;
                ObservableCollection<ITipoMuestra> lineasTipoMuestra=new ObservableCollection<ITipoMuestra>();
                ObservableCollection<ILineasParametros> lineasParametros = new ObservableCollection<ILineasParametros>();

                RevisionOferta revisionUltima = PersistenceManager<RevisionOferta>.SelectByProperty("IdOferta", SelectedOferta.Id).OrderByDescending(r => r.Id).FirstOrDefault();
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

                Revisiones ventanaRevision = new Revisiones(Tecnicos, revisionNueva);
                ventanaRevision.UCRevision.CargarTipoMuestra(lineasTipoMuestra);
                ventanaRevision.UCRevision.CargarParametro(lineasParametros);

                ventanaRevision.ShowDialog();
                if (ventanaRevision.DialogResult == true)
                    this.SelectedValue = SelectedValue;

            }
            else
            {
                MessageBox.Show("Seleccione oferta para editar o añadir una revisión");
            }
        }

        private RevisionOferta CargarRevision(RevisionOferta revisionUltima)
        {
            RevisionOferta r= revisionUltima.Clone(typeof(RevisionOferta)) as RevisionOferta;
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
            var tiposRevisionUltima=PersistenceManager<TipoMuestraRevision>.SelectByProperty("IdRevision", revisionUltima.Id);
            ObservableCollection<ITipoMuestra> lista = new ObservableCollection<ITipoMuestra>();
            ITipoMuestra tm;
            tiposRevisionUltima.ForEach(t => {
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
            var parametrosRevisionUltima = PersistenceManager<LineasRevisionOferta>.SelectByProperty("IdRevisionOferta", revisionUltima.Id);
            ObservableCollection<ILineasParametros> lista = new ObservableCollection<ILineasParametros>();
            ILineasParametros lp;
            parametrosRevisionUltima.ForEach(p => {
                lp = new ILineasParametros
                {
                    IdRelacion = p.IdRevisionOferta,
                    Cantidad=p.Cantidad,
                    Metodo=p.Metodo,
                    IdParametro=p.IdParametro
                };
                lista.Add(lp);
            });
            return lista;
        }

        private ObservableCollection<ITipoMuestra> CargarTipoMuestra(Peticion peticion)
        {
            var tiposPeticion = PersistenceManager<TipoMuestraPeticion>.SelectByProperty("IdPeticion",peticion.Id);
            ObservableCollection<ITipoMuestra> lista = new ObservableCollection<ITipoMuestra>();
            ITipoMuestra tm;
            tiposPeticion.ForEach(t => {
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
            var parametrosPeticion = PersistenceManager<LineasPeticion>.SelectByProperty("IdPeticion", peticion.Id);
            ObservableCollection<ILineasParametros> lista = new ObservableCollection<ILineasParametros>();
            ILineasParametros lp;
            parametrosPeticion.ForEach(p => {
                lp = new ILineasParametros
                {
                    IdRelacion = p.IdPeticion,
                    Cantidad = p.Cantidad,
                    Metodo = p.Metodo,
                    IdParametro = p.IdParametro
                };
                lista.Add(lp);
            });
            return lista;
        }

        private void NuevaPeticion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NuevaPeticion np = new NuevaPeticion();
                np.ShowDialog();
                if (np.DialogResult ?? true)
                    ReloadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
