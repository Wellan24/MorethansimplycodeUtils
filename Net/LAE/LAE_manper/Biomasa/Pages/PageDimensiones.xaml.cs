using Cartif.Extensions;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Biomasa.Controles;
using LAE.Comun.Calculos;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Biomasa.Modelo;
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
    /// Lógica de interacción para PageDimensiones.xaml
    /// </summary>
    public partial class PageDimensiones : UserControl
    {

        private MedicionPNT medicion;
        public MedicionPNT Medicion
        {
            get { return medicion; }
            set
            {
                medicion = value;
                UCMedicion.Medicion = Medicion;
            }
        }

        private DimensionesPelet dimensiones;
        public DimensionesPelet Dimensiones
        {
            get { return dimensiones; }
            set
            {
                dimensiones = value;
                CargarDimensiones();
                UCEquipos.GenerarPanel(Medicion, Dimensiones.IdParametro);
                RealizarCalculo();
            }
        }

        public PageDimensiones()
        {
            InitializeComponent();
        }

        private void CargarDimensiones()
        {
            
            panelDimensiones.Build(Dimensiones,
                new TypePanelSettings<DimensionesPelet>
                {
                    Fields = new FieldSettings
                    {
                        ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                               .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(Dimensiones.IdParametro))
                               .SetLabel("Versión")
                               .AddSelectionChanged((s, e) => RealizarCalculo())
                    },
                    IsUpdating=true
                });

            panelDimensiones["IdVProcedimiento"].SelectedIndex = 0;

            foreach (ClasePelet clase in Dimensiones.Clases)
                CrearPanelClasePelet(clase);
        }

        private void CrearPanelClasePelet(ClasePelet clase)
        {
            Border b = new Border() { BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Colors.LightGray) };
            listaClasesPelet.Children.Add(b);
            StackPanel s = new StackPanel();
            b.Child = s;

            Button btn = new Button() { Content = "Borrar panel", Margin = new Thickness(10) };
            btn.Click += new RoutedEventHandler((sender, e) => BorrarPanel(b));
            s.Children.Add(btn);

            TypePanel panelClasePelet = new TypePanel();
            panelClasePelet.Build(clase,
                new TypePanelSettings<ClasePelet>
                {
                    Fields = new FieldSettings
                    {
                        ["IdDiametro"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaDiametroClasePelet.GetDiametrosClase())
                            .SetDisplayMemberPath("Nombre")
                            .SetLabel("Diámetro"),
                        ["Porcentaje"]=PropertyControlSettingsEnum.TextBoxDefault
                            .SetLabel("Porcentaje(%)")
                    },
                    IsUpdating = true
                });

            panelClasePelet["IdDiametro"].SelectedIndex = 0;

            s.Children.Add(panelClasePelet);

            ControlDiametrosPelet tabla2 = new ControlDiametrosPelet() { Clase = clase, UpdateData = () => RealizarCalculo() };
            s.Children.Add(tabla2);

            ControlLongitudesPelet tabla = new ControlLongitudesPelet() { Clase = clase, UpdateData = () => RealizarCalculo() };
            s.Children.Add(tabla);

            TypePanel panelCalculos = new TypePanel(); /* no cambiarle de orden - 2º TypePanel*/
            panelCalculos.Build(clase,
                new TypePanelSettings<ClasePelet>
                {
                    ColumnWidths = new int[] { 1, 1, 1, 1 },
                    Fields = new FieldSettings
                    {
                        ["MediaLongitud2"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                            .SetLabel("Valor medio longitud(mm)"),
                        ["DesviacionLongitud2"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                            .SetLabel("Desv. estándar longitud"),
                        ["MediaDiametro2"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                            .SetLabel("Valor medio diametro(mm)"),
                        ["DesviacionDiametro2"] = PropertyControlSettingsEnum.TextBoxEmptyToNullReadOnly
                            .SetLabel("Desv. estándar diametro(mm)"),
                    }
                });
            s.Children.Add(panelCalculos);

            CrearInformes(s);
        }

        private void CrearInformes(StackPanel s)
        {
            Grid grid = new Grid();
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(gridCol1);
            grid.ColumnDefinitions.Add(gridCol2);

            StackPanel st1 = new StackPanel();
            StackPanel st2 = new StackPanel();

            Grid.SetColumn(st1, 0);
            Grid.SetColumn(st2, 1);

            st1.Children.Add(new Label() { Content = "Longitud" });
            st2.Children.Add(new Label() { Content = "Diámetro" });

            ControlInforme informeLongitud = new ControlInforme() { Name= "InformeLongitud" };
            /* this.RegisterName("InformeLongitud", informeLongitud); solo puedo registrar un nombre una vez-> como un id unico*/
            st1.Children.Add(informeLongitud);

            ControlInforme informeDiametro = new ControlInforme() { Name = "InformeDiametro" };
            /* this.RegisterName("InformeDiametro", informeDiametro); */
            st2.Children.Add(informeDiametro);

            grid.Children.Add(st1);
            grid.Children.Add(st2);

            s.Children.Add(grid);
        }

        private void BorrarPanel(Border b)
        {
            StackPanel st = b.Child as StackPanel;
            TypePanel panelClase= st.Children.OfType<TypePanel>().FirstOrDefault();
            ClasePelet clase = panelClase.InnerValue as ClasePelet;

            Dimensiones.Clases.Remove(clase);
            listaClasesPelet.Children.Remove(b);
            RealizarCalculo();
        }

        
        private void RealizarCalculo()
        {
            listaClasesPelet.Children.OfType<Border>().ForEach(b =>
            {
                StackPanel st = b.Child as StackPanel;
                LongitudPelet[] longitudes = st.Children.OfType<ControlLongitudesPelet>().FirstOrDefault().Clase.Longitudes.ToArray();
                DiametroPelet[] diametros = st.Children.OfType<ControlDiametrosPelet>().FirstOrDefault().Clase.Diametros.ToArray();
                
                TypePanel panelCalculo = st.Children.OfType<TypePanel>().ElementAt(1) as TypePanel;
                ClasePelet clase = panelCalculo.InnerValue as ClasePelet;

                ControlInforme informeLongitud = st.Children.OfType<Grid>().FirstOrDefault().Children.OfType<StackPanel>().ElementAt(0).Children.OfType<ControlInforme>().FirstOrDefault();
                ControlInforme informeDiametro = st.Children.OfType<Grid>().FirstOrDefault().Children.OfType<StackPanel>().ElementAt(1).Children.OfType<ControlInforme>().FirstOrDefault();

                if (longitudes.Count() == 0) {
                    panelCalculo["MediaLongitud2"].SetInnerContent(String.Empty);
                    panelCalculo["DesviacionLongitud2"].SetInnerContent(String.Empty);

                    informeLongitud.panelResultado.Clear();
                }
                else
                {
                    clase.MediaLongitud = Calcular.Promedio(longitudes.Select(l => Valor.Of(l.Medida, "Milimetros")).ToArray()).Value;
                    clase.DesviacionLongitud = Calcular.DesviacionEstandar(longitudes.Select(l => Valor.Of(l.Medida, "Milimetros")).ToArray()).Value;

                    panelCalculo["MediaLongitud2"].SetInnerContent(Calcular.VisualizeDecimals(clase.MediaLongitud, 1));
                    panelCalculo["DesviacionLongitud2"].SetInnerContent(Calcular.VisualizeDecimals(clase.DesviacionLongitud, 1));

                    
                    ResultadoInforme resultadoLongitud = Calcular.Resultado(clase.MediaLongitud ?? 0, Dimensiones.IdVProcedimiento, 2, 1, 1);
                    informeLongitud.Resultado = resultadoLongitud;
                }

                if(!Array.Exists(diametros, d => d.Medida != null))
                {
                    panelCalculo["MediaDiametro2"].SetInnerContent(String.Empty);
                    panelCalculo["DesviacionDiametro2"].SetInnerContent(String.Empty);

                    informeDiametro.panelResultado.Clear();
                }
                else
                {
                    clase.MediaDiametro = Calcular.Promedio(diametros.Where(d => d.Medida != null).Select(d => Valor.Of(d.Medida, "Milimetros")).ToArray()).Value;
                    clase.DesviacionDiametro = Calcular.DesviacionEstandar(diametros.Where(d => d.Medida != null).Select(d => Valor.Of(d.Medida, "Milimetros")).ToArray()).Value;

                    panelCalculo["MediaDiametro2"].SetInnerContent(Calcular.VisualizeDecimals(clase.MediaDiametro, 1));
                    panelCalculo["DesviacionDiametro2"].SetInnerContent(Calcular.VisualizeDecimals(clase.DesviacionDiametro, 1));

                    ResultadoInforme resultadoDiametro = Calcular.Resultado(clase.MediaDiametro ?? 0, Dimensiones.IdVProcedimiento, 3, 1, 1);
                    informeDiametro.Resultado = resultadoDiametro;
                }                

            });
        }
        

        private void NuevoDiametro_Click(object sender, RoutedEventArgs e)
        {
            ClasePelet clase = new ClasePelet() { };
            clase.Diametros = new List<DiametroPelet>();
            clase.Longitudes = new List<LongitudPelet>();

            Dimensiones.Clases.Add(clase);
            CrearPanelClasePelet(clase);
        }
    }
}
