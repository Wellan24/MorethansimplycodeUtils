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
using LAE.Comun.Calculos;
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
        private double? HumedadViejo3 { get; set; }

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
                    ColumnWidths = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["MediaPorcentajeHumedaC"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("Media b.h.(HUM3)")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["MediaPorcentajeSecaC"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("Media b.s.")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["CV_C"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("CV")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["MediaPorcentajeHumedaH"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("Media b.h.(HUM3)")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["MediaPorcentajeSecaH"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("Media b.s.")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["CV_H"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("CV")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["MediaPorcentajeHumedaN"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("Media b.h.(HUM3)")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["MediaPorcentajeSecaN"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("Media b.s.")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["CV_N"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("CV")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
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
                        ["IdHumedad3"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaHumedad3.GetHumedades(ParamCHN.IdMuestra))
                            .SetLabel("HU3"),
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
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefault
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
                else
                {
                    Func<Double?, Double?, Double?> funcCN = (porc, hum3) => porc * (100.0 / (100.0 - hum3));
                    Func<Double?, Double?, Double?> funcH = (porc, hum3) => (porc - hum3 / hidro) * (100.0 / (100.0 - hum3));
                    MuestraEnsayo[] muestrasEnsayo = PersistenceManager.SelectByProperty<MuestraEnsayo>("IdMuestra", ParamCHN.IdMuestra).ForEach(m => m.LoadHumedad()).ToArray();

                    Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeC == null), "C", r => r.PorcentajeC, funcCN, muestrasEnsayo);
                    Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeH == null), "H", r => r.PorcentajeH, funcH, muestrasEnsayo);
                    Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeN == null), "N", r => r.PorcentajeN, funcCN, muestrasEnsayo);

                }
            }
        }
        private void Calculo(Boolean vaciar, String param, Func<ReplicaChn, double?> funcPorcentaje, Func<double?, double?, double?> funcPorcentajeSeca, MuestraEnsayo[] muestraEnsayo)
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

                ValorHumedad[] valoresPorcentaje2 = ParamCHN.Replicas.Where(r => r.Valido == true)
                    .Select(r =>
                    {
                        MuestraEnsayo muestra = FactoriaMuestraEnsayo.GetMuestra(r.IdEnsayo, ParamCHN.IdMuestra);
                        muestra.LoadHumedad();
                        return new ValorHumedad { Valor = Valor.Of(funcPorcentaje(r), "%"), Humedad = muestra.Humedad, Humedad3 = muestra.Humedad3 };
                    }).ToArray();

                RealizarCalculo(param, valoresPorcentaje2, funcPorcentajeSeca);

            }
        }

        private void RealizarCalculo(string param, ValorHumedad[] valoresPorcentajeHumedad, Func<double?, double?, double?> funcPorcentajeSeca)
        {
            Valor[] valoresPorcentaje = valoresPorcentajeHumedad.Select(p => p.Valor).ToArray();
            ParamCHN[param].MediaPorcentajeHumeda = Calcular.Promedio(valoresPorcentaje).Value;
            ParamCHN[param].MediaPorcentajeSeca = Calcular.PromedioSecado(valoresPorcentajeHumedad, funcPorcentajeSeca).Value;
            ParamCHN[param].CV = Calcular.CoeficienteVariacion(valoresPorcentaje).Value;
            ParamCHN[param].Aceptado = Calcular.EsAceptado(ParamCHN[param].CV ?? 0, ParamCHN.IdVProcedimiento, ParamCHN[param].IdParametro, ParamCHN[param].MediaPorcentajeHumeda);

            panelCalculos["MediaPorcentajeHumeda" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].MediaPorcentajeHumeda, 2));
            panelCalculos["MediaPorcentajeSeca" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].MediaPorcentajeSeca, 2));
            panelCalculos["CV_" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].CV, 2));

            labelAceptacion(param).Aceptacion(ParamCHN[param].Aceptado);
            ResultadoInforme resultado = Calcular.Resultado(ParamCHN[param].MediaPorcentajeSeca ?? 0, ParamCHN.IdVProcedimiento, ParamCHN[param].IdParametro, 2, 1);
            UCInforme(param).Resultado = resultado;
        }

        private void VaciarCalculo(string param)
        {
            panelCalculos["MediaPorcentajeHumeda" + param].SetInnerContent(String.Empty);
            panelCalculos["MediaPorcentajeSeca" + param].SetInnerContent(String.Empty);
            panelCalculos["CV_" + param].SetInnerContent(String.Empty);
            UCInforme(param).panelResultado.Clear();
            labelAceptacion(param).Visibility = Visibility.Collapsed;
        }

        //private void RealizarCalculo()
        //{
        //    if (panelCHN.GetValidatedInnerValue<Chn>() != default(Chn))
        //    {
        //        //HumedadViejo3 = PersistenceManager.SelectByID<Humedad3>(ParamCHN.IdHumedad3).MediaHumedadTotalCalculado;

        //        if (ParamCHN.Replicas.Where(r => r.Valido == true).Count() == 0)
        //        {
        //            VaciarCalculo("C");
        //            VaciarCalculo("H");
        //            VaciarCalculo("N");
        //        }
        //        else
        //        {
        //            Func<Double?, Double?> funcCN = media => media * (100.0 / (100.0 - HumedadViejo3));
        //            Func<Double?, Double?> funcH = media => (media - HumedadViejo3 / hidro) * (100.0 / (100.0 - HumedadViejo3));
        //            Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeC == null), "C", r => r.PorcentajeC, funcCN);
        //            Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeH == null), "H", r => r.PorcentajeH, funcH);
        //            Calculo(ParamCHN.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeN == null), "N", r => r.PorcentajeN, funcCN);

        //        }
        //    }
        //}

        //private void Calculo(Boolean vaciar, String param, Func<ReplicaChn, double?> funcPorcentaje, Func<double?, double?> funcPorcentajeSeca)
        //{
        //    if (vaciar)
        //    {
        //        VaciarCalculo(param);
        //    }
        //    else
        //    {
        //        Valor[] valoresPorcentaje = ParamCHN.Replicas.Where(r => r.Valido == true)
        //            .Select(funcPorcentaje)
        //            .Select(v => Valor.Of(v, "%")).ToArray();
        //        RealizarCalculo(param, valoresPorcentaje, funcPorcentajeSeca);
        //    }
        //}

        //private void RealizarCalculo(string param, Valor[] valoresPorcentaje, Func<double?, double?> funcPorcentajeSeca)
        //{
        //    //double? humedadViejo3 = PersistenceManager.SelectByID<Humedad3>(ParamCHN.IdHumedad3).MediaHumedadTotalCalculado;

        //    ParamCHN[param].MediaPorcentajeHumeda = Calcular.Promedio(valoresPorcentaje).Value; //error
        //    ParamCHN[param].MediaPorcentajeSeca = funcPorcentajeSeca(ParamCHN[param].MediaPorcentajeHumeda);
        //    ParamCHN[param].CV = Calcular.CoeficienteVariacion(valoresPorcentaje).Value;
        //    ParamCHN[param].Aceptado = Calcular.EsAceptado(ParamCHN[param].CV ?? 0, ParamCHN.IdVProcedimiento, ParamCHN[param].IdParametro, ParamCHN[param].MediaPorcentajeHumeda);

        //    panelCalculos["MediaPorcentajeHumeda" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].MediaPorcentajeHumeda, 2));
        //    panelCalculos["MediaPorcentajeSeca" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].MediaPorcentajeSeca, 2));
        //    panelCalculos["CV_" + param].SetInnerContent(Calcular.VisualizeDecimals(ParamCHN[param].CV, 2));

        //    labelAceptacion(param).Aceptacion(ParamCHN[param].Aceptado);
        //    ResultadoInforme resultado = Calcular.Resultado(ParamCHN[param].MediaPorcentajeSeca ?? 0, ParamCHN.IdVProcedimiento, ParamCHN[param].IdParametro, 2, 1);
        //    UCInforme(param).Resultado = resultado;
        //}

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
