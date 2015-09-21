/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.crud;

import com.morethansimplycode.data.Data;
import com.morethansimplycode.data.DataAnnotationUtil;
import java.util.List;

/**
 *
 * @author Oscar Date 20-may-2015 Time 20:07:54
 */
public class DataTableModel extends CRUDTableModel {

    private final Class<? extends Data> d;
    private Data[] data;
    private int rowCount;
    private final int columnCount;
    private final String[] columnNames;
    private final String[] fieldNames;
    private final Class[] columnClasses;

    public DataTableModel(Class<? extends Data> d, Data[] data) {

        this.data = data;
        rowCount = data.length;
        columnNames = DataAnnotationUtil.recoverTableInfoColumnNames(d);
        fieldNames = DataAnnotationUtil.recoverTableInfoFields(d);
        columnClasses = DataAnnotationUtil.recoverTableInfoAutoClasses(d);
        columnCount = columnNames.length;
        this.d = d;
    }

    public DataTableModel(Class<? extends Data> d, List<Data> data) {

        this.data = data.toArray(new Data[data.size()]);
        rowCount = data.size();
        columnNames = DataAnnotationUtil.recoverTableInfoColumnNames(d);
        fieldNames = DataAnnotationUtil.recoverTableInfoFields(d);
        columnClasses = DataAnnotationUtil.recoverTableInfoAutoClasses(d);
        columnCount = columnNames.length;
        this.d = d;
    }

    @Override
    public Class<? extends Data> getDataClass() {
        return d;
    }

    @Override
    public CRUDTableModel setData(Data[] d) {

        data = d;
        rowCount = d.length;
        return this;
    }

    @Override
    public CRUDTableModel setData(List<Data> d) {

        data = d.toArray(new Data[d.size()]);
        rowCount = d.size();
        return this;
    }

    @Override
    public Data[] getData() {

        return data;
    }

    @Override
    public int getRowCount() {

        return rowCount;
    }

    @Override
    public int getColumnCount() {

        return columnCount;
    }

    @Override
    public String getColumnName(int columnIndex) {

        return columnNames[columnIndex];
    }

    @Override
    public Class<?> getColumnClass(int columnIndex) {

        return columnClasses[columnIndex];
    }

    @Override
    public boolean isCellEditable(int rowIndex, int columnIndex) {

        return false;
    }

    @Override
    public Object getValueAt(int rowIndex, int columnIndex) {

        return data[rowIndex].get(fieldNames[columnIndex]);
    }

    
}
