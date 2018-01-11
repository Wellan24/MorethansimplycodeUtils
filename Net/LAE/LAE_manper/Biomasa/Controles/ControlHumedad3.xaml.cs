using Cartif.Expectation;
using Cartif.Extensions;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Comun.Calculos;
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Biomasa.Modelo;
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

namespace LAE.Biomasa.Controles
{
    /// <summary>
    /// Lógica de interacción para ControlHumedad3.xaml
    /// </summary>
    public partial class ControlHumedad3 : UserControl, IMedicion
    {

        private MedicionPNT medicion;
        public MedicionPNT Medicion
        {
            get { return medicion; }
            set
            {
                medicion = value;
                UCMedicion.Medicion = Medicion;
                Humedad = FactoriaHumedad3.GetParametro(medicion.Id) ?? FactoriaHumedad3.GetDefault();

            }
        }

        private Humedad3 humedad;
        public Humedad3 Humedad
        {
            get { return humedad; }
            set
            {
                if (humedad == null)
                {
                    humedad = value;
                    CargarHumedad();
                    UCEquipos.GenerarPanel(Medicion, Humedad.IdParametro);
                }
                else
                {
                    humedad = value;
                }
                RealizarCalculo();
            }
        }

        public Action Calculo { get; set; }

        public ControlHumedad3()
        {
            InitializeComponent();
        }

        private void CargarHumedad()
        {
            panelHumedad.Build(Humedad,
                new TypePanelSettings<Humedad3>
                {
                    Fields = new FieldSettings
                    {
                        ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(Humedad.IdParametro))
                            .SetLabel("Versión")
                            .AddSelectionChanged((s, e) => RealizarCalculo()),
                    },
                    IsUpdating = true
                });
            panelHumedad["IdVProcedimiento"].SelectedIndex = 0;

            foreach (ReplicaHumedad3 replica in Humedad.Replicas)
                CrearPanelReplica(replica);

            UCCalculo.Humedad = Humedad;
        }

        private void CrearPanelReplica(ReplicaHumedad3 replica)
        {
            TypePanel panelReplica = new TypePanel();
            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaHumedad3>
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
                        ["HumedadTotal2"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                            .SetLabel("Humedad total(%)")
                            .AddTextChanged((s, e) => { }),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => RealizarCalculo(),
                        SelectionChanged = (s, e) => RealizarCalculo()
                    },
                    PanelValidation = Expectation<ReplicaHumedad3>
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
                ReplicaHumedad3 replica = tp.InnerValue as ReplicaHumedad3;
                if (tp.GetValidatedInnerValue<ReplicaHumedad3>() != default(ReplicaHumedad3))
                {
                    Valor m1 = Valor.Of(replica.M1, replica.IdUdsM1 ?? 0);
                    Valor m2 = Valor.Of(replica.M2, replica.IdUdsM2 ?? 0);
                    Valor m3 = Valor.Of(replica.M3, replica.IdUdsM3 ?? 0);
                    replica.HumedadTotal = Calcular.Humedad3_8_11(m1, m2, m3)?.Value;

                    tp["HumedadTotal2"].SetInnerContent(Calcular.VisualizeDecimals(replica.HumedadTotal, 2));
                }
                else
                {
                    replica.HumedadTotal = null;
                    tp["HumedadTotal2"].SetInnerContent(String.Empty);
                }
            });

            if (Humedad.Replicas.Exists(r => r.Valido == true && r.HumedadTotal == null) || Humedad.Replicas.Where(r => r.Valido == true).Count() == 0)
            {
                UCCalculo.Clear();
            }
            else
            {
                Valor[] valoresHumedad = Humedad.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.HumedadTotal, "%")).ToArray();
                Humedad.MediaHumedadTotal = Calcular.Promedio(valoresHumedad).Value;
                Humedad.Diferencia = Calcular.DiferenciaAbsoluta(valoresHumedad).Value;

                Humedad.Aceptado = Calcular.EsAceptado(Humedad.Diferencia ?? 0, Humedad.IdVProcedimiento, Humedad.IdParametro, Humedad.MediaHumedadTotal);

                UCCalculo.Humedad = Humedad;
            }
            Calculo();
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            int idGramos = Unidad.Of("Gramos").Id;

            ReplicaHumedad3 replica = new ReplicaHumedad3()
            {
                IdUdsM1 = idGramos,
                IdUdsM2 = idGramos,
                IdUdsM3 = idGramos,
                Valido = true,
                Num = Humedad.Replicas[Humedad.Replicas.Count - 1].Num + 1
            };
            Humedad.Replicas.Add(replica);
            CrearPanelReplica(replica);

            RealizarCalculo();
        }

        private void Deletereplica_Click(object sender, RoutedEventArgs e)
        {
            if (Humedad.Replicas.Count > 0)
            {
                Humedad.Replicas.RemoveAt(Humedad.Replicas.Count - 1);
                listaReplicas.Children.RemoveAt(listaReplicas.Children.Count - 1);

                RealizarCalculo();
            }
        }

        public void BorrarMedicion()
        {
            UCMedicion.Medicion.Id = 0;
            Humedad.Id = 0;
            Humedad.Replicas.ForEach(r => r.Id = 0);

            /*Limpio el panel de equipos*/
            UCEquipos.GenerarPanel(Medicion, Humedad.IdParametro);
        }



    }
}
