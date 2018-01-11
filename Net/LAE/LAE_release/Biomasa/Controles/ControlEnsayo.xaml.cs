using GenericForms.Abstract;
using GenericForms.Settings;
using LAE.Comun.Modelo.Procedimientos;
using LAE.Comun.Persistence;
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

namespace LAE.Biomasa.Controles
{
    /// <summary>
    /// Lógica de interacción para ControlEnsayo.xaml
    /// </summary>
    public partial class ControlEnsayo : UserControl
    {
        private EnsayoPNT ensayo;

        public EnsayoPNT Ensayo
        {
            get { return ensayo; }
            set
            {
                ensayo = value;
                GenerarPanelEnsayo();
            }
        }

        public ControlEnsayo()
        {
            InitializeComponent();
        }

        private void GenerarPanelEnsayo()
        {
            int idTipoEquipo = PersistenceManager.SelectByID<Equipo>(Ensayo.IdEquipo, null, "IdTipo").IdTipo;
            panelEnsayo.Build(Ensayo,
                new TypePanelSettings<EnsayoPNT>
                {
                    Fields = new FieldSettings
                    {
                        ["FechaInicio"] = PropertyControlSettingsEnum.DateTimeDefaultNoEmpty
                            .SetLabel("Fecha ensayo"),
                        ["IdEquipo"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(FactoriaEquipos.GetEquipoByTipo(idTipoEquipo))
                            .SetLabel("*Equipo ensayo")
                            .SetDisplayMemberPath("Nombre")
                    },
                    IsUpdating = true
                });
        }
    }
}
