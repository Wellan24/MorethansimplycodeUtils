using Cartif.Expectation;
using Cartif.Extensions;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Biomasa.Controles;
using LAE.Comun.Calculos;
using LAE.Comun.Clases;
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
using LAE.Biomasa.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LAE.Biomasa.Pages
{
    /// <summary>
    /// Lógica de interacción para PageSCl.xaml
    /// </summary>
    public partial class PageSCl : UserControl
    {
        private SCl[] paramSCl;
        public SCl[] ParamSCl
        {
            get { return paramSCl; }
            set
            {
                paramSCl = value;
                CargarSCl();
                RealizarCalculo();
            }
        }

        public PageSCl()
        {
            InitializeComponent();
            DatosSCl();
        }

        private void DatosSCl()
        {
            SCl prueba1 = new SCl() { Blanco = 0.89, IdUdsBlanco = 23, IdParametro = 14, IdVProcedimiento = 8 };
            SCl prueba2 = new SCl() { Blanco = 0.92, IdUdsBlanco = 23, IdParametro = 15, IdVProcedimiento = 8 };

            prueba1.Replicas = new List<ReplicaSCl>();
            prueba1.Replicas.Add(new ReplicaSCl() { M = 0.6023, IdUdsM = 12, Muestra = 3.24, IdUdsMuestra = 23, Num = 1, Valido = true });
            prueba1.Replicas.Add(new ReplicaSCl() { M = 0.5997, IdUdsM = 12, Muestra = 3.72, IdUdsMuestra = 23, Num = 2, Valido = true });

            prueba2.Replicas = new List<ReplicaSCl>();
            prueba2.Replicas.Add(new ReplicaSCl() { M = 0.6023, IdUdsM = 12, Muestra = 1.88, IdUdsMuestra = 23, Num = 1, Valido = true });
            prueba2.Replicas.Add(new ReplicaSCl() { M = 0.5997, IdUdsM = 12, Muestra = 2.13, IdUdsMuestra = 23, Num = 2, Valido = true });

            SCl[] prueba = new SCl[] { prueba1, prueba2 };

            ParamSCl = prueba;
        }

        private void CargarSCl()
        {
            foreach (SCl scl in ParamSCl)
            {
                String parametro = PersistenceManager.SelectByID<ParametroProcedimiento>(scl.IdParametro).Nombre;

                CrearPanelParametro(scl, parametro);
                foreach (ReplicaSCl replica in scl.Replicas)
                {
                    CrearPanelReplica(replica, parametro);
                }
                CrearPanelCalculo(scl, parametro);
            }
        }

        private void CrearPanelParametro(SCl scl, string parametro)
        {
            TypePanel panelParametro = GetPanelParametro(parametro);
            panelParametro.Build(scl,
                new TypePanelSettings<SCl>
                {
                    ColumnWidths = new int[] { 3, 2 },
                    Fields = new FieldSettings
                    {
                        ["Blanco"] = PropertyControlSettingsEnum.TextBoxEmptyToNull,
                        ["IdUdsBlanco"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Densidad"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                    },
                    IsUpdating = true,
                    PanelValidation = Expectation<SCl>
                        .ShouldNotBe().AddCriteria(h => h.Blanco == null),
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => CambioTexto(parametro)
                    }
                });
        }

        private void CrearPanelReplica(ReplicaSCl replica, String parametro)
        {
            TypePanel panelReplica = new TypePanel();

            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaSCl>
                {
                    ColumnWidths = new int[] { 2, 3, 1, 3, 1, 3 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("Replica" + replica.Num)
                            .AddCheckedChanged((s, e) => CambioReplica(parametro)),
                        ["M"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m"),
                        ["IdUdsM"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["Muestra"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("muestra b.h."),
                        ["IdUdsMuestra"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Densidad"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["Porcentaje2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("% b.h.")
                            .AddTextChanged((s, e) => { })
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => CambioTexto(parametro)
                    },
                    PanelValidation = Expectation<ReplicaSCl>
                        .ShouldNotBe().AddCriteria(h => h.M == null)
                        .NotBe().AddCriteria(h => h.Muestra == null)
                });

            switch (parametro)
            {
                case "S":
                    listaReplicasS.Children.Add(panelReplica);
                    break;
                case "Cl":
                    listaReplicasCl.Children.Add(panelReplica);
                    break;
            }
        }

        private void CrearPanelCalculo(SCl scl, string parametro)
        {
            TypePanel panelCalculo = GetPanelCalculo(parametro);

            panelCalculo.Build(scl,
                new TypePanelSettings<SCl>
                {
                    Fields = new FieldSettings
                    {
                        ["MediaPorcentajeHumeda2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Media % b.h.")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["MediaPorcentajeSeca2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Media % b.s.")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["CV2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("CV")
                                .SetReadOnly(true)
                                .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                    }
                });
        }

        private TypePanel GetPanelParametro(String parametro)
        {
            TypePanel panelParametro = new TypePanel();
            switch (parametro)
            {
                case "S":
                    panelParametro = panelS;
                    break;
                case "Cl":
                    panelParametro = panelCl;
                    break;
            }
            return panelParametro;
        }

        private StackPanel GetListaReplicas(String parametro)
        {
            StackPanel listaReplicas = new StackPanel();
            switch (parametro)
            {
                case "S":
                    listaReplicas = listaReplicasS;
                    break;
                case "Cl":
                    listaReplicas = listaReplicasCl;
                    break;
            }
            return listaReplicas;
        }

        private TypePanel GetPanelCalculo(string parametro)
        {
            TypePanel panelCalculo = new TypePanel();
            switch (parametro)
            {
                case "S":
                    panelCalculo = panelCalculosS;
                    break;
                case "Cl":
                    panelCalculo = panelCalculosCl;
                    break;
            }

            return panelCalculo;
        }

        private Label GetLabel(string parametro)
        {
            Label labelAceptacion = new Label();
            switch (parametro)
            {
                case "S":
                    labelAceptacion = labelAceptacionS;
                    break;
                case "Cl":
                    labelAceptacion = labelAceptacionCl;
                    break;
            }
            return labelAceptacion;
        }

        private ControlInforme GetInforme(string parametro)
        {
            ControlInforme informe = new ControlInforme();
            switch (parametro)
            {
                case "S":
                    informe = UCInformeS;
                    break;
                case "Cl":
                    informe = UCInformeCl;
                    break;
            }
            return informe;
        }

        private void RealizarCalculo()
        {
            RealizarCalculo("S");
            RealizarCalculo("Cl");
        }

        private void CambioTexto(String parametro)
        {
            RealizarCalculo(parametro);
        }

        private void CambioReplica(String parametro)
        {
            RealizarCalculo(parametro);
        }

        private void RealizarCalculo(string p)
        {
            Valor probeta = Valor.Of(0.05, "Litros"); /*temporal - inventario probetas*/
            double humedadTotal = 7.68596414113616; /*temporal - abra que obtenerlo de la page: humedad3*/

            ParametroProcedimiento parametro = PersistenceManager.SelectByProperty<ParametroProcedimiento>("Nombre", p).FirstOrDefault();
            SCl param = ParamSCl.Where(pa => pa.IdParametro == parametro.Id).FirstOrDefault();

            TypePanel panelParametro = GetPanelParametro(p);
            StackPanel listaReplicas = GetListaReplicas(p);
            TypePanel panelCalculo = GetPanelCalculo(p);
            Label labelAceptacion = GetLabel(p);
            ControlInforme informe = GetInforme(p);

            if (panelParametro.GetValidatedInnerValue<SCl>() != default(SCl))
            {
                Valor blanco = Valor.Of(param.Blanco, param.IdUdsBlanco ?? 0);

                listaReplicas.Children.OfType<TypePanel>().ForEach(tp =>
                {
                    ReplicaSCl replica = tp.InnerValue as ReplicaSCl;
                    if (tp.GetValidatedInnerValue<ReplicaSCl>() != default(ReplicaSCl))
                    {
                        Valor m = Valor.Of(replica.M, replica.IdUdsM ?? 0);
                        Valor muestra = Valor.Of(replica.Muestra, replica.IdUdsMuestra ?? 0);
                        replica.Porcentaje = Calcular.SCl_8_8(m, probeta, muestra, blanco, p).Value;

                        tp["Porcentaje2"].SetInnerContent(Calcular.VisualizeDecimals(replica.Porcentaje,3));
                    }
                    else
                    {
                        replica.Porcentaje = null;
                        tp["Porcentaje2"].SetInnerContent(String.Empty);
                    }
                });

                if(param.Replicas.Exists(r=>r.Valido==true && r.Porcentaje==null) || param.Replicas.Where(r => r.Valido == true).Count() == 0)
                {
                    panelCalculo.Clear();
                    informe.panelResultado.Clear();
                }
                else
                {
                    Valor[] valoresPorcentaje = param.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.Porcentaje, "%")).ToArray();
                    param.MediaPorcentejaHumeda = Calcular.Promedio(valoresPorcentaje).Value;
                    param.MediaPorcentajeSeca = param.MediaPorcentejaHumeda * (100 / (100 - humedadTotal));
                    param.CV = Calcular.CoeficienteVariacion(valoresPorcentaje).Value;
                    param.Aceptado = Calcular.EsAceptado(param.CV ?? 0, param.IdVProcedimiento, param.IdParametro, param.MediaPorcentajeSeca);

                    panelCalculo["MediaPorcentajeHumeda2"].SetInnerContent(Calcular.VisualizeDecimals(param.MediaPorcentejaHumeda, 3));
                    panelCalculo["MediaPorcentajeSeca2"].SetInnerContent(Calcular.VisualizeDecimals(param.MediaPorcentajeSeca, 3));
                    panelCalculo["CV2"].SetInnerContent(Calcular.VisualizeDecimals(param.CV, 3));

                    labelAceptacion.Aceptacion(param.Aceptado);
                    ResultadoInforme resultado = Calcular.Resultado(param.MediaPorcentajeSeca ?? 0, param.IdVProcedimiento, param.IdParametro, 3, 3);
                    informe.Resultado = resultado;
                }
            }
            else
            {
                panelCalculo.Clear();
                informe.panelResultado.Clear();
            }
        }

        private void AddreplicaS_Click(object sender, RoutedEventArgs e)
        {
            GenerateReplica("S");
        }

        private void AddreplicaCl_Click(object sender, RoutedEventArgs e)
        {
            GenerateReplica("Cl");
        }

        private void GenerateReplica(string p)
        {
            ParametroProcedimiento parametro = PersistenceManager.SelectByProperty<ParametroProcedimiento>("Nombre", p).FirstOrDefault();
            SCl param = ParamSCl.Where(pa => pa.IdParametro == parametro.Id).FirstOrDefault();

            ReplicaSCl replica = new ReplicaSCl()
            {
                IdUdsMuestra = 23,
                Valido = true,
                Num = param.Replicas[param.Replicas.Count - 1].Num + 1
            };

            param.Replicas.Add(replica);
            CrearPanelReplica(replica, p);

            RealizarCalculo(p);
        }
    }
}
