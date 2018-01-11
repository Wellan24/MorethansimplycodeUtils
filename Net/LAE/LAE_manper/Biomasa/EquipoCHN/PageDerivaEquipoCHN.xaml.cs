using Cartif.Logs;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
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
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;

namespace LAE.Biomasa.Pages
{
    /// <summary>
    /// Lógica de interacción para PageDerivaEquipoCHN.xaml
    /// </summary>
    public partial class PageDerivaEquipoCHN : UserControl
    {

        private EnsayoPNT ensayo;
        public EnsayoPNT Ensayo
        {
            get { return ensayo; }
            set
            {
                ensayo = value;
                UCEnsayo.Ensayo = Ensayo;
                UCEquipos.GenerarPanel(ensayo);
            }
        }

        private ChnDeriva chnDeriva;
        public ChnDeriva CHNderiva
        {
            get { return chnDeriva; }
            set
            {
                chnDeriva = value;
                CargarCHNderiva();

                panelCHN["IdVProcedimiento"].SelectedIndex = 0;
                panelCHN["IdMaterialReferencia"].SelectedIndex = 0;
                CambioProcedimiento();
            }
        }

        private RoutedEventHandler AnalisisGuardado;
        public event RoutedEventHandler VisibilidadAnalisis
        {
            add { AnalisisGuardado += value; }
            remove { AnalisisGuardado += value; }
        }

        public PageDerivaEquipoCHN()
        {
            InitializeComponent();
        }

        private void CargarCHNderiva()
        {
            panelCHN.Build(CHNderiva,
                new TypePanelSettings<ChnDeriva>
                {
                    Fields = new FieldSettings
                    {
                        ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(new Chn()["C"].IdParametro))
                            .SetLabel("* Versión")
                            .AddSelectionChanged((s, e) => CambioProcedimiento()),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaTecnicos.GetTecnicos())
                            .SetLabel("* Técnico"),
                        ["IdMaterialReferencia"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaChnMaterialesReferencia.GetMaterialDeriva(CHNderiva.Id, true))
                            .SetLabel("* Material Ref. Cert.(MRC)"),
                        ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetHeightMultiline(45)
                    },
                    IsUpdating = true
                });

            panelBlanco.Build(CHNderiva,
                new TypePanelSettings<ChnDeriva>
                {
                    Fields = new FieldSettings
                    {
                        ["BlancoC"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("C < x%"),
                        ["BlancoH"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("H < x%"),
                        ["BlancoN"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("N < x%"),

                    },
                    IsUpdating = true
                });

            foreach (ReplicaChnDeriva replica in CHNderiva.Replicas)
                CrearPanelReplica(replica);


            panelDeriva.Build(CHNderiva,
                new TypePanelSettings<ChnDeriva>
                {
                    Fields = new FieldSettings
                    {
                        ["ValorDerivaC"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("C: -"),
                        ["ValorDerivaH"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("H: -"),
                        ["ValorDerivaN"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("N: -"),

                    },
                    IsUpdating = true
                });
        }

        private void CrearPanelReplica(ReplicaChnDeriva replica)
        {
            TypePanel panelReplica = new TypePanel();
            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaChnDeriva>
                {
                    ColumnWidths = new int[] { 3, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefaultSmall
                            .SetLabel("Replica"),
                        ["MasaN"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("masa N"),
                        ["IdUdsMasaN"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["ValorN"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("valor N"),
                        ["IdUdsValorN"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Porcentaje"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["MasaH"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("masa H"),
                        ["IdUdsMasaH"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["ValorH"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("valor H"),
                        ["IdUdsValorH"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Porcentaje"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["MasaC"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("masa C"),
                        ["IdUdsMasaC"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["ValorC"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("valor C"),
                        ["IdUdsValorC"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Porcentaje"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                    },
                    IsUpdating = true
                });

            listaReplicas.Children.Add(panelReplica);
        }

        private void CambioProcedimiento()
        {

            ChnDeriva deriva = panelCHN.InnerValue as ChnDeriva;
            if (deriva != null)
            {
                ChnConstante constante = PersistenceManager.SelectByProperty<ChnConstante>("IdVProcedimiento", deriva.IdVProcedimiento).FirstOrDefault();
                if (constante != null)
                {
                    panelBlanco["BlancoC"].Label = String.Format("C < {0:0.0}%", constante.BlancoC);
                    panelBlanco["BlancoH"].Label = String.Format("H < {0:0.0}%", constante.BlancoH);
                    panelBlanco["BlancoN"].Label = String.Format("N < {0:0.000}%", constante.BlancoN);

                    panelDeriva["ValorDerivaC"].Label = String.Format("C: {0:0.0} - {1:0.0}", constante.DesvMinC, constante.DesvMaxC);
                    panelDeriva["ValorDerivaH"].Label = String.Format("H: {0:0.0} - {1:0.0}", constante.DesvMinH, constante.DesvMaxH);
                    panelDeriva["ValorDerivaN"].Label = String.Format("N: {0:0.0} - {1:0.0}", constante.DesvMinN, constante.DesvMaxN);
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MarcarChecks(true);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MarcarChecks(false);
        }

        private void MarcarChecks(bool marcar)
        {
            panelBlanco["BlancoC"].IsCheceked = marcar;
            panelBlanco["BlancoH"].IsCheceked = marcar;
            panelBlanco["BlancoN"].IsCheceked = marcar;

            panelDeriva["ValorDerivaC"].IsCheceked = marcar;
            panelDeriva["ValorDerivaH"].IsCheceked = marcar;
            panelDeriva["ValorDerivaN"].IsCheceked = marcar;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (panelCHN.GetValidatedInnerValue<ChnDeriva>() != default(ChnDeriva))
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        /*Guardar Ensayo*/
                        PersistenceDataManipulation.Guardar(conn, Ensayo);
                        /*Guardar Deriva*/
                        CHNderiva.IdEnsayo = Ensayo.Id;
                        PersistenceDataManipulation.Guardar(conn, CHNderiva);
                        PersistenceDataManipulation.GuardarElement1N(conn, CHNderiva, CHNderiva.Replicas, c => c.Id, "IdCHNderiva");
                        PersistenceDataManipulation.Borrar1N(conn, CHNderiva.Replicas, CHNderiva.Id, "IdCHNderiva");
                        /*GuardarEquipos*/
                        GuardarEquipos(conn);

                        trans.Commit();
                        MessageBox.Show("Datos guardados con éxito");
                        AnalisisGuardado(sender, e);
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();

                        CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error al guardar los datos del CCI", ex);
                        MessageBox.Show("Error al guardar los datos del CCI. Por favor, informa a soporte.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
        }

        private void GuardarEquipos(NpgsqlConnection conn)
        {
            List<EquipoEnsayo> equiposCHN = UCEquipos.GetEquipos();
            PersistenceDataManipulation.Guardar(conn, equiposCHN, Ensayo.Id, "IdEnsayo");
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            int idGramos = Unidad.Of("Gramos").Id;
            int idPorc = Unidad.Of("%").Id;

            ReplicaChnDeriva replica = new ReplicaChnDeriva()
            {
                IdUdsMasaC = idGramos,
                IdUdsMasaH = idGramos,
                IdUdsMasaN = idGramos,
                IdUdsValorC = idPorc,
                IdUdsValorH = idPorc,
                IdUdsValorN = idPorc,
                Valido = true
            };

            CHNderiva.Replicas.Add(replica);
            CrearPanelReplica(replica);
        }

        private void Deletereplica_Click(object sender, RoutedEventArgs e)
        {
            int numReplicas = CHNderiva.Replicas.Count;
            if (numReplicas > 0)
            {
                CHNderiva.Replicas.RemoveAt(numReplicas - 1);
                listaReplicas.Children.RemoveAt(listaReplicas.Children.Count - 1);
            }
        }
    }
}
