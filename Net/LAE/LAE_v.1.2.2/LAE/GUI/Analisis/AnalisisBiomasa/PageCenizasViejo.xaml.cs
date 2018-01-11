using Cartif.Expectation;
using Cartif.Extensions;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Calculos;
using LAE.Clases;
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
    /// Lógica de interacción para PageCenizasViejo.xaml
    /// </summary>
    public partial class PageCenizasViejo : UserControl
    {

        private MedicionPNT medicion;
        public MedicionPNT Medicion
        {
            get { return medicion; }
            set
            {
                medicion = value;
                UCMedicion.Medicion = Medicion;
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
                RealizarCalculo();
            }
        }

        public PageCenizasViejo()
        {
            InitializeComponent();
            //DatosCenizas();
        }

        //private void DatosCenizas()
        //{
        //    Cenizas prueba = new Cenizas() { IdVProcedimiento = 3 };
        //    prueba.Replicas = new List<ReplicaCeniza>();
        //    prueba.Replicas.Add(new ReplicaCeniza() { M1 = 78.5465, IdUdsM1 = 12, M2 = 85.3625, IdUdsM2 = 12, M3 = 78.5738, IdUdsM3 = 12, Num = 1, Valido = true });
        //    prueba.Replicas.Add(new ReplicaCeniza() { M1 = 60.785, IdUdsM1 = 12, M2 = 66.235, IdUdsM2 = 12, M3 = 60.8144, IdUdsM3 = 12, Num = 2, Valido = true });
        //    Cenizas = prueba;
        //} 

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

            panelCalculos.Build(Cenizas,
                new TypePanelSettings<Cenizas>
                {
                    Fields = new FieldSettings
                    {
                        ["MediaCenizasHumeda2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Media b.h.")
                                .SetReadOnly(true),
                        ["MediaCenizasSeca2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Media b.s.")
                                .SetReadOnly(true),
                        ["Dif2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetReadOnly(true)
                            .SetLabel("Dif."),
                    }
                });
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
                            .AddCheckedChanged(CambioReplica),
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
                        TextChanged = CambioTexto
                    },
                    PanelValidation = Expectation<ReplicaCeniza>
                        .ShouldNotBe().AddCriteria(h => h.M1 == null)
                        .NotBe().AddCriteria(h => h.M2 == null)
                        .NotBe().AddCriteria(h => h.M3 == null)
                });
            listaReplicas.Children.Add(panelReplica);
        }

        private void CambioReplica(object sender, RoutedEventArgs e)
        {
            RealizarCalculo();
        }

        private void CambioTexto(object sender, TextChangedEventArgs e)
        {
            RealizarCalculo();
        }
        private void RealizarCalculo()
        {
            //double humedadTotal = 7.6538377;/*temporal - abra que obtenerlo de la page: humedad total*/

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
                panelCalculos.Clear();
                labelAceptacion.Visibility = Visibility.Collapsed;
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

                panelCalculos["MediaCenizasHumeda2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.MediaCenizasHumeda, 2));
                panelCalculos["MediaCenizasSeca2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.MediaCenizasSeca, 2));
                panelCalculos["Dif2"].SetInnerContent(Calcular.VisualizeDecimals(Cenizas.MediaCenizasSeca, 2));

                labelAceptacion.Aceptacion(Cenizas.Aceptado);

                ResultadoInforme resultado = Calcular.Resultado(Cenizas.MediaCenizasHumeda ?? 0, Cenizas.IdVProcedimiento, Cenizas.IdParametro, 1, 1);
                UCInforme.Resultado = resultado;
            }
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
    }
}
