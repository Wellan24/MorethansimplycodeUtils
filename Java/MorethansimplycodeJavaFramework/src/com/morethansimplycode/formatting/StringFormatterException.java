/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.formatting;

/**
 * Exception to handle Formatting errors. It's an unchecked
 * exception, so there is no need of catch it unless you want.
 * 
 * @author Oscar
 */
public class StringFormatterException extends RuntimeException{

    public StringFormatterException() {
    }

    public StringFormatterException(String message) {
        super(message);
    }    
    
    @Override
    public String toString() {
        return super.toString();
    }

    @Override
    public String getMessage() {
        return "This is an Exception in StringFormatter: " + super.getMessage();
    }
    
}
