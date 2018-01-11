using Cartif.Expectation;
using Cartif.Extensions;
using Cartif.XamlResources;
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
    /// Lógica de interacción para PageDensidadViejo.xaml
    /// </summary>
    public partial class PageDensidadViejo : UserControl
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

        private Densidad densidad;
        public Densidad Densidad
        {
            get { return densidad; }
            set
            {
                densidad = value;
                CargarDensidad();
                RealizarCalculo();
            }
        }

        //private Valor humedad;
        //public Valor Humedad
        //{
        //    get { return humedad; }
        //    set
        //    {
        //        humedad = value;
        //        //labelHumedad.Content = (Humedad!=null)?  String.Format("Humedad total= {0:#.#}", humedad.Value):"Humedad total= No calculada";
        //        RealizarCalculo();
        //    }
        //}

        //public void HumedadTotal(Valor v)
        //{
        //    Humedad = v;
        //}

        public PageDensidadViejo()
        {
            InitializeComponent();
            //DatosDensidad();
        }

        //private void DatosDensidad()
        //{
        //    Densidad prueba = new Densidad() { };
        //    prueba.Replicas = new List<ReplicaDensidad>();
        //    prueba.Replicas.Add(new ReplicaDensidad() { M1 = 1079.4, IdUdsM1 = 12, M2 = 4245.9, IdUdsM2 = 12, Num = 1, Valido = true });
        //    prueba.Replicas.Add(new ReplicaDensidad() { M1 = 1079.4, IdUdsM1 = 12, M2 = 4236.3, IdUdsM2 = 12, Num = 2, Valido = true });
        //    Densidad = prueba;
        //}

        private void CargarDensidad()
        {
            panelDensidad.Build(Densidad,
                new TypePanelSettings<Densidad>
                {
                    Fields = new FieldSettings
                    {
                        ["IdVProcedimiento"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaVersionProcedimiento.GetVersion(Densidad.IdParametro))
                            .SetLabel("Versión"),
                        ["IdCubo"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaMaterialPNT.GetMaterial(Densidad.IdParametro))
                            .SetLabel("Cubo"),
                        ["IdHumedad"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaHumedadTotal.GetHumedades(Medicion.IdMuestra))
                            .SetLabel("Humedad")
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        SelectionChanged = (s, e) => RealizarCalculo()
                    }
                });

            panelDensidad["IdVProcedimiento"].SelectedIndex = 0; /* selectedindex solo cambio el indice si no tiene ninguno previamente */
            panelDensidad["IdCubo"].SelectedIndex = 0;

            foreach (ReplicaDensidad replica in Densidad.Replicas)
                CrearPanelReplica(replica);


            panelCalculos.Build(Densidad,
                new TypePanelSettings<Densidad>
                {
                    Fields = new FieldSettings
                    {
                        ["MediaDensidadHumeda2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Media b.h.")
                            .SetReadOnly(true),
                        ["MediaDensidadSeca2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Media b.s.")
                            .SetReadOnly(true),
                        ["Dif2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetReadOnly(true)
                            .SetLabel("Dif."),
                    }
                });
        }

        private void CrearPanelReplica(ReplicaDensidad replica)
        {
            TypePanel panelReplica = new TypePanel();
            panelReplica.Build(replica,
                new TypePanelSettings<ReplicaDensidad>
                {
                    ColumnWidths = new int[] { 3, 3, 1, 3, 1, 3 },
                    Fields = new FieldSettings
                    {
                        ["Valido"] = PropertyControlSettingsEnum.CheckBoxDefault
                            .SetLabel("Replica" + replica.Num)
                            .AddCheckedChanged((s, e) => RealizarCalculo()),
                        ["M1"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2081"),
                        ["IdUdsM1"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["M2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("m\u2082"),
                        ["IdUdsM2"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaUnidades.GetUnidadesByTipo("Masa"))
                            .SetDisplayMemberPath("Abreviatura")
                            .SetLabel("")
                            .SetEnabled(false),
                        ["Densidad2"] = PropertyControlSettingsEnum.TextBoxEmptyToNull
                            .SetLabel("Densidad(kg/m3)")
                            .SetReadOnly(true)
                            .AddTextChanged((s, e) => { }),
                    },
                    IsUpdating = true,
                    DefaultSettings = new PropertyControlSettings
                    {
                        TextChanged = (s, e) => RealizarCalculo(),
                        SelectionChanged = (s, e) => RealizarCalculo()
                    },
                    PanelValidation = Expectation<ReplicaDensidad>
                        .ShouldNotBe().AddCriteria(h => h.M1 == null)
                        .NotBe().AddCriteria(h => h.M2 == null)
                });
            listaReplicas.Children.Add(panelReplica);
        }

        private void RealizarCalculo()
        {
            

            if (panelDensidad.GetValidatedInnerValue<Densidad>() != default(Densidad))
            {
                MaterialPNT cubo = PersistenceManager.SelectByID<MaterialPNT>(Densidad.IdCubo);
                double? humedadTotal = PersistenceManager.SelectByID<HumedadTotal>(Densidad.IdHumedad).MediaHumedadTotalCalculado;

                Valor volumen = Valor.Of(cubo.Capacidad, cubo.IdUdsCapacidad ?? 0);
                listaReplicas.Children.OfType<TypePanel>().ForEach(tp =>
                {
                    ReplicaDensidad replica = tp.InnerValue as ReplicaDensidad;
                    if (tp.GetValidatedInnerValue<ReplicaDensidad>() != default(ReplicaDensidad))
                    {
                        Valor m1 = Valor.Of(replica.M1, replica.IdUdsM1 ?? 0);
                        Valor m2 = Valor.Of(replica.M2, replica.IdUdsM2 ?? 0);
                        replica.Densidad = Calcular.Densidad_8_2(volumen, m1, m2).Value;

                        tp["Densidad2"].SetInnerContent(Calcular.VisualizeDecimals(replica.Densidad, 0));
                    }
                    else
                    {
                        replica.Densidad = null;
                        tp["Densidad2"].SetInnerContent(String.Empty);
                    }
                });

                if (Densidad.Replicas.Exists(r => r.Valido == true && r.Densidad == null) || Densidad.Replicas.Where(r => r.Valido == true).Count() == 0)
                {
                    panelCalculos.Clear();
                    labelAceptacion.Visibility = Visibility.Collapsed;
                    UCInforme.panelResultado.Clear();
                }
                else
                {
                    Densidad.MediaDensidadHumeda = Calcular.Promedio(Densidad.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.Densidad, "kg/m3")).ToArray()).Value;
                    Densidad.MediaDensidadSeca = Densidad.MediaDensidadHumeda * ((100 - humedadTotal) / 100.0);

                    Densidad.Dif = Calcular.DiferenciaAbsolutaMaxima(Densidad.Replicas.Where(r => r.Valido == true).Select(r => Valor.Of(r.Densidad, "kg/m3")).ToArray()).Value;

                    panelCalculos["MediaDensidadHumeda2"].SetInnerContent(Calcular.VisualizeDecimals(Densidad.MediaDensidadHumeda, 0));
                    panelCalculos["MediaDensidadSeca2"].SetInnerContent(Calcular.VisualizeDecimals(Densidad.MediaDensidadSeca, 0));
                    panelCalculos["Dif2"].SetInnerContent(Calcular.VisualizeDecimals(Densidad.Dif, 3));

                    Densidad.Aceptado = Calcular.EsAceptado(Densidad.Dif ?? 0, Densidad.IdVProcedimiento, Densidad.IdParametro, Densidad.MediaDensidadHumeda);

                    labelAceptacion.Aceptacion(Densidad.Aceptado);

                    ResultadoInforme resultado = Calcular.Resultado(Densidad.MediaDensidadHumeda ?? 0, Densidad.IdVProcedimiento, Densidad.IdParametro, -1, 0);
                    UCInforme.Resultado = resultado;
                }
            }
            else {
                panelCalculos.Clear();
                labelAceptacion.Visibility = Visibility.Collapsed;
                UCInforme.panelResultado.Clear();
            }
        }

        private void Addreplica_Click(object sender, RoutedEventArgs e)
        {
            int idGramos = Unidad.Of("Gramos").Id;

            ReplicaDensidad replica = new ReplicaDensidad()
            {
                IdUdsM1 = idGramos,
                IdUdsM2 = idGramos,
                Valido = true
            };
            int nCont = Densidad.Replicas.Count;
            replica.Num = (nCont == 0) ? 1 : Densidad.Replicas[nCont - 1].Num + 1;


            Densidad.Replicas.Add(replica);
            CrearPanelReplica(replica);

            RealizarCalculo();
        }

        private void Deletereplica_Click(object sender, RoutedEventArgs e)
        {
            if (Densidad.Replicas.Count > 0)
            {
                Densidad.Replicas.RemoveAt(Densidad.Replicas.Count - 1);
                listaReplicas.Children.RemoveAt(listaReplicas.Children.Count - 1);

                RealizarCalculo();
            }
        }

        public void RecargarHumedad()
        {
            int? n = panelDensidad["IdHumedad"].SelectedIndex;
            panelDensidad["IdHumedad"].InnerValues = FactoriaHumedadTotal.GetHumedades(Medicion.IdMuestra);
            panelDensidad["IdHumedad"].SelectedIndex = n;
        }
    }
}
