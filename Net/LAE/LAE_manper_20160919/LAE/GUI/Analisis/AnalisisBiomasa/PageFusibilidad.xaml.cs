using Cartif.Expectation;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using GUI.Controls;
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
    /// Lógica de interacción para PageFusibilidad.xaml
    /// </summary>
    public partial class PageFusibilidad : UserControl
    {
        private Fusibilidad[] paramFusibilidad;
        public Fusibilidad[] ParamFusibilidad
        {
            get { return paramFusibilidad; }
            set
            {
                paramFusibilidad = value;
                CargarFusibilidad();
                RealizarCalculo();
            }
        }

        public PageFusibilidad()
        {
            InitializeComponent();
            DatosFusibilidad();
        }

        private void DatosFusibilidad()
        {
            Fusibilidad prueba1 = new Fusibilidad() { IdParametro = 10, IdVProcedimiento = 7 };
            Fusibilidad prueba2 = new Fusibilidad() { IdParametro = 11, IdVProcedimiento = 7 };
            Fusibilidad prueba3 = new Fusibilidad() { IdParametro = 12, IdVProcedimiento = 7 };
            Fusibilidad prueba4 = new Fusibilidad() { IdParametro = 13, IdVProcedimiento = 7 };

            /*SST*/
            prueba1.Replicas = new List<ReplicaFusibilidad>();
            prueba1.Replicas.Add(new ReplicaFusibilidad() { Valor = 1148, IdUdsValor = 7, Num = 1, Valido = true });
            prueba1.Replicas.Add(new ReplicaFusibilidad() { Valor = 1183, IdUdsValor = 7, Num = 1, Valido = true });

            /*DT*/
            prueba2.Replicas = new List<ReplicaFusibilidad>();
            prueba2.Replicas.Add(new ReplicaFusibilidad() { Valor = 1232, IdUdsValor = 7, Num = 1, Valido = true });
            prueba2.Replicas.Add(new ReplicaFusibilidad() { Valor = 1258, IdUdsValor = 7, Num = 1, Valido = true });

            /*HT*/
            prueba3.Replicas = new List<ReplicaFusibilidad>();
            prueba3.Replicas.Add(new ReplicaFusibilidad() { Valor = 1470, IdUdsValor = 7, Num = 1, Valido = true });
            prueba3.Replicas.Add(new ReplicaFusibilidad() { Valor = 1483, IdUdsValor = 7, Num = 1, Valido = true });

            /*FT*/
            prueba4.Replicas = new List<ReplicaFusibilidad>();
            prueba4.Replicas.Add(new ReplicaFusibilidad() { Valor = 1480, IdUdsValor = 7, Num = 1, Valido = true });
            prueba4.Replicas.Add(new ReplicaFusibilidad() { Valor = 1491, IdUdsValor = 7, Num = 1, Valido = true });

            Fusibilidad[] prueba = new Fusibilidad[] { prueba1, prueba2, prueba3, prueba4 };

            ParamFusibilidad = prueba;
        }

        private void CargarFusibilidad()
        {
            foreach (Fusibilidad fusib in ParamFusibilidad)
            {
                String parametro = PersistenceManager.SelectByID<ParametroProcedimiento>(fusib.IdParametro).Nombre;

                foreach (ReplicaFusibilidad replica in fusib.Replicas)
                {
                    CrearPanelReplica(replica, parametro);
                }

                CrearPanelCalculo(fusib, parametro);
            }

        }

        private void CrearPanelReplica(ReplicaFusibilidad replica, String parametro)
        {
            TypePanel panelReplica = new TypePanel();

            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaFusibilidad>
                {
                    ColumnWidths = new int[] { 2, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("Replica" + replica.Num)
                            .AddCheckedChanged((s, e) => CambioReplica(parametro)),
                        ["Valor"] = PropertyControlSettingsEnum.TextBoxEmptyToNull,
                        ["IdUdsValor"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Temperatura"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => CambioTexto(parametro)
                    },
                    PanelValidation = Expectation<ReplicaFusibilidad>
                        .ShouldNotBe().AddCriteria(h => h.Valor == null)
                });

            switch (parametro)
            {
                case "SST":
                    listaReplicasSST.Children.Add(panelReplica);
                    break;
                case "DT":
                    listaReplicasDT.Children.Add(panelReplica);
                    break;
                case "HT":
                    listaReplicasHT.Children.Add(panelReplica);
                    break;
                case "FT":
                    listaReplicasFT.Children.Add(panelReplica);
                    break;
            }
        }

        private void CrearPanelCalculo(Fusibilidad fusib, String parametro)
        {
            TypePanel panelCalculo = GetPanel(parametro);
            panelCalculo.Build(fusib,
                new TypePanelSettings<Fusibilidad>
                {
                    ColumnWidths = new int[] { 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["MediaTemperatura2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Media(ºC)")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["CV2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("CV")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"])
                    }
                });
        }

        private TypePanel GetPanel(String parametro)
        {
            TypePanel panelCalculo = new TypePanel();
            switch (parametro)
            {
                case "SST":
                    panelCalculo = panelCalculosSST;
                    break;
                case "DT":
                    panelCalculo = panelCalculosDT;
                    break;
                case "HT":
                    panelCalculo = panelCalculosHT;
                    break;
                case "FT":
                    panelCalculo = panelCalculosFT;
                    break;
            }
            return panelCalculo;
        }

        private Label GetLabel(String parametro)
        {
            Label labelAceptacion = new Label();
            switch (parametro)
            {
                case "SST":
                    labelAceptacion = labelAceptacionSST;
                    break;
                case "DT":
                    labelAceptacion = labelAceptacionDT;
                    break;
                case "HT":
                    labelAceptacion = labelAceptacionHT;
                    break;
                case "FT":
                    labelAceptacion = labelAceptacionFT;
                    break;
            }
            return labelAceptacion;
        }

        private ControlInforme GetInforme(String parametro)
        {
            ControlInforme informe = new ControlInforme();
            switch (parametro)
            {
                case "SST":
                    informe = UCInformeSST;
                    break;
                case "DT":
                    informe = UCInformeDT;
                    break;
                case "HT":
                    informe = UCInformeHT;
                    break;
                case "FT":
                    informe = UCInformeFT;
                    break;
            }
            return informe;
        }

        private void CambioTexto(string parametro)
        {
            RealizarCalculo(parametro);
        }

        private void CambioReplica(string parametro)
        {
            RealizarCalculo(parametro);
        }

        private void RealizarCalculo()
        {
            RealizarCalculo("SST");
            RealizarCalculo("DT");
            RealizarCalculo("HT");
            RealizarCalculo("FT");
        }

        private void RealizarCalculo(string p)
        {
            ParametroProcedimiento parametro = PersistenceManager.SelectByProperty<ParametroProcedimiento>("Nombre", p).FirstOrDefault();
            Fusibilidad param = ParamFusibilidad.Where(pa => pa.IdParametro == parametro.Id).FirstOrDefault();

            TypePanel panelCalculo = GetPanel(p);
            Label labelAceptacion = GetLabel(p);
            ControlInforme informe = GetInforme(p);

            if (param.Replicas.Exists(r => r.Valido == true && (r.Valor.Equals(String.Empty) || r.Valor == null))) //TODO usar expectation
            {
                panelCalculo.Clear();
                informe.panelResultado.Clear();
            }
            else
            {
                Valor[] valoresTemperatura = param.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.Valor, "ºC")).ToArray();
                param.MediaTemperatura = Calcular.Promedio(valoresTemperatura).Value;
                param.CV = Calcular.CoeficienteVariacion(valoresTemperatura).Value;
                param.Aceptado = Calcular.EsAceptado(param.CV ?? 0, param.IdVProcedimiento, param.IdParametro, param.MediaTemperatura);

                panelCalculo["MediaTemperatura2"].SetInnerContent(Calcular.VisualizeDecimals(param.MediaTemperatura, 0));
                panelCalculo["CV2"].SetInnerContent(Calcular.VisualizeDecimals(param.CV, 1));

                labelAceptacion.Aceptacion(param.Aceptado);

                ResultadoInforme resultado = Calcular.Resultado(param.MediaTemperatura ?? 0, param.IdVProcedimiento, param.IdParametro, 0, 0);
                informe.Resultado = resultado;
            }
        }

        private void AddreplicaSST_Click(object sender, RoutedEventArgs e)
        {
            GenerateReplica("SST");
        }

        private void AddreplicaDT_Click(object sender, RoutedEventArgs e)
        {
            GenerateReplica("DT");
        }

        private void AddreplicaHT_Click(object sender, RoutedEventArgs e)
        {
            GenerateReplica("HT");
        }

        private void AddreplicaFT_Click(object sender, RoutedEventArgs e)
        {
            GenerateReplica("FT");
        }

        private void GenerateReplica(string p)
        {
            ParametroProcedimiento parametro = PersistenceManager.SelectByProperty<ParametroProcedimiento>("Nombre", p).FirstOrDefault();
            Fusibilidad param = ParamFusibilidad.Where(pa => pa.IdParametro == parametro.Id).FirstOrDefault();

            ReplicaFusibilidad replica = new ReplicaFusibilidad()
            {
                IdUdsValor = 7,
                Valido = true,
                Num = param.Replicas[param.Replicas.Count - 1].Num + 1
            };

            param.Replicas.Add(replica);
            CrearPanelReplica(replica, p);

            RealizarCalculo(p);
        }
    }
}
