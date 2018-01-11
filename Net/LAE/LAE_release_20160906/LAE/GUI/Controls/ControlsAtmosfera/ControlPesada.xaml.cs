using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Calculos;
using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Cartif.Extensions;

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlPesada.xaml
    /// </summary>
    public partial class ControlPesada : UserControl
    {
        private Medicion medicion;
        public Medicion Medicion
        {
            get { return medicion; }
            set
            {
                medicion = value;
                GenerarMedicion();
            }
        }

        private Caudal caudales;
        public Caudal Caudales
        {
            get { return caudales; }
            set
            {
                caudales = value;
                GenerarMedicion2();
            }
        }

        public ControlPesada()
        {
            InitializeComponent();


        }

        public void GenerarPrueba()
        {

            medicion = new Medicion()
            {
                Pesadas = new ObservableCollection<Pesada>()
            };
            medicion.Pesadas.Add(new Pesada() { Valor = 91.66, IdUdsValor = 11, Tiempo = 60, IdUdsTiempo = 4 });
            //medicion.Pesadas.Add(new Pesada() { Valor = 91.66, IdUdsValor = 11, Tiempo = 1, IdUdsTiempo = 5 });
            medicion.Pesadas.Add(new Pesada() { Valor = 91.68, IdUdsValor = 11, Tiempo = 2, IdUdsTiempo = 5 });
            medicion.Pesadas.Add(new Pesada() { Valor = 91.71, IdUdsValor = 11, Tiempo = 3, IdUdsTiempo = 5 });
            GenerarMedicion();

        }

        public void GenerarPrueba2()
        {
            Caudales = new Caudal() { Volumen = 2.5, IdUdsVolumen = 3, Tiempo = 2, IdUdsTiempo = 6 };
        }

        private void GenerarMedicion()
        {
            foreach (Pesada pesada in medicion.Pesadas)
            {
                TypePanel panelPesada = new TypePanel();
                panelPesada.Build(pesada,
                    new TypePanelSettings<Pesada>
                    {
                        Fields = new FieldSettings
                        {
                            ["Tiempo"] = PropertyControlSettingsEnum.TextBoxDefault,
                            ["IdUdsTiempo"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Tiempo"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetColumnSpan(1),
                            ["Valor"] = PropertyControlSettingsEnum.TextBoxDefault,
                            ["IdUdsValor"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetColumnSpan(1),
                        },
                        IsUpdating = true
                    });
                listaPesadas.Children.Add(panelPesada);
            }

        }

        private void GenerarMedicion2()
        {
            panelPrueba.Build(Caudales,
                    new TypePanelSettings<Caudal>
                    {
                        Fields = new FieldSettings
                        {
                            ["Volumen"] = PropertyControlSettingsEnum.TextBoxDefault,
                            ["IdUdsVolumen"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Volumen"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetColumnSpan(1),
                            ["Tiempo"] = PropertyControlSettingsEnum.TextBoxDefault,
                            ["IdUdsTiempo"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Tiempo"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetColumnSpan(1),
                        },
                        IsUpdating = true
                    });
        }

        private void Calcular_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<double, double>[] puntos = medicion.Pesadas
                .Map(p => new KeyValuePair<double, double>(p.Tiempo, p.Valor)).ToArray();

            pesadaMinuto0.Text = Calcular.Pendiente(puntos).GetY(0).ToString();

        }

        public class Caudal
        {
            public double Volumen { get; set; }
            public int IdUdsVolumen { get; set; }
            public double Tiempo { get; set; }
            public int IdUdsTiempo { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double a = 1;
            Valor caudal= Calcular.Caudal(Valor.Of(Caudales.Volumen, Caudales.IdUdsVolumen), Valor.Of(Caudales.Tiempo, Caudales.IdUdsTiempo));
            //caudal.Convert("l/s");
            prueba.Text = caudal.ToString();
        }
    }
}
