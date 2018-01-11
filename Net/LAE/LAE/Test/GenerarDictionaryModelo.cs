using Cartif.EasyDatabase;
using LAE.AccesoDatos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;
using Persistence;

namespace Scripts
{
    public class GenerarDictionaryModelo
    {
        public static String[] GetColumnas(String nombre)
        {
            using (EasyConnection c = PersistenceDataBase.GetEasyConnection())
            {
                List<String> s = new List<string>(10);
                c.ExecuteEasyQuery((rdr) => s.Add(rdr.GetString(0)),
                    @"SELECT c.column_name
                             FROM information_schema.columns c
                             WHERE UPPER(c.table_name) = upper('" + nombre + @"')
                             ORDER BY c.ordinal_position");

                return s.ToArray();
            }
        }

        public static String GenerateDictionary(String nombre)
        {
            String[] columns = GetColumnas(nombre);
            String dictionary = @"
    public class Factoria" + nombre.Capitalize() + @"
    {
        // TODO Rellenar esto con Selects necesarias.
    } 
";
            dictionary += @"
    [TableProperties(""" + nombre + @""")]
    public class " + (nombre[nombre.Length - 1] == 's' ? nombre.Remove(nombre.Length - 1) : nombre).Capitalize() + @" : PersistenceData
    {";
            foreach (String column in columns)
            {
                dictionary += @"
        [ColumnProperties(""" + column + @""")]
        public String """ + column + @"_remove"" { get; set; }
";
            }
            dictionary += "    }\r\n}";
            return dictionary;
        }
    }
}
