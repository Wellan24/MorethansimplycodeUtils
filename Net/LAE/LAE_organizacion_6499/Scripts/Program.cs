using Cartif.EasyDatabase;
using LAE.AccesoDatos;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LAE.Modelo;
using Persistence;
using Cartif.Logs;
using NpgsqlTypes;

namespace Scripts
{
    class Program
    {
        static void Main(string[] args)
        {
            //CartifLogs.Configure();
            //prueba();Console.ReadKey();
            //Console.WriteLine(GenerarDictionaryModelo.GenerateDictionary("clientes"));
            prueba();
            Console.ReadKey();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Pruebas this Program. </summary>
        /// <remarks> Oscvic, 05/02/2016. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public static void prueba()
        {
            Console.WriteLine($"Nombre: {PersistentAttributesUtil.GetTableName<Cliente>().ClassName}");
            Console.WriteLine($"ID: {PersistentAttributesUtil.GetIdColumn<Cliente>().PropertyName}");

            Console.WriteLine();

            foreach (ColumnPropertiesInfo c in PersistentAttributesUtil.GetTableColumns<Cliente>(false))
            {
                Console.WriteLine($"\t {c.PropertyName} - {c.DbName} * {nameof(c.DbName)}");
            }

            //new Cliente { Id = 24 }.Delete();
            //new Cliente { Id = 21, Nombre = "Pepin2" }.Insert(false);
            new Cliente { Id = 15, Nombre = "Pepin15" }.Update();
            new Cliente { Id = 16, Nombre = "Pepin19" }.Update(toUpdateId: 19);
        }

        public static void prueba2()
        {
            try
            {
                using (NpgsqlConnection conn = PersistenceDataBase.GetConnection())
                {
                    var c = conn.Query<Cliente>("Select * from clientes where nombre_cliente = @Nombre", new Cliente() { Nombre = "El Nombre" });
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

}
