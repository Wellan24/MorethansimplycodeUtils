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
        }

        public ControlRevision()
        {
            InitializeComponent();
            lineasTipoMuestra = new ObservableCollection<ITipoMuestra>() { new ITipoMuestra() };
            lineasParametros = new ObservableCollection<ILineasParametros>() { new ILineasParametros() };

        }

        private void GenerarPanelRevisionTomaMuestra()
        {
            panelRevisionTomaMuestra.Build<RevisionOferta>(Revision,
                new TypePanelSettings<RevisionOferta>
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
                            Label = "Toma de Muestra"
                        },
                        ["LugarMuestra"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Lugar de toma de muestra"),
                        ["NumPuntosMuestreo"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Nº de puntos de muestreo"),
                        ["Num"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Número revisión"),
                        ["Importe"] = PropertyControlSettingsEnum.TextBoxDefault,
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
                                .SetLabel("Técnico"),
                        ["FechaEmision"] = PropertyControlSettingsEnum.DateTimeDefault,
                        ["Objeto"] = PropertyControlSettingsEnum.TextBoxDefault
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
            TipoMuestraRevision[] lineas = PersistenceManager<TipoMuestraRevision>.SelectByProperty("IdRevision", Revision.Id).ToArray();
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

        }

        private void CargarParametros()
        {
            LineasRevisionOferta[] lineas = PersistenceManager<LineasRevisionOferta>.SelectByProperty("IdRevisionOferta", Revision.Id).ToArray();
            lineas.ForEach(l =>
            {
                ILineasParametros t = new ILineasParametros
                {
                    Id = l.Id,
                    Cantidad = l.Cantidad,
                    Metodo = l.Metodo,
                    IdParametro = l.IdParametro,
                    IdRelacion = l.IdRevisionOferta
                };
                lineasParametros.Add(t);
            });

        }

        public void CargarTipoMuestra(ObservableCollection<ITipoMuestra> lineas)
        {
            lineas.ForEach(l => lineasTipoMuestra.Add(l));
        }

        public void CargarParametro(ObservableCollection<ILineasParametros> lineas)
        {
            lineas.ForEach(l => lineasParametros.Add(l));
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

        public bool ValidarRevision()
        {
            return panelRevisionCondiciones.GetValidatedInnerValue<RevisionOferta>() != default(RevisionOferta);
        }

    }
}
