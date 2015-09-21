/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.management;

import com.morethansimplycode.data.Data;
import com.morethansimplycode.data.DataAnnotationUtil;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.time.LocalDate;
import java.util.Arrays;
import java.util.List;
import java.util.Set;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * The purpose of this class is to give a bunch of general methods to Data
 * persistence
 *
 * @author Oscar
 */
public class DataManagementDatabaseOracle implements DataManagementDatabase {

    public int top = -1;
    private static DataManagementDatabaseOracle instance;
    private Statement statement;

    /**
     * Gets the instance for this DataManagementDatabase
     *
     * @return The instance
     */
    public static DataManagementDatabase getInstance() {

        if (instance == null) {
            instance = new DataManagementDatabaseOracle();
        }

        return instance;
    }

    /**
     * Executes a query without result.
     *
     * @param connection The connection to use
     * @param query The query to execute.
     * @return Returns -1 if the query fails.
     */
    @Override
    public int executeNonQuery(Connection connection, String query) {

        try (Statement localStatement = connection.createStatement()) {

            return localStatement.executeUpdate(query);

        } catch (SQLException ex) {
            Logger.getLogger(DataManagementDatabaseMysql.class.getName()).log(Level.SEVERE, null, ex);
        }

        return -1;
    }

    /**
     * Executes a query with the given connection.
     *
     * @param connection The connection to use
     * @param query The query to execute
     * @return Devuelve un ResultSet con los datos de la consulta o null si hay
     * una excepción
     */
    @Override
    public synchronized ResultSet executeQuery(Connection connection, String query) {

        try {
            statement = connection.createStatement();
            //statement.closeOnCompletion();
            System.out.println(query);
            return statement.executeQuery(query);

        } catch (SQLException ex) {
            Logger.getLogger(DataManagementDatabaseMysql.class.getName()).log(Level.SEVERE, null, ex);
        }

        return null;
    }

    /**
     * Insert a Data object to the DataBase. This uses the DataDBInfo interface,
     * or the name of the class and all its keys.
     *
     * @param connection
     * @param d
     * @param autoNum
     * @return True if the insert is successfull or false otherwise.
     */
    @Override
    public boolean insertData(Connection connection, Data d, boolean autoNum) {

        StringBuilder text = createInsertQuery(d, DataAnnotationUtil.recoverDBInfoColumns(d.getClass()));
        return executeNonQuery(connection, text.toString()) != -1;
    }

    /**
     * This method check if the Data exists in the data base, comparing it with
     * the PrimaryKeys
     *
     * @param connection The connection to use
     * @param d
     * @return True if it exists, false if not
     */
    @Override
    public boolean existsByPrimaryKey(Connection connection, Data d) {

        try (Statement localStatement = connection.createStatement()) {

            String[] primaryKeys = DataAnnotationUtil.recoverDBInfoPrimaryKeys(d.getClass());

            Object[] values = new Object[primaryKeys.length];
            for (int i = 0; i < values.length; i++) {
                values[i] = d.get(primaryKeys[i]);
            }

            ResultSet rs = localStatement.executeQuery(createSelectQueryByPrimaryKey(d.getClass(), "1", values).toString());
            return rs.next();

        } catch (SQLException ex) {
            Logger.getLogger(DataManagementDatabaseMysql.class.getName()).log(Level.SEVERE, null, ex);
        }
        return false;
    }

    /**
     * This method check if the Data exists in the data base, comparing it using
     * all the keys.
     *
     * @param connection The connection to use
     * @param d
     * @return True if the Data exists, false if not
     */
    @Override
    public boolean existsByAllColumns(Connection connection, Data d) {

        Set<String> set = d.keySet();
        return existsByColumns(connection, set.toArray(new String[set.size()]), d);
    }

    /**
     * This method check if the Data exists in the data base, comparing it using
     * the given keys.
     *
     * @param connection The connection to use
     * @param d
     * @param columns
     * @return True if the Data exists, false if not
     */
    @Override
    public boolean existsByColumns(Connection connection, String[] columns, Data d) {

        try (Statement localStatement = connection.createStatement()) {

            Object[] values = new Object[columns.length];

            for (int i = 0; i < values.length; i++) {
                values[i] = d.get(columns[i]);
            }

            ResultSet rs = localStatement.executeQuery(createSelectQueryByColumns(d.getClass(), "1", columns, values).toString());
            return rs.next();

        } catch (SQLException ex) {
            Logger.getLogger(DataManagementDatabaseMysql.class.getName()).log(Level.SEVERE, null, ex);
        }
        return false;
    }

    /**
     * Creates a Select Query with this format: "Select ${selectColumns} from
     * ${table}
     *
     * @param keys The columns to use in the Select
     * @param tableName The name of the table to use in the Select
     * @return A String builder with the text of the query.
     */
    @Override
    public StringBuilder createSelectQuery(String[] keys, String tableName) {

        return createSelectQuery(keys, tableName, null);
    }

    /**
     * Creates a Select Query with this format: "Select ${selectColumns} from
     * ${table}
     *
     * @param keys The columns to use in the Select
     * @param tableName The name of the table to use in the Select
     * @param where The where to use in the Select
     * @return A String builder with the text of the query.
     */
    @Override
    public StringBuilder createSelectQuery(String[] keys, String tableName, String where) {

        StringBuilder text = new StringBuilder("Select ");

        for (String clave : keys) {
            text.append(clave).append(",");
        }

        text.replace(text.length() - 1, text.length(), " from ");
        text.append(tableName).append(" ");

        if (where != null && !where.isEmpty()) {
            text.append(" where ").append(where);
        }

        addTop(text);
        return text;
    }

    /**
     * Creates a Select Query with this format: "Select ${selectColumns} from
     * ${table} where whereColumns[i] = ${columnValue} [, ...]
     *
     * @param d The class to use
     * @param selectColumns The columns to Select
     * @param whereColumns The Columns in the where
     * @param valuesWhereColumns The values of the Columns in the where
     * @return A String builder with the text of the query.
     */
    @Override
    public StringBuilder createSelectQueryByColumns(Class<? extends Data> d, String selectColumns, String[] whereColumns, Object... valuesWhereColumns) {

        StringBuilder text = new StringBuilder("Select ");
        text.append(selectColumns).append(" from ");
        text.append(DataAnnotationUtil.recoverDBInfoTableName(d)).append(" where ");

        for (int i = 0; i < whereColumns.length; i++) {

            String primaryKey = whereColumns[i];
            Object value = valuesWhereColumns[i];

            if (value instanceof String) {
                text.append(primaryKey).append(" = '").append(value).append("'");
            } else {
                text.append(primaryKey).append(" = ").append(value);
            }

            if (i != whereColumns.length - 1) {
                text.append(",");
            }
        }

        addTop(text);

        return text;
    }

    /**
     * Creates a Select Query with this format: "Select String.join(",",
     * ${selectColumns}) from ${table} where whereColumns[i] = ${columnValue} [,
     * ...]
     *
     * @param d The class to use
     * @param selectColumns The columns to Select
     * @param whereColumns The Columns in the where
     * @param valuesWhereColumns The values of the Columns in the where
     * @return A String builder with the text of the query.
     */
    @Override
    public StringBuilder createSelectQueryByColumns(Class<? extends Data> d, String[] selectColumns, String[] whereColumns, Object... valuesWhereColumns) {

        return createSelectQueryByColumns(d, String.join(",", selectColumns), whereColumns, valuesWhereColumns);
    }

    /**
     * Creates a Select Query with this format: "Select ${columns} from ${table}
     * where primaryKey[i] = ${columnValue} [, ...]
     *
     * @param d The class to use
     * @param columns The columns to Select
     * @param primaryKeyValues The values of the primary key/keys
     * @return A String builder with the text of the query.
     */
    @Override
    public StringBuilder createSelectQueryByPrimaryKey(Class<? extends Data> d, String columns, Object... primaryKeyValues) {

        String[] primaryKeys = DataAnnotationUtil.recoverDBInfoPrimaryKeys(d);

        StringBuilder text = new StringBuilder("Select ");
        text.append(columns).append(" from ");
        text.append(DataAnnotationUtil.recoverDBInfoTableName(d)).append(" where ");

        for (int i = 0; i < primaryKeyValues.length; i++) {

            String primaryKey = primaryKeys[i];
            Object value = primaryKeyValues[i];

            if (value instanceof String) {
                text.append(primaryKey).append(" = '").append(value).append("'");
            } else {
                text.append(primaryKey).append(" = ").append(value);
            }

            if (i != primaryKeys.length - 1) {
                text.append(" and ");
            }
        }

        addTop(text);
        return text;
    }

    /**
     * Creates a Select Query with this format: "Select
     * String.join(",",${columns}) from ${table} where primaryKey[i] =
     * ${columnValue} [, ...]
     *
     * @param d The class of the data to select
     * @param columns The columns you want to select
     * @param primaryKeyValues The values of the primary key/keys
     * @return A String builder with the text of the query.
     */
    @Override
    public StringBuilder createSelectQueryByPrimaryKey(Class<? extends Data> d, String[] columns, Object... primaryKeyValues) {

        return createSelectQueryByPrimaryKey(d, String.join(",", columns));
    }

    /**
     * Creates a Insert Query.
     *
     * @param d The data to insert
     * @return An StringBuilder with the text of the Query
     */
    @Override
    public StringBuilder createInsertQuery(Data d) {

        return createInsertQuery(d, null, false);
    }

    /**
     * Creates a Insert Query.
     *
     * @param d The class of the data to select
     * @param keys The keys to use in the insert
     * @return An StringBuilder with the text of the Query
     */
    @Override
    public StringBuilder createInsertQuery(Data d, String[] keys) {

        return createInsertQuery(d, keys, false);
    }

    /**
     * Creates a Insert Query.
     *
     * @param d The class of the data to select
     * @param keys The keys to use in the insert
     * @return An StringBuilder with the text of the Query
     */
    public StringBuilder createAutoNumericInsertQuery(Data d, String[] keys) {

        return createInsertQuery(d, keys, true);
    }

    /**
     * Creates a Insert Query with autonumeric key defined in DataDBInfo
     * annotation
     *
     * @param d The class of the data to select
     * @param keys The keys to use in the insert
     * @param auto If it have autonumeric values
     * @return An StringBuilder with the text of the Query
     */
    @Override
    public StringBuilder createInsertQuery(Data d, String[] keys, boolean auto) {

        StringBuilder textoSentencia = new StringBuilder("insert into ");
        textoSentencia.append(DataAnnotationUtil.recoverDBInfoTableName(d.getClass()));
        textoSentencia.append("(");
        List<String> autoNumKeys = Arrays.asList(DataAnnotationUtil.recoverDBInfoAutoNumKeys(d.getClass()));

        if (keys == null) {
            keys = DataAnnotationUtil.recoverDBInfoColumns(d.getClass());
        }

        for (String clave : keys) {

            if (!autoNumKeys.contains(clave)) {
                textoSentencia.append(clave).append(",");
            }
        }
        textoSentencia.replace(textoSentencia.length() - 1, textoSentencia.length(), ")");
        textoSentencia.append(" VALUES(");
        for (String clave : keys) {
            if (!autoNumKeys.contains(clave)) {
                Object rec = d.get(clave);
                if (rec instanceof String || rec instanceof LocalDate) {

                    textoSentencia.append("'");
                    textoSentencia.append(rec.toString());
                    textoSentencia.append("'");
                } else if (rec instanceof Integer) {

                    textoSentencia.append(rec);
                } else if (rec instanceof Float) {

                    textoSentencia.append(rec);
                } else {

                    textoSentencia.append(rec);
                }
                textoSentencia.append(" ,");
            }
        }
        textoSentencia.replace(textoSentencia.length() - 2, textoSentencia.length(), ")");
        System.out.println(textoSentencia.toString());
        return textoSentencia;
    }

    /**
     * Updates a Data object
     *
     * @param d The Data to update
     * @param connection The connection to use
     * @return
     */
    @Override
    public boolean updateDato(Data d, Connection connection) {

        if (existsByPrimaryKey(connection, d)) {

            return executeNonQuery(connection, createUpdateQuery(d).toString()) == 1;
        }

        return false;
    }

    /**
     * Creates the update Query to this Data object
     *
     * @param d The Data to update
     * @return
     */
    @Override
    public StringBuilder createUpdateQuery(Data d) {

        String[] claves = DataAnnotationUtil.recoverDBInfoColumns(d.getClass());
        StringBuilder textoSentencia = new StringBuilder("update ");
        textoSentencia.append(DataAnnotationUtil.recoverDBInfoTableName(d.getClass()));
        textoSentencia.append(" set ");
        for (int i = 1; i < claves.length; i++) {

            Object rec = d.get(claves[i]);
            textoSentencia.append(claves[i]).append("=");

            if (rec instanceof String || rec instanceof LocalDate) {

                textoSentencia.append("'");
                textoSentencia.append(rec.toString());
                textoSentencia.append("'");
            } else if (rec instanceof Integer) {

                textoSentencia.append(rec);
            } else if (rec instanceof Float) {

                textoSentencia.append(rec);
            } else {

                textoSentencia.append(rec);
            }
            textoSentencia.append(" ,");
        }

        textoSentencia.replace(textoSentencia.length() - 2, textoSentencia.length(), " where ");
        textoSentencia.append(claves[0]).append(" = ").append(d.get(claves[0]));
        System.out.println(textoSentencia);
        return textoSentencia;
    }

    /**
     * Creates a Select Query with this format: "Select ${selectColumns} from
     * ${table}
     *
     * @param d The Data class to Select
     * @return A String builder with the text of the query.
     */
    @Override
    public StringBuilder createSelectQuery(Class<? extends Data> d) {

        String[] keys = DataAnnotationUtil.recoverDBInfoColumns(d);
        String tableName = DataAnnotationUtil.recoverDBInfoTableName(d);
        return createSelectQuery(keys, tableName);
    }

    /**
     * Creates a Select Query with this format: "Select ${selectColumns} from
     * ${table}
     *
     * @param d The Data class to Select
     * @param where The where to use in the Select
     * @return A String builder with the text of the query.
     */
    @Override
    public StringBuilder createSelectQuery(Class<? extends Data> d, String where) {

        String[] keys = DataAnnotationUtil.recoverDBInfoColumns(d);
        String tableName = DataAnnotationUtil.recoverDBInfoTableName(d);
        return createSelectQuery(keys, tableName, where);
    }

    @Override
    public DataManagementDatabase top(int recordsToRecover) {

        top = recordsToRecover;
        return this;
    }

    private void addTop(StringBuilder text) {

        if (top != -1) {

            text.append(" ROWNUM <= ").append(top);
            top = -1;
        }
    }

}
