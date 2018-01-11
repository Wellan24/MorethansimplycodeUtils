using Cartif.Util;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
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

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlFocoPlanMedicion.xaml
    /// </summary>
    public partial class ControlFocoPlanMedicion : UserControl
    {
        private FocoAtmosfera foco;
        public FocoAtmosfera Foco
        {
            get { return foco; }
            set
            {
                foco = value;
                GenerarDatosFoco();
                GenerarZonaMuestreo();
                GenerarPuntosMuestreo();
            }
        }

        public ControlFocoPlanMedicion()
        {
            InitializeComponent();
        }

        private void GenerarDatosFoco()
        {
            panelDatos.Build(Foco,
                new TypePanelSettings<FocoAtmosfera>
                {
                    Fields = new FieldSettings
                    {
                        ["Denominacion"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35)
                            .SetLabel("Denominación del foco"),
                        ["Descripcion"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35)
                            .SetLabel("Descripción del proceso asociado al foco")
                            .SetColumnSpan(2),
                        ["IdRegimen"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaRegimenFPMAtmosfera.GetRegimenes())
                            .SetDisplayMemberPath("Nombre")
                            .SetLabel("Régimen de la emisión"),
                        ["Depuracion"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlCheckBox),
                            CheckedChanged = CambioDepuracion
                        },
                        ["TecnicaDepuracion"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Descripción depuración"),
                        ["Combustible"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35),
                        ["Condiciones"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35)
                            .SetLabel("Condiciones de funcionamiento"),
                        ["Accesos"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetHeightMultiline(35)
                            .SetLabel("Descripción accesos zona de muestreo")
                            .SetControlToolTipText("Descripción accesos zona de muestreo"),
                    },
                    IsUpdating = true
                });
            panelDatos["TecnicaDepuracion"].ChangeVisibiliy(false);

            panelConexion.Build(Foco,
                new TypePanelSettings<FocoAtmosfera>
                {
                    Fields = new FieldSettings
                    {
                        ["Circular"] = new PropertyControlSettings
                        {
                            InnerValues = new ComboBoxItem<Boolean>[] {
                                ComboBoxItem<Boolean>.Create("Sección circular", true),
                                ComboBoxItem<Boolean>.Create("Sección rectangular", false)
                            },
                            Type = typeof(PropertyControlComboBox),
                            Label = "*Tipo de sección",
                            SelectionChanged = CambioSeccion
                        },
                        ["DiametroChimenea"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Diametro chimenea"),
                        ["IdUdsDiamChimenea"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Longitud"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel(""),
                        ["Lado1Chimenea"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Lado 1 chimenea"),
                        ["Lado2Chimenea"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Lado 2 chimenea"),
                    },
                    IsUpdating = true
                });

            panelConexion["DiametroChimenea"].Visibility = Visibility.Collapsed;
            panelConexion["Lado1Chimenea"].Visibility = Visibility.Collapsed;
            panelConexion["Lado2Chimenea"].Visibility = Visibility.Collapsed;

            panelMedicion.Build(Foco,
                new TypePanelSettings<FocoAtmosfera>
                {
                    ColumnWidths = new int[] { 3, 1, 3, 1, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["DiametroConex"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetLabel("Diámetro conexiones"),
                        ["IdUdsDiametroConex"] = PropertyControlSettingsEnum.ComboBoxDefaultLarge
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Longitud"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel(""),
                        ["PerturbacionInferior"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetLabel("Distancia perturbación inferior L1"),
                        ["IdUdsPerturbacionInferior"] = PropertyControlSettingsEnum.ComboBoxDefaultLarge
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Longitud"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel(""),
                        ["PerturbacionSuperior"] = PropertyControlSettingsEnum.TextBoxDefaultLarge
                            .SetLabel("Distancia perturbación superior o salida L2"),
                        ["IdUdsPerturbacionSuperior"] = PropertyControlSettingsEnum.ComboBoxDefaultLarge
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Longitud"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel(""),

                    },
                    IsUpdating = true
                });
        }

        private void CambioDepuracion(object sender, RoutedEventArgs e)
        {
            FocoAtmosfera foco = panelDatos.InnerValue as FocoAtmosfera;
            if (foco.Depuracion == true)
            {
                panelDatos["TecnicaDepuracion"].ChangeVisibiliy(true);
            }
            else
            {
                panelDatos["TecnicaDepuracion"].ChangeVisibiliy(false);
                panelDatos["TecnicaDepuracion"].SetInnerContent(null);
            }
        }

        private void CambioSeccion(object sender, SelectionChangedEventArgs e)
        {
            FocoAtmosfera f = panelConexion.InnerValue as FocoAtmosfera;
            bool circular = f.Circular == true;

            panelConexion["DiametroChimenea"].ChangeVisibiliy(circular);
            panelConexion["Lado1Chimenea"].ChangeVisibiliy(!circular);
            panelConexion["Lado2Chimenea"].ChangeVisibiliy(!circular);


            if (circular)
            {
                panelConexion["Lado1Chimenea"].SetInnerContent(0);
                panelConexion["Lado1Chimenea"].SetInnerContent(0);
            }
            else
            {
                panelConexion["DiametroChimenea"].SetInnerContent(0);
            }
        }

        private void GenerarZonaMuestreo()
        {
            MuestreoFocoAtm[] muestreos = PersistenceManager.SelectAll<MuestreoFocoAtm>().ToArray();
            PropertyControlCheckBox pc;
            foreach (MuestreoFocoAtm item in muestreos)
            {
                pc = new PropertyControlCheckBox()
                {
                    Label = item.Nombre,
                    Tag = item.Id
                };

                zonasMuestreo.Children.Add(pc);
            }
        }

        private void GenerarPuntosMuestreo()
        {
            ControlLineaCabeceraPuntoMuestreo lineaCabecera = new ControlLineaCabeceraPuntoMuestreo() { Conexion = foco.Conexiones.FirstOrDefault() };
            lineasPuntosMuestreo.Children.Add(lineaCabecera);

            foreach (ConexionFocoAtm conexion in foco.Conexiones)
            {
                ControlLineaPuntoMuestreo lineaPuntos = new ControlLineaPuntoMuestreo() { Conexion = conexion };
                lineasPuntosMuestreo.Children.Add(lineaPuntos);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
