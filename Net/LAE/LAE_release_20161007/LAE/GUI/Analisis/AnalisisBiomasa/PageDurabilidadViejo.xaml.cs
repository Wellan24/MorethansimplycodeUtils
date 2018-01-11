using Cartif.Expectation;
using Cartif.Extensions;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Calculos;
using LAE.Clases;
using LAE.Modelo;
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
    /// Lógica de interacción para PageDurabilidadViejo.xaml
    /// </summary>
    public partial class PageDurabilidadViejo : UserControl
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

        private Finos finos;
        public Finos Finos
        {
            get { return finos; }
            set
            {
                finos = value;
                CargarFinos();
                RealizarCalculoFinos();
            }
        }

        private Durabilidad durabilidad;
        public Durabilidad Durabilidad
        {
            get { return durabilidad; }
            set
            {
                durabilidad = value;
                CargarDurabilidad();
                RealizarCalculosDurabilidad();
            }
        }

        public PageDurabilidadViejo()
        {
            InitializeComponent();
            //    DatosFinos();
            //    DatosDurabilidad();
        }

        //private void DatosFinos()
        //{
        //    /*Finos pruebaFinos = new Finos() { IdVProcedimiento = 9 };*/
        //    Finos pruebaFinos = new Finos() { };

        //    pruebaFinos.Replicas = new List<ReplicaFinos>();

        //    pruebaFinos.Replicas.Add(new ReplicaFinos() { M1 = 504.83, IdUdsM1 = 12, M2 = 504.68, IdUdsM2 = 12, Num = 1, Valido = true });
        //    pruebaFinos.Replicas.Add(new ReplicaFinos() { M1 = 504.39, IdUdsM1 = 12, M2 = 504.27, IdUdsM2 = 12, Num = 2, Valido = true });

        //    Finos = pruebaFinos;
        //}

        //private void DatosDurabilidad()
        //{
        //    /*Durabilidad pruebaDurabilidad = new Durabilidad() { IdVProcedimiento = 9 };*/
        //    Durabilidad pruebaDurabilidad = new Durabilidad() { };

        //    pruebaDurabilidad.Replicas = new List<ReplicaDurabilidad>();

        //    pruebaDurabilidad.Replicas.Add(new ReplicaDurabilidad() { M2 = 504.68, IdUdsM2 = 12, M3 = 496.12, IdUdsM3 = 12, Num = 1, Valido = true });
        //    pruebaDurabilidad.Replicas.Add(new ReplicaDurabilidad() { M2 = 504.27, IdUdsM2 = 12, M3 = 496.13, IdUdsM3 = 12, Num = 2, Valido = true });

        //    Durabilidad = pruebaDurabilidad;
        //}

        private void CargarFinos()
        {
            panelFinos.Build(Finos,
                   new TypePanelSettings<Finos>
                   {
                       Fields = new FieldSettings
                       {
                           ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                               .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(Finos.IdParametro))
                               .SetLabel("Versión")
                               .AddSelectionChanged((s, e) => CambioProcedimiento())
                       },
                       IsUpdating = true,
                   });

            panelFinos["IdVProcedimiento"].SelectedIndex = 0;

            foreach (ReplicaFinos replica in Finos.Replicas)
                CrearPanelReplicaFinos(replica);

            panelCalculosFinos.Build(Finos,
                new TypePanelSettings<Finos>
                {
                    Fields = new FieldSettings
                    {
                        ["MediaFinos2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Promedio FIN(%)")
                            .SetReadOnly(true)
                    }
                });
        }

        private void CargarDurabilidad()
        {
            CambioProcedimiento();
            foreach (ReplicaDurabilidad replica in Durabilidad.Replicas)
                CrearPanelReplicaDurabilidad(replica);

            panelCalculosDurabilidad.Build(Durabilidad,
                new TypePanelSettings<Durabilidad>
                {
                    Fields = new FieldSettings
                    {
                        ["Durabilidad2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Promedio DUR(%)")
                            .SetReadOnly(true),
                        ["Dif2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Dif")
                            .SetReadOnly(true),
                    }
                });
        }

        private void CrearPanelReplicaFinos(ReplicaFinos replica)
        {
            TypePanel panelReplicaFinos = new TypePanel();
            panelReplicaFinos.Build(replica,
                new TypePanelSettings<ReplicaFinos>
                {
                    ColumnWidths = new int[] { 3, 3, 1, 3, 1, 3 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("Replica" + replica.Num)
                        .AddCheckedChanged((s, e) => CambioReplica(s, replica.Num)),
                        ["M1"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2081"),
                        ["IdUdsM1"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["M2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2082")
                            .AddTextChanged((s, e) => CambioMasa2(s, replica.Num, replica.M2)),
                        ["IdUdsM2"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false)
                            .AddSelectionChanged((s, e) => CambioUdsMasa2(s, replica.Num, replica.IdUdsM2)),
                        ["Finos2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Finos(%)")
                            .SetReadOnly(true)
                            .AddTextChanged((s, e) => { }),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => RealizarCalculoFinos(),
                        SelectionChanged = (s, e) => RealizarCalculoFinos()
                    },
                    PanelValidation = Expectation<ReplicaFinos>
                        .ShouldNotBe().AddCriteria(h => h.M1 == null)
                        .NotBe().AddCriteria(h => h.M2 == null)
                });
            listaReplicasFinos.Children.Add(panelReplicaFinos);
        }

        private void CrearPanelReplicaDurabilidad(ReplicaDurabilidad replica)
        {
            TypePanel panelReplicaDurabilidad = new TypePanel();
            panelReplicaDurabilidad.Build(replica,
                new TypePanelSettings<ReplicaDurabilidad>
                {
                    ColumnWidths = new int[] { 3, 1, 3 },
                    Fields = new FieldSettings
                    {
                        ["M3"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2083"),
                        ["IdUdsM3"] = PropertyControlSettingsEnum.ComboBoxDefault
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["Durabilidad2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Durabilidad(%)")
                            .SetReadOnly(true)
                            .AddTextChanged((s, e) => { }),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => RealizarCalculosDurabilidad(),
                        SelectionChanged = (s, e) => RealizarCalculosDurabilidad()
                    },
                    PanelValidation = Expectation<ReplicaDurabilidad>
                        .ShouldNotBe().AddCriteria(h => h.M3 == null)
                });
            listaReplicasDurabilidad.Children.Add(panelReplicaDurabilidad);
        }

        private void CambioMasa2(object s, int num, double? m2)
        {
            Durabilidad.Replicas.Where(r => r.Num == num).FirstOrDefault().M2 = m2;
            RealizarCalculoFinos();
            RealizarCalculosDurabilidad();
        }

        private void CambioUdsMasa2(object s, int num, int? idUdsM2)
        {
            Durabilidad.Replicas.Where(r => r.Num == num).FirstOrDefault().IdUdsM2 = idUdsM2;
            RealizarCalculoFinos();
            RealizarCalculosDurabilidad();
        }

        private void CambioProcedimiento()
        {
            RealizarCalculoFinos();
            if (Durabilidad != null) {
                Durabilidad.IdVProcedimiento = Finos.IdVProcedimiento;
                RealizarCalculosDurabilidad();
            }
        }

        private void CambioReplica(object sender, int num)
        {
            CheckBox checkbox = sender as CheckBox;
            if (checkbox != null)
                Durabilidad.Replicas.Where(r => r.Num == num).FirstOrDefault().Valido = checkbox.IsChecked;

            RealizarCalculoFinos();
            RealizarCalculosDurabilidad();
        }

        private void RealizarCalculoFinos()
        {
            if (panelFinos.GetValidatedInnerValue<Finos>() != default(Finos))
            {
                listaReplicasFinos.Children.OfType<TypePanel>().ForEach(tp =>
                {
                    ReplicaFinos replica = tp.InnerValue as ReplicaFinos;
                    if (tp.GetValidatedInnerValue<ReplicaFinos>() != default(ReplicaFinos))
                    {
                        Valor m1 = Valor.Of(replica.M1, replica.IdUdsM1 ?? 0);
                        Valor m2 = Valor.Of(replica.M2, replica.IdUdsM2 ?? 0);
                        replica.Finos = Calcular.Finos_8_9(m1, m2).Value;

                        tp["Finos2"].SetInnerContent(Calcular.VisualizeDecimals(replica.Finos, 2));
                    }
                    else
                    {
                        replica.Finos = null;
                        tp["Finos2"].SetInnerContent(String.Empty);
                    }
                });

                if (Finos.Replicas.Exists(r => r.Valido == true && r.Finos == null) || Finos.Replicas.Where(r => r.Valido == true).Count() == 0)
                {
                    panelCalculosFinos.Clear();
                    labelAceptacionFinos.Visibility = Visibility.Collapsed;
                    UCInformeFinos.panelResultado.Clear();
                }
                else
                {
                    Finos.MediaFinos = Calcular.Promedio(Finos.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.Finos, "%")).ToArray()).Value;

                    panelCalculosFinos["MediaFinos2"].SetInnerContent(Calcular.VisualizeDecimals(Finos.MediaFinos, 2));

                    ResultadoInforme resultado = Calcular.Resultado(Finos.MediaFinos ?? 0, Finos.IdVProcedimiento, Finos.IdParametro, 1, 1);
                    UCInformeFinos.Resultado = resultado;
                }
            }
            else
            {
                panelCalculosFinos.Clear();
                labelAceptacionFinos.Visibility = Visibility.Collapsed;
                UCInformeFinos.panelResultado.Clear();
            }

        }

        private void RealizarCalculosDurabilidad()
        {
            if (panelFinos.GetValidatedInnerValue<Finos>() != default(Finos))/*en panelFinos tenga la versión del procedimiento de durabilidad y finos*/
            {
                listaReplicasDurabilidad.Children.OfType<TypePanel>().ForEach(tp =>
                {
                    ReplicaDurabilidad replica = tp.InnerValue as ReplicaDurabilidad;
                    if (tp.GetValidatedInnerValue<ReplicaDurabilidad>() != default(ReplicaDurabilidad) && replica.M2 != null) /* En durabilidad replica.M2 no esta en el panel porque se coge de finos */
                    {
                        Valor m2 = Valor.Of(replica.M2, replica.IdUdsM2 ?? 0);
                        Valor m3 = Valor.Of(replica.M3, replica.IdUdsM3 ?? 0);
                        replica.Durabilidad = Calcular.Durabilidad_8_9(m2, m3).Value;

                        tp["Durabilidad2"].SetInnerContent(Calcular.VisualizeDecimals(replica.Durabilidad, 2));
                    }
                    else
                    {
                        replica.Durabilidad = null;
                        tp["Durabilidad2"].SetInnerContent(String.Empty);
                    }
                });

                if (Durabilidad.Replicas.Exists(r => r.Valido == true && r.Durabilidad == null) || Durabilidad.Replicas.Where(r => r.Valido == true).Count() == 0)
                {
                    panelCalculosDurabilidad.Clear();
                    labelAceptacionDurabilidad.Visibility = Visibility.Collapsed;
                    UCInformeDurabilidad.panelResultado.Clear();
                }
                else
                {
                    Durabilidad.MediaDurabilidad = Calcular.Promedio(Durabilidad.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.Durabilidad, "%")).ToArray()).Value;
                    Durabilidad.Dif = Calcular.DiferenciaAbsoluta(Durabilidad.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.Durabilidad, "%")).ToArray()).Value;

                    panelCalculosDurabilidad["Durabilidad2"].SetInnerContent(Calcular.VisualizeDecimals(Durabilidad.MediaDurabilidad, 2));
                    panelCalculosDurabilidad["Dif2"].SetInnerContent(Calcular.VisualizeDecimals(Durabilidad.Dif, 2));

                    Durabilidad.Aceptado = Calcular.EsAceptado(Durabilidad.Dif ?? 0, Durabilidad.IdVProcedimiento, Durabilidad.IdParametro, Durabilidad.MediaDurabilidad);

                    labelAceptacionDurabilidad.Aceptacion(Durabilidad.Aceptado);

                    ResultadoInforme resultado = Calcular.Resultado(Durabilidad.MediaDurabilidad ?? 0, Durabilidad.IdVProcedimiento, Durabilidad.IdParametro, 1, 1);
                    UCInformeDurabilidad.Resultado = resultado;
                }
            }
            else
            {
                panelCalculosDurabilidad.Clear();
                labelAceptacionDurabilidad.Visibility = Visibility.Collapsed;
                UCInformeDurabilidad.panelResultado.Clear();
            }
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            int idGramos = Unidad.Of("Gramos").Id;
            AddReplicaFinos(idGramos);
            AddReplicaDurabilidad(idGramos);

            RealizarCalculoFinos();
            RealizarCalculosDurabilidad();
        }

        private void AddReplicaFinos(int idGramos)
        {
            ReplicaFinos replicaFinos = new ReplicaFinos()
            {
                IdUdsM1 = idGramos,
                IdUdsM2 = idGramos,
                Valido = true
            };
            int nContFinos = Finos.Replicas.Count;
            replicaFinos.Num = (nContFinos == 0) ? 1 : Finos.Replicas[nContFinos - 1].Num + 1;

            Finos.Replicas.Add(replicaFinos);
            CrearPanelReplicaFinos(replicaFinos);
        }

        private void AddReplicaDurabilidad(int idGramos)
        {
            ReplicaDurabilidad replicaDurabilidad = new ReplicaDurabilidad()
            {
                IdUdsM2 = idGramos,
                IdUdsM3 = idGramos,
                Valido = true
            };
            int nContDurabilidad = Durabilidad.Replicas.Count;
            replicaDurabilidad.Num = (nContDurabilidad == 0) ? 1 : Durabilidad.Replicas[nContDurabilidad - 1].Num + 1;

            Durabilidad.Replicas.Add(replicaDurabilidad);
            CrearPanelReplicaDurabilidad(replicaDurabilidad);
        }

        private void Deletereplica_Click(object sender, RoutedEventArgs e)
        {
            if (Finos.Replicas.Count > 0 && Durabilidad.Replicas.Count > 0)
            {
                Finos.Replicas.RemoveAt(Finos.Replicas.Count - 1);
                listaReplicasFinos.Children.RemoveAt(listaReplicasFinos.Children.Count - 1);

                Durabilidad.Replicas.RemoveAt(Durabilidad.Replicas.Count - 1);
                listaReplicasDurabilidad.Children.RemoveAt(listaReplicasDurabilidad.Children.Count - 1);

                RealizarCalculoFinos();
                RealizarCalculosDurabilidad();
            }
        }
    }
}
