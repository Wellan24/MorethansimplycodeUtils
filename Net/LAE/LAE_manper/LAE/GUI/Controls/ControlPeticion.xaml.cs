using Cartif.Expectation;
using Cartif.Extensions;
using Cartif.Util;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using GUI.Windows;
using LAE.Clases;
using LAE.Modelo;
using LAE.Comun.Persistence;
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
using LAE.Comun.Modelo;

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlPeticion.xaml
    /// </summary>
    public partial class ControlPeticion : UserControl
    {
        public List<ITipoMuestra> LineasTipoMuestra { get; set; }

        private int[] ListaTiposMuestra { get; set; }

        private List<ControlLineaParametro> lineasPuntosControl;
        public List<ControlLineaParametro> LineasPuntosControl
        {
            get { return lineasPuntosControl; }
            set
            {
                lineasPuntosControl = value;
                listaPuntosControl.Children.Clear();
                lineasPuntosControl.ForEach(l =>
                {
                    listaPuntosControl.Children.Add(l);
                    l.DeleteControl = (lc) => RemoveElementLineasPuntosControl(lc);
                });

            }
        }

        public void AddElementLineasPuntosControl(ControlLineaParametro c)
        {
            LineasPuntosControl.Add(c);
            listaPuntosControl.Children.Add(c);
            ActualizarValores(ListaTiposMuestra);
            c.DeleteControl = (lc) => RemoveElementLineasPuntosControl(lc);
        }

        public void RemoveElementLineasPuntosControl(ControlLineaParametro c)
        {
            LineasPuntosControl.Remove(c);
            listaPuntosControl.Children.Remove(c);
        }

        public void ClearListaPuntosControl()
        {
            listaPuntosControl.Children.Clear();
        }

        public void Reload(List<ControlLineaParametro> lista)
        {
            LineasPuntosControl = lista;
        }

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
                if (peticion.IdTecnico == 0)
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
            ClearListaPuntosControl();
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
            LineasTipoMuestra = new List<ITipoMuestra>() { };
            LineasPuntosControl = new List<ControlLineaParametro>() { };

            UCTipoMuestra.ActualizarComboParametros = (id) => { ActualizarValores(id); ListaTiposMuestra = id; };
            UCTipoMuestra.CanDeleteTipoMuestra = (id) => { return ComprobarBorrar(id); };

        }

        private bool ComprobarBorrar(int id)
        {
            bool canDelete = true;
            lineasPuntosControl.ForEach(l =>
            {
                l.PuntoControl.Lineas.ForEach(lp =>
                {
                    if (PersistenceManager.SelectByID<Parametro>(lp.IdParametro).IdTipoMuestra == id)
                        canDelete = false;
                });
            });
            return canDelete;
        }

        private void ActualizarValores(int[] id)
        {
            LineasPuntosControl.ForEach(l => l.TiposMuestraSeleccionados = id);
        }

        public void CargarNuevaPeticion(Peticion pet)
        {
            peticion = pet; /* NO Peticion, porque no quiero ejecutar los métodos del get */
            panelPeticionCliente.InnerValue = pet;
            panelPeticionTomaMuestra.InnerValue = pet;
            panelPeticionCondiciones.InnerValue = pet;

        }

        private void GenerarPanelPeticionCliente()
        {
            panelPeticionCliente.Build(new Peticion(),
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
                    PanelValidation = Expectation<Peticion>.ShouldNotBe().AddCriteria(p => p.IdContacto == 0)
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
            panelPeticionTomaMuestra.Build(Peticion,
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
                            Validate = ValidateEnum.noEmpty,
                            OnValid = ValidationsEnum.RightWithoutMessage,
                            OnInvalid = ValidationsEnum.DefaultWrong
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
            UCTipoMuestra.LineasTipoMuestra = LineasTipoMuestra;
        }

        private void GenerarAddParametros()
        {
            if (LineasPuntosControl.Count == 0)
                AddElementLineasPuntosControl(new ControlLineaParametro { PuntoControl = new IPuntoControl { } });
            else
                Reload(LineasPuntosControl);
        }

        private void GenerarPanelPeticionCondiciones()
        {
            panelPeticionCondiciones.Build(Peticion,
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
                        ["PlazoRealizacion"] = PropertyControlSettingsEnum.TextBoxDefault
                                .SetLabel("Plazo Realización"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Tecnicos)
                                .SetLabel("* Técnico")
                                .SetEnabled(false),
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
            LineasTipoMuestra.Clear();

            TipoMuestraPeticion[] lineas = PersistenceManager.SelectByProperty<TipoMuestraPeticion>("IdPeticion", Peticion.Id).ToArray();
            lineas.ForEach(l =>
            {
                ITipoMuestra t = new ITipoMuestra
                {
                    Id = l.Id,
                    IdTipoMuestra = l.IdTipoMuestra,
                    IdRelacion = l.IdPeticion
                };
                LineasTipoMuestra.Add(t);
            });
            ListaTiposMuestra = lineas.Select(l => l.IdTipoMuestra).ToArray();
        }

        public void CargarTipoMuestra(List<ITipoMuestra> lineas)
        {
            LineasTipoMuestra.Clear();
            lineas.ForEach(l => LineasTipoMuestra.Add(l));
        }

        private void CargarParametros()
        {
            CargarParametros(Peticion);
        }

        public void CargarParametros(Peticion p)
        {
            LineasPuntosControl = Util.GetParametrosFromPeticion(p.Id);
            ActualizarValores(ListaTiposMuestra);
        }

        public void CargarParametros(int idRevision)
        {
            LineasPuntosControl = Util.GetParametrosFromRevision(idRevision);
            ActualizarValores(ListaTiposMuestra);
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
                Contacto[] c = PersistenceManager.SelectByProperty<Contacto>("IdCliente", p.IdCliente).ToArray();
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

        private void NuevoControl_Click(object sender, RoutedEventArgs e)
        {
            AddElementLineasPuntosControl(new ControlLineaParametro { PuntoControl = new IPuntoControl { } });
        }
    }
}
