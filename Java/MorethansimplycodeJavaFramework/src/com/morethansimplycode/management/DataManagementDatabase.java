/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.management;

import com.morethansimplycode.data.Data;
import java.sql.Connection;
import java.sql.ResultSet;

/**
 *
 * @author Oscar
 */
public interface DataManagementDatabase {
    
    /**
     * Executes a query without result.
     *
     * @param connection The connection to use
     * @param query The query to execute
     * @return The number of rows changed
     */
    public int executeNonQuery(Connection connection, String query);

    /**
     * Executes a query that expects a result
     * 
     * @param connection The connection to use
     * @param query The query to execute
     * @return Devuelve un ResultSet con los datos de la consulta o null si hay
     * una excepción
     */
    public ResultSet executeQuery(Connection connection, String query);

    /**
     * Insert a Data object to the DataBase. This uses the DataDBInfo interface,
     * or the name of the class and all its keys.
     *
     * @param connection
     * @param d
     * @return True if the insert is successfull or false otherwise.
     */
    public boolean insertData(Connection connection, Data d, boolean autoNum);

    /**
     * This method check if the Data exists in the data base, comparing it with
     * the PrimaryKeys
     *
     * @param connection
     * @param d
     * @return True if it exists, false if not
     */
    public boolean existsByPrimaryKey(Connection connection, Data d);

    /**
     * This method check if the Data exists in the data base, comparing it using
     * all the keys.
     *
     * @param connection
     * @param d
     * @return True if the Data exists, false if not
     */
    public boolean existsByAllColumns(Connection connection, Data d);

    /**
     * This method check if the Data exists in the data base, comparing it using
     * the given keys.
     *
     * @param connection
     * @param d
     * @param columns
     * @return True if the Data exists, false if not
     */
    public boolean existsByColumns(Connection connection, String[] columns, Data d);

    /**
     * Creates a Select Query with this format: "Select ${selectColumns} from
     * ${table} where whereColumns[i] = ${columnValue} [, ...]
     *
     * @param selectColumns
     * @param whereColumns
     * @param d
     * @return A String builder with the text of the query.
     */
    public StringBuilder createSelectQueryByColumns(Class<? extends Data> d, String selectColumns, String[] whereColumns, Object... valuesWhereColumns);

    /**
     * Creates a Select Query with this format: "Select String.join(",",
     * ${selectColumns}) from ${table} where whereColumns[i] = ${columnValue} [,
     * ...]
     *
     * @param selectColumns
     * @param whereColumns
     * @param d
     * @return A String builder with the text of the query.
     */
    public StringBuilder createSelectQueryByColumns(Class<? extends Data> d, String[] selectColumns, String[] whereColumns, Object... valuesWhereColumns);

    /**
     * Creates a Select Query with this format: "Select ${columns} from ${table}
     * where primaryKey[i] = ${columnValue} [, ...]
     *
     * @param columns
     * @param d
     * @return A String builder with the text of the query.
     */
    public StringBuilder createSelectQueryByPrimaryKey(Class<? extends Data> d, String columns, Object... primaryKeyValues);

    /**
     * Creates a Select Query.
     *
     * @param columns
     * @param d The class of the objects to select
     * @return A String builder with the text of the query.
     */
    public StringBuilder createSelectQueryByPrimaryKey(Class<? extends Data> d, String[] columns, Object... primaryKeyValues);

    /**
     * Creates a Insert Query.
     *
     * @param d The data to insert
     * @return An StringBuilder with the text of the Query
     */
    public StringBuilder createInsertQuery(Data d);

    /**
     * Creates a Insert Query.
     *
     * @param d
     * @param claves
     * @return An StringBuilder with the text of the Query
     */
    public StringBuilder createInsertQuery(Data d, String[] claves);

    /**
     * Creates a Insert Query with autonumeric key defined in DataDBInfo
     * annotation
     *
     * @param d
     * @param claves
     * @param auto
     * @return An StringBuilder with the text of the Query
     */
    public StringBuilder createInsertQuery(Data d, String[] claves, boolean auto);

    public boolean updateDato(Data d, Connection connection);

    public StringBuilder createUpdateQuery(Data d);
    
    public StringBuilder createSelectQuery(String[] claves, String nombreTabla);
    
    public StringBuilder createSelectQuery(Class<? extends Data> d);
    
    public StringBuilder createSelectQuery(Class<? extends Data> d, String where);

    public StringBuilder createSelectQuery(String[] claves, String nombreTabla, String where);

    public DataManagementDatabase top(int recordsToRecover);
}
