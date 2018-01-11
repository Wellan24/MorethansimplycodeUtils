using Cartif.Logs;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Comun.Calculos;
using LAE.Comun.Clases;
using LAE.Biomasa.Modelo;
using Npgsql;
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
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;

namespace LAE.Biomasa.Controles
{
    /// <summary>
    /// Lógica de interacción para ControlCHNcci.xaml
    /// </summary>
    public partial class ControlCHNcci : UserControl
    {
        private int IdEnsayo;

        private ChnControl chncontrol;
        public ChnControl CHNcontrol
        {
            get { return chncontrol; }
            set
            {
                chncontrol = value;
                CargarCHN(this);
                RealizarCalculo();
            }
        }

        public event RoutedEventHandler BackButtonClick
        {
            add { bBack.Click += value; }
            remove { bBack.Click -= value; }
        }

        public ControlCHNcci(int idEnsayo)
        {
            InitializeComponent();
            IdEnsayo = idEnsayo;
        }

        private void CargarCHN(ControlCHNcci instance)
        {
            panelCHNcci.Build(instance.CHNcontrol, new TypePanelSettings<ChnControl>
            {
                Fields = new FieldSettings
                {
                    ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                        .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(instance.CHNcontrol["C"].IdParametro))
                        .SetLabel("* Versión"),
                    ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                        .SetInnerValues(FactoriaTecnicos.GetTecnicos())
                        .SetLabel("* Técnico"),
                    ["IdMaterialReferencia"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                        .SetInnerValues(FactoriaChnMaterialesReferencia.GetMaterial(false))
                        .SetLabel("* Material Referencia"),
                    ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                        .SetHeightMultiline(45)
                },
                IsUpdating = true,
                DefaultSettings = new PropertyControlSettings
                {
                    SelectionChanged = (s, e) => instance.RealizarCalculo()
                }
            });

            if (CHNcontrol != null)
            {
                foreach (ReplicaChnControl replica in CHNcontrol.Replicas)
                    instance.CrearPanelReplica(replica);
            }

            instance.GenerarPanelCalculo1();
            instance.GenerarPanelCalculo2();
        }

        private void CrearPanelReplica(ReplicaChnControl replica)
        {
            TypePanel panelReplica = new TypePanel();
            panelReplica.Build(replica, new TypePanelSettings<ReplicaChnControl>
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
                            .SetEnabled(false)
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

        private void GenerarPanelCalculo1()
        {
            panelCalculoCV.Build(new ChnControl(),
                new TypePanelSettings<ChnControl>
                {
                    Fields = new FieldSettings
                    {
                        ["CV_C"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("CV")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["CV_H"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("CV")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["CV_N"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("CV")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"])
                    }
                });
        }

        private void GenerarPanelCalculo2()
        {
            panelCalculoEr.Build(new ChnControl(),
                new TypePanelSettings<ChnControl>
                {
                    Fields = new FieldSettings
                    {
                        ["Er_C"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("Er")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["Er_H"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("Er")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"]),
                        ["Er_N"] = PropertyControlSettingsEnum.TextBoxLargeEmptyToNull
                            .SetLabel("Er")
                            .SetReadOnly(true)
                            .SetColor((SolidColorBrush)Application.Current.Resources["TextBoxDisabledBrush"])
                    }
                });
        }

        private void RealizarCalculo()
        {
            RealizarCalculoCV();
            RealizarCalculoEr();
        }

        private void RealizarCalculoCV()
        {
            if (CHNcontrol.Replicas.Where(r => r.Valido == true).Count() == 0)
            {
                VaciarCalculoCV("C");
                VaciarCalculoCV("H");
                VaciarCalculoCV("N");
            }
            else
            {
                CalculoCV(CHNcontrol.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeC == null), "C", r => r.PorcentajeC);
                CalculoCV(CHNcontrol.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeH == null), "H", r => r.PorcentajeH);
                CalculoCV(CHNcontrol.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeN == null), "N", r => r.PorcentajeN);
            }
        }

        private void RealizarCalculoEr()
        {
            if (panelCHNcci.GetValidatedInnerValue<ChnControl>() == default(ChnControl) || CHNcontrol.Replicas.Where(r => r.Valido == true).Count() == 0)
            {
                VaciarCalculoEr("C");
                VaciarCalculoEr("H");
                VaciarCalculoEr("N");
            }
            else
            {
                ChnMaterialReferencia materialReferencia = PersistenceManager.SelectByID<ChnMaterialReferencia>(CHNcontrol.IdMaterialReferencia);

                CalculoEr(CHNcontrol.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeC == null), "C", r => r.PorcentajeC, Valor.Of(materialReferencia.PorcentajeC, "%"));
                CalculoEr(CHNcontrol.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeH == null), "H", r => r.PorcentajeH, Valor.Of(materialReferencia.PorcentajeH, "%"));
                CalculoEr(CHNcontrol.Replicas.Where(r => r.Valido == true).Any(r => r.PorcentajeN == null), "N", r => r.PorcentajeN, Valor.Of(materialReferencia.PorcentajeN, "%"));
            }
        }

        private void CalculoCV(Boolean vaciar, String param, Func<ReplicaChnControl, double?> funcPorcentaje)
        {
            if (vaciar)
            {
                VaciarCalculoCV(param);
            }
            else
            {
                Valor[] valoresPorcentaje = CHNcontrol.Replicas.Where(r => r.Valido == true)
                    .Select(funcPorcentaje)
                    .Select(v => Valor.Of(v, "%")).ToArray();

                RealizarCalculoCV(param, valoresPorcentaje);
            }
        }

        private void RealizarCalculoCV(string param, Valor[] valoresPorcentaje)
        {
            CHNcontrol[param].CV = Calcular.CoeficienteVariacion(valoresPorcentaje).Value;
            CHNcontrol[param].AceptadoCV = Calcular.EsAceptado(CHNcontrol[param].CV ?? 0, CHNcontrol.IdVProcedimiento, CHNcontrol[param].IdParametro);

            panelCalculoCV["CV_" + param].SetInnerContent(Calcular.VisualizeDecimals(CHNcontrol[param].CV, 2));
            labelAceptacionCV(param).Aceptacion(CHNcontrol[param].AceptadoCV);
        }

        private void VaciarCalculoCV(string param)
        {
            panelCalculoCV["CV_" + param].SetInnerContent(String.Empty);
            labelAceptacionCV(param).Visibility = Visibility.Collapsed;
        }

        private void CalculoEr(Boolean vaciar, String param, Func<ReplicaChnControl, double?> funcPorcentaje, Valor valorCCI)
        {
            if (vaciar)
            {
                VaciarCalculoEr(param);
            }
            else
            {
                Valor[] valoresPorcentaje = CHNcontrol.Replicas.Where(r => r.Valido == true)
                    .Select(funcPorcentaje)
                    .Select(v => Valor.Of(v, "%")).ToArray();

                RealizarCalculoEr(param, valoresPorcentaje, valorCCI);
            }
        }

        private void RealizarCalculoEr(string param, Valor[] valoresPorcentaje, Valor valorCCI)
        {
            CHNcontrol[param].Er = Calcular.CHN_CCI_8_5(valorCCI, valoresPorcentaje).Value;
            CHNcontrol[param].AceptadoEr = Calcular.EsAceptado(CHNcontrol[param].Er ?? 0, CHNcontrol.IdVProcedimiento, CHNcontrol[param].IdParametro_CCI);

            panelCalculoEr["Er_" + param].SetInnerContent(Calcular.VisualizeDecimals(CHNcontrol[param].Er, 2));
            labelAceptacionEr(param).Aceptacion(CHNcontrol[param].AceptadoEr, true);
        }

        private void VaciarCalculoEr(string param)
        {
            panelCalculoEr["Er_" + param].SetInnerContent(String.Empty);
            labelAceptacionEr(param).Visibility = Visibility.Collapsed;
        }

        private Label labelAceptacionCV(String param)
        {
            switch (param)
            {
                case "C":
                    return labelAceptacionCV_C;
                case "H":
                    return labelAceptacionCV_H;
                case "N":
                    return labelAceptacionCV_N;
                default:
                    return new Label();
            }
        }

        private Label labelAceptacionEr(String param)
        {
            switch (param)
            {
                case "C":
                    return labelAceptacionEr_C;
                case "H":
                    return labelAceptacionEr_H;
                case "N":
                    return labelAceptacionEr_N;
                default:
                    return new Label();
            }
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            int idPorcentaje = Unidad.Of("%").Id;

            ReplicaChnControl replica = new ReplicaChnControl
            {
                IdUdsPorcentajeC = idPorcentaje,
                IdUdsPorcentajeH = idPorcentaje,
                IdUdsPorcentajeN = idPorcentaje,
                Valido = true
            };
            int nCont = CHNcontrol.Replicas.Count;
            replica.Num = (nCont == 0) ? 1 : CHNcontrol.Replicas[nCont - 1].Num + 1;

            CHNcontrol.Replicas.Add(replica);
            CrearPanelReplica(replica);

            RealizarCalculo();
        }

        private void Deletereplica_Click(object sender, RoutedEventArgs e)
        {
            if (CHNcontrol.Replicas.Count > 0)
            {
                CHNcontrol.Replicas.RemoveAt(CHNcontrol.Replicas.Count - 1);
                listaReplicas.Children.RemoveAt(listaReplicas.Children.Count - 1);

                RealizarCalculo();
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {

            if (panelCHNcci.GetValidatedInnerValue<ChnControl>() != default(ChnControl))
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        PersistenceDataManipulation.Guardar(conn, CHNcontrol);
                        PersistenceDataManipulation.GuardarElement1N(conn, CHNcontrol, CHNcontrol.Replicas, c => c.Id, "IdCHN");
                        PersistenceDataManipulation.Borrar1N(conn, CHNcontrol.Replicas, CHNcontrol.Id, "IdCHN");
                        trans.Commit();
                        MessageBox.Show("Datos guardados con éxito");

                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();

                        CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar los datos del CCI", ex);
                        MessageBox.Show("Error al guardar los datos del CCI. Por favor, informa a soporte.");
                        bBack.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent)); /* execute click event in button back (bBack) */

                    }
                }
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }

        }
    }
}
