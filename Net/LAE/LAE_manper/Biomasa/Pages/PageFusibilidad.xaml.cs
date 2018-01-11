using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Biomasa.Controles;
using LAE.Biomasa.Modelo;
using LAE.Comun.Calculos;
using LAE.Comun.Clases;
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;
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

namespace LAE.Biomasa.Pages
{
    /// <summary>
    /// Lógica de interacción para PageFusibilidad.xaml
    /// </summary>
    public partial class PageFusibilidad : UserControl
    {
        private Fusibilidad fusibilidad;

        public Fusibilidad Fusibilidad
        {
            get { return fusibilidad; }
            set
            {
                fusibilidad = value;
                CargarFusibilidad();
                RealizarCalculo();
            }
        }

        public PageFusibilidad()
        {
            InitializeComponent();
            GenerarPanelCalculo();
        }

        private void GenerarPanelCalculo()
        {
            panelCalculos.Build(new Fusibilidad(),
                new TypePanelSettings<Fusibilidad>
                {

                    ColumnWidths = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["MediaTemperatura_SST"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media(ºC)"),
                        ["CV_SST"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("CV"),
                        ["MediaTemperatura_DT"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media(ºC)"),
                        ["CV_DT"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("CV"),
                        ["MediaTemperatura_HT"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media(ºC)"),
                        ["CV_HT"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("CV"),
                        ["MediaTemperatura_FT"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("Media(ºC)"),
                        ["CV_FT"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNullReadOnly
                            .SetLabel("CV"),
                    }
                });
        }

        private void CargarFusibilidad()
        {
            panelFusibilidad.Build(Fusibilidad,
                new TypePanelSettings<Fusibilidad>
                {
                    Fields = new FieldSettings
                    {
                        ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(Fusibilidad["SST"].IdParametro))
                            .SetLabel("Versión"),
                        ["Finalizado"] = PropertyControlSettingsEnum.CheckBoxDefault,
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        SelectionChanged = (s, e) => RealizarCalculo()
                    }
                });

            panelFusibilidad["IdVProcedimiento"].SelectedIndex = 0;

            foreach (ReplicaFusibilidad replica in Fusibilidad.Replicas)
            {
                CrearPanelReplica(replica);
            }
        }

        private void CrearPanelReplica(ReplicaFusibilidad replica)
        {
            TypePanel panelReplica = new TypePanel();
            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaFusibilidad>
                {
                    ColumnWidths = new int[] { 2, 2, 1, 2, 1, 2, 1, 1, 2, 1, 1, 2 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefaultSmall
                            .SetLabel("Replica" + replica.Num),
                        ["TemperaturaSst"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("SST(ºC)"),
                        ["IdUdsTemperaturaSst"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Temperatura"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["TemperaturaDT"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("DT(ºC)"),
                        ["IdUdsTemperaturaDT"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Temperatura"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["TemperaturaHT"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("HT(ºC)"),
                        ["MayorHT"] = PropertyControlSettingsEnum.CheckBoxDefaultSmall
                            .SetLabel(">"),
                        ["IdUdsTemperaturaHT"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Temperatura"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["TemperaturaFT"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("FT(ºC)"),
                        ["MayorFT"] = PropertyControlSettingsEnum.CheckBoxDefaultSmall
                            .SetLabel(">"),
                        ["IdUdsTemperaturaFT"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Temperatura"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["IdEnsayo"] = PropertyControlSettingsEnum.ComboBoxDefault
                            //.SetTargetNull(null)
                            .SetLabel("Ensayo")
                            .SetInnerValues(FactoriaEnsayoPNT.GetEnsayos(Fusibilidad.IdMuestra, "Analizador fusibilidad"))
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => RealizarCalculo(),
                        SelectionChanged = (s, e) => RealizarCalculo(),
                        CheckedChanged = (s, e) => RealizarCalculo()
                    }
                });

            listaReplicas.Children.Add(panelReplica);


        }

        private void RealizarCalculo()
        {
            if (panelFusibilidad.GetValidatedInnerValue<Fusibilidad>() != default(Fusibilidad))
            {
                if (Fusibilidad.Replicas.Where(r => r.Valido == true).Count() == 0)
                {
                    VaciarCalculo("SST");
                    VaciarCalculo("DT");
                    VaciarCalculo("HT");
                    VaciarCalculo("FT");
                }
                else
                {
                    Calculo(Fusibilidad.Replicas.Where(r => r.Valido == true)
                        .Any(r => r.TemperaturaSst == null), "SST", r => r.TemperaturaSst);
                    Calculo(Fusibilidad.Replicas.Where(r => r.Valido == true)
                        .Any(r => r.TemperaturaDT == null), "DT", r => r.TemperaturaDT);
                    Calculo(Fusibilidad.Replicas.Where(r => r.Valido == true)
                        .Any(r => r.TemperaturaHT == null), "HT", r => r.TemperaturaHT, Fusibilidad.Replicas.Where(r => r.Valido == true && r.MayorHT == true).FirstOrDefault());
                    Calculo(Fusibilidad.Replicas.Where(r => r.Valido == true)
                        .Any(r => r.TemperaturaFT == null), "FT", r => r.TemperaturaFT, Fusibilidad.Replicas.Where(r => r.Valido == true && r.MayorFT == true).FirstOrDefault());
                }
            }
        }

        private void Calculo(Boolean vaciar, String param, Func<ReplicaFusibilidad, int?> funcTemperatura, ReplicaFusibilidad replicaMaximo = null)
        {
            if (vaciar)
            {
                VaciarCalculo(param);
            }
            else if (replicaMaximo != null)
            {
                double maximo = funcTemperatura(replicaMaximo) ?? 0;
                panelCalculos["MediaTemperatura_" + param].SetInnerContent(">" + Calcular.VisualizeDecimals(maximo, 0));
                panelCalculos["CV_" + param].SetInnerContent(String.Empty);
                labelAceptacion(param).Visibility = Visibility.Collapsed;
                ResultadoInforme resultado = Calcular.Resultado(maximo, -1);
                UCInforme(param).Resultado = resultado;

            }
            else
            {
                Valor[] valoresTemperatura = Fusibilidad.Replicas.Where(r => r.Valido == true)
                                                .Select(funcTemperatura)
                                                .Select(v => Valor.Of(v, "ºC")).ToArray();

                RealizarCalculo(param, valoresTemperatura);
            }
        }

        private void RealizarCalculo(String param, Valor[] valoresTemperatura)
        {
            Fusibilidad[param].MediaTemperatura = Calcular.Promedio(valoresTemperatura).Value;
            Fusibilidad[param].CV = Calcular.CoeficienteVariacion(valoresTemperatura).Value;
            Fusibilidad[param].Aceptado = Calcular.EsAceptado(Fusibilidad[param].CV ?? 0, Fusibilidad.IdVProcedimiento, Fusibilidad[param].IdParametro, Fusibilidad[param].MediaTemperatura);

            panelCalculos["MediaTemperatura_" + param].SetInnerContent(Calcular.VisualizeDecimals(Fusibilidad[param].MediaTemperatura, 1));
            panelCalculos["CV_" + param].SetInnerContent(Calcular.VisualizeDecimals(Fusibilidad[param].CV, 1));
            labelAceptacion(param).Aceptacion(Fusibilidad[param].Aceptado);
            ResultadoInforme resultado = Calcular.Resultado(Fusibilidad[param].MediaTemperatura ?? 0, Fusibilidad.IdVProcedimiento, Fusibilidad[param].IdParametro, -1, 0);
            UCInforme(param).Resultado = resultado;
        }

        private void VaciarCalculo(String param)
        {
            panelCalculos["MediaTemperatura_" + param].SetInnerContent(String.Empty);
            panelCalculos["CV_" + param].SetInnerContent(String.Empty);
            VaciarResultado(param);
        }

        private void VaciarResultado(String param)
        {
            UCInforme(param).panelResultado.Clear();
            labelAceptacion(param).Visibility = Visibility.Collapsed;
        }

        private Label labelAceptacion(String param)
        {
            switch (param)
            {
                case "SST":
                    return labelAceptacionSst;
                case "DT":
                    return labelAceptacionDT;
                case "HT":
                    return labelAceptacionHT;
                case "FT":
                    return labelAceptacionFT;
                default:
                    return new Label();
            }
        }

        private ControlInforme UCInforme(String param)
        {
            switch (param)
            {
                case "SST":
                    return UCInformeSst;
                case "DT":
                    return UCInformeDT;
                case "HT":
                    return UCInformeHT;
                case "FT":
                    return UCInformeFT;
                default:
                    return new ControlInforme();
            }
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            int idTemperatura = Unidad.Of("ºC").Id;

            ReplicaFusibilidad replica = new ReplicaFusibilidad()
            {
                IdUdsTemperaturaSst = idTemperatura,
                IdUdsTemperaturaDT = idTemperatura,
                IdUdsTemperaturaHT = idTemperatura,
                IdUdsTemperaturaFT = idTemperatura,
                Valido = true
            };
            int nCont = Fusibilidad.Replicas.Count;
            replica.Num = (nCont == 0) ? 1 : Fusibilidad.Replicas[nCont - 1].Num + 1;

            Fusibilidad.Replicas.Add(replica);
            CrearPanelReplica(replica);

            RealizarCalculo();
        }

        private void Deletereplica_Click(object sender, RoutedEventArgs e)
        {
            if (Fusibilidad.Replicas.Count > 0)
            {
                Fusibilidad.Replicas.RemoveAt(Fusibilidad.Replicas.Count - 1);
                listaReplicas.Children.RemoveAt(listaReplicas.Children.Count - 1);

                RealizarCalculo();
            }
        }
    }
}
