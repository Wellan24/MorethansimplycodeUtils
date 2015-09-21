/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.management;

import com.morethansimplycode.data.Data;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * The purpose of this class is to wrapp a DataManagement object to give a
 * Singleton like class using it.
 *
 * @author Oscar
 */
public class SingletonDataManagement {

    private DataManagement dataManagment;

    private DataListenerImpl dataListener;

    public static SingletonDataManagement instance;

    private SingletonDataManagement() {

    }

    public static SingletonDataManagement getInstance() {

        if (instance == null) {
            instance = new SingletonDataManagement();
        }

        return instance;
    }

    public DataManagement getDataManagment() {
        return dataManagment;
    }

    public void setDataManagment(DataManagement dataManagment) {
        this.dataManagment = dataManagment;
        this.dataListener = new DataListenerImpl();
    }

    public void addDataListener(DataListener dataListener) {
        this.dataListener.addDataListener(dataListener);
    }

    public void addDataListeners(DataListener[] dataListener) {
        this.dataListener.addDataListeners(dataListener);
    }

    public void addDataListeners(List<DataListener> dataListener) {
        this.dataListener.addDataListeners(dataListener);
    }

    /**
     * Recover an Array of Data of the given class with the given where clausule
     *
     * @param d The Data class to recover from the Table
     * @return An ArrayList&lt;Data&gt; with the recovered Data
     */
    public ArrayList<Data> recoverData(Class<? extends Data> d) {

        return dataManagment.recoverData(d);
    }

    /**
     * Recover an Array of Data of the given class with the given where clausule
     *
     * @param d The Data class to recover from the Table
     * @param where
     * @return An ArrayList&lt;Data&gt; with the recovered Data
     */
    public ArrayList<Data> recoverData(Class<? extends Data> d, String where) {

        return dataManagment.recoverData(d, where);
    }

    /**
     * Recover an Array of Data of the given class with the given where clausule
     *
     * @param d The Data class to recover from the Table
     * @param p The DataProcessor to use
     * @return An ArrayList&lt;Data&gt; with the recovered Data
     */
    public ArrayList<Data> recoverData(Class<? extends Data> d, DataProcessor p) {
        return dataManagment.recoverData(d, p);
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
        return dataManagment.recoverData(d, p, where);
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * call all the DataListeners of the Class recovered or Data.class, using
     * handleDataRecoveryNotCached
     *
     * @param d The Data class to recover from the Table
     * @return The instance to allow chains
     */
    public SingletonDataManagement recoveryDataAsync(Class<? extends Data> d) {

        dataManagment.recoverDataAsync(d, dataListener);
        return this;
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * call all the DataListeners of the Class recovered or Data.class, using
     * handleDataRecoveryNotCached
     *
     * @param d The Data class to recover from the Table
     * @param where The where clausule
     *
     * @return The instance to allow chains
     */
    public SingletonDataManagement recoveryDataAsync(Class<? extends Data> d, String where) {

        dataManagment.recoverDataAsync(d, dataListener, where);
        return this;
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * call all the DataListeners of the Class recovered or Data.class, using
     * handleDataRecoveryNotCached
     *
     * @param d The Data class to recover from the Table
     * @param p The processor
     * @param where The where clausule
     * @return The instance to allow chains
     */
    public SingletonDataManagement recoveryDataAsync(Class<? extends Data> d, DataProcessor p, String where) {

        dataManagment.recoverDataAsync(d, dataListener, p, where);
        return this;
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * call all the DataListeners of the Class recovered or Data.class, using
     * handleDataRecoveryCached if cached is true and
     * handleDataRecoveryNotCached if cached is false
     *
     * @param d The Data class to recover from the Table
     * @param p The processor
     * @param where The where clausule
     * @param cached True if cached with the table name or no
     * @return The instance to allow chains
     */
    public SingletonDataManagement recoveryDataAsync(Class<? extends Data> d, DataProcessor p, String where, boolean cached) {

        dataManagment.recoverDataAsync(d, dataListener, p, where, where);
        return this;
    }

    /**
     * This method uses ArrayList&lt;Data&gt; in a separated Thread and then
     * call all the DataListeners of the Class recovered or Data.class, using
     * handleDataRecoveryCached
     *
     * @param d The Data class to recover from the Table
     * @param p The processor
     * @param where The where clausule
     * @param key The key used to cache the data
     * @return The instance to allow chains
     */
    public SingletonDataManagement recoveryDataAsync(Class<? extends Data> d, DataProcessor p, String where, String key) {

        dataManagment.recoverDataAsync(d, dataListener, p, where, true);
        return this;
    }

    public SingletonDataManagement top(int top) {

        dataManagment.top(top);
        return this;
    }

    /**
     * Get the DataManagementDatabase object, used by this class to create the
     * queries. Use it if you want to create it.
     *
     * @return The DataManagementDatabase object.
     */
    public DataManagementDatabase getDataManagementDatabase() {
        return dataManagment.getDataManagementDatabase();
    }

    /**
     * Use this method to recover Cached Data saved with the given key.
     *
     * @param key The key of the data in the cache.
     * @return The data saved with that key.
     */
    public ArrayList<Data> getDataFromCache(String key) {
        return dataManagment.getDataFromCache(key);
    }

    /**
     * This calls commit() method from the connection.
     *
     * @throws SQLException
     */
    public void commit() throws SQLException {
        dataManagment.commit();
    }

    /**
     * Inserts data.
     *
     * @param d The data to be inserted.
     * @return
     */
    public boolean insertData(Data d) {
        return dataManagment.insertData(d);
    }

    /**
     * Inserts data.
     *
     * @param d The data to be inserted.
     * @return
     */
    public boolean insertAutoNumericData(Data d) {
        return dataManagment.insertAutoNumericData(d);
    }

    /**
     * Check the data using its primarykey
     *
     * @param d The Data to check
     * @return if the Data exists.
     */
    public boolean existsByPrimaryKey(Data d) {
        return dataManagment.existsByPrimaryKey(d);
    }

    /**
     * Check the data using all its columns
     *
     * @param d The Data to check
     * @return if the Data exists.
     */
    public boolean existsByAllColumns(Data d) {
        return dataManagment.existsByAllColumns(d);
    }

    /**
     * Check the data using the given columns
     *
     * @param d The Data to check
     * @return if the Data exists.
     */
    public boolean existsByColumns(String[] columns, Data d) {
        return dataManagment.existsByColumns(columns, d);
    }

    /**
     * Update the given Data
     *
     * @param d The Data to update
     * @return if successfull
     */
    public boolean updateDato(Data d) {
        return dataManagment.updateDato(d);
    }

    /**
     * This method close the connection of this object and clear its cache. If
     * you want to keep the cache, use the method copyCache()
     *
     * @throws Exception
     */
    public void close() throws Exception {
        dataManagment.close();
    }

    /**
     * Search in the Database with the specified DataSearch
     *
     * @param search The DataSearch to use
     * @return An ArrayList&lt;Data&gt; with the recovered Data
     */
    public ArrayList<Data> searchData(DataSearch search) {
        return dataManagment.searchData(search);
    }

}

class DataListenerImpl implements DataListener {

    private final ArrayList<DataListener> userDataListeners;
    private final Class<? extends Data> listenedDataClass = Data.class;

    private Class<? extends Data> listeningDataClass = Data.class;

    public DataListenerImpl() {
        userDataListeners = new ArrayList<>();
    }

    @Override
    public Class<? extends Data> getClassforHandle() {

        return listenedDataClass;
    }

    @Override
    public boolean isListeningClass(Class<? extends Data> dataClass) {

        listeningDataClass = dataClass;
        return dataClass.equals(listenedDataClass) || dataClass.getSuperclass().equals(listenedDataClass);
    }

    @Override
    public void handleDataRecoveryNotCached(ArrayList<Data> data, DataProcessor processor) {

        userDataListeners.stream().filter((dl) -> (dl.isListeningClass(listeningDataClass))).forEach((dl) -> {
            dl.handleDataRecoveryNotCached(data, processor);
        });
    }

    @Override
    public void handleDataRecoveryCached(String key, DataProcessor processor) {

        userDataListeners.stream().filter((dl) -> (dl.isListeningClass(listeningDataClass))).forEach((dl) -> {
            dl.handleDataRecoveryCached(key, processor);
        });
    }

    public void addDataListener(DataListener dataListener) {
        userDataListeners.add(dataListener);
    }

    public void addDataListeners(DataListener[] dataListener) {
        userDataListeners.addAll(Arrays.asList(dataListener));
    }

    public void addDataListeners(List<DataListener> dataListener) {
        userDataListeners.addAll(dataListener);
    }

}
