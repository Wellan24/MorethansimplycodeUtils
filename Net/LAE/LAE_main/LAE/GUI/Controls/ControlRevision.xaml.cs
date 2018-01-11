using Cartif.Extensions;
using Cartif.Util;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Modelo;
using Persistence;
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

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlRevision.xaml
    /// </summary>
    public partial class ControlRevision : UserControl
    {
        public ObservableCollection<ITipoMuestra> lineasTipoMuestra;
        public ObservableCollection<ILineasParametros> lineasParametros;

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
            UCLineaParametro.panelParametros.ClearGrid();
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

        public void CargarNuevaRevision(RevisionOferta rev)
        {
            revision = rev;
            panelRevisionCondiciones.InnerValue = rev;
            panelRevisionTomaMuestra.InnerValue = rev;
        }

        private void GenerarPanelRevisionTomaMuestra()
        {
            panelRevisionTomaMuestra.Build<RevisionOferta>(Revision,
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
                        ["Importe"] = PropertyControlSettingsEnum.TextBoxDefault,
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        ReadOnly = Revision.Enviada,
                    }
                });
        }

        private void GenerarAddTipoMuestra()
        {
            UCTipoMuestra.Editable = !Revision.Enviada;
            UCTipoMuestra.LineasTipoMuestra = lineasTipoMuestra;
        }

        private void GenerarAddParametros()
        {
            UCLineaParametro.Editable = !Revision.Enviada;
            UCLineaParametro.LineasParametros = lineasParametros;
        }

        private void GenerarPanelPeticionCondiciones()
        {
            panelRevisionCondiciones.Build<RevisionOferta>(Revision,
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
                        ["PlazoRealizacion"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Tecnicos)
                                .SetLabel("* Técnico"),
                        ["FechaEmision"] = PropertyControlSettingsEnum.DateTimeDefaultNoEmpty
                                .SetLabel("* Fecha de emisión"),
                        ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                                                    .SetHeightMultiline(90)
                                                    .SetColumnSpan(2),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        ReadOnly = Revision.Enviada,
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
                lineasTipoMuestra.Add(t);
            });

            UCLineaParametro.TiposMuestraSeleccionados = lineas.Select(l => l.IdTipoMuestra).ToArray();

        }

        private void CargarParametros()
        {
            LineasRevisionOferta[] lineas = PersistenceManager.SelectByProperty<LineasRevisionOferta>("IdRevisionOferta", Revision.Id).ToArray();
            lineas.ForEach(l =>
            {
                ILineasParametros t = new ILineasParametros
                {
                    Id = l.Id,
                    Cantidad = l.Cantidad,
                    IdParametro = l.IdParametro,
                    IdRelacion = l.IdRevisionOferta
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

    }
}
