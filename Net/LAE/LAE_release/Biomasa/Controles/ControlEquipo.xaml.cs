using GenericForms.Abstract;
using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Biomasa.Modelo;
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
using LAE.Comun.Modelo.Procedimientos;

namespace LAE.Biomasa.Controles
{
    /// <summary>
    /// Lógica de interacción para ControlEquipo.xaml
    /// </summary>
    public partial class ControlEquipo : UserControl
    {

        public ControlEquipo()
        {
            InitializeComponent();
        }

        public void GenerarPanel(MedicionPNT medicion, int idParametro)
        {
            listaEquipos.Children.Clear();

            List<Equipo> equiposDefecto = FactoriaEquipoProcedimiento.GetEquipos(idParametro);
            EquipoMedicion[] equiposGuardados = FactoriaEquipoMedicion.GetEquipos(medicion);
            EquipoMedicion equipo;
            if (equiposGuardados.Count() > 0)
            {
                foreach (EquipoMedicion equipoMed in equiposGuardados)
                {
                    /*Añado equipo guardado si no esta en lista por defecto*/
                    Equipo equipoAdd = PersistenceManager.SelectByID<Equipo>(equipoMed.IdEquipo);
                    if (!equiposDefecto.Contains(equipoAdd))
                        equiposDefecto.Add(equipoAdd);
                    int idTipo = equipoAdd.IdTipo;

                    CrearPanelEquipo(equipoMed, equiposDefecto, idTipo);
                }
            }
            else
            {
                int[] idTiposEquipos = equiposDefecto.GroupBy(e => e.IdTipo).OrderBy(e => e.Key).Select(e => e.Key).ToArray();
                foreach (int idTipo in idTiposEquipos)
                {
                    equipo = new EquipoMedicion();
                    equipo.IdMedicion = medicion.Id;
                    equipo.IdEquipo = (equiposDefecto.Where(e => e.IdTipo == idTipo && e.Predefinido == true).FirstOrDefault() ?? equiposDefecto.Where(e => e.IdTipo == idTipo).FirstOrDefault()).Id;

                    CrearPanelEquipo(equipo, equiposDefecto, idTipo);
                }

            }
        }

        private void CrearPanelEquipo(EquipoMedicion equipo, List<Equipo> equiposDefecto, int idTipo)
        {
            TypePanel panelEquipo = new TypePanel();
            panelEquipo.Build(equipo,
                new TypePanelSettings<EquipoMedicion>
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

        public List<EquipoMedicion> GetEquipos()
        {
            return listaEquipos.Children.OfType<TypePanel>().Select(p => p.InnerValue as EquipoMedicion).ToList();
        }


    }
}
