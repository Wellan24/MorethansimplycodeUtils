using Cartif.EasyDatabase;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartif.Extensions;
using LAE.Comun.Persistence;

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

        public static Columna[] GetColumnasMejorado(String nombreTabla, String nombreSchema = null)
        {
            using (EasyConnection c = PersistenceDataBase.GetEasyConnection())
            {
                List<Columna> s = new List<Columna>(10);
                c.ExecuteEasyQuery((rdr) => s.Add(new Columna() { NombreColumnaBBDD = rdr.GetString(0), Tipo = rdr.GetString(1), IsNullable = rdr.GetString(2) }),
                    @"SELECT c.column_name, c.data_type, c.is_nullable 
                             FROM information_schema.columns c
                             WHERE UPPER(c.table_name) = upper('" + nombreTabla + @"')" +
                             ((nombreSchema != null && nombreSchema.Length > 0) ? "AND UPPER(C.table_schema) = UPPER('" + nombreSchema + "')" : " ")
                             + @"ORDER BY c.ordinal_position");

                return s.ToArray();
            }
        }

        public static String GenerateDictionaryMejorado(String nombreTabla, String nombreSchema = null)
        {

            Columna[] columns = GetColumnasMejorado(nombreTabla, nombreSchema);
            String dictionary = @"using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Modelo
{
                public class Factoria" + ReplaceUnderScoreAndCapitalize(nombreTabla) + @"
                {
                    // TODO Rellenar esto con Selects necesarias.
                }
";
            dictionary += @"
                [TableProperties(""" + ((nombreSchema != null && nombreSchema.Length > 0) ? nombreSchema + @"." : @"") + nombreTabla + @""")]
                public class " + ReplaceUnderScoreAndCapitalize(nombreTabla[nombreTabla.Length - 1] == 's' ? nombreTabla.Remove(nombreTabla.Length - 1) : nombreTabla) + @" : PersistenceData
                {";
            foreach (Columna column in columns)
            {
                String PropertyName = GetPropertyName(column.NombreColumnaBBDD);

                dictionary += @"
                    [ColumnProperties(""" + column.NombreColumnaBBDD + @"""" + (PropertyName.Equals("Id") ? ", IsId = true, IsAutonumeric = true" : "") + @")]
                    public " + Tipo(column) + @" " + PropertyName + @"{ get; set; }
";
            }
            dictionary += "                }\r\n}";
            return dictionary;
        }

        public static String ReplaceUnderScoreAndCapitalize(String text)
        {
            StringBuilder sb = new StringBuilder(text.Capitalize());
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '_')
                {
                    sb.Remove(i, 1);
                    if (sb.Length > i)
                        sb[i] = Char.ToUpper(sb[i]);
                    i--;
                }
            }
            return sb.ToString();
        }

        public static String GetPropertyName(String text)
        {
            return text.Split('_')[0].Capitalize();
        }

        public static String Tipo(Columna columna)
        {
            String tipo;
            String nullable = columna.IsNullable.Equals("YES") ? "?" : "";
            switch (columna.Tipo)
            {
                case "integer":
                    tipo = "int" + nullable;
                    break;
                case "character varying":
                case "text":
                    tipo = "String";
                    break;
                case "boolean":
                    tipo = "Boolean" + nullable;
                    break;
                case "timestamp without time zone":
                    tipo = "DateTime" + nullable;
                    break;
                case "numeric":
                    tipo = "decimal" + nullable;
                    break;
                default:
                    tipo = "XXX" + nullable;
                    break;
            }



            return tipo;
        }

        public class Columna
        {
            public String NombreColumnaBBDD { get; set; }
            public String Tipo { get; set; }
            public String IsNullable { get; set; }
        }
    }
}
