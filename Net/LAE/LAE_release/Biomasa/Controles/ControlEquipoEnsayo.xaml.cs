using GenericForms.Abstract;
using GenericForms.Implemented;
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
    /// Lógica de interacción para ControlEquipoEnsayo.xaml
    /// </summary>
    public partial class ControlEquipoEnsayo : UserControl
    {
        public ControlEquipoEnsayo()
        {
            InitializeComponent();
        }

        public void GenerarPanel(EnsayoPNT ensayo)
        {
            listaEquipos.Children.Clear();

            List<Equipo> equiposDefecto = FactoriaEquipoProcedimiento.GetEquiposEnsayo(ensayo.IdEquipo);
            EquipoEnsayo[] equiposGuardados = FactoriaEquipoEnsayo.GetEquipos(ensayo);
            EquipoEnsayo equipo;
            if (equiposGuardados.Count() > 0)
            {
                foreach (EquipoEnsayo equipoEns in equiposGuardados)
                {
                    /*Añado equipo guardado si no esta en la lista por defecto*/
                    Equipo equipoAdd = PersistenceManager.SelectByID<Equipo>(equipoEns.IdEquipo);
                    if (!equiposDefecto.Contains(equipoAdd))
                        equiposDefecto.Add(equipoAdd);
                    int idTipo = equipoAdd.IdTipo;

                    CrearPanelEquipo(equipoEns, equiposDefecto, idTipo);
                }
            }
            else
            {
                int[] idTiposEquipo = equiposDefecto.GroupBy(e => e.IdTipo).OrderBy(e => e.Key).Select(e => e.Key).ToArray();
                foreach (int idTipo in idTiposEquipo)
                {
                    equipo = new EquipoEnsayo();
                    equipo.IdEnsayo = ensayo.Id;
                    equipo.IdEquipo = (equiposDefecto.Where(e => e.IdTipo == idTipo && e.Predefinido == true).FirstOrDefault() ??
                                        equiposDefecto.Where(e => e.IdTipo == idTipo).FirstOrDefault()
                                       ).Id;

                    CrearPanelEquipo(equipo, equiposDefecto, idTipo);
                }
            }
        }

        private void CrearPanelEquipo(EquipoEnsayo equipo, List<Equipo> equiposDefecto, int idTipo)
        {
            TypePanel panelEquipo = new TypePanel();
            panelEquipo.Build(equipo,
                new TypePanelSettings<EquipoEnsayo>
                {
                    Fields = new FieldSettings
                    {
                        ["IdEquipo"] = PropertyControlSettingsEnum.ComboBoxDefaultNoEmpty
                            .SetInnerValues(equiposDefecto.Where(e => e.IdTipo == idTipo).ToArray())
                            .SetLabel("* "+PersistenceManager.SelectByID<TipoEquipo>(idTipo).Nombre)
                            .SetDisplayMemberPath("Nombre")
                    }
                });
            listaEquipos.Children.Add(panelEquipo);
        }

        public List<EquipoEnsayo> GetEquipos()
        {
            return listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoEnsayo).ToList();
        }
    }
}
