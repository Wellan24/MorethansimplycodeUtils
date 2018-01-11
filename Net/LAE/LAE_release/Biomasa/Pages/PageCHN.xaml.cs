using Cartif.Expectation;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Comun.Calculos;
using LAE.Comun.Persistence;
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
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Modelo;
using Cartif.Extensions;
using LAE.Biomasa.Modelo;
using LAE.Comun.Clases;
using LAE.Biomasa.Controles;

namespace LAE.Biomasa.Pages
{
    /// <summary>
    /// Lógica de interacción para PageCHN.xaml
    /// </summary>
    public partial class PageCHN : UserControl
    {
        private Chn paramCHN;
        public Chn ParamCHN
        {
            get { return paramCHN; }
            set
            {
                paramCHN = value;
                CargarCHN();
                RealizarCalculo();
            }
        }

        private const double hidro = 8.937;

        public PageCHN()
        {
            InitializeComponent();
            GenerarPanelCalculo();
        }

        private void GenerarPanelCalculo()
        {
            panelCalculos.Build(new Chn(),
                new TypePanelSettings<Chn>
                {
                    ColumnWidths = new int[] { 1, 1, 1, 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["MediaPorcentajeHu3C"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media b.h.(HU3)"),
                        ["MediaPorcentajeSecaC"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media b.s."),
                        ["MediaPorcentajeHu3H"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media b.h.(HUM)"),
                        ["MediaPorcentajeSecaH"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media b.s."),
                        ["MediaPorcentajeHu3N"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media b.h.(HUM)"),
                        ["MediaPorcentajeSecaN"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media b.s."),
                        ["MediaPorcentajeHumC"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media b.h.(HUM)"),
                        ["CV_C"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("CV"),
                        ["MediaPorcentajeHumH"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media b.h.(HUM)"),
                        ["CV_H"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("CV"),
                        ["MediaPorcentajeHumN"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media b.h.(HUM)"),
                        ["CV_N"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("CV"),
                    },
                });
        }

        private void CargarCHN()
        {
            panelCHN.Build(ParamCHN,
                new TypePanelSettings<Chn>
                {
                    Fields = new FieldSettings
                    {
                        ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(paramCHN["C"].IdParametro))
                            .SetLabel("Versión"),
                        ["Finalizado"] = PropertyControlSettingsEnum.CheckBoxDefault,
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        SelectionChanged = (s, e) => RealizarCalculo()
                    }
                });

            panelCHN["IdVProcedimiento"].SelectedIndex = 0;

            foreach (ReplicaChn replica in ParamCHN.Replicas)
            {
                CrearPanelReplica(replica);
            }
        }

        private void CrearPanelReplica(ReplicaChn replica)
        {
            TypePanel panelReplica = new TypePanel();
            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaChn>
                {
                    ColumnWidths = new int[] { 1, 2, 1, 2, 1, 2, 1, 2 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefaultSmall
                            .SetLabel("Replica" + replica.Num)
                            .AddCheckedChanged((s, e) => RealizarCalculo()),
                        ["PorcentajeC"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("(%)b.h."),
                        ["IdUdsPorcentajeC"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Porcentaje"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["PorcentajeH"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("(%)b.h."),
                        ["IdUdsPorcentajeH"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Porcentaje"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["PorcentajeN"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("(%)b.h."),
                        ["IdUdsPorcentajeN"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Porcentaje"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["IdEnsayo"] = PropertyControlSettingsEnum.ComboBoxDefault
                        .SetLabel("Ensayo")
                        .SetInnerValues(FactoriaEnsayoPNT.GetEnsayos(ParamCHN.IdMuestra, "Analizador elemental"))
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => RealizarCalculo(),
                        SelectionChanged = (s, e) => RealizarCalculo()
                    }
                });

            listaReplicas.Children.Add(panelReplica);
        }

        private void RealizarCalculo()
        {
            if (panelCHN.GetValidatedInnerValue<Chn>() != default(Chn))
            {

                if (ParamCHN.Replicas.Where(r => r.Valido == true).Count() == 0)
                {
                    VaciarCalculo("C");
                    VaciarCalculo("H");
                    VaciarCalculo("N");
                }
                else if (ParamCHN.Replicas.Any(r => r.Valido == true && (r.IdEnsayo ?? 0) <= 0))
                {
                    Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeC == null), "C", r => r.PorcentajeC);
                    Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeH == null), "H", r => r.PorcentajeH);
                    Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeN == null), "N", r => r.PorcentajeN);
                }
                else
                {
                    Func<Double?, Double?, Double?> funcCNSeca = (porc, hum3) => porc * (100.0 / (100.0 - hum3));
                    Func<Double?, Double?, Double?> funcHSeca = (porc, hum3) => (porc - hum3 / hidro) * (100.0 / (100.0 - hum3));
                    Func<Double?, Double?, Double?> funcCNHumeda = (porc, hum) => porc * ((100.0 - hum) / 100.0);
                    Func<Double?, Double?, Double?> funcHHumeda = (porc, hum) => (porc * ((100 - hum) / 100.0)) + (hum / hidro);

                    MuestraEnsayo[] muestrasEnsayo = PersistenceManager.SelectByProperty<MuestraEnsayo>("IdMuestra", ParamCHN.IdMuestra).ForEach(m => m.LoadHumedad()).ToArray();

                    Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeC == null), "C", r => r.PorcentajeC, funcCNSeca, funcCNHumeda, muestrasEnsayo);
                    Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeH == null), "H", r => r.PorcentajeH, funcHSeca, funcHHumeda, muestrasEnsayo);
                    Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeN == null), "N", r => r.PorcentajeN, funcCNSeca, funcCNHumeda, muestrasEnsayo);

                }
            }
        }

        private void Calculo(Boolean vaciar, String param, Func<ReplicaChn, double?> funcPorcentaje)
        {
            if (vaciar)
            {
                VaciarCalculo(param);
            }
            else
            {
                Valor[] valoresPorcentaje = ParamCHN.Replicas.Where(r => r.Valido == true)
                        .Select(funcPorcentaje)
                        .Select(v => Valor.Of(v, "%")).ToArray();
                RealizarCalculo(param, valoresPorcentaje);

            }
        }

        private void Calculo(Boolean vaciar, String param, Func<ReplicaChn, double?> funcPorcentaje, Func<double?, double?, double?> funcPorcentajeSeca, Func<double?, double?, double?> funcPorcentajeHumeda, MuestraEnsayo[] muestraEnsayo)
        {
            if (vaciar)
            {
                VaciarCalculo(param);
            }
            else
            {
                ValorHumedad[] valoresPorcentajeHumedad = ParamCHN.Replicas.Where(r => r.Valido == true)
                    .Select(r =>
                    {
                        MuestraEnsayo muestra = FactoriaMuestraEnsayo.GetMuestra(r.IdEnsayo ?? 0, ParamCHN.IdMuestra);
                        muestra.LoadHumedad();
                        return new ValorHumedad { Valor = Valor.Of(funcPorcentaje(r), "%"), Humedad = muestra.Humedad, Humedad3 = muestra.Humedad3 };
                    }).ToArray();

                RealizarCalculo(param, valoresPorcentajeHumedad, funcPorcentajeSeca, funcPorcentajeHumeda);

            }
        }

        private void RealizarCalculo(string param, Valor[] valoresPorcentaje)
        {
            ParamCHN[param].MediaPorcentajeHu3 = Calcular.Promedio(valoresPorcentaje).Value;
            panelCalculos["MediaPorcentajeHu3" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].MediaPorcentajeHu3, 2));
            VaciarCalculo(param, false);
        }

        private void RealizarCalculo(string param, ValorHumedad[] valoresPorcentajeHumedad, Func<double?, double?, double?> funcPorcentajeSeca, Func<double?, double?, double?> funcPorcentajeHumeda)
        {
            Valor[] valoresPorcentaje = valoresPorcentajeHumedad.Select(p => p.Valor).ToArray();
            ParamCHN[param].MediaPorcentajeHu3 = Calcular.Promedio(valoresPorcentaje).Value;
            ParamCHN[param].MediaPorcentajeSeca = Calcular.PromedioCorreccionHumedad(valoresPorcentajeHumedad, funcPorcentajeSeca)?.Value;
            ParamCHN[param].MediaPorcentajeHum = Calcular.PromedioCorreccionHumedad(valoresPorcentajeHumedad, funcPorcentajeSeca, funcPorcentajeHumeda)?.Value;
            ParamCHN[param].CV = Calcular.CoeficienteVariacion(valoresPorcentaje).Value;
            ParamCHN[param].Aceptado = Calcular.EsAceptado(ParamCHN[param].CV ?? 0, ParamCHN.IdVProcedimiento, ParamCHN[param].IdParametro, ParamCHN[param].MediaPorcentajeHu3);

            panelCalculos["MediaPorcentajeHu3" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].MediaPorcentajeHu3, 2));
            panelCalculos["MediaPorcentajeSeca" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].MediaPorcentajeSeca, 2));
            panelCalculos["MediaPorcentajeHum" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].MediaPorcentajeHum, 2));
            panelCalculos["CV_" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].CV, 2));

            if (ParamCHN[param].MediaPorcentajeSeca == null)
            {
                VaciarResultado(param);
            }
            else
            {
                labelAceptacion(param).Aceptacion(ParamCHN[param].Aceptado);
                ResultadoInforme resultado = Calcular.Resultado(ParamCHN[param].MediaPorcentajeSeca ?? 0, ParamCHN.IdVProcedimiento, ParamCHN[param].IdParametro, 2, 1);
                UCInforme(param).Resultado = resultado;
            }
        }

        private void VaciarCalculo(string param, bool humedad3Incluido = true)
        {
            if (humedad3Incluido)
                panelCalculos["MediaPorcentajeHu3" + param].SetInnerContent(String.Empty);
            panelCalculos["MediaPorcentajeSeca" + param].SetInnerContent(String.Empty);
            panelCalculos["MediaPorcentajeHum" + param].SetInnerContent(String.Empty);
            panelCalculos["CV_" + param].SetInnerContent(String.Empty);
            VaciarResultado(param);
        }

        private void VaciarResultado(string param)
        {
            UCInforme(param).panelResultado.Clear();
            labelAceptacion(param).Visibility = Visibility.Collapsed;
        }

        private Label labelAceptacion(String param)
        {
            switch (param)
            {
                case "C":
                    return labelAceptacionC;
                case "H":
                    return labelAceptacionH;
                case "N":
                    return labelAceptacionN;
                default:
                    return new Label();
            }
        }

        private ControlInforme UCInforme(String param)
        {
            switch (param)
            {
                case "C":
                    return UCInformeC;
                case "H":
                    return UCInformeH;
                case "N":
                    return UCInformeN;
                default:
                    return new ControlInforme();
            }
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            int idPorcentaje = Unidad.Of("%").Id;

            ReplicaChn replica = new ReplicaChn()
            {
                IdUdsPorcentajeC = idPorcentaje,
                IdUdsPorcentajeH = idPorcentaje,
                IdUdsPorcentajeN = idPorcentaje,
                Valido = true
            };
            int nCont = ParamCHN.Replicas.Count;
            replica.Num = (nCont == 0) ? 1 : ParamCHN.Replicas[nCont - 1].Num + 1;

            ParamCHN.Replicas.Add(replica);
            CrearPanelReplica(replica);

            RealizarCalculo();
        }

        private void Deletereplica_Click(object sender, RoutedEventArgs e)
        {
            if (ParamCHN.Replicas.Count > 0)
            {
                ParamCHN.Replicas.RemoveAt(ParamCHN.Replicas.Count - 1);
                listaReplicas.Children.RemoveAt(listaReplicas.Children.Count - 1);

                RealizarCalculo();
            }
        }
    }

}
