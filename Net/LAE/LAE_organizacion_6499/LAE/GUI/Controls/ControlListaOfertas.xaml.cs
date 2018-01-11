using Cartif.Extensions;
using Cartif.Logs;
using GenericForms.Abstract;
using GenericForms.Settings;
using GUI.Wizards;
using LAE.Modelo;
using LAE.Comun.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlListaOfertas.xaml
    /// </summary>
    public partial class ControlListaOfertas : UserControl, INotifyPropertyChanged
    {
        private bool cargar = false;

        public event PropertyChangedEventHandler PropertyChanged;
        private Oferta[] ListaOfertas;
        public Oferta SelectedOferta;

        private Cliente[] Clientes;
        private Contacto[] Contactos;
        private Tecnico[] Tecnicos;

        private Object selectedValue;
        public Object SelectedValue
        {
            get { return selectedValue; }
            set
            {
                selectedValue = value;
                if (SelectedValue != null)
                {
                    /* fill oferta */
                    SelectedOferta = SelectedValue as Oferta;
                    panelOfertas.InnerValue = SelectedOferta;

                    CambiarEstadoAnulada();
                }
                OnPropertyChanged("SelectedValue");
            }
        }

        public event RoutedEventHandler VerDetallesClick
        {
            add { linkVerDetalles.Click += value; }
            remove { linkVerDetalles.Click -= value; }
        }

        public ControlListaOfertas()
        {
            InitializeComponent();
        }

        public void CargarDatosIniciales(Cliente[]c, Contacto[] co, Tecnico[] te)
        {
            Clientes = c;
            Contactos = co;
            Tecnicos = te;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void controlListaOfertas_Loaded(object sender, RoutedEventArgs e)
        {
            if (cargar)
            {
                SelectedOferta = new Oferta();
                ListaOfertas = new Oferta[] { new Oferta() };

                /* limpiar */
                panelOfertas.ClearGrid();

                /* cargar */
                CargarGridOfertas();
                GenerarPanelOferta();

                CambiarEstadoAnulada();
            }
            else
                cargar = true;
        }

        private void CambiarEstadoAnulada()
        {
            bool anulada = (SelectedValue as Oferta)?.Anulada ?? false;
            /* botones */
            bAnularOferta.IsEnabled = !anulada;
            bGuardarOferta.IsEnabled = !anulada;

            /* panel y grid */
            panelOfertas.IsEnabled = !anulada;

            /* aviso */
            if (anulada)
                AvisoOfertaAnulada.Visibility = Visibility.Visible;
            else
                AvisoOfertaAnulada.Visibility = Visibility.Hidden;
        }

        private void CargarGridOfertas()
        {
            gridOfertas.Build(ListaOfertas, new TypeGridSettings()
            {
                Columns = new ColumnGridSettings
                {
                    ["Codigo"] = TypeGridColumnSettingsEnum.DefaultColum
                        .SetLabel("Código oferta"),
                    ["AnnoOferta"] = TypeGridColumnSettingsEnum.DefaultColum
                        .SetLabel("Año oferta")
                        .SetFormat("dd/MM/yy"),
                    ["IdCliente"] = new TypeGridColumnSettings
                    {
                        Label = "Empresa",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = Clientes,
                            Path = "Id",
                            DisplayPath= "Nombre"
                        }
                    },
                    ["IdContacto"] = new TypeGridColumnSettings
                    {
                        Label = "Contacto",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = Contactos,
                            Path = "Id"
                        }
                    },
                    ["IdTecnico"] = new TypeGridColumnSettings
                    {
                        Label = "Técnico",
                        ColumnCombo = new TypeGCComboSettings
                        {
                            InnerValues = Tecnicos,
                            Path = "Id"
                        }
                    },
                },
                ForegroundRow = (o) =>
                {
                    Oferta oferta = o as Oferta;
                    if (oferta != null && oferta.Anulada)
                        return new SolidColorBrush(Colors.Gray);
                    return new SolidColorBrush(Colors.Black);
                }
            });


            ListaOfertas = PersistenceManager.SelectAll<Oferta>()
                .OrderByDescending(c => c.AnnoOferta)
                .ThenByDescending(c => c.NumCodigoOferta)
                .ToArray();
            gridOfertas.FillDataGrid(ListaOfertas);
        }

        private void GenerarPanelOferta()
        {
            panelOfertas.Build(SelectedOferta,
                new TypePanelSettings<Oferta>
                {
                    Fields = new FieldSettings
                    {
                        ["Codigo"] = PropertyControlSettingsEnum.TextBoxDefault
                                .SetLabel("Código oferta")
                                .SetEnabled(false),
                        ["AnnoOferta"] = PropertyControlSettingsEnum.DateTimeDefault
                                .SetLabel("* Año oferta")
                                .AddValueChanged((sender, e)=> {
                                    if (SelectedOferta.Id != 0)
                                    {
                                        int annoViejo = SelectedOferta.AnnoOferta.Year;
                                        int annoNUevo = ((DateTime)e.NewValue).Year;
                                        if (annoNUevo != annoViejo)
                                        {
                                            MessageBox.Show("No se puede cambiar de año la oferta");
                                            ((Xceed.Wpf.Toolkit.DateTimePicker)sender).Value = (DateTime)e.OldValue;
                                        }
                                    }
                                }),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(Tecnicos)
                                .SetLabel("* Técnico"),
                        ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(Clientes)
                                .SetLabel("* Empresa")
                                .AddSelectionChanged(RefreshIdContacto),
                        ["IdContacto"] = PropertyControlSettingsEnum.ComboBoxDefault
                                .SetInnerValues(RecuperarContactos())
                                .SetLabel("* Contacto"),
                    },
                    
                });

        }

        private Contacto[] RecuperarContactos()
        {
            Oferta o = panelOfertas.InnerValue as Oferta;
            if (o != null)
                return PersistenceManager.SelectByProperty<Contacto>("IdCliente", o.IdCliente).ToArray();

            return new Contacto[0];
        }

        private void RefreshIdContacto(object sender, SelectionChangedEventArgs e)
        {
            panelOfertas["IdContacto"].InnerValues = RecuperarContactos();
        }

        private void GuardarOferta_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                if (panelOfertas.GetValidatedInnerValue<Oferta>() != default(Oferta))
                {
                    /* update oferta */
                    Oferta ofertaActualizada = panelOfertas.InnerValue as Oferta;
                    ofertaActualizada.Update();

                    /* update grid */
                    ActualizarGridOfertas(ofertaActualizada);

                    MessageBox.Show("Oferta actualizada con éxito");

                }
                else
                    MessageBox.Show("Datos erróneos. Por favor, revisa la información");
            }
            else
                MessageBox.Show("Seleccione una oferta");
        }

        private void AnularOferta_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOferta != null && SelectedOferta?.Id != 0)
            {
                if (!FactoriaRevisionesOferta.ExisteRevisionEnviadaOAceptada(SelectedOferta))
                {

                    MessageBoxResult messageBoxResult = MessageBox.Show("¿Estas seguro que deseas anular la oferta? Una vez anulada ya no se podrá editar", "Anular oferta", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        SelectedOferta.Anulada = true;
                        SelectedOferta.Update();

                        ActualizarGridOfertas(SelectedOferta);
                        CambiarEstadoAnulada();

                        MessageBox.Show("Oferta anulada con éxito");
                    }
                }
                else
                {
                    MessageBox.Show("Imposible anular una oferta que contiene revisiones enviadas al cliente o aceptadas");
                }
            }
        }

        private void ActualizarGridOfertas(Oferta ofertaActualizada)
        {
            int indice = Array.FindIndex(ListaOfertas, o => o.Id == ofertaActualizada.Id);
            ListaOfertas[indice] = ofertaActualizada?.Clone(typeof(Oferta)) as Oferta;
            gridOfertas.FillDataGrid(ListaOfertas);
            gridOfertas.DataGrid.SelectedIndex = indice;
        }

        private void VentanaNuevaOferta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NuevaOferta np = new NuevaOferta();
                np.Owner = Window.GetWindow(this);
                np.ShowDialog();

                if (np.DialogResult ?? true)
                    ReloadOfertas();
            }
            catch (Exception ex)
            {
                CartifLogs.GenerarLog(TipoLog.From("BaseDatos"), "Error general", ex);
                MessageBox.Show("Error. Por favor, informa a soporte.");
            }

        }

        private void ReloadOfertas()
        {
            ListaOfertas= PersistenceManager.SelectAll<Oferta>()
                .OrderByDescending(c => c.AnnoOferta)
                .ThenByDescending(c => c.NumCodigoOferta)
                .ToArray();

            gridOfertas.FillDataGrid(ListaOfertas);
            gridOfertas.DataGrid.SelectedIndex = 0;

        }
    }
}