/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.crud;

import com.morethansimplycode.data.Data;
import com.morethansimplycode.data.DataAnnotationUtil;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.util.ArrayList;
import java.util.Arrays;
import javax.swing.JTable;

/**
 *
 * @author Oscar
 */
public class CRUDTable extends JTable {

    private ArrayList<CRUDTableListener> listeners;
    private CRUDTableModel model;

    public CRUDTable() {

        addMouseListener(new MouseAdapter() {
            @Override
            public void mouseClicked(MouseEvent e) {
                int fila = rowAtPoint(e.getPoint());
                int columna = columnAtPoint(e.getPoint());
                if ((fila > -1) && (columna > -1))
                    callSelectionChanged(fila, columna);
            }
        });
    }

    public boolean addCRUDTableListener(CRUDTableListener listener) {

        if (listeners == null)
            listeners = new ArrayList<>();

        return listeners.add(listener);
    }

    public boolean addCRUDTableListeners(CRUDTableListener[] listeners) {

        if (this.listeners == null)
            this.listeners = new ArrayList<>();

        return this.listeners.addAll(Arrays.asList(listeners));
    }

    public void callSelectionChanged(int newSelectedRow, int newSelectedColumn) {

        if (listeners != null)
            listeners.stream().forEach((l) -> {
                l.onItemClicked(newSelectedRow, newSelectedColumn);
            });
    }

    public void setModel(CRUDTableModel model) {

        this.model = model;
        super.setModel(model);
    }

    public ArrayList<CRUDTableListener> getListeners() {
        return listeners;
    }

    public CRUDTableModel getCRUDTableModel() {
        return model;
    }

    public Data[] getData() {
        return model.getData();
    }
}
