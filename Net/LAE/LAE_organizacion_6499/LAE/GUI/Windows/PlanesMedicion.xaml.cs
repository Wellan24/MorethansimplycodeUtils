using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using GUI.Controls;
using LAE.Modelo;
using MahApps.Metro.Controls;
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
using System.Windows.Shapes;
using LAE.Comun.Modelo;

namespace GUI.Windows
{
    /// <summary>
    /// Lógica de interacción para PlanMedicion.xaml
    /// </summary>
    public partial class PlanesMedicion : MetroWindow
    {
        public static readonly DependencyProperty IconTitleProperty =
               DependencyProperty.Register("IconTitle", typeof(ImageSource), typeof(PlanesMedicion), new PropertyMetadata(null));
        public ImageSource IconTitle
        {
            get { return (ImageSource)GetValue(IconTitleProperty); }
            set { SetValue(IconTitleProperty, value); }
        }

        private PlanMedicionAtmosfera planMedicion;
        public PlanMedicionAtmosfera PlanMedicion
        {
            get { return planMedicion; }
            set
            {
                planMedicion = value;
                CargarDatos();
            }
        }

        private int idCliente;
        public int IdCliente
        {
            get { return idCliente; }
            set
            {
                idCliente = value;
                GenerarCliente();
            }
        }

        private int idContacto;
        public int IdContacto
        {
            get { return idContacto; }
            set
            {
                idContacto = value;
                GenerarContacto();
            }
        }

        public PlanesMedicion(int idCliente, int idContacto, PlanMedicionAtmosfera plan)
        {
            InitializeComponent();
            IconTitle = new BitmapImage(new Uri("pack://application:,,,/LAE;component/images/cabecera.png", UriKind.Absolute));
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            PlanMedicion = plan;
            IdCliente = idCliente;
            IdContacto = idContacto;
        }

        private void CargarDatos()
        {
            PlanMedicion.Equipos = new ObservableCollection<EquiposPMAtmosfera>() { new EquiposPMAtmosfera() };
            PlanMedicion.Fechas = new ObservableCollection<FechaPMAtmosfera>() { new FechaPMAtmosfera() };
            PlanMedicion.Personal = new ObservableCollection<PersonalPMAtmosfera>() { new PersonalPMAtmosfera() };

            GenerarDatosPlan();
            GenerarAddEquipos();
            GenerarAddFechas();
            GenerarAddPersonal();
            GenerarObservacionesYFirma();

            AddFoco();
        }

        private void GenerarCliente()
        {
            Cliente c = PersistenceManager.SelectByID<Cliente>(IdCliente);

            panelCliente.Build(c,
                new TypePanelSettings<Cliente>
                {
                    Fields = new FieldSettings
                    {
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetEnabled(false)
                            .SetLabel("Cliente"),
                        ["Telefono"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetEnabled(false),
                        ["Email"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetEnabled(false),
                    },
                    IsUpdating = true
                });
        }

        private void GenerarContacto()
        {
            Contacto c = PersistenceManager.SelectByID<Contacto>(IdContacto);
            panelContacto.Build(c,
                new TypePanelSettings<Contacto>
                {
                    ColumnWidths = new int[] { 1, 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["Nombre"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Apellidos"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Telefono"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["Email"] = PropertyControlSettingsEnum.TextBoxDefault,
                    },
                    DefaultSettings = new PropertyControlSettings
                    {
                        Enabled = false
                    }
                });
        }

        private void GenerarDatosPlan()
        {

            panelObjetivo.Build(PlanMedicion,
                new TypePanelSettings<PlanMedicionAtmosfera>
                {
                    ColumnWidths = new int[] { 1 },
                    Fields = new FieldSettings
                    {
                        ["Objetivo"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35)
                            .SetLabel("Objetivo de la medición"),
                    },
                    IsUpdating = true
                });

            panelInstalacion.Build(PlanMedicion,
                new TypePanelSettings<PlanMedicionAtmosfera>
                {
                    ColumnWidths = new int[] { 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["Descripcion"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35)
                            .SetLabel("Descripción de la planta")
                            .SetColumnSpan(3),
                        ["Modificaciones"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35)
                            .SetLabel("Modificaciones introducidas desde última inspección")
                            .SetControlToolTipText("Modificaciones introducidas desde última inspección"),
                        ["MedidasCorreccion"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35)
                            .SetLabel("Medidas adoptadas para corrección defectos de la última inspección")
                            .SetControlToolTipText("Medidas adoptadas para corrección defectos de la última inspección"),
                        ["NuevosDefectos"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35)
                            .SetLabel("Nuevos defectos detectados"),
                    },
                    IsUpdating = true
                });

        }

        private void GenerarAddEquipos()
        {
            panelEquipos.Build(new EquiposPMAtmosfera(),
                new TypePanelSettings<EquiposPMAtmosfera>
                {
                    ColumnWidths = new int[] { 3, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["IdEquipo"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaEquipoPMAtmosfera.GetEquipos())
                                        .SetLabel("* EQUIPO/INSTRUMENTAL")
                                        .SetDisplayMemberPath("Nombre")
                                        .AddKeyDownCombo(
                                            (sender, e) => { if (e.Key == Key.Enter) AddEquipo(); }
                                        )
                                        .AddSelectionChanged(CambioEquipo),
                        ["Descripcion"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowRightIcon,
                            Click = (sender, e) => { AddEquipo(); }
                        },
                    },
                    IsUpdating = true
                });

            gridEquipos.Build(PlanMedicion.Equipos,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdEquipo"] = new TypeGridColumnSettings
                        {
                            Label = "Equipo/Instrumental",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = FactoriaEquipoMuestraAgua.GetEquipos(),
                                Path = "Id",
                                DisplayPath = "Nombre"
                            },
                            Width = 3
                        },
                        ["Descripcion"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("Otros")
                            .SetWidth(3),
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
                                    DeleteEquipo(sender);
                                }
                            }
                        }
                        .SetWidth(1),
                    }
                });

            if (PlanMedicion.Equipos.Count() == 1 && PlanMedicion.Equipos.First().Id == 0)
                PlanMedicion.Equipos.Clear();

            panelEquipos["Descripcion"].Visibility = Visibility.Collapsed;
        }

        private void CambioEquipo(object sender, SelectionChangedEventArgs e)
        {
            EquiposPMAtmosfera eq = panelEquipos.InnerValue as EquiposPMAtmosfera;
            if (eq.IdEquipo == FactoriaEquipoPMAtmosfera.GetIdOtros())
                panelEquipos["Descripcion"].Visibility = Visibility.Visible;
            else {
                panelEquipos["Descripcion"].Visibility = Visibility.Collapsed;
                panelEquipos["Descripcion"].SetInnerContent(null);
            }
        }

        private void AddEquipo()
        {
            if (!panelEquipos.AddElementToList(PlanMedicion.Equipos))
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeleteEquipo(object sender)
        {
            EquiposPMAtmosfera lineaBorrada = ((FrameworkElement)sender).DataContext as EquiposPMAtmosfera;
            PlanMedicion.Equipos.Remove(lineaBorrada);
        }

        private void GenerarAddFechas()
        {
            panelFechas.Build(new FechaPMAtmosfera(),
                new TypePanelSettings<FechaPMAtmosfera>
                {
                    ColumnWidths = new int[] { 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["FechaPrevista"] = PropertyControlSettingsEnum.DateTimeDefaultNoEmpty
                                        .SetLabel("* Fechas previstas medición")
                                        .AddKeyDownCombo(
                                            (sender, e) => { if (e.Key == Key.Enter) AddFecha(); }
                                        ),
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) => { AddFecha(); }
                        },
                    },
                    IsUpdating = true
                });

            gridFechas.Build(PlanMedicion.Fechas,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["FechaPrevista"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetFormat("dd/MM/yy HH:mm")
                            .SetLabel("Fechas Previstas")
                            .SetWidth(3),
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
                                    DeleteFecha(sender);
                                }
                            }
                        }
                        .SetWidth(1),
                    }
                });

            if (PlanMedicion.Fechas.Count() == 1 && PlanMedicion.Fechas.First().Id == 0)
                PlanMedicion.Fechas.Clear();
        }

        private void AddFecha()
        {
            if (!panelFechas.AddElementToList(PlanMedicion.Fechas))
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeleteFecha(object sender)
        {
            FechaPMAtmosfera lineaBorrada = ((FrameworkElement)sender).DataContext as FechaPMAtmosfera;
            PlanMedicion.Fechas.Remove(lineaBorrada);
        }

        private void GenerarAddPersonal()
        {
            panelPersonal.Build(new PersonalPMAtmosfera(),
                new TypePanelSettings<PersonalPMAtmosfera>
                {
                    ColumnWidths = new int[] { 3, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaTecnicos.GetTecnicos())
                                        .SetLabel("* Personal asignado")
                                        .AddKeyDownCombo(
                                            (sender, e) => { if (e.Key == Key.Enter) AddPersonal(); }
                                        )
                                        .AddSelectionChanged(CambioEquipo),
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) => { AddPersonal(); }
                        },
                    },
                    IsUpdating = true
                });

            gridPersonal.Build(PlanMedicion.Personal,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdTecnico"] = new TypeGridColumnSettings
                        {
                            Label = "Equipo/Instrumental",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = FactoriaTecnicos.GetTecnicos(),
                                Path = "Id",
                            },
                            Width = 3
                        },
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
                                    DeletePersonal(sender);
                                }
                            }
                        }
                        .SetWidth(1),
                    }
                });

            if (PlanMedicion.Personal.Count() == 1 && PlanMedicion.Personal.First().Id == 0)
                PlanMedicion.Personal.Clear();
        }

        private void AddPersonal()
        {
            if (!panelPersonal.AddElementToList(PlanMedicion.Personal))
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeletePersonal(object sender)
        {
            PersonalPMAtmosfera lineaBorrada = ((FrameworkElement)sender).DataContext as PersonalPMAtmosfera;
            PlanMedicion.Personal.Remove(lineaBorrada);
        }

        private void GenerarObservacionesYFirma()
        {
            panelObservaciones.Build(PlanMedicion,
                new TypePanelSettings<PlanMedicionAtmosfera>
                {
                    ColumnWidths = new int[] { 2, 1 },
                    Fields = new FieldSettings
                    {
                        ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetHeightMultiline(35)
                            .SetLabel("Objetivo de la medición"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaTecnicos.GetTecnicos())
                                        .SetLabel("* Técnico"),
                    },
                    IsUpdating = true
                });
        }

        private ControlFocoPlanMedicion AddFoco()
        {
            FocoAtmosfera foco = new FocoAtmosfera() { };
            return AddFoco(foco);
        }

        private ControlFocoPlanMedicion AddFoco(FocoAtmosfera foco)
        {
            foco.Conexiones = new ObservableCollection<ConexionFocoAtm>() {
                new ConexionFocoAtm() {
                    NumConexion = 1,
                    PuntosMuestreo = new ObservableCollection<PuntoConexFocoAtmosfera>() {
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 21,
                            Velocidad = 23,
                            Localizacion=5
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 22,
                            Velocidad = 23,
                            Localizacion=13.7m
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 21,
                            Velocidad = 23,
                            Localizacion=24.9m
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 22,
                            Velocidad = 23,
                            Localizacion=40.6m
                        },new PuntoConexFocoAtmosfera() {
                            Temperatura = 21,
                            Velocidad = 23,
                            Localizacion=70m
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 22,
                            Velocidad = 23,
                            Localizacion=99.4m
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 21,
                            Velocidad = 23,
                            Localizacion=115.1m
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 22,
                            Velocidad = 23,
                            Localizacion=120.3m
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 22,
                            Velocidad = 23,
                            Localizacion=135
                        }
                    }
                },
                new ConexionFocoAtm() {
                    NumConexion = 2,
                    PuntosMuestreo = new ObservableCollection<PuntoConexFocoAtmosfera>() {
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 23,
                            Velocidad = 23,
                            Localizacion=5
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 24,
                            Velocidad = 23,
                            Localizacion=5
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 23,
                            Velocidad = 23,
                            Localizacion=5
                        }
                        ,new PuntoConexFocoAtmosfera() {
                            Temperatura = 23,
                            Velocidad = 13,
                            Localizacion=5
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 24,
                            Velocidad = 13,
                            Localizacion=5
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 23,
                            Velocidad = 13,
                            Localizacion=5
                        }
                        ,new PuntoConexFocoAtmosfera() {
                            Temperatura = 23,
                            Velocidad = 13,
                            Localizacion=5
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 24,
                            Velocidad = 13,
                            Localizacion=5
                        },
                        new PuntoConexFocoAtmosfera() {
                            Temperatura = 23,
                            Velocidad = 13,
                            Localizacion=5
                        }
                    }
                }
            };

            ControlFocoPlanMedicion cfpm = new ControlFocoPlanMedicion() { Foco = foco };
            listaFocos.Children.Add(cfpm);
            return cfpm;
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    }
}
