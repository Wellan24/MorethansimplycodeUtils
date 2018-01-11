using Cartif.Extensions;
using Cartif.Util;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using GUI.Windows;
using LAE.Modelo;
using Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlPeticion.xaml
    /// </summary>
    public partial class ControlPeticion : UserControl
    {
        public ObservableCollection<ITipoMuestra> lineasTipoMuestra;
        public ObservableCollection<ILineasParametros> lineasParametros;

        private Tecnico[] tecnicos;
        public Tecnico[] Tecnicos
        {
            get { return tecnicos; }
            set { tecnicos = value; }
        }

        private Peticion peticion;
        public Peticion Peticion
        {
            get { return peticion; }
            set
            {
                peticion = value;
                if (peticion.IdTecnico==0)
                    peticion.IdTecnico = FactoriaTecnicos.director().Id;
                RecargarPagina();
            }
        }

        public void RecargarPagina()
        {
            /* limpiar */
            panelPeticionCliente.ClearGrid();
            panelPeticionTomaMuestra.ClearGrid();
            UCTipoMuestra.panelTipoMuestra.ClearGrid();
            UCLineaParametro.panelParametros.ClearGrid();
            panelPeticionCondiciones.ClearGrid();

            /* generar */
            GenerarPanelPeticionCliente();
            GenerarPanelPeticionTomaMuestra();

            GenerarAddTipoMuestra();
            GenerarAddParametros();

            GenerarPanelPeticionCondiciones();

            if (Peticion.Id != 0)
            {
                CargarTipoMuestra();
                CargarParametros();
            }
            ExecuteRefreshFrecuencia();
            ExecuteRefreshTomaMuestra();
        }

        public ControlPeticion()
        {
            InitializeComponent();
            lineasTipoMuestra = new ObservableCollection<ITipoMuestra>() { new ITipoMuestra() };
            lineasParametros = new ObservableCollection<ILineasParametros>() { new ILineasParametros() };

            UCTipoMuestra.actualizarComboParametros = (id) => UCLineaParametro.TiposMuestraSeleccionados = id;
            UCTipoMuestra.canDeleteTipoMuestra = (id) =>
            {
                bool canDelete = true;
                UCLineaParametro.LineasParametros.ForEach(
                    l =>
                    {
                        if (PersistenceManager.SelectByID<Parametro>(l.IdParametro).IdTipoMuestra == id)
                            canDelete = false;
                    });
                return canDelete;
            };
        }

        public void CargarNuevaPeticion(Peticion pet)
        {
            peticion = pet;
            panelPeticionCliente.InnerValue = pet;
            panelPeticionTomaMuestra.InnerValue = pet;
            panelPeticionCondiciones.InnerValue = pet;

        }

        private void GenerarPanelPeticionCliente()
        {
            panelPeticionCliente.Build<Peticion>(new Peticion(),
                new TypePanelSettings<Peticion>
                {
                    Fields = new FieldSettings
                    {
                        ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(FactoriaClientes.RecuperarClientes())
                                .SetLabel("* Empresa")
                                .AddSelectionChanged(RefreshIdContacto),
                        ["Boton"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.PlusIcon,
                            Click = (sender, e) => { NuevoCliente(); }
                        },
                        ["IdContacto"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(RecuperarContactos())
                                .SetLabel("* Contacto"),
                        ["Boton2"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.PlusIcon,
                            Click = (sender, e) => { NuevoContacto(); }
                        },
                    },
                    IsUpdating = true,
                    ColumnWidths = new int[] { 3, 1, 3, 1 },
                    PanelValidation = Expectation<Peticion>.ShouldNot().AddTest(p => p.IdContacto == 0)
                });
            panelPeticionCliente.InnerValue = Peticion;
        }

        private void NuevoCliente()
        {
            NuevoCliente nc = new NuevoCliente();
            nc.Owner = Window.GetWindow(this);
            nc.ShowDialog();
            if (nc.DialogResult ?? false)
            {
                panelPeticionCliente["IdCliente"].InnerValues = FactoriaClientes.RecuperarClientes();
            }
        }

        private void NuevoContacto()
        {
            NuevoContacto nc = new NuevoContacto(Tecnicos);
            nc.Owner = Window.GetWindow(this);
            nc.ShowDialog();

            if (nc.DialogResult ?? false)
            {
                RefreshIdContacto(null, null);
            }
        }

        private void GenerarPanelPeticionTomaMuestra()
        {
            panelPeticionTomaMuestra.Build<Peticion>(Peticion,
                new TypePanelSettings<Peticion>
                {
                    Fields = new FieldSettings
                    {
                        ["RequiereTomaMuestra"] = new PropertyControlSettings
                        {
                            InnerValues = new ComboBoxItem<Boolean>[] {
                                ComboBoxItem<Boolean>.Create("Cliente", false),
                                ComboBoxItem<Boolean>.Create("CARTIF", true)
                            },
                            Type = typeof(PropertyControlComboBox),
                            SelectionChanged = RefreshTomaMuestra,
                            Label = "* Toma de Muestra",
                            Validate=ValidateEnum.noEmpty,
                            OnValid=ValidationsEnum.RightWithoutMessage,
                            OnInvalid=ValidationsEnum.DefaultWrong
                        },
                        ["LugarMuestra"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Lugar de toma de muestra"),
                        ["NumPuntosMuestreo"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Nº de puntos de muestreo"),
                    },
                    IsUpdating = true
                });
        }

        private void GenerarAddTipoMuestra()
        {
            UCTipoMuestra.LineasTipoMuestra = lineasTipoMuestra;
        }

        private void GenerarAddParametros()
        {
            UCLineaParametro.LineasParametros = lineasParametros;
        }

        private void GenerarPanelPeticionCondiciones()
        {
            panelPeticionCondiciones.Build<Peticion>(Peticion,
                new TypePanelSettings<Peticion>
                {
                    Fields = new FieldSettings
                    {
                        ["TrabajoPuntual"] = new PropertyControlSettings
                        {
                            InnerValues = new ComboBoxItem<Boolean>[] {
                                ComboBoxItem<Boolean>.Create("Puntual", true),
                                ComboBoxItem<Boolean>.Create("Periódico", false)
                            },
                            Type = typeof(PropertyControlComboBox),
                            Label = "Tipo de trabajo",
                            SelectionChanged = RefreshFrecuencia
                        },
                        ["Frecuencia"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["PlazoRealizacion"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Tecnicos)
                                .SetLabel("* Técnico"),
                        ["Fecha"] = PropertyControlSettingsEnum.DateTimeDefaultNoEmpty
                                .SetLabel("* Fecha"),

                        ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                                                    .SetHeightMultiline(90)
                                                    .SetColumnSpan(2),

                    },
                    IsUpdating = true
                });
            ExecuteRefreshFrecuencia();
            ExecuteRefreshTomaMuestra();
        }

        private void CargarTipoMuestra()
        {
            lineasTipoMuestra.Clear();
            TipoMuestraPeticion[] lineas = PersistenceManager.SelectByProperty<TipoMuestraPeticion>("IdPeticion", Peticion.Id).ToArray();
            lineas.ForEach(l =>
            {
                ITipoMuestra t = new ITipoMuestra
                {
                    Id = l.Id,
                    IdTipoMuestra = l.IdTipoMuestra,
                    IdRelacion = l.IdPeticion
                };
                lineasTipoMuestra.Add(t);
            });

            UCLineaParametro.TiposMuestraSeleccionados = lineas.Select(l => l.IdTipoMuestra).ToArray();
        }

        private void CargarParametros()
        {
            lineasParametros.Clear();
            LineasPeticion[] lineas = PersistenceManager.SelectByProperty<LineasPeticion>("IdPeticion", Peticion.Id).ToArray();
            lineas.ForEach(l =>
            {
                ILineasParametros t = new ILineasParametros
                {
                    Id = l.Id,
                    Cantidad = l.Cantidad,
                    IdParametro = l.IdParametro,
                    IdRelacion = l.IdPeticion
                };
                lineasParametros.Add(t);
            });

        }

        public void CargarTipoMuestra(ObservableCollection<ITipoMuestra> lineas)
        {
            lineasTipoMuestra.Clear();
            lineas.ForEach(l => lineasTipoMuestra.Add(l));

            UCLineaParametro.TiposMuestraSeleccionados = lineas.Select(l => l.IdTipoMuestra).ToArray();
        }

        public void CargarParametro(ObservableCollection<ILineasParametros> lineas)
        {
            lineasParametros.Clear();
            lineas.ForEach(l => lineasParametros.Add(l));
        }

        private void RefreshFrecuencia(object sender, SelectionChangedEventArgs e)
        {
            ExecuteRefreshFrecuencia();
        }

        private void ExecuteRefreshFrecuencia()
        {
            Peticion p = (Peticion)panelPeticionCondiciones.InnerValue;
            bool puntual = p.TrabajoPuntual ?? false;

            if (puntual)
                panelPeticionCondiciones["Frecuencia"].SetInnerContent(null);
            panelPeticionCondiciones["Frecuencia"].Enabled = !puntual;
        }

        private void RefreshTomaMuestra(object sender, SelectionChangedEventArgs e)
        {
            ExecuteRefreshTomaMuestra();
        }

        private void ExecuteRefreshTomaMuestra()
        {
            Peticion p = (Peticion)panelPeticionTomaMuestra.InnerValue;
            bool requiereToma = p.RequiereTomaMuestra ?? true;

            if (!requiereToma)
            {
                panelPeticionTomaMuestra["LugarMuestra"].SetInnerContent(null);
                panelPeticionTomaMuestra["NumPuntosMuestreo"].SetInnerContent(null);
            }
            panelPeticionTomaMuestra["LugarMuestra"].Enabled = requiereToma;
            panelPeticionTomaMuestra["NumPuntosMuestreo"].Enabled = requiereToma;
        }

        private void RefreshIdContacto(object sender, SelectionChangedEventArgs e)
        {
            panelPeticionCliente["IdContacto"].InnerValues = RecuperarContactos();
        }



        private Contacto[] RecuperarContactos()
        {
            Peticion p = panelPeticionCliente.InnerValue as Peticion;
            if (p != null)
            {
                Contacto[] c= PersistenceManager.SelectByProperty<Contacto>("IdCliente", p.IdCliente).ToArray();
                Array.Sort(c);
                return c;
            }

            return new Contacto[0];
        }

        public bool ValidarPeticion()
        {
            return (panelPeticionCliente.GetValidatedInnerValue<Peticion>() != default(Peticion) &&
                panelPeticionTomaMuestra.GetValidatedInnerValue<Peticion>() != default(Peticion) &&
                panelPeticionCondiciones.GetValidatedInnerValue<Peticion>() != default(Peticion));
        }

        public void SetEnabled(bool enabled)
        {
            /* activo y desactivo todos los controles */
            this.IsEnabled = enabled;

            /* compruebo los campos que pueden desactivarse en función de otros campos*/
            if (enabled)
            {
                ExecuteRefreshFrecuencia();
                ExecuteRefreshTomaMuestra();
            }
        }

    }
}
