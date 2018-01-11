using GenericForms.Abstract;
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
    /// Lógica de interacción para ControlOferta.xaml
    /// </summary>
    public partial class ControlOferta : UserControl
    {
        private Cliente[] clientes;
        public Cliente[] Clientes
        {
            get { return clientes; }
            set { clientes = value; }
        }

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
                GenerarPanelOferta();
                CargarDatos();
            }
        }

        private void GenerarPanelOferta()
        {
            panelOferta.Build<Oferta>(new Oferta(),
                new TypePanelSettings<Oferta>
                {
                    Fields = new FieldSettings
                    {
                        ["CodigoOferta"]=PropertyControlSettingsEnum.TextBoxDefault,
                        ["AnnoOferta"]=PropertyControlSettingsEnum.TextBoxDefault,
                        ["IdCliente"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Clientes)
                                .SetLabel("Cliente")
                                .AddSelectionChanged(RefreshIdContacto),
                        ["IdContacto"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(RecuperarContactos())
                                .SetLabel("Contacto"),
                        ["IdTecnico"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                                .SetInnerValues(Tecnicos)
                                .SetLabel("Técnico")
                    },
                    IsUpdating = true
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
                return PersistenceManager<Contacto>.SelectByProperty("IdCliente", o.IdCliente).ToArray();

            return new Contacto[0];
        }

        public bool ValidarOferta()
        {
            return panelOferta.GetValidatedInnerValue<Oferta>() != default(Oferta);
        }
    }
}
