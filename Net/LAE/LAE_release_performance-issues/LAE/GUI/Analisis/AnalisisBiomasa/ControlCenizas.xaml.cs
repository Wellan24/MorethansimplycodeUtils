using Cartif.Expectation;
using Cartif.Extensions;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Calculos;
using LAE.Modelo;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI.Analisis
{
    /// <summary>
    /// Lógica de interacción para ControlCenizas.xaml
    /// </summary>
    public partial class ControlCenizas : UserControl, IMedicion
    {

        private MedicionPNT medicion;
        public MedicionPNT Medicion
        {
            get { return medicion; }
            set
            {
                medicion = value;
                UCMedicion.Medicion = Medicion;
                Cenizas = FactoriaCenizas.GetParametro(medicion.Id) ?? FactoriaCenizas.GetDefault();

            }
        }

        private Cenizas cenizas;
        public Cenizas Cenizas
        {
            get { return cenizas; }
            set
            {
                cenizas = value;
                CargarCenizas();
                UCEquipos.GenerarPanel(Medicion, Cenizas.IdParametro);
                RealizarCalculo();
            }
        }

        public Action Calculo { get; set; }

        public ControlCenizas()
        {
            InitializeComponent();
        }

        private void CargarCenizas()
        {

            panelCenizas.Build(Cenizas,
                new TypePanelSettings<Cenizas>
                {
                    Fields = new FieldSettings
                    {
                        ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(Cenizas.IdParametro))
                            .SetLabel("Versión"),
                        ["IdHumedad3"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaHumedad3.GetHumedades(Medicion.IdMuestra))
                            .SetLabel("Humedad")
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        SelectionChanged = (s, e) => RealizarCalculo()
                    }
                });
            panelCenizas["IdVProcedimiento"].SelectedIndex = 0;

            foreach (ReplicaCeniza replica in Cenizas.Replicas)
                CrearPanelReplica(replica);

            UCCalculo.Cenizas = Cenizas;

        }

        private void CrearPanelReplica(ReplicaCeniza replica)
        {
            TypePanel panelReplica = new TypePanel();
            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaCeniza>
                {
                    ColumnWidths = new int[] { 3, 3, 1, 3, 1, 3, 1, 3 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("Replica" + replica.Num)
                            .AddCheckedChanged((s, e) => RealizarCalculo()),
                        ["M1"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2081"),
                        ["IdUdsM1"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["M2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2082"),
                        ["IdUdsM2"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["M3"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2083"),
                        ["IdUdsM3"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["Cenizas2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Cenizas(%)")
                            .SetReadOnly(true)
                            .AddTextChanged((s, e) => { }),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => RealizarCalculo(),
                        SelectionChanged = (s, e) => RealizarCalculo()
                    },
                    PanelValidation = Expectation<ReplicaCeniza>
                        .ShouldNotBe().AddCriteria(h => h.M1 == null)
                        .NotBe().AddCriteria(h => h.M2 == null)
                        .NotBe().AddCriteria(h => h.M3 == null)
                });
            listaReplicas.Children.Add(panelReplica);
        }

        private void RealizarCalculo()
        {

            listaReplicas.Children.OfType<TypePanel>().ForEach(tp =>
            {
                ReplicaCeniza replica = tp.InnerValue as ReplicaCeniza;
                if (tp.GetValidatedInnerValue<ReplicaCeniza>() != default(ReplicaCeniza))
                {
                    Valor m1 = Valor.Of(replica.M1, replica.IdUdsM1 ?? 0);
                    Valor m2 = Valor.Of(replica.M2, replica.IdUdsM2 ?? 0);
                    Valor m3 = Valor.Of(replica.M3, replica.IdUdsM3 ?? 0);
                    replica.Cenizas = Calcular.Cenizas_8_3(m1, m2, m3)?.Value;

                    tp["Cenizas2"].SetInnerContent(Calcular.VisualizeDecimals(replica.Cenizas, 2));
                }
                else
                {
                    replica.Cenizas = null;
                    tp["Cenizas2"].SetInnerContent(String.Empty);
                }

            });


            if ((Cenizas.Replicas.Exists(r => r.Valido == true && r.Cenizas == null) || Cenizas.Replicas.Where(r => r.Valido == true).Count() == 0) || panelCenizas.GetValidatedInnerValue<Cenizas>() == default(Cenizas))
            {
                UCCalculo.Clear();
                UCInforme.panelResultado.Clear();
            }
            else
            {
                Valor[] valoresCenizas = Cenizas.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.Cenizas, "%")).ToArray();
                double humedadTotal = PersistenceManager.SelectByID<Humedad3>(Cenizas.IdHumedad3).MediaHumedadTotalCalculado ?? 0;

                Cenizas.MediaCenizasHumeda = Calcular.Promedio(valoresCenizas).Value;
                Cenizas.MediaCenizasSeca = Cenizas.MediaCenizasHumeda * (100.0 / (100.0 - humedadTotal));
                Cenizas.Dif = Calcular.DiferenciaAbsoluta(valoresCenizas).Value;

                Cenizas.Aceptado = Calcular.EsAceptado(Cenizas.Dif ?? 0, Cenizas.IdVProcedimiento, Cenizas.IdParametro, Cenizas.MediaCenizasHumeda);

                UCCalculo.Cenizas = Cenizas;

                ResultadoInforme resultado = Calcular.Resultado(Cenizas.MediaCenizasHumeda ?? 0, Cenizas.IdVProcedimiento, Cenizas.IdParametro, 1, 1);
                UCInforme.Resultado = resultado;
            }
            Calculo();
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            ReplicaCeniza replica = new ReplicaCeniza()
            {
                IdUdsM1 = 12,
                IdUdsM2 = 12,
                IdUdsM3 = 12,
                Valido = true,
                Num = Cenizas.Replicas[Cenizas.Replicas.Count - 1].Num + 1
            };
            Cenizas.Replicas.Add(replica);
            CrearPanelReplica(replica);

            RealizarCalculo();
        }

        private void Deletereplica_Click(object sender, RoutedEventArgs e)
        {
            if (Cenizas.Replicas.Count > 0)
            {
                Cenizas.Replicas.RemoveAt(Cenizas.Replicas.Count - 1);
                listaReplicas.Children.RemoveAt(listaReplicas.Children.Count - 1);

                RealizarCalculo();
            }
        }

        public void RecargarHumedad()
        {
            int? n = panelCenizas["IdHumedad3"].SelectedIndex;
            panelCenizas["IdHumedad3"].InnerValues = FactoriaHumedad3.GetHumedades(Medicion.IdMuestra);
            panelCenizas["IdHumedad3"].SelectedIndex = n;
        }

        public void BorrarMedicion()
        {
            UCMedicion.Medicion.Id = 0;
            Cenizas.Id = 0;
            Cenizas.Replicas.ForEach(r => r.Id = 0);

            /*Limpio el panel de equipos*/
            UCEquipos.GenerarPanel(Medicion, Cenizas.IdParametro);
        }


    }
}
