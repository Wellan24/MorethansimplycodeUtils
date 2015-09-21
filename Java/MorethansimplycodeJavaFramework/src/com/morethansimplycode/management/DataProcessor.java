/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.management;

import com.morethansimplycode.data.Data;

/**
 * This class is used in the Recover Data Process to process the data while
 * recovering it.
 *
 * @author Oscar
 */
public interface DataProcessor {

    /**
     * Process a Data and return if the Data is valid to add it to the result
     *
     * @param d The Data to Process
     * @return 
     */
    public boolean process(Data d);

    /**
     * this method is executed after all the Data is processed
     */
    public void commit();

    /**
     * This Method return an object with the info you want to return after all
     * is proccessed.
     *
     * @return
     */
    public Object getProcesed();

    /**
     * Check if an object is valid for process with this DataProcessor
     *
     * @param d
     * @return true if d is valid
     */
    public boolean isValid(Data d);
}
