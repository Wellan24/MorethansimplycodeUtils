using Cartif.Expectation;
using Cartif.Extensions;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
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
    /// Lógica de interacción para ControlHumedad3Viejo.xaml
    /// </summary>
    public partial class ControlHumedad3Viejo : UserControl
    {
        private MedicionPNT medicion;
        public MedicionPNT Medicion
        {
            get { return medicion; }
            set
            {
                medicion = value;
                UCMedicion.Medicion = Medicion;
                Humedad = FactoriaHumedad3.GetParametro(medicion.Id)??FactoriaHumedad3.GetDefault();
            }
        }

        private Humedad3 humedad;
        public Humedad3 Humedad
        {
            get { return humedad; }
            set
            {
                humedad = value;
                CargarHumedad();
                RealizarCalculo();
            }
        }

        public Action<ControlHumedad3Viejo> DeleteControl { get; set; }

        public ControlHumedad3Viejo()
        {
            InitializeComponent();
        }

        private void CargarHumedad()
        {
            panelHumedad.Build(Humedad,
                new TypePanelSettings<Humedad3>
                {
                    Fields = new FieldSettings
                    {
                        ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(Humedad.IdParametro))
                            .SetLabel("Versión")
                            .AddSelectionChanged((s, e) => RealizarCalculo()),
                    },
                    IsUpdating = true
                });
            panelHumedad["IdVProcedimiento"].SelectedIndex = 0;

            foreach (ReplicaHumedad3 replica in Humedad.Replicas)
                CrearPanelReplica(replica);


            panelCalculos.Build(Humedad,
                new TypePanelSettings<Humedad3>
                {
                    Fields = new FieldSettings
                    {
                        ["MediaHumedadTotal2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Valor Medio")
                                .SetReadOnly(true),
                        ["Dif2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                                .SetLabel("Dif.")
                                .SetReadOnly(true),
                    }
                });
        }

        private void CrearPanelReplica(ReplicaHumedad3 replica)
        {
            TypePanel panelReplica = new TypePanel();
            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaHumedad3>
                {
                    ColumnWidths = new int[] { 3, 3, 1, 3, 1, 3, 1, 3 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("Replica" + replica.Num)
                            .AddCheckedChanged(CambioReplica),
                        ["M1"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2081"),
                        ["IdUdsM1"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["M2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2082"),
                        ["IdUdsM2"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["M3"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2083"),
                        ["IdUdsM3"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["HumedadTotal2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Humedad total(%)")
                            .AddTextChanged((s, e) => { })
                            .SetReadOnly(true),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = CambioTexto
                    },
                    PanelValidation = Expectation<ReplicaHumedad3>
                        .ShouldNotBe().AddCriteria(h => h.M1 == null)
                        .NotBe().AddCriteria(h => h.M2 == null)
                        .NotBe().AddCriteria(h => h.M3 == null)
                });
            listaReplicas.Children.Add(panelReplica);
        }

        private void CambioReplica(object sender, RoutedEventArgs e)
        {
            RealizarCalculo();
        }

        private void CambioTexto(object sender, TextChangedEventArgs e)
        {
            RealizarCalculo();
        }

        private void RealizarCalculo()
        {
            listaReplicas.Children.OfType<TypePanel>().ForEach(tp =>
            {
                ReplicaHumedad3 replica = tp.InnerValue as ReplicaHumedad3;
                if (tp.GetValidatedInnerValue<ReplicaHumedad3>() != default(ReplicaHumedad3))
                {
                    Valor m1 = Valor.Of(replica.M1, replica.IdUdsM1 ?? 0);
                    Valor m2 = Valor.Of(replica.M2, replica.IdUdsM2 ?? 0);
                    Valor m3 = Valor.Of(replica.M3, replica.IdUdsM3 ?? 0);
                    replica.HumedadTotal = Calcular.Humedad3_8_11(m1, m2, m3)?.Value;

                    tp["HumedadTotal2"].SetInnerContent(Calcular.VisualizeDecimals(replica.HumedadTotal, 2));
                }
                else
                {
                    replica.HumedadTotal = null;
                    tp["HumedadTotal2"].SetInnerContent(String.Empty);
                }
            });

            if (Humedad.Replicas.Exists(r => r.Valido == true && r.HumedadTotal == null) || Humedad.Replicas.Where(r => r.Valido == true).Count() == 0)
            {
                panelCalculos.Clear();
            }
            else
            {
                Valor[] valoresHumedad = Humedad.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.HumedadTotal, "%")).ToArray();
                Humedad.MediaHumedadTotal = Calcular.Promedio(valoresHumedad).Value;
                Humedad.Diferencia = Calcular.DiferenciaAbsoluta(valoresHumedad).Value;

                Humedad.Aceptado = Calcular.EsAceptado(Humedad.Diferencia ?? 0, Humedad.IdVProcedimiento, Humedad.IdParametro, Humedad.MediaHumedadTotal);

                panelCalculos["MediaHumedadTotal2"].SetInnerContent(Calcular.VisualizeDecimals(Humedad.MediaHumedadTotal, 1));
                panelCalculos["Dif2"].SetInnerContent(Calcular.VisualizeDecimals(Humedad.Diferencia, 2));

                labelAceptacion.Aceptacion(Humedad.Aceptado);
            }
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            int idGramos = Unidad.Of("Gramos").Id;

            ReplicaHumedad3 replica = new ReplicaHumedad3()
            {
                IdUdsM1 = idGramos,
                IdUdsM2 = idGramos,
                IdUdsM3 = idGramos,
                Valido = true,
                Num = Humedad.Replicas[Humedad.Replicas.Count - 1].Num + 1
            };
            Humedad.Replicas.Add(replica);
            CrearPanelReplica(replica);

            RealizarCalculo();
        }

        private void Deletereplica_Click(object sender, RoutedEventArgs e)
        {
            if (Humedad.Replicas.Count > 0)
            {
                Humedad.Replicas.RemoveAt(Humedad.Replicas.Count - 1);
                listaReplicas.Children.RemoveAt(listaReplicas.Children.Count - 1);

                RealizarCalculo();
            }
        }

        private void BorrarMedicion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Validar que puedo borrarla");
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Estás seguro que deseas eliminar la medición? Si al finalizar la edición guarda los cambios la medición será borrada", "Borrar medición", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
                DeleteControl(this);
        }
    }
}
