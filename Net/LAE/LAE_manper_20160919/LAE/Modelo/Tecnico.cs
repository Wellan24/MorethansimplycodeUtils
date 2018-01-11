using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
    public class FactoriaTecnicos
    {
        private static Tecnico[] tecnicos = null;
        public static Tecnico[] GetTecnicos()
        {
            if (tecnicos == null)
                tecnicos = PersistenceManager.SelectAll<Tecnico>().ToArray();
            return tecnicos;
        }

        public static Tecnico director()
        {
            return PersistenceManager.SelectByProperty<Tecnico>("Director", true).FirstOrDefault();
        }
    }

    [TableProperties("tecnicos")]
    public class Tecnico : PersistenceData, IModelo
    {
        [ColumnProperties("id_tecnico", IsId = true, IsAutonumeric = true)]
        public int Id { get; set; }

        [ColumnProperties("dni_tecnico")]
        public String Dni { get; set; }

        [ColumnProperties("nombre_tecnico")]
        public String Nombre { get; set; }

        [ColumnProperties("primerapellido_tecnico")]
        public String PrimerApellido { get; set; }

        [ColumnProperties("segundoapellido_tecnico")]
        public String SegundoApellido { get; set; }

        [ColumnProperties("director_tecnico")]
        public Boolean Director { get; set; }

        public override bool Equals(object obj)
        {
            Tecnico item = obj as Tecnico;
            if (item != null)
                return item.Id.Equals(Id);
            return false;
        }

        public override String ToString()
        {
            return Nombre + " " + PrimerApellido + " " + SegundoApellido;
        }
    }
}
