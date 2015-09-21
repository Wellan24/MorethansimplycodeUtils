/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.formatting;

/**
 *
 * @author Oscar
 */
public interface Formattable {

    public String toString(String format);

    public String toString(CustomFormatter formatter, String format);
}
