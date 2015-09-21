/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.functions;

/**
 * Functional interface to process and Object.
 *
 * @author Oscar
 */
public interface Process {   
    
    /**
     * This method process an Object..
     *
     * @param toProcess The Object to process
     * @return The result of process the Object to process.
     */
    public Object execute(Object toProcess);
}
