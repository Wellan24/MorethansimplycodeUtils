/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package test;

import com.morethansimplycode.data.Data;
import com.morethansimplycode.data.DataDBInfo;
import com.morethansimplycode.data.DataDetailInfo;
import com.morethansimplycode.data.DataTableInfo;

/**
 *
 * @author Oscar
 */
@DataDBInfo(columns = {"ID", "NOMBRE", "SUELDO", "JEFE"}, tableName = "empleados1", primaryKey = {"ID"})
@DataTableInfo(fields = {"NOMBRE", "SUELDO", "JEFE"}, classes = {String.class, Integer.class, String.class}, columnNames = {"NOMBRE", "SUELDO", "JEFE"})
@DataDetailInfo(fields = {"NOMBRE", "SUELDO", "JEFE"}, namesShow = {"Nombre", "Sueldo", "Jefe"})
public class Empleado1 extends Data {

    public Empleado1() {
        super(3);
    }

    public Empleado1(int initialCapacity) {
        super(initialCapacity);
    }

    private Empleado1(Empleado1 emple) {
        super(emple);
    }

    @Override
    public Data copia() {

        return new Empleado1(this);
    }

    @Override
    public String toString() {
        return this.get("ID") + " - " + this.get("NOMBRE");
    }

}
