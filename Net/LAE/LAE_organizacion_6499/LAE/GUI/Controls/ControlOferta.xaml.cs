using Cartif.Extensions;
using Cartif.XamlResources;
using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using GUI.Windows;
using LAE.Modelo;
using LAE.Comun.Persistence;
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
using LAE.Comun.Modelo;

namespace GUI.Controls
{
    /// <summary>
    /// Lógica de interacción para ControlOferta.xaml
    /// </summary>
    public partial class ControlOferta : UserControl
    {

        private Tecnico[] tecnicos;
        public Tecnico[] Tecnicos
        {
            get { return tecnicos; }
            set { tecnicos = value; }
        }

        public ControlOferta()
        {
            InitializeComponent();
        }

        public Oferta oferta;
        public Oferta Oferta
        {
            get { return oferta; }
            set
            {
                oferta = value;
                if (oferta.IdTecnico == 0)
                    oferta.IdTecnico = FactoriaTecnicos.director().Id;
                RecargarPagina();
            }
        }
        
        public void RecargarPagina()
        {
            panelOferta.ClearGrid();
            GenerarPanelOferta();
            CargarDatos();
        }

        private void GenerarPanelOferta()
        {
            panelOferta.Build(new Oferta(),
                new TypePanelSettings<Oferta>
                {
                    Fields = new FieldSettings
                    {
                        ["AnnoOferta"]=PropertyControlSettingsEnum.DateTimeDefaultNoEmpty
                                .SetColumnSpan(2)
                                .SetLabel("* Año oferta"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetColumnSpan(2)
                                .SetInnerValues(Tecnicos)
                                .SetLabel("* Técnico")
                                .SetEnabled(false),
                        ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(FactoriaClientes.RecuperarClientes())
                                .SetLabel("* Empresa")
                                .AddSelectionChanged(RefreshIdContacto),
                        ["Boton"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.PlusIcon,
                            Click = (sender, e) => { NuevoCliente(); }
                        },
                        ["IdContacto"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(RecuperarContactos())
                                .SetLabel("* Contacto"),
                        ["Boton2"] = new PropertyControlSettings
                        {
                            Type = typeof(PropertyControlButton),
                            DesignPath = CSVPath.PlusIcon,
                            Click = (sender, e) => { NuevoContacto(); }
                        },
                    },
                    IsUpdating = true,
                    ColumnWidths=new int[] {3,1,3,1}
                });
            
        }

        private void CargarDatos()
        {
            panelOferta.InnerValue = Oferta;
        }

        private void RefreshIdContacto(object sender, SelectionChangedEventArgs e)
        {
            panelOferta["IdContacto"].InnerValues = RecuperarContactos();
        }

        private Contacto[] RecuperarContactos()
        {
            Oferta o = panelOferta.InnerValue as Oferta;
            if (o != null)
                return PersistenceManager.SelectByProperty<Contacto>("IdCliente", o.IdCliente).ToArray();

            return new Contacto[0];
        }

        public bool ValidarOferta()
        {
            return panelOferta.GetValidatedInnerValue<Oferta>() != default(Oferta);
        }

        private void NuevoCliente()
        {
            NuevoCliente nc = new NuevoCliente();
            nc.Owner = Window.GetWindow(this);
            nc.ShowDialog();
            if (nc.DialogResult ?? false)
            {
                panelOferta["IdCliente"].InnerValues = FactoriaClientes.RecuperarClientes();
            }
        }

        private void NuevoContacto()
        {
            NuevoContacto nc = new NuevoContacto(Tecnicos);
            nc.ShowDialog();
            if (nc.DialogResult ?? false)
            {
                RefreshIdContacto(null, null);
            }
        }
    }
}
