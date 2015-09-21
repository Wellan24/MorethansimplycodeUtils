/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.functions;

/**
 * Functional interface to check a condition in an Object.
 * 
 * @author Oscar
 */
public interface Checkout {
    /**
     * 
     * This method checks if the given Object meets a condition.
     *
     * @param toCheck The Object to check
     * @return True if the Object to check if it meets a condition.
     */
    public boolean check(Object toCheck);
}
