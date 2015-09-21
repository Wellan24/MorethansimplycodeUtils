/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.data.fechas;

import java.util.Calendar;

/**
 * @author Oscar
 * Date 19-nov-2014
 * Time 18:12:53
 */
public class CRUDDateUtil {

    private static CRUDDate fechaActual;

    /**
     * Devuelve los <u>días</u> de diferencia entre dos fechas. <br>
     * Para un  resultado positivo, la fecha mayor debe ir primero
     *
     * @param fechaUno
     * @param fechaDos
     * @return
     */
    public static int calcularDiferenciaFechas(CRUDDate fechaUno, CRUDDate fechaDos) {

        return (fechaUno.getDia() - fechaDos.getDia())
                + (fechaUno.getMes() - fechaDos.getMes()) * 30
                + (fechaUno.getAño() - fechaDos.getAño()) * 365;
    }
    
    public static void calculaFechaActual(){
        
        if (fechaActual == null) {

            Calendar c = Calendar.getInstance();
            fechaActual = new CRUDDate(c.get(Calendar.DATE) + "/" + (c.get(Calendar.MONTH) + 1) + "/" + c.get(Calendar.YEAR));
        }
    }

    public static CRUDDate getFechaActual() {
        
        if(fechaActual == null)
            calculaFechaActual();
        
        return fechaActual.clonar();
    }   
    
}
