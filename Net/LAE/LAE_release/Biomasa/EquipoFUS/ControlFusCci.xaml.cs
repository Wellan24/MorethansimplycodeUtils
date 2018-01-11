using Cartif.Logs;
using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Biomasa.Modelo;
using LAE.Comun.Calculos;
using LAE.Comun.Clases;
using LAE.Comun.Modelo;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
using Npgsql;
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
    /// Lógica de interacción para ControlFusCci.xaml
    /// </summary>
    public partial class ControlFusCci : UserControl
    {
        private int IdEnsayo;

        private FusibilidadControl fusibilidadControl;
        public FusibilidadControl FusibilidadControl
        {
            get { return fusibilidadControl; }
            set
            {
                fusibilidadControl = value;
                CargarFusibilidad();
                RealizarCalculo();
            }
        }

        public event RoutedEventHandler BackButtonClick
        {
            add { bBack.Click += value; }
            remove { bBack.Click -= value; }
        }

        public ControlFusCci(int idEnsayo)
        {
            InitializeComponent();
            IdEnsayo = idEnsayo;
            GenerarPanelCalculo();
        }

        private void GenerarPanelCalculo()
        {
            panelCalculo.Build(new FusibilidadControl(),
                new TypePanelSettings<FusibilidadControl>
                {
                    Fields = new FieldSettings
                    {
                        ["Dif"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                            .SetLabel("Dif"),
                    }
                });
        }

        private void CargarFusibilidad()
        {
            panelFusCci.Build(FusibilidadControl, new TypePanelSettings<FusibilidadControl>
            {
                Fields = new FieldSettings
                {
                    ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                        .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(FusibilidadControl.IdParametro))
                        .SetLabel("* Versión"),
                    ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                        .SetInnerValues(FactoriaTecnicos.GetTecnicos())
                        .SetLabel("* Técnico"),
                    ["IdMaterialReferencia"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                        .SetInnerValues(FactoriaFusibilidadMaterialesreferencia.GetMaterial(FusibilidadControl.Id, true))
                        .SetLabel("* Material Referencia"),
                    ["Observaciones"] = PropertyControlSettingsEnum.TextBoxDefault
                        .SetHeightMultiline(45)
                },
                IsUpdating = true,
                DefaultSettings = new PropertyControlSettings
                {
                    SelectionChanged = (s, e) => RealizarCalculo()
                }
            });

            panelFusReplica.Build(FusibilidadControl.Replica, new TypePanelSettings<ReplicaFusibilidadControl>
            {
                Fields = new FieldSettings
                {
                    ["Temperatura"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("ºC"),
                    ["IdUdsTemperatura"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Temperatura"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                },
                IsUpdating = true,
                DefaultSettings = new PropertyControlSettings
                {
                    TextChanged = (s, e) => RealizarCalculo(),
                    SelectionChanged = (s, e) => RealizarCalculo()
                }
            });
        }

        private void RealizarCalculo()
        {
            if (FusibilidadControl.Replica.Temperatura == null || panelFusCci.GetValidatedInnerValue<FusibilidadControl>() == default(FusibilidadControl))
            {
                VaciarCalculo();
            }
            else
            {
                Calculo();
            }
        }

        private void Calculo()
        {
            FusibilidadMaterialesreferencia materialReferencia = PersistenceManager.SelectByID<FusibilidadMaterialesreferencia>(FusibilidadControl.IdMaterialReferencia);
            Valor valorCCI = Valor.Of(materialReferencia.Temperatura, Unidad.Of(materialReferencia.IdUdsTemperatura ?? 0));
            Valor valorTemperatura = Valor.Of(FusibilidadControl.Replica.Temperatura, FusibilidadControl.Replica.IdUdsTemperatura ?? 0);

            FusibilidadControl.Dif = Calcular.Fusibilidad_CCI_8_7(valorCCI, valorTemperatura).Value;
            FusibilidadControl.Aceptado = Calcular.EsAceptado(FusibilidadControl.Dif ?? 0, FusibilidadControl.IdVProcedimiento, FusibilidadControl.IdParametro, FusibilidadControl.Replica.Temperatura);

            panelCalculo["Dif"].SetInnerContent(Calcular.VisualizeDecimals(FusibilidadControl.Dif, 0));
            labelAceptacion.Aceptacion(FusibilidadControl.Aceptado, true);

        }

        private void VaciarCalculo()
        {
            panelCalculo["Dif"].SetInnerContent(String.Empty);
            labelAceptacion.Visibility = Visibility.Collapsed;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (panelFusCci.GetValidatedInnerValue<FusibilidadControl>() != default(FusibilidadControl))
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                using (NpgsqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        PersistenceDataManipulation.Guardar(conn, FusibilidadControl);
                        PersistenceDataManipulation.Guardar(conn, FusibilidadControl.Replica, FusibilidadControl.Id, "IdFusibilidad");
                        
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
        }
    }
}
