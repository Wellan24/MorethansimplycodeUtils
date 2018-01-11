using Cartif.Extensions;
using Cartif.Util;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Clases;
using LAE.Modelo;
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
using LAE.Comun.Modelo;

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlRevision.xaml
    /// </summary>
    public partial class ControlRevision : UserControl
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

        private RevisionOferta revision;
        public RevisionOferta Revision
        {
            get { return revision; }
            set
            {
                revision = value;
                if (revision.IdTecnico == 0)
                    revision.IdTecnico = FactoriaTecnicos.director().Id;
                RecargarPagina();
            }
        }

        public void RecargarPagina()
        {
            /* limpiar */
            panelRevisionTomaMuestra.ClearGrid();
            UCTipoMuestra.panelTipoMuestra.ClearGrid();
            LineasPuntosControl.ForEach(pc => RemoveElementLineasPuntosControl(pc));
            ClearListaPuntosControl();
            panelRevisionCondiciones.ClearGrid();

            /* generar */
            GenerarPanelRevisionTomaMuestra();
            GenerarAddTipoMuestra();
            GenerarAddParametros();
            GenerarPanelPeticionCondiciones();
            if (Revision.Id != 0)
            {
                CargarTipoMuestra();
                CargarParametros();
            }
            ExecuteRefreshFrecuencia();
            ExecuteRefreshTomaMuestra();
        }

        public ControlRevision()
        {
            InitializeComponent();
            LineasTipoMuestra = new List<ITipoMuestra>();
            LineasPuntosControl = new List<ControlLineaParametro> { };

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

        public void CargarNuevaRevision(RevisionOferta rev)
        {
            revision = rev; /* NO Revision, porque no quiero ejecutar los métodos del get */
            panelRevisionCondiciones.InnerValue = rev;
            panelRevisionTomaMuestra.InnerValue = rev;
        }

        private void GenerarPanelRevisionTomaMuestra()
        {
            panelRevisionTomaMuestra.Build(Revision,
                new TypePanelSettings<RevisionOferta>
                {
                    Fields = new FieldSettings
                    {

                        ["Num"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Número revisión")
                            .SetEnabled(false),
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
                        //["Importe"] = PropertyControlSettingsEnum.TextBoxDefault,
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        ReadOnly = (Revision.Enviada || Revision.Aceptada),
                    }
                });
        }

        private void GenerarAddTipoMuestra()
        {
            UCTipoMuestra.Editable = (!Revision.Enviada && !Revision.Aceptada);
            UCTipoMuestra.LineasTipoMuestra = LineasTipoMuestra;
        }

        private void GenerarAddParametros()
        {
            if (LineasPuntosControl.Count == 0)
                AddElementLineasPuntosControl(new ControlLineaParametro { PuntoControl = new IPuntoControl { } });
            else {
                Reload(LineasPuntosControl);
            }
            LineasPuntosControl.ForEach(l => l.Editable = (!Revision.Enviada && !Revision.Aceptada));
        }

        private void GenerarPanelPeticionCondiciones()
        {
            panelRevisionCondiciones.Build(Revision,
                new TypePanelSettings<RevisionOferta>
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
                                .SetEnabled(false)
                                .SetReadOnly(true),
                        ["FechaEmision"] = PropertyControlSettingsEnum.DateTimeDefaultNoEmpty
                                .SetLabel("* Fecha de emisión"),
                        ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                                                    .SetHeightMultiline(90)
                                                    .SetColumnSpan(2),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        ReadOnly = (Revision.Enviada || Revision.Aceptada),
                    }
                });
            ExecuteRefreshFrecuencia();
            ExecuteRefreshTomaMuestra();
        }

        private void CargarTipoMuestra()
        {
            TipoMuestraRevision[] lineas = PersistenceManager.SelectByProperty<TipoMuestraRevision>("IdRevision", Revision.Id).ToArray();
            lineas.ForEach(l =>
            {
                ITipoMuestra t = new ITipoMuestra
                {
                    Id = l.Id,
                    IdTipoMuestra = l.IdTipoMuestra,
                    IdRelacion = l.IdRevision
                };
                LineasTipoMuestra.Add(t);
            });

            ListaTiposMuestra = lineas.Select(l => l.IdTipoMuestra).ToArray();
        }

        private void CargarParametros()
        {
            CargarParametros(Revision, true);
        }

        public void CargarParametros(RevisionOferta r, bool withId = false)
        {
            LineasPuntosControl = Util.GetParametrosFromRevision(r.Id, withId);
            ActualizarValores(ListaTiposMuestra);
        }

        public void CargarParametros(int idRevision)
        {
            LineasPuntosControl = Util.GetParametrosFromRevision(idRevision);
            ActualizarValores(ListaTiposMuestra);
        }

        public void CargarParametros(Peticion p)
        {
            LineasPuntosControl = Util.GetParametrosFromPeticion(p.Id);
            ActualizarValores(ListaTiposMuestra);
        }

        public void CargarParametros(List<ControlLineaParametro> lineas)
        {
            listaPuntosControl.Children.Clear();
            lineas.ForEach(l =>
            {
                ControlLineaParametro c = new ControlLineaParametro { PuntoControl = l.PuntoControl.Clone(typeof(IPuntoControl)) as IPuntoControl };
                AddElementLineasPuntosControl(c);
            });

            ListaTiposMuestra = LineasTipoMuestra.Select(l => l.IdTipoMuestra).ToArray();
            ActualizarValores(ListaTiposMuestra);
        }

        public void CargarTipoMuestra(List<ITipoMuestra> lineas)
        {
            LineasTipoMuestra.Clear();
            lineas.ForEach(l => LineasTipoMuestra.Add(l));
            ListaTiposMuestra = lineas.Select(l => l.IdTipoMuestra).ToArray();
        }

        private void RefreshFrecuencia(object sender, SelectionChangedEventArgs e)
        {
            ExecuteRefreshFrecuencia();
        }

        private void ExecuteRefreshFrecuencia()
        {
            RevisionOferta r = (RevisionOferta)panelRevisionCondiciones.InnerValue;
            bool puntual = r.TrabajoPuntual ?? false;

            if (puntual)
                panelRevisionCondiciones["Frecuencia"].SetInnerContent(null);
            panelRevisionCondiciones["Frecuencia"].Enabled = !puntual;

        }

        private void RefreshTomaMuestra(object sender, SelectionChangedEventArgs e)
        {
            ExecuteRefreshTomaMuestra();
        }

        private void ExecuteRefreshTomaMuestra()
        {
            RevisionOferta r = (RevisionOferta)panelRevisionTomaMuestra.InnerValue;

            bool requiereToma = r.RequiereTomaMuestra ?? false;

            if (!requiereToma)
            {
                panelRevisionTomaMuestra["LugarMuestra"].SetInnerContent(null);
                panelRevisionTomaMuestra["NumPuntosMuestreo"].SetInnerContent(null);
            }
            panelRevisionTomaMuestra["LugarMuestra"].Enabled = requiereToma;
            panelRevisionTomaMuestra["NumPuntosMuestreo"].Enabled = requiereToma;
        }

        public bool ValidarRevision()
        {
            return (panelRevisionTomaMuestra.GetValidatedInnerValue<RevisionOferta>() != default(RevisionOferta) &&
                panelRevisionCondiciones.GetValidatedInnerValue<RevisionOferta>() != default(RevisionOferta));
        }

        private void NuevoControl_Click(object sender, RoutedEventArgs e)
        {
            AddElementLineasPuntosControl(new ControlLineaParametro { PuntoControl = new IPuntoControl { } });
        }
    }
}
