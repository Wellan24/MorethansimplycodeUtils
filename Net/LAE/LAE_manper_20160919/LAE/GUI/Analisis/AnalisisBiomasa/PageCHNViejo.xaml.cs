using Cartif.Expectation;
using Cartif.Extensions;
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
    /// Lógica de interacción para PageCHNViejo.xaml
    /// </summary>
    public partial class PageCHNViejo : UserControl
    {
        private CHNviejo[] paramCHN;
        public CHNviejo[] ParamCHN
        {
            get { return paramCHN; }
            set
            {
                paramCHN = value;
                CargarCHN();
                RealizarCalculo();
            }
        }


        public PageCHNViejo()
        {
            InitializeComponent();
            DatosCHN();
        }

        private void DatosCHN()
        {
            CHNviejo prueba1 = new CHNviejo() { IdParametro = 6, IdVProcedimiento = 5 };
            CHNviejo prueba2 = new CHNviejo() { IdParametro = 7, IdVProcedimiento = 5 };
            CHNviejo prueba3 = new CHNviejo() { IdParametro = 8, IdVProcedimiento = 5 };

            prueba1.Replicas = new List<ReplicaCHNviejo>();
            prueba1.Replicas.Add(new ReplicaCHNviejo() { Porcentaje = 46.29, IdUdsPorcentaje = 17, Num = 1, Valido = true });
            prueba1.Replicas.Add(new ReplicaCHNviejo() { Porcentaje = 46.17, IdUdsPorcentaje = 17, Num = 2, Valido = true });

            prueba2.Replicas = new List<ReplicaCHNviejo>();
            prueba2.Replicas.Add(new ReplicaCHNviejo() { Porcentaje = 6.494, IdUdsPorcentaje = 17, Num = 1, Valido = true });
            prueba2.Replicas.Add(new ReplicaCHNviejo() { Porcentaje = 6.461, IdUdsPorcentaje = 17, Num = 2, Valido = true });

            prueba3.Replicas = new List<ReplicaCHNviejo>();
            prueba3.Replicas.Add(new ReplicaCHNviejo() { Porcentaje = 0.0517, IdUdsPorcentaje = 17, Num = 1, Valido = true });
            prueba3.Replicas.Add(new ReplicaCHNviejo() { Porcentaje = 0.0257, IdUdsPorcentaje = 17, Num = 2, Valido = true });

            CHNviejo[] prueba = new CHNviejo[] { prueba1, prueba2, prueba3 };

            ParamCHN = prueba;
        }

        private void CargarCHN()
        {
            foreach (CHNviejo chn in ParamCHN)
            {
                String parametro = PersistenceManager.SelectByID<ParametroProcedimiento>(chn.IdParametro).Nombre;

                foreach (ReplicaCHNviejo replica in chn.Replicas)
                {
                    CrearPanelReplica(replica, parametro);
                }

                CrearPanelCalculo(chn, parametro);
            }
        }

        private void CrearPanelReplica(ReplicaCHNviejo replica, String parametro)
        {
            TypePanel panelReplica = new TypePanel();

            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaCHNviejo>
                {
                    ColumnWidths = new int[] { 3, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("Replica" + replica.Num)
                            .AddCheckedChanged((s, e) => CambioReplica(parametro)),
                        ["Porcentaje"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("(%)b.h."),
                        ["IdUdsPorcentaje"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Porcentaje"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => CambioTexto(parametro)
                    },
                    PanelValidation = Expectation<ReplicaCHNviejo>
                        .ShouldNotBe().AddCriteria(h => h.Porcentaje == null)
                });

            switch (parametro)
            {
                case "C":
                    listaReplicasC.Children.Add(panelReplica);
                    break;
                case "H":
                    listaReplicasH.Children.Add(panelReplica);
                    break;
                case "N":
                    listaReplicasN.Children.Add(panelReplica);
                    break;

            }
        }

        private void CrearPanelCalculo(CHNviejo chn, string parametro)
        {
            TypePanel panelCalculo = GetPanel(parametro);

            panelCalculo.Build(chn,
                new TypePanelSettings<CHNviejo>
                {
                    Fields = new FieldSettings
                    {
                        ["MediaPorcentajeHumeda2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Media b.h.")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["MediaPorcentajeSeca2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Media b.s.")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["CV2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("CV")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                    }
                });
        }

        private TypePanel GetPanel(String parametro)
        {
            TypePanel panelCalculo = new TypePanel();
            switch (parametro)
            {
                case "C":
                    panelCalculo = panelCalculosC;
                    break;
                case "H":
                    panelCalculo = panelCalculosH;
                    break;
                case "N":
                    panelCalculo = panelCalculosN;
                    break;
            }
            return panelCalculo;
        }

        private Label GetLabel(String parametro)
        {
            Label labelAceptacion = new Label();
            switch (parametro)
            {
                case "C":
                    labelAceptacion = labelAceptacionC;
                    break;
                case "H":
                    labelAceptacion = labelAceptacionH;
                    break;
                case "N":
                    labelAceptacion = labelAceptacionN;
                    break;
            }
            return labelAceptacion;
        }

        private ControlInforme GetInforme(String parametro)
        {
            ControlInforme informe = new ControlInforme();
            switch (parametro)
            {
                case "C":
                    informe = UCInformeC;
                    break;
                case "H":
                    informe = UCInformeH;
                    break;
                case "N":
                    informe = UCInformeN;
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
            RealizarCalculo("C");
            RealizarCalculo("H");
            RealizarCalculo("N");
        }

        private void RealizarCalculo(String p)
        {
            double humedadTotal = 7.68596414113616;/*temporal - abra que obtenerlo de la page: humedad3*/
            const double hidro = 8.937; /*preguntar por esta constante*/

            ParametroProcedimiento parametro = PersistenceManager.SelectByProperty<ParametroProcedimiento>("Nombre", p).FirstOrDefault();
            CHNviejo param = ParamCHN.Where(pa => pa.IdParametro == parametro.Id).FirstOrDefault();

            TypePanel panelCalculo = GetPanel(p);
            Label labelAceptacion = GetLabel(p);
            ControlInforme informe = GetInforme(p);

            if (param.Replicas.Exists(r => r.Valido == true && (r.Porcentaje.Equals(String.Empty) || r.Porcentaje == null))) //TODO usar expectation
            {
                panelCalculo.Clear();
                informe.panelResultado.Clear();
            }
            else
            {
                Valor[] valoresPorcentaje = param.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.Porcentaje, "%")).ToArray();
                param.MediaPorcentejaHumeda = Calcular.Promedio(valoresPorcentaje).Value;
                if (p.Equals("H"))
                    param.MediaPorcentajeSeca = (param.MediaPorcentejaHumeda - (humedadTotal / hidro)) * (100.0 / (100.0 - humedadTotal));
                else
                    param.MediaPorcentajeSeca = param.MediaPorcentejaHumeda * (100.0 / (100.0 - humedadTotal));
                param.CV = Calcular.CoeficienteVariacion(valoresPorcentaje).Value;
                param.Aceptado = Calcular.EsAceptado(param.CV ?? 0, param.IdVProcedimiento, param.IdParametro, param.MediaPorcentajeSeca);

                panelCalculo["MediaPorcentajeHumeda2"].SetInnerContent(Calcular.VisualizeDecimals(param.MediaPorcentejaHumeda, 2));
                panelCalculo["MediaPorcentajeSeca2"].SetInnerContent(Calcular.VisualizeDecimals(param.MediaPorcentajeSeca, 2));
                panelCalculo["CV2"].SetInnerContent(Calcular.VisualizeDecimals(param.CV, 2));

                labelAceptacion.Aceptacion(param.Aceptado);
                ResultadoInforme resultado;
                if (p.Equals("N"))
                    resultado = Calcular.Resultado(param.MediaPorcentajeSeca ?? 0, param.IdVProcedimiento, param.IdParametro, 3, 3);
                else
                    resultado = Calcular.Resultado(param.MediaPorcentajeSeca ?? 0, param.IdVProcedimiento, param.IdParametro, 2, 1);
                informe.Resultado = resultado;
            }
        }

        private void AddreplicaC_Click(object sender, RoutedEventArgs e)
        {
            GenerateReplica("C");
        }

        private void AddreplicaH_Click(object sender, RoutedEventArgs e)
        {
            GenerateReplica("H");
        }

        private void AddreplicaN_Click(object sender, RoutedEventArgs e)
        {
            GenerateReplica("N");
        }

        private void GenerateReplica(string p)
        {
            ParametroProcedimiento parametro = PersistenceManager.SelectByProperty<ParametroProcedimiento>("Nombre", p).FirstOrDefault();
            CHNviejo param = ParamCHN.Where(pa => pa.IdParametro == parametro.Id).FirstOrDefault();

            ReplicaCHNviejo replica = new ReplicaCHNviejo()
            {
                IdUdsPorcentaje = 17,
                Valido = true,
                Num = param.Replicas[param.Replicas.Count - 1].Num + 1,
            };

            param.Replicas.Add(replica);
            CrearPanelReplica(replica, p);

            RealizarCalculo(p);
        }
    }
}
