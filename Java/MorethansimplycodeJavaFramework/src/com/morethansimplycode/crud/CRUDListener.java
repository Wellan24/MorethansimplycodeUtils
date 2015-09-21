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
public interface CRUDListener {
    
    /**
     * This method is called when the selecction in a CRUDComponent changes
     * @param newSelectionPosition 
     */
    public void onSelectionChanged(int newSelectionPosition);
}
