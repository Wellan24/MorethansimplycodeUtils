using Cartif.Expectation;
using Cartif.Extensions;
using Cartif.Util;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Calculos;
using LAE.Clases;
using LAE.Modelo;
using Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace GUI.Pages
{
    /// <summary>
    /// Lógica de interacción para HumedadTotalViejo.xaml
    /// </summary>
    public partial class PageHumedadTotalViejo : UserControl
    {
        private MedicionPNT medicion;
        public MedicionPNT Medicion
        {
            get { return medicion; }
            set
            {
                medicion = value;
                medicion.AddFechaFin = true;
                UCMedicion.Medicion = Medicion;
            }
        }

        private HumedadTotal humedad;
        public HumedadTotal Humedad
        {
            get { return humedad; }
            set
            {
                humedad = value;
                CargarHumedad();
                RealizarCalculo();
            }
        }
        
        public Action<Valor> CambioHumedadTotal { get; set; }

        public PageHumedadTotalViejo()
        {
            InitializeComponent();
            //DatosHumedad();
        }

        //private void DatosHumedad()
        //{
        //    HumedadTotal prueba = new HumedadTotal() { M4 = 472.7, IdUdsM4 = 12, M5 = 472.5, IdUdsM5 = 12, IdVProcedimiento = 1 };
        //    prueba.Replicas = new List<ReplicaHumedadTotal>();
        //    prueba.Replicas.Add(new ReplicaHumedadTotal() { M1 = 443.6, IdUdsM1 = 12, M2 = 796.2, IdUdsM2 = 12, M3 = 769, IdUdsM3 = 12, Num = 1, Valido = true });
        //    prueba.Replicas.Add(new ReplicaHumedadTotal() { M1 = 453.6, IdUdsM1 = 12, M2 = 801.3, IdUdsM2 = 12, M3 = 774.5, IdUdsM3 = 12, Num = 2, Valido = true });
        //    Humedad = prueba;
        //}

        private void CargarHumedad()
        {
            panelHumedad.Build(Humedad,
                new TypePanelSettings<HumedadTotal>
                {
                    ColumnWidths = new int[] { 3, 3, 1, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(Humedad.IdParametro))
                            .SetLabel("Versión"),
                        ["M4"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2084"),
                        ["IdUdsM4"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["M5"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2085"),
                        ["IdUdsM5"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                    },
                    IsUpdating = true,
                    PanelValidation = Expectation<HumedadTotal>
                        .ShouldNotBe().AddCriteria(h => h.M4 == null)
                        .NotBe().AddCriteria(h => h.M5 == null),
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => RealizarCalculo(),
                        SelectionChanged = (s, e) => RealizarCalculo()
                    }
                });

            panelHumedad["IdVProcedimiento"].SelectedIndex = 0;

            foreach (ReplicaHumedadTotal replica in Humedad.Replicas)
                CrearPanelReplica(replica);


            panelCalculos.Build(Humedad,
                new TypePanelSettings<HumedadTotal>
                {
                    Fields = new FieldSettings
                    {
                        ["MediaHumedadTotal2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Valor Medio")
                                .SetReadOnly(true),
                        ["CV2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("CV")
                                .SetReadOnly(true),
                    }
                });
        }

        private void CrearPanelReplica(ReplicaHumedadTotal replica)
        {
            TypePanel panelReplica = new TypePanel();
            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaHumedadTotal>
                {
                    ColumnWidths = new int[] { 3, 3, 1, 3, 1, 3, 1, 3 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("Replica" + replica.Num)
                            .AddCheckedChanged((s, e) => RealizarCalculo()),
                        ["M1"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2081"),
                        ["IdUdsM1"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["M2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2082"),
                        ["IdUdsM2"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["M3"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2083"),
                        ["IdUdsM3"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["HumedadTotal2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Humedad total(%)")
                            .AddTextChanged((s, e) => { })
                            .SetReadOnly(true),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => RealizarCalculo(),
                        SelectionChanged = (s, e) => RealizarCalculo()
                    },
                    PanelValidation = Expectation<ReplicaHumedadTotal>
                        .ShouldNotBe().AddCriteria(h => h.M1 == null)
                        .NotBe().AddCriteria(h => h.M2 == null)
                        .NotBe().AddCriteria(h => h.M3 == null)
                });
            listaReplicas.Children.Add(panelReplica);
        }

        private void RealizarCalculo()
        {
            if (panelHumedad.GetValidatedInnerValue<HumedadTotal>() != default(HumedadTotal))
            {

                Valor m4 = Valor.Of(Humedad.M4, Humedad.IdUdsM4 ?? 0);
                Valor m5 = Valor.Of(Humedad.M5, Humedad.IdUdsM5 ?? 0);

                listaReplicas.Children.OfType<TypePanel>().ForEach(tp =>
                {
                    ReplicaHumedadTotal replica = tp.InnerValue as ReplicaHumedadTotal;
                    if (tp.GetValidatedInnerValue<ReplicaHumedadTotal>() != default(ReplicaHumedadTotal))
                    {

                        Valor m1 = Valor.Of(replica.M1, replica.IdUdsM1 ?? 0);
                        Valor m2 = Valor.Of(replica.M2, replica.IdUdsM2 ?? 0);
                        Valor m3 = Valor.Of(replica.M3, replica.IdUdsM3 ?? 0);
                        replica.HumedadTotal = Calcular.HumedadTotal_8_1(m1, m2, m3, m4, m5)?.Value;

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
                    panelCalculos.Clear();
                    labelAceptacion.Visibility = Visibility.Collapsed;
                    UCInforme.panelResultado.Clear();
                    CambioHumedadTotal(null);
                }
                else
                {
                    Valor[] valoresHumedades = Humedad.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.HumedadTotal, "%")).ToArray();
                    Humedad.MediaHumedadTotal = Calcular.Promedio(valoresHumedades).Value;
                    Humedad.CV = Calcular.CoeficienteVariacion(valoresHumedades).Value;
                    Humedad.Aceptado = Calcular.EsAceptado(Humedad.CV ?? 0, Humedad.IdVProcedimiento, Humedad.IdParametro, Humedad.MediaHumedadTotal);

                    panelCalculos["MediaHumedadTotal2"].SetInnerContent(Calcular.VisualizeDecimals(Humedad.MediaHumedadTotal, 2));
                    panelCalculos["CV2"].SetInnerContent(Calcular.VisualizeDecimals(Humedad.CV, 2));

                    labelAceptacion.Aceptacion(Humedad.Aceptado);

                    ResultadoInforme resultado = Calcular.Resultado(Humedad.MediaHumedadTotal ?? 0, Humedad.IdVProcedimiento, Humedad.IdParametro, 1, 2);

                    UCInforme.Resultado = resultado;

                    Valor v = Valor.Of(Humedad.MediaHumedadTotal, "%");
                    CambioHumedadTotal(v);
                }
            }
            else
            {
                panelCalculos.Clear();
                labelAceptacion.Visibility = Visibility.Collapsed;
                UCInforme.panelResultado.Clear();
            }
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            int idGramos = Unidad.Of("Gramos").Id;

            ReplicaHumedadTotal replica = new ReplicaHumedadTotal()
            {
                IdUdsM1 = idGramos,
                IdUdsM2 = idGramos,
                IdUdsM3 = idGramos,
                Valido = true,
                Num = Humedad.Replicas[Humedad.Replicas.Count - 1].Num + 1
            };

            int nCont = Humedad.Replicas.Count;
            replica.Num = (nCont == 0) ? 1 : Humedad.Replicas[nCont - 1].Num + 1;

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
    }
}
