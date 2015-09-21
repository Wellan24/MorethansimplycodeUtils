/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.crud;

import javax.swing.table.TableModel;

/**
 *
 * @author Oscar
 */
public interface CRUDModel{
    
    /**
     * Use by CRUDDetail to set the fields
     * @return An array with the names to use
     */
    public String[] getDetailName();
    /**
     * @return the table model used in a table
     */
    public TableModel getTableModel();
    /**
     * Use this to set the model to use in the table related
     * @param model 
     */
    public void setTableModel(CRUDTableModel model);
    public CRUDListener[] getCRUDListener();
    public void addCRUDListener();
}
