using Cartif.Extensions;
using Dapper;
using LAE.Comun.Persistence;
using LAE.Comun.Modelo;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAE.Comun.Persistence
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary> A dato lae manipulation </summary>
    /// <remarks> Manper, 2016-06-01. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class PersistenceDataManipulation
    {
        #region Save

        /// <summary> Save a list of element reference a one table</summary>
        /// <remarks> Manper 27/05/2016</remarks>
        /// <typeparam name="T">Generic type Tabla Referenciada </typeparam>
        /// <typeparam name="TLista">Generic type Tabla Hija (tabla a guardar)</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="elemento">Referenced element</param>
        /// <param name="elementosActualizar">Elements to save</param>
        /// <param name="selectorValorPropiedad">Function to get value property of foreign key</param>
        /// <param name="nombrePropiedad">Name property of foreign key</param>
        public static void GuardarElement1N<T, TLista>(NpgsqlConnection conn, T elemento, IEnumerable<TLista> elementosActualizar,
            Func<T, Object> selectorValorPropiedad, String nombrePropiedad, String sql=null)
            where T : PersistenceData, IModelo
            where TLista : PersistenceData, IModelo
        {
            Object valorPropiedad = selectorValorPropiedad(elemento);
            Guardar(conn, elementosActualizar.ToList(), valorPropiedad, nombrePropiedad, sql);
        }

        /// <summary> Save mulitple list of elements referenced a multiple tables </summary>
        /// <remarks> Manper 27/05/2016</remarks>
        /// <typeparam name="T">Generic type Tablas Referenciadas</typeparam>
        /// <typeparam name="TLista">Generic type Tablas Hijas (tablas a guardar)</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="listaElementos">List of referenced elements</param>
        /// <param name="selectorElementosActualizar">Function to select elements to save</param>
        /// <param name="selectorValorPropiedad">Function to get values property of foreign keys</param>
        /// <param name="nombrePropiedad">Name property of foreign keys</param>
        public static void GuardarListadoNN<T, TLista>(NpgsqlConnection conn, IEnumerable<T> listaElementos, Func<T, IEnumerable<TLista>> selectorElementosActualizar,
            Func<T, Object> selectorValorPropiedad, String nombrePropiedad)
            where T : PersistenceData
            where TLista : PersistenceData, IModelo
        {
            listaElementos.ForEach(elemento =>
            {
                Object valorPropiedad = selectorValorPropiedad(elemento);
                Guardar(conn, selectorElementosActualizar(elemento).ToList(), valorPropiedad, nombrePropiedad);
            });
        }

        /// <summary> Save element referenced a multiple tables</summary>
        /// <typeparam name="T">Generic type Tablas Referenciadas</typeparam>
        /// <typeparam name="TElement">Generic type Tabla Hija (tabla a guardar)</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="listaElementos">List of referenced elements</param>
        /// <param name="selectorElementoActualizar">Function to select element to save</param>
        /// <param name="selectorValorPropiedad">Function to get value property of foreign keys</param>
        /// <param name="nombrePropiedad">Name property of foreign keys</param>
        public static void GuardarListadoN1<T, TElement>(NpgsqlConnection conn, IEnumerable<T> listaElementos, Func<T, TElement> selectorElementoActualizar,
            Func<T, Object> selectorValorPropiedad, String nombrePropiedad)
            where T : PersistenceData
            where TElement : PersistenceData, IModelo
        {
            listaElementos.ForEach(elemento =>
            {
                Object valorPropiedad = selectorValorPropiedad(elemento);
                Guardar(conn, selectorElementoActualizar(elemento), valorPropiedad, nombrePropiedad);
            });
        }

        /// <summary> Save a list of elements </summary>
        /// <remarks> Manper 27/05/2016</remarks>
        /// <typeparam name="T">Generit type</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="lista">List objects to save</param>
        /// <param name="valorPropiedad">Value property of foreign key</param>
        /// <param name="nombrePropiedad">Name property of foreign key</param>
        public static void Guardar<T>(NpgsqlConnection conn, List<T> lista, Object valorPropiedad = null, String nombrePropiedad = null, String sql=null) where T : PersistenceData, IModelo
        {
            foreach (T item in lista)
            {
                Guardar(conn, item, valorPropiedad, nombrePropiedad, sql);
            }
        }

        /// <summary> Save a object </summary>
        /// <remarks> Manper 27/05/2016</remarks>
        /// <typeparam name="T">Generit type</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="obj">Object to save</param>
        /// <param name="valorPropiedad">Value property of foreign key</param>
        /// <param name="nombrePropiedad">Name property of foreign key</param>
        /// <returns></returns>
        public static int Guardar<T>(NpgsqlConnection conn, T obj, Object valorPropiedad = null, String nombrePropiedad = null, String sql=null) where T : PersistenceData, IModelo
        {
            if (obj.Id == 0)
            {
                if (valorPropiedad != null)
                    typeof(T).GetProperty(nombrePropiedad).SetValue(obj, valorPropiedad);
                int id;
                if (sql == null)
                    id = obj.Insert(conn);
                else
                    id = conn.Query<int>(sql, obj).FirstOrDefault();
                if (id == 0)
                    throw new PersistenceDataException("Error al guardar " + typeof(T).Name);
                typeof(T).GetProperty("Id").SetValue(obj, id);
            }
            else
            {
                if (!obj.Update(conn))
                    throw new PersistenceDataException("Error al actualizar " + typeof(T).Name);
            }
            return obj.Id;
        }

        #endregion

        #region Delete

        /// <summary> Delete mulitple list of elements referenced a multiple tables by comparison between the database and the list of param selectorElementosNoBorrar</summary>
        /// <typeparam name="T">Generic type Tablas Referenciadas</typeparam>
        /// <typeparam name="TLista">Generic type Tablas Hijas (tablas a borrar)</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="listaElementos">List of Tuple composed of referenced element and check to delete</param>
        /// <param name="selectorElementosNoBorrar">Function to select existing elements (that haven't delete)</param>
        /// <param name="selectorValorPropiedad">Function to get values property of foreign keys</param>
        /// <param name="nombrePropiedad">Name property of foreign keys</param>
        public static void BorrarListadoNN<T, TLista>(NpgsqlConnection conn, IEnumerable<Tuple<T, Boolean>> listaElementos, Func<T, IEnumerable<TLista>> selectorElementosNoBorrar,
            Func<T, Object> selectorValorPropiedad, String nombrePropiedad)
            where T : PersistenceData
            where TLista : PersistenceData, IModelo
        {
            listaElementos.ForEach(tuple =>
            {
                T elemento = tuple.Item1;
                Boolean borrarTodo = tuple.Item2;
                Object valorPropiedad = selectorValorPropiedad(elemento);
                Borrar1N(conn, selectorElementosNoBorrar(elemento).ToList(), valorPropiedad, nombrePropiedad, borrarTodo);
            });
        }
        
        /// <summary> Delete mulitple list of elements referenced a multiple tables by comparison between the database and the list of param selectorElementosNoBorrar</summary>
         /// <typeparam name="T">Generic type Tablas Referenciadas</typeparam>
         /// <typeparam name="TLista">Generic type Tablas Hijas (tablas a borrar)</typeparam>
         /// <param name="conn">Connection</param>
         /// <param name="listaElementos">List of Tuple composed of referenced element and check to delete</param>
         /// <param name="selectorElementosNoBorrar">Function to select existing elements (that haven't delete)</param>
         /// <param name="selectorValorPropiedad">Function to get values property of foreign keys</param>
         /// <param name="nombrePropiedad">Name property of foreign keys</param>
        public static void BorrarListadoNN<T, TLista>(NpgsqlConnection conn, IEnumerable<T> listaElementos, Func<T, IEnumerable<TLista>> selectorElementosNoBorrar,
            Func<T, Object> selectorValorPropiedad, String nombrePropiedad)
            where T : PersistenceData
            where TLista : PersistenceData, IModelo
        {
            listaElementos.ForEach(t =>
            {
                T elemento = t;
                Object valorPropiedad = selectorValorPropiedad(elemento);
                Borrar1N(conn, selectorElementosNoBorrar(elemento).ToList(), valorPropiedad, nombrePropiedad);
            });
        }

        /// <summary> Delete list of elements </summary>
        /// <typeparam name="T">Generic type element to delete</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="listaGuardada">list of existing elements (that haven't delete) </param>
        /// <param name="idFK">value property of foreign key</param>
        /// <param name="nombreIdFK">Name property of foreign key</param>
        /// <param name="borrarTodo">boolean to indicate that delete all elements (without check list of existing elements)</param>
        public static void Borrar1N<T>(NpgsqlConnection conn, List<T> listaGuardada, Object idFK, String nombreIdFK, Boolean borrarTodo=false)
            where T : PersistenceData, IModelo
        {
            List<T> listaBBDD = PersistenceManager.SelectByProperty<T>(nombreIdFK, idFK).ToList();
            foreach (T item in listaBBDD)
            {
                if ((borrarTodo || listaGuardada == null || listaGuardada.Where(l => l.Id == item.Id).Count() == 0) && !item.Delete(conn))
                    throw new PersistenceDataException("Error al borrar " + typeof(T).Name);
            }
        }

        /// <summary>Delete list of elements (without check with the database)</summary>
        /// <typeparam name="T">Generic type element to delete</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="listaBorrar">List of elements to delete</param>
        public static void Borrar<T>(NpgsqlConnection conn, List<T> listaBorrar) where T : PersistenceData
        {
            foreach (T item in listaBorrar)
            {
                if (!item.Delete(conn))
                    throw new PersistenceDataException("Error al borrar " + typeof(T).Name);
            }
        }


        /// <summary>Delete mulitple list of elements referenced multiple tables searching the database </summary>
        /// <typeparam name="T">Generic type Tablas Referenciadas</typeparam>
        /// <typeparam name="TLista">Generic type Tablas Hijas (tablas a borrar)</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="listaElementos">List of referenced elements to delete</param>
        /// <param name="selectorValorPropiedad">Function to get values property of foreign keys</param>
        /// <param name="nombrePropiedad">Name property of foreign keys</param>
        public static void BorrarListadoNN<T, TLista>(NpgsqlConnection conn, IEnumerable<T> listaElementos, Func<T, Object> selectorValorPropiedad, String nombrePropiedad)
            where T : PersistenceData
            where TLista : PersistenceData, IModelo
        {
            listaElementos.ForEach(elem =>
            {
                Object valorPropiedad = selectorValorPropiedad(elem);
                Borrar1N<TLista>(conn, null, valorPropiedad, nombrePropiedad, true);
            });
        }

        /// <summary>Delete mulitple list of elements referenced a table searching the database </summary>
        /// <typeparam name="T">Generic type Tabla Referenciada</typeparam>
        /// <typeparam name="TLista">Generic type Tablas Hijas (tablas a borrar)</typeparam>
        /// <param name="conn">Connection</param>
        /// <param name="element">Referenced element to delete</param>
        /// <param name="selectorValorPropiedad">Function to get value property of foreign key</param>
        /// <param name="nombrePropiedad">Name property of foreign key</param>
        public static void BorrarListado1N<T, TLista>(NpgsqlConnection conn, T element, Func<T, Object> selectorValorPropiedad, String nombrePropiedad)
            where T : PersistenceData
            where TLista : PersistenceData, IModelo
        {
            Object valorPropiedad = selectorValorPropiedad(element);
            Borrar1N<TLista>(conn, null, valorPropiedad, nombrePropiedad, true);
        }

        #endregion

        #region LoadData

        /// <summary>Fill a list search data in database</summary>
        /// <typeparam name="T">Generic type to search in database</typeparam>
        /// <param name="propiedad">Name property of foreign key</param>
        /// <param name="valor">Value property of foreign key</param>
        /// <param name="lista">List to fill</param>
        public static void LoadData<T>(String propiedad, int valor, ObservableCollection<T> lista) where T : PersistenceData
        {
            if (lista != null)
            {
                var listaAdd = PersistenceManager.SelectByProperty<T>(propiedad, valor);
                listaAdd.ForEach(l => lista.Add(l));
            }
        }

        /// <summary>Set elment search the data in database </summary>
        /// <typeparam name="T">Generic type to search in database</typeparam>
        /// <param name="propiedad">Name property of foreign key</param>
        /// <param name="valor">Value property of foreign key</param>
        /// <param name="obj">Element to set value</param>
        public static void LoadData<T>(String propiedad, int valor, ref T obj) where T : PersistenceData
        {
            var valorAdd = PersistenceManager.SelectByProperty<T>(propiedad, valor).FirstOrDefault();
            if (valorAdd != null)
                obj = valorAdd;
        }
        #endregion
    }
}
