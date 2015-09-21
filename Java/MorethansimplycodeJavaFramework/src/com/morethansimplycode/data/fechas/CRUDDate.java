/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.data.fechas;

import com.morethansimplycode.data.Data;

/**
 *
 * @author Oscar
 */
public class CRUDDate<T extends Data> implements Comparable<CRUDDate>{
    
    int day;
    int mes;
    int año;
    
    /**
     * Crea un objeto que representa una fecha
     * @param fecha La fecha en formato dd/mm/aaaa
     */
    public CRUDDate(String fecha) {
        
        String[] f = fecha.split("/");
        this.day = Integer.parseInt(f[0]);
        this.mes = Integer.parseInt(f[1]);
        this.año = Integer.parseInt(f[2]);
    }
    
    private CRUDDate(CRUDDate fecha) {
        
        this.day = fecha.getDia();
        this.mes = fecha.getMes();
        this.año = fecha.getAño();
    }

    public int getDia() {
        return day;
    }

    public void setDia(int dia) {
        this.day = dia;
    }

    public int getMes() {
        return mes;
    }

    public void setMes(int mes) {
        this.mes = mes;
    }

    public int getAño() {
        return año;
    }

    public void setAño(int año) {
        this.año = año;
    }

    @Override
    public String toString() {
        
        return ((day < 10)?"0"+day: day) + "/" + ((mes < 10)?"0"+mes: mes)  + "/" + año; 
    } 

    @Override
    public int compareTo(CRUDDate o) {
        
        if(año != o.año)
            return año > o.año ? 1 : -1;
        if(mes != o.mes)
            return mes > o.mes ? 1 : -1;
        if(day != o.day)
            return day > o.day ? 1 : -1;
        
        return 0;
    }
    
    
    public CRUDDate clonar(){
        
        return new CRUDDate(this);
    }
    
}
