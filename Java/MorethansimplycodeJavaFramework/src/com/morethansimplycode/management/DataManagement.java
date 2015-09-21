/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.management;

import com.morethansimplycode.data.Data;
import com.morethansimplycode.data.DataAnnotationUtil;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * This class manage a connection to a DataBase and its uses.
 *
 * @author Oscar
 */
public class DataManagement implements AutoCloseable {

    /**
     * The Connection used to connect to the required DataBase.
     */
    private Connection connection;

    /**
     * The DataCache object that represents the cache.
     */
    private DataCache dataCache;

    /**
     * The DataCache object that represents the cache.
     */
    private DataManagementDatabase dataManagementDatabase;

    private int top = -1;

    /**
     * A contructor using a Connection
     *
     * @param connection the connection to use.
     */
    public DataManagement(Connection connection) {
        try {
            this.connection = connection;
            dataManagementDatabase = DataManagementDatabaseFactory
                    .getDataManagementDatabase(connection.getMetaData().getDatabaseProductName());

        } catch (SQLException ex) {
            Logger.getLogger(DataManagement.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    /**
     * Creates a connections using the given parameters.
     *
     * @param className The class name of the driver
     * @param url The url to connect the Database
     */
    public DataManagement(String className, String url) {

        try {

            Class.forName(className);
            this.connection = DriverManager.getConnection(url);
            dataManagementDatabase = DataManagementDatabaseFactory
                    .getDataManagementDatabase(connection.getMetaData().getDatabaseProductName());

        } catch (SQLException | ClassNotFoundException ex) {
            Logger.getLogger(DataManagement.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    /**
     * Creates a connections using the given parameters
     *
     * @param className The class name of the driver
     * @param url The url to connect the Database
     * @param user The user used or an empty string
     * @param password The password for the user
     */
    public DataManagement(String className, String url, String user, String password) {

        try {

            Class.forName(className);
            this.connection = DriverManager.getConnection(url, user, password);
            dataManagementDatabase = DataManagementDatabaseFactory
                    .getDataManagementDatabase(connection.getMetaData().getDatabaseProductName());

        } catch (SQLException | ClassNotFoundException ex) {
            Logger.getLogger(DataManagement.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    /**
     * Adds top clausule to the Selects
     *
     * @param top
     * @return The instance to allow chains
     */
    public DataManagement top(int top) {

        this.top = top;
        return this;
    }

    /**
     * Get the DataManagementDatabase object, used by this class to create the
     * queries. Use it if you want to create it.
     *
     * @return The DataManagementDatabase object.
     */
    public DataManagementDatabase getDataManagementDatabase() {

        return dataManagementDatabase;
    }

    /**
     * Recover an Array of Data of the given class with the given where clausule
     *
     * @param d The Data class to recover from the Table
     * @return An ArrayList&lt;Data&gt; with the recovered Data
     */
    public ArrayList<Data> recoverData(Class<? extends Data> d) {

        return recoverData(d, null, "");
    }

    /**
     * Search in the Database with the specified DataSearch
     *
     * @param search The DataSearch to use
     * @return An ArrayList&lt;Data&gt; with the recovered Data
     */
    public ArrayList<Data> searchData(DataSearch search) {

        ArrayList<Data> ret = new ArrayList<>();
        Class<? extends Data> dataClass = search.getDataClass();
        String[] keys = DataAnnotationUtil.recoverDBInfoColumns(dataClass);

        try (ResultSet rs = dataManagementDatabase.executeQuery(
                connection, search.toString())) {

            Data data;
            while (rs.next()) {

                data = dataClass.newInstance();
                for (String key : keys) {
                    data.put(key, rs.getObject(key));
                }
                ret.add(data);
            }
        } catch (SQLException ex) {
            Logger.getLogger(DataManagement.class.getName()).log(Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            Logger.getLogger(DataManagement.class.getName()).log(Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            Logger.getLogger(DataManagement.class.getName()).log(Level.SEVERE, null, ex);
        }

        return ret;
    }

    /**
     * Recover an Array of Data of the given class with the given where clausule
     *
     * @param d The Data class to recover from the Table
     * @param where The where clausule to use
     * @return An ArrayList&lt;Data&gt; with the recovered Data
     */
    public ArrayList<Data> recoverData(Class<? extends Data> d, String where) {

        return recoverData(d, null, where);
    }

    /**
     * Recover an Array of Data of the given class with the given where clausule
     *
     * @param d The Data class to recover from the Table
     * @param p The DataProcessor to use
     * @return An ArrayList&lt;Data&gt; with the recovered Data
     */
    public ArrayList<Data> recoverData(Class<? extends Data> d, DataProcessor p) {

        return recoverData(d, p, null);
    }

    /**
     * Recover an Array of Data of the given class with the given where clausule
     *
     * @param d The class to recover the data
     * @param p The DataProcessor to use
     * @param where The where clausule to use
     * @return An ArrayList&lt;Data&gt; with the recovered Data
     */
    public ArrayList<Data> recoverData(Class<? extends Data> d, DataProcessor p, String where) {

        ArrayList<Data> ret = new ArrayList<>();
        String[] keys = DataAnnotationUtil.recoverDBInfoColumns(d);
        String tableName = DataAnnotationUtil.recoverDBInfoTableName(d);
        
        try (ResultSet rs = dataManagementDatabase.executeQuery(connection,
                dataManagementDatabase.top(top).createSelectQuery(keys, tableName, where).toString())) {

            Data data;
            while (rs.next()) {

                data = d.newInstance();
                for (String key : keys) {
                    data.put(key, rs.getObject(key));
                }
                if (p != null) {

                    if (p.isValid(data) && p.process(data)) {
                        ret.add(data);
                    }

                } else {
                    ret.add(data);
                }
            }

            if (p != null) {
                p.commit();
            }
            
            top = -1;
            return ret;
        } catch (SQLException | InstantiationException | IllegalAccessException ex) {
            Logger.getLogger(DataManagement.class.getName()).log(Level.SEVERE, null, ex);
        }

        return ret;
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * gives it to a DataListener, using handleDataRecoveryNotCached
     *
     * @param d The class to recover the data
     * @param listener The listener of the method
     */
    public void recoverDataAsync(Class<? extends Data> d, DataListener listener) {

        new Thread(() -> {

            ArrayList<Data> data = recoverData(d);

            if (listener.isListeningClass(d)) {
                listener.handleDataRecoveryNotCached(data, null);
            }

        }).start();
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * gives it to a DataListener, using handleDataRecoveryNotCached
     *
     * @param d The class to recover the data
     * @param listener The listener of the method
     * @param where The where clausule
     */
    public void recoverDataAsync(Class<? extends Data> d, DataListener listener, String where) {

        new Thread(() -> {

            ArrayList<Data> data = recoverData(d, where);

            if (listener.isListeningClass(d)) {
                listener.handleDataRecoveryNotCached(data, null);
            }

        }).start();
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * gives it to a DataListener, using handleDataRecoveryNotCached
     *
     * @param d The class to recover the data
     * @param listener The listener of the method
     * @param p The processor
     * @param where The where clausule
     */
    public void recoverDataAsync(Class<? extends Data> d, DataListener listener, DataProcessor p, String where) {

        new Thread(() -> {

            ArrayList<Data> data = recoverData(d, p, where);

            if (listener.isListeningClass(d)) {
                listener.handleDataRecoveryNotCached(data, null);
            }

        }).start();
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * gives it to a DataListener, using handleDataRecoveryCached if cached is
     * true and handleDataRecoveryNotCached if cached is false
     *
     * @param d The class to recover the data
     * @param listener The listener of the method
     * @param p The processor
     * @param where The where clausule
     * @param cached True if cached with the table name or no
     */
    public void recoverDataAsync(Class<? extends Data> d, DataListener listener, DataProcessor p, String where, boolean cached) {

        new Thread(() -> {

            String tableName = DataAnnotationUtil.recoverDBInfoTableName(d);
            ArrayList<Data> data = recoverData(d, p, where);
            addDataToCache(tableName, data);

            if (listener.isListeningClass(d)) {
                listener.handleDataRecoveryCached(tableName, p);
            }

        }).start();
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * gives it to a DataListener, using handleDataRecoveryCached
     *
     * @param d The class to recover the data
     * @param listener The listener of the method
     * @param p The processor
     * @param where The where clausule
     * @param key The key used to cache the data
     */
    public void recoverDataAsync(Class<? extends Data> d, DataListener listener, DataProcessor p, String where, String key) {

        new Thread(() -> {

            ArrayList<Data> data = recoverData(d, p, where);
            addDataToCache(key, data);

            if (listener.isListeningClass(d)) {
                listener.handleDataRecoveryCached(key, p);
            }

        }).start();
    }

    /**
     * This method add an ArrayList&lt;Data&gt; to the cache, in order to not
     * repeat the connection to the DataBase when it's not needed. It's for
     * internal use only, so that's why it's private.
     *
     * @param key The key used in he Map of the cache.
     * @param data The data to Cache
     */
    private void addDataToCache(String key, ArrayList<Data> data) {

        if (dataCache == null) {
            dataCache = new DataCache();
        }

        dataCache.putCacheData(key, data);
    }

    /**
     * Use this method to recover Cached Data saved with the given key.
     *
     * @param key The key of the data in the cache.
     * @return The data saved with that key.
     */
    public ArrayList<Data> getDataFromCache(String key) {

        return dataCache.getCachedData(key);
    }

    /**
     * This calls commit() method from the connection.
     *
     * @throws SQLException
     */
    public void commit() throws SQLException {
        connection.commit();
    }

    /**
     * Inserts data.
     *
     * @param d The data to be inserted.
     * @return
     */
    public boolean insertData(Data d) {
        return dataManagementDatabase.insertData(connection, d, false);
    }

    /**
     * Inserts data.
     *
     * @param d The data to be inserted.
     * @return
     */
    public boolean insertAutoNumericData(Data d) {
        return dataManagementDatabase.insertData(connection, d, true);
    }

    /**
     * Check the data using its primarykey
     *
     * @param d The Data to check
     * @return if the Data exists.
     */
    public boolean existsByPrimaryKey(Data d) {
        return dataManagementDatabase.existsByPrimaryKey(connection, d);
    }

    /**
     * Check the data using all its columns
     *
     * @param d The Data to check
     * @return if the Data exists.
     */
    public boolean existsByAllColumns(Data d) {
        return dataManagementDatabase.existsByAllColumns(connection, d);
    }

    /**
     * Check the data using the given columns
     *
     * @param columns The columns to use to check if exists
     * @param d The Data to check
     * @return if the Data exists.
     */
    public boolean existsByColumns(String[] columns, Data d) {
        return dataManagementDatabase.existsByColumns(connection, columns, d);
    }

    /**
     * Update the given Data
     *
     * @param d The Data to update
     * @return if successfull
     */
    public boolean updateDato(Data d) {
        return dataManagementDatabase.updateDato(d, connection);
    }

    /**
     * This method close the connection of this object and clear its cache. If
     * you want to keep the cache, use the method copyCache()
     *
     */
    @Override
    public void close() {

        try {

            if (dataCache != null)
                this.dataCache.clearCache();

            if (connection != null)
                this.connection.close();

            this.dataManagementDatabase = null;
            this.dataCache = null;
            this.connection = null;

        } catch (SQLException ex) {
            Logger.getLogger(DataManagement.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    /**
     * This method that returns a copy of the cache.
     *
     * @return A new HashMap&lt;String, ArrayList&lt;Data&gt;&gt;
     */
    public HashMap<String, ArrayList<Data>> copyCache() {
        return dataCache.copyCache();
    }

}
