/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.data;

import java.util.HashMap;

/**
 *
 * @author Administrador
 */
public abstract class Data extends HashMap<String, Object> {
    
    public Data(int initialCapacity) {
        super(initialCapacity);
    }

    public Data(Data m) {
        super(m);
    }
    
    /**
     * Sirve para realizar copias mediante un contructor privado
     *
     * @return
     */
    public abstract Data copia();
    // TODO Add the method add returning it self to chain calls
}
