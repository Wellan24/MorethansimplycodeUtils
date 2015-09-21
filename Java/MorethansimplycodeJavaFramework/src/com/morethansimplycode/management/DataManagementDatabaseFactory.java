/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.management;

/**
 *
 * @author Oscar Date 20-may-2015 Time 18:45:35
 */
public class DataManagementDatabaseFactory {

    public static DataManagementDatabase getDataManagementDatabase(String name) {

        switch (name) {
            case "oracle":
            case "Oracle":
                return DataManagementDatabaseOracle.getInstance();
            case "MySQL":
                return DataManagementDatabaseMysql.getInstance();
            default:
                throw new IllegalArgumentException(name + 
                        " is not a supported Database, please check the name "
                        + "or use another database provider");
        }
    }
}
