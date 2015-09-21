/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.crud;

/**
 *
 * @author Oscar
 */
public interface CRUDTableListener {

    /**
     * This method is called when an item is clicked in a table
     *
     * @param newSelectedRow The new Row
     * @param newSelectedColumn the new Column
     */
    public void onItemClicked(int newSelectedRow, int newSelectedColumn);
}
