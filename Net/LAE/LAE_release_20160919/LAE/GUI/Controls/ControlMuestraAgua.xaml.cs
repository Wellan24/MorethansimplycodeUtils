using Cartif.Extensions;
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
    /// Lógica de interacción para ControlMuestraAgua.xaml
    /// </summary>
    public partial class ControlMuestraAgua : UserControl
    {

        public Action<ControlMuestraAgua> DisabledControl { get; set; }
        public Action<ControlMuestraAgua> EnabledControl { get; set; }

        private MuestraAgua muestra;
        public MuestraAgua Muestra
        {
            get { return muestra; }
            set
            {
                muestra = value;
                //GenerarBotonBorrado();
                GenerarDatosMuestra();
                GenerarAddMaterial();
                GenerarAddLocalizacion();
                GenerarTipoMuestra();
                GenerarAddParamInsitu();
                GenerarParamLab();
                GenerarAddAlicuota();
                GenerarAddEquipos();

            }
        }

        public ControlMuestraAgua()
        {
            InitializeComponent();
        }

        //private void GenerarBotonBorrado()
        //{
        //    botonEnable.Build(new object(),
        //        new TypePanelSettings<object>
        //        {
        //            Fields = new FieldSettings
        //            {
        //                ["BotonDisabled"] = new PropertyControlSettings
        //                {
        //                    Type = typeof(PropertyControlButton),
        //                    DesignPath = CSVPath.RemoveIcon,
        //                    Click = (sender, e) => { DisableMuestra(sender, e); },
        //                    BackgroundColor = new SolidColorBrush(Color.FromArgb(255, 181, 0, 0))
        //                },
        //                ["BotonEnabled"] = new PropertyControlSettings
        //                {
        //                    Type = typeof(PropertyControlButton),
        //                    DesignPath = CSVPath.AddIcon,
        //                    Click = (sender, e) => { EnableMuestra(sender, e); },
        //                    BackgroundColor = new SolidColorBrush(Color.FromArgb(255, 91, 137, 0))
        //                }
        //            },
        //            IsUpdating = true
        //        });
        //    botonEnable["BotonEnabled"].Visibility = Visibility.Collapsed;
        //}

        //private void EnableMuestra(object sender, RoutedEventArgs e)
        //{
        //    EnabledControl(this);
        //    botonEnable["BotonDisabled"].Visibility = Visibility.Visible;
        //    botonEnable["BotonEnabled"].Visibility = Visibility.Collapsed;
        //}

        //private void DisableMuestra(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que deseas eliminar la muestra? Si al finalizar la edición guarda los cambios la muestra será borrada", "Borrar muestra", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
        //    if (messageBoxResult == MessageBoxResult.Yes)
        //    {
        //        DisabledControl(this);
        //        botonEnable["BotonDisabled"].Visibility = Visibility.Collapsed;
        //        botonEnable["BotonEnabled"].Visibility = Visibility.Visible;
        //    }
        //}

        private void GenerarDatosMuestra()
        {
            panelMuestra.Build(Muestra,
                new TypePanelSettings<MuestraAgua>
                {
                    ColumnWidths = new int[] { 3, 1, 3, 1, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["GetCodigoLae"] =PropertyControlSettingsEnum.TextBoxDefault
                            .SetEnabled(false)
                            .SetColumnSpan(6)
                            .SetLabel("Identificación"),
                        ["Descripcion"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetHeightMultiline(60)
                            .SetColumnSpan(4),
                        ["Fecha"] = PropertyControlSettingsEnum.DateTimeDefault
                            .SetColumnSpan(2),
                        ["Hora"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Hora comienzo")
                            .SetColumnSpan(2),
                        ["Duracion"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Duración")
                            .SetColumnSpan(1),
                        ["IdUdsDuracion"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Tiempo"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetColumnSpan(1),
                        ["Volumen"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetColumnSpan(1),
                        ["IdUdsVolumen"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Volumen"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetColumnSpan(1),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaTecnicos.GetTecnicos())
                            .SetLabel("* Técnico")
                            .SetColumnSpan(2),
                        ["Incidencias"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetHeightMultiline(60)
                            .SetColumnSpan(4),
                    },
                    IsUpdating = true
                });

        }

        private void GenerarAddMaterial()
        {
            panelMaterial.Build(new MaterialesMuestraAgua(),
                new TypePanelSettings<MaterialesMuestraAgua>
                {
                    ColumnWidths = new int[] { 3, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["IdMaterial"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaMaterialMuestraAgua.GetTiposMaterial())
                                        .SetLabel("* MATERIAL A MUESTREAR")
                                        .AddKeyDownCombo(
                                            (sender, e) => { if (e.Key == Key.Enter) AddMaterial(); }
                                        )
                                        .AddSelectionChanged(CambioMaterial),
                        ["Descripcion"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) => { AddMaterial(); }
                        },
                    }
                });

            gridMaterial.Build(Muestra.Materiales,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdMaterial"] = new TypeGridColumnSettings
                        {
                            Label = "Tipo de material",
                            Width = 2,
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = FactoriaMaterialMuestraAgua.GetTiposMaterial(),
                                Path = "Id"
                            }
                        },
                        ["Descripcion"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("Otros"),
                        ["Borrar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.RemoveIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    DeleteMaterial(sender);
                                }
                            }
                        }
                        .SetWidth(1),
                    }
                });

            if (Muestra.Materiales.Count() == 1 && Muestra.Materiales.First().IdMaterial == 0)
                Muestra.Materiales.Clear();


            panelMaterial["Descripcion"].Visibility = Visibility.Collapsed;
        }

        private void CambioMaterial(object sender, SelectionChangedEventArgs e)
        {
            MaterialesMuestraAgua m = panelMaterial.InnerValue as MaterialesMuestraAgua;
            if (m.IdMaterial == FactoriaMaterialMuestraAgua.GetIdOtros())
                panelMaterial["Descripcion"].Visibility = Visibility.Visible;
            else {
                panelMaterial["Descripcion"].Visibility = Visibility.Collapsed;
                panelMaterial["Descripcion"].SetInnerContent(null);
            }
        }

        private void AddMaterial()
        {
            if (!panelMaterial.AddElementToList(Muestra.Materiales))
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeleteMaterial(object sender)
        {
            MaterialesMuestraAgua lineaBorrada = ((FrameworkElement)sender).DataContext as MaterialesMuestraAgua;
            Muestra.Materiales.Remove(lineaBorrada);
        }

        private void GenerarAddLocalizacion()
        {
            panelLocalizacion.Build(new LocalizacionesMuestraAgua(),
                new TypePanelSettings<LocalizacionesMuestraAgua>
                {
                    ColumnWidths = new int[] { 3, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["IdLocalizacion"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaLocalizacionMuestraAgua.GetTiposLocalizacion())
                                        .SetLabel("* LOCALIZACIÓN")
                                        .AddKeyDownCombo(
                                            (sender, e) => { if (e.Key == Key.Enter) AddLocalizacion(); }
                                        )
                                        .AddSelectionChanged(CambioLocalizacion),
                        ["Descripcion"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) => { AddLocalizacion(); }
                        },
                    }
                });

            gridLocalizacion.Build(Muestra.Localizaciones,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdLocalizacion"] = new TypeGridColumnSettings
                        {
                            Label = "Localización",
                            Width = 2,
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = FactoriaLocalizacionMuestraAgua.GetTiposLocalizacion(),
                                Path = "Id"
                            }
                        },
                        ["Descripcion"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("Otros"),
                        ["Borrar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.RemoveIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    DeleteLocalizacion(sender);
                                }
                            }
                        }
                        .SetWidth(1),
                    }
                });

            if (Muestra.Localizaciones.Count() == 1 && Muestra.Localizaciones.First().IdLocalizacion == 0)
                Muestra.Localizaciones.Clear();


            panelLocalizacion["Descripcion"].Visibility = Visibility.Collapsed;
        }

        private void CambioLocalizacion(object sender, SelectionChangedEventArgs e)
        {
            LocalizacionesMuestraAgua l = panelLocalizacion.InnerValue as LocalizacionesMuestraAgua;
            if (l.IdLocalizacion == FactoriaLocalizacionMuestraAgua.GetIdOtros())
                panelLocalizacion["Descripcion"].Visibility = Visibility.Visible;
            else {
                panelLocalizacion["Descripcion"].Visibility = Visibility.Collapsed;
                panelLocalizacion["Descripcion"].SetInnerContent(null);
            }
        }

        private void AddLocalizacion()
        {
            if (!panelLocalizacion.AddElementToList(Muestra.Localizaciones))
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeleteLocalizacion(object sender)
        {
            LocalizacionesMuestraAgua lineaBorrada = ((FrameworkElement)sender).DataContext as LocalizacionesMuestraAgua;
            Muestra.Localizaciones.Remove(lineaBorrada);
        }

        private void GenerarTipoMuestra()
        {
            panelTipoMuestra.Build(Muestra.TipoMuestra,
                new TypePanelSettings<TiposMuestraMuestraAgua>
                {
                    ColumnWidths = new int[] { 3, 1, 3, 1, 3, 1 },
                    Fields = new FieldSettings
                    {
                        ["IdTipoMuestra"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaTipoMuestraMuestraAgua.GetTiposMuestra())
                                        .SetLabel("* TIPO DE MUESTRA")
                                        .SetColumnSpan(2)
                                        .SetDisplayMemberPath("Nombre")
                                        .AddSelectionChanged(CambioTipoMuestra),
                        ["Horas"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetColumnSpan(1),
                        ["IdUdsHoras"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Tiempo"))
                            .SetDisplayMemberPath("Nombre")
                            .SetLabel("")
                            .SetColumnSpan(1),
                        ["NumPorciones"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetColumnSpan(2),
                        ["Intervalo"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetColumnSpan(1),
                        ["IdUdsIntervalo"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Tiempo"))
                            .SetDisplayMemberPath("Nombre")
                            .SetLabel("")
                            .SetColumnSpan(1),
                        ["Volumen"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetColumnSpan(1),
                        ["IdUdsVolumen"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Volumen"))
                            .SetDisplayMemberPath("Nombre")
                            .SetLabel("")
                            .SetColumnSpan(1),
                    },
                    IsUpdating = true
                });
        }

        private void CambioTipoMuestra(object sender, SelectionChangedEventArgs e)
        {
            TiposMuestraMuestraAgua tipos = panelTipoMuestra.InnerValue as TiposMuestraMuestraAgua;
            bool esPuntual = (tipos.IdTipoMuestra == FactoriaTipoMuestraMuestraAgua.GetIdPuntual());
            if (esPuntual)
            {
                panelTipoMuestra["Horas"].SetInnerContent(null);
                panelTipoMuestra["NumPorciones"].SetInnerContent(null);
                panelTipoMuestra["Intervalo"].SetInnerContent(null);
                panelTipoMuestra["Volumen"].SetInnerContent(null);
            }

            panelTipoMuestra["Horas"].Enabled = !esPuntual;
            panelTipoMuestra["NumPorciones"].Enabled = !esPuntual;
            panelTipoMuestra["Intervalo"].Enabled = !esPuntual;
            panelTipoMuestra["Volumen"].Enabled = !esPuntual;
        }

        private void GenerarAddParamInsitu()
        {
            panelParamInsitu.Build(new ParamsInsituMuestraAgua(),
                new TypePanelSettings<ParamsInsituMuestraAgua>
                {
                    ColumnWidths = new int[] { 4, 1, 3 },
                    Fields = new FieldSettings
                    {
                        ["IdParamsInsitu"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaParamInsituMuestraAgua.GetParametros())
                                        .SetLabel("* PARÁMETRO IN SITU")
                                        .SetDisplayMemberPath("Nombre")
                                        .AddKeyDownCombo(
                                            (sender, e) => { if (e.Key == Key.Enter) AddParamInsitu(); }
                                        )
                                        .AddSelectionChanged(CambioParamInsitu)
                                        .SetColumnSpan(2),
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) => { AddParamInsitu(); },
                            ColumnSpan = 1
                        },
                        ["Temperatura"] = PropertyControlSettingsEnum.TextBoxDefault
                                        .SetColumnSpan(2),
                        ["IdUdsTemperatura"] = PropertyControlSettingsEnum.ComboBoxDefault
                                        .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Temperatura"))
                                        .SetDisplayMemberPath("Abreviatura")
                                        .SetLabel("")
                                        .SetColumnSpan(1),
                        ["Caudal"] = PropertyControlSettingsEnum.TextBoxDefault
                                        .SetColumnSpan(1),
                        ["IdUdsCaudal"] = PropertyControlSettingsEnum.ComboBoxDefault
                                        .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Caudal"))
                                        .SetDisplayMemberPath("Abreviatura")
                                        .SetLabel("")
                                        .SetColumnSpan(1),
                        ["CaudalimetroLAE"] = new PropertyControlSettings
                        {
                            InnerValues = new ComboBoxItem<Boolean>[] {
                                ComboBoxItem<Boolean>.Create("Caudalímetro LAE", true),
                                ComboBoxItem<Boolean>.Create("Caudalímetro", false)
                            },
                            Type = typeof(PropertyControlComboBox),
                            Label = "Caudalímetro",
                            ColumnSpan = 1
                        },
                    }
                });

            gridParamInsitu.Build(Muestra.ParametrosInsitu,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdParamsInsitu"] = new TypeGridColumnSettings
                        {
                            Label = "Parám. in situ",
                            Width = 3,
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = FactoriaParamInsituMuestraAgua.GetParametros(),
                                Path = "Id",
                                DisplayPath = "Nombre"
                            }
                        },
                        ["Temperatura"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("Tª")
                            .SetWidth(3),
                        ["IdUdsTemperatura"] = new TypeGridColumnSettings
                        {
                            Label = "Uds",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = FactoriaUnidades.GetUnidades(),
                                Path = "Id",
                                DisplayPath = "Abreviatura"
                            },
                            Width = 2
                        },
                        ["Caudal"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetWidth(3),
                        ["IdUdsCaudal"] = new TypeGridColumnSettings
                        {
                            Label = "Uds",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = FactoriaUnidades.GetUnidades(),
                                Path = "Id",
                                DisplayPath = "Abreviatura"
                            },
                            Width = 2
                        },
                        ["CaudalimetroLAE"] = new TypeGridColumnSettings
                        {
                            Label = "Caudalímetro",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[]
                                {
                                    ComboBoxItem<Boolean>.Create("Caudalímetro LAE", true),
                                    ComboBoxItem<Boolean>.Create("Caudalímetro", false)
                                }
                            },
                            Width = 3
                        },
                        ["Borrar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.RemoveIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    DeleteParamInsitu(sender);
                                }
                            }
                        }
                        .SetWidth(2),
                    }
                });

            if (Muestra.ParametrosInsitu.Count() == 1 && Muestra.ParametrosInsitu.First().IdParamsInsitu == 0)
                Muestra.ParametrosInsitu.Clear();


            MostrarTemperatura(false);
            MostrarCaudal(false);
        }

        private void CambioParamInsitu(object sender, SelectionChangedEventArgs e)
        {
            ParamsInsituMuestraAgua p = panelParamInsitu.InnerValue as ParamsInsituMuestraAgua;
            bool esParamTemperatura = (FactoriaParamInsituMuestraAgua.GetIdsMostrarTemperatura().Contains(p.IdParamsInsitu));
            bool esParamCaudal = (FactoriaParamInsituMuestraAgua.GetIdsMostrarCaudal().Contains(p.IdParamsInsitu));

            MostrarTemperatura(esParamTemperatura);
            MostrarCaudal(esParamCaudal);
        }

        private void MostrarTemperatura(bool visible)
        {
            if (!visible)
            {
                panelParamInsitu["Temperatura"].SetInnerContent(null);
                panelParamInsitu["IdUdsTemperatura"].SetInnerContent(null);
            }
            panelParamInsitu["Temperatura"].ChangeVisibiliy(visible);
            panelParamInsitu["IdUdsTemperatura"].ChangeVisibiliy(visible);
        }

        private void MostrarCaudal(bool visible)
        {
            if (!visible)
            {
                panelParamInsitu["Caudal"].SetInnerContent(null);
                panelParamInsitu["IdUdsCaudal"].SetInnerContent(null);
                panelParamInsitu["CaudalimetroLAE"].SetInnerContent(null);
            }
            panelParamInsitu["Caudal"].ChangeVisibiliy(visible);
            panelParamInsitu["IdUdsCaudal"].ChangeVisibiliy(visible);
            panelParamInsitu["CaudalimetroLAE"].ChangeVisibiliy(visible);
        }

        private void AddParamInsitu()
        {
            if (!panelParamInsitu.AddElementToList(Muestra.ParametrosInsitu))
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeleteParamInsitu(object sender)
        {
            ParamsInsituMuestraAgua lineaBorrada = ((FrameworkElement)sender).DataContext as ParamsInsituMuestraAgua;
            Muestra.ParametrosInsitu.Remove(lineaBorrada);
        }

        private void GenerarParamLab()
        {
            Parametro[] param = FactoriaParametros.GetParametrosByTipo("Aguas");

            gridParamLab.Build(Muestra.ParametrosLaboratorio,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdParametro"] = new TypeGridColumnSettings
                        {
                            Label = "Parámetro laboratorio",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = param,
                                Path = "Id"
                            }
                        },
                    }
                });

            if (Muestra.ParametrosLaboratorio.Count() == 1 && Muestra.ParametrosLaboratorio.First().IdParametro == 0)
                Muestra.ParametrosLaboratorio.Clear();
        }

        private void GenerarAddAlicuota()
        {
            panelAlicuotas.Build(new AlicuotaMuestraAgua(),
                new TypePanelSettings<AlicuotaMuestraAgua>
                {
                    ColumnWidths = new int[] { 3, 3, 3 },
                    Fields = new FieldSettings
                    {
                        ["NumAlicuotas"] = PropertyControlSettingsEnum.TextBoxDefaultNoEmpty
                            .SetLabel("* Nº ALICUOTAS"),
                        ["RecipienteVidrio"] = new PropertyControlSettings
                        {
                            InnerValues = new ComboBoxItem<Boolean>[] {
                                ComboBoxItem<Boolean>.Create("Vidrio", true),
                                ComboBoxItem<Boolean>.Create("PE", false)
                            },
                            Type = typeof(PropertyControlComboBox),
                            Label = "Tipo Recipiente",
                            SelectionChanged = CambioRecipiente
                        },
                        ["TipoVidrio"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("T. Vidrio"),
                        ["TipoLavado"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("T. Lavado"),
                        ["Refrigeracion"] = PropertyControlSettingsEnum.CheckBoxDefault,
                        ["Conservante"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlCheckBox),
                            CheckedChanged = CambioConservante
                        },
                        ["TecnicaConservacion"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Técnica")
                            .SetColumnSpan(2),
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) => { AddAlicuota(); },
                        },
                    }
                });

            gridAlicuotas.Build(Muestra.Alicuotas,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["NumAlicuotas"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("Nº"),
                        ["RecipienteVidrio"] = new TypeGridColumnSettings
                        {
                            Label = "T. recipiente",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[]
                                {
                                    ComboBoxItem<Boolean>.Create("Vidrio", true),
                                    ComboBoxItem<Boolean>.Create("PE", false)
                                }
                            },
                        },
                        ["TipoVidrio"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("T. Vidrio"),
                        ["TipoLavado"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("T. Lavado"),
                        ["Refrigeracion"] = new TypeGridColumnSettings
                        {
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[]
                                {
                                    ComboBoxItem<Boolean>.Create("Si", true),
                                    ComboBoxItem<Boolean>.Create("No", false)
                                }
                            },
                        },
                        ["Conservante"] = new TypeGridColumnSettings
                        {
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = new ComboBoxItem<Boolean>[]
                                {
                                    ComboBoxItem<Boolean>.Create("Si", true),
                                    ComboBoxItem<Boolean>.Create("No", false)
                                }
                            },
                        },
                        ["TecnicaConservacion"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("Técnica"),
                        ["Borrar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.RemoveIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    DeleteAlicuota(sender);
                                }
                            }
                        }
                        .SetWidth(1),
                    }
                });

            if (Muestra.Alicuotas.Count() == 1 && Muestra.Alicuotas.First().Id == 0)
                Muestra.Alicuotas.Clear();

            panelAlicuotas["TipoVidrio"].ChangeVisibiliy(false);
            panelAlicuotas["TecnicaConservacion"].ChangeVisibiliy(false);
        }

        private void CambioRecipiente(object sender, SelectionChangedEventArgs e)
        {
            AlicuotaMuestraAgua alic = panelAlicuotas.InnerValue as AlicuotaMuestraAgua;
            if (alic.RecipienteVidrio == true)
            {
                panelAlicuotas["TipoVidrio"].ChangeVisibiliy(true);
            }
            else
            {
                panelAlicuotas["TipoVidrio"].ChangeVisibiliy(false);
                panelAlicuotas["TipoVidrio"].SetInnerContent(null);
            }
        }

        private void CambioConservante(object sender, RoutedEventArgs e)
        {
            AlicuotaMuestraAgua alic = panelAlicuotas.InnerValue as AlicuotaMuestraAgua;

            panelAlicuotas["TecnicaConservacion"].ChangeVisibiliy(alic.Conservante);
            if(!alic.Conservante)
                panelAlicuotas["TecnicaConservacion"].SetInnerContent(null);
        }

        private void AddAlicuota()
        {
            if (!panelAlicuotas.AddElementToList(Muestra.Alicuotas))
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeleteAlicuota(object sender)
        {
            AlicuotaMuestraAgua lineaBorrada = ((FrameworkElement)sender).DataContext as AlicuotaMuestraAgua;
            Muestra.Alicuotas.Remove(lineaBorrada);
        }

        private void GenerarAddEquipos()
        {
            panelEquipos.Build(new EquiposMuestraAgua(),
                new TypePanelSettings<EquiposMuestraAgua>
                {
                    ColumnWidths = new int[] { 3, 3 },
                    Fields = new FieldSettings
                    {
                        ["IdEquipo"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                        .SetInnerValues(FactoriaEquipoMuestraAgua.GetEquipos())
                                        .SetLabel("* EQUIPO/INSTRUMENTAL")
                                        .SetDisplayMemberPath("Nombre")
                                        .AddKeyDownCombo(
                                            (sender, e) => { if (e.Key == Key.Enter) AddEquipo(); }
                                        )
                                        .AddSelectionChanged(CambioEquipo),
                        ["Codigo"] = PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("XX"),
                        ["Descripcion"] = PropertyControlSettingsEnum.TextBoxDefault,
                        ["BotonAdd"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.ArrowDownIcon,
                            Click = (sender, e) => { AddEquipo(); }
                        },
                    },
                    IsUpdating = true
                });

            gridEquipos.Build(Muestra.Equipos,
                new TypeGridSettings
                {
                    Columns = new ColumnGridSettings
                    {
                        ["IdEquipo"] = new TypeGridColumnSettings
                        {
                            Label = "Equipo/Instrumental",
                            ColumnCombo = new TypeGCComboSettings
                            {
                                InnerValues = FactoriaEquipoMuestraAgua.GetEquipos(),
                                Path = "Id",
                                DisplayPath = "Nombre"
                            },
                            Width = 3
                        },
                        ["Codigo"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("XX")
                            .SetWidth(2),
                        ["Descripcion"] = TypeGridColumnSettingsEnum.DefaultColum
                            .SetLabel("Otros")
                            .SetWidth(3),
                        ["Borrar"] = new TypeGridColumnSettings
                        {
                            ColumnButton = new TypeGCButtonSettings
                            {
                                DesingPath = CSVPath.RemoveIcon,
                                Color = Colors.White,
                                Size = 25,
                                Margin = 4,
                                Click = (sender, e) =>
                                {
                                    DeleteEquipo(sender);
                                }
                            }
                        }
                        .SetWidth(1),
                    }
                });

            if (Muestra.Equipos.Count() == 1 && Muestra.Equipos.First().IdEquipo == 0)
                Muestra.Equipos.Clear();

            panelEquipos["Codigo"].Visibility = Visibility.Collapsed;
            panelEquipos["Descripcion"].Visibility = Visibility.Collapsed;
        }

        private void CambioEquipo(object sender, SelectionChangedEventArgs e)
        {
            EquiposMuestraAgua eq = panelEquipos.InnerValue as EquiposMuestraAgua;
            if (eq.IdEquipo == FactoriaEquipoMuestraAgua.GetIdEquipoConCodigo())
                panelEquipos["Codigo"].Visibility = Visibility.Visible;
            else {
                panelEquipos["Codigo"].Visibility = Visibility.Collapsed;
                panelEquipos["Codigo"].SetInnerContent(null);
            }

            if (eq.IdEquipo == FactoriaEquipoMuestraAgua.GetIdOtros())
                panelEquipos["Descripcion"].Visibility = Visibility.Visible;
            else {
                panelEquipos["Descripcion"].Visibility = Visibility.Collapsed;
                panelEquipos["Descripcion"].SetInnerContent(null);
            }
        }

        private void AddEquipo()
        {
            if (!panelEquipos.AddElementToList(Muestra.Equipos))
                MessageBox.Show("Datos erróneos. Por favor, revisa la información");
        }

        private void DeleteEquipo(object sender)
        {
            EquiposMuestraAgua lineaBorrada = ((FrameworkElement)sender).DataContext as EquiposMuestraAgua;
            Muestra.Equipos.Remove(lineaBorrada);
        }


        private void DeleteControlMuestra(object s, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que deseas eliminar la muestra? Si al finalizar la edición guarda los cambios la muestra será borrada", "Borrar muestra", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DisabledControl(this);
            }
        }

        public bool Validar()
        {
            /* no valido los paneles que sean para añadir datos a un grid */
            return (panelMuestra.GetValidatedInnerValue<MuestraAgua>() != default(MuestraAgua) && panelTipoMuestra.GetValidatedInnerValue<TiposMuestraMuestraAgua>() != default(TiposMuestraMuestraAgua));
        }

        public void SetEnabledControls(bool enabled)
        {
            panelMuestra.IsEnabled = enabled;
            panelMaterial.IsEnabled = enabled;
            gridMaterial.IsEnabled = enabled;
            panelLocalizacion.IsEnabled = enabled;
            gridLocalizacion.IsEnabled = enabled;
            panelTipoMuestra.IsEnabled = enabled;
            panelParamInsitu.IsEnabled = enabled;
            gridParamInsitu.IsEnabled = enabled;
            gridParamLab.IsEnabled = enabled;
            panelAlicuotas.IsEnabled = enabled;
            gridAlicuotas.IsEnabled = enabled;
            panelEquipos.IsEnabled = enabled;
            gridEquipos.IsEnabled = enabled;
        }
    }
}
