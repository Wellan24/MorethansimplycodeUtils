/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.management;

import com.morethansimplycode.data.Data;
import com.morethansimplycode.data.DataAnnotationUtil;
import java.time.LocalDate;
import java.util.HashMap;
import java.util.Set;

/**
 *
 * @author Oscar Date 28-may-2015 Time 1:17:50
 */
public class DataSearch {

    private final Class<? extends Data> dataClass;
    private final StringBuilder query;
    boolean firstAdd;

    public DataSearch(Class<? extends Data> dataClass) {

        this.dataClass = dataClass;
        query = new StringBuilder("Select ");
        String[] keys = DataAnnotationUtil.recoverDBInfoColumns(dataClass);
        String tableName = DataAnnotationUtil.recoverDBInfoTableName(dataClass);

        for (String key : keys) {
            query.append(key).append(",");
        }

        query.replace(query.length() - 1, query.length(), " from ");
        query.append(tableName).append(" ");

        firstAdd = false;
    }

    public DataSearch addAndSearch(String key, Object value) {

        if (firstAdd)
            query.append(" and ");
        else
            query.append(" where ");

        if (value instanceof String || value instanceof LocalDate)
            query.append(key).append(" = '").append(value).append("'");
        else
            query.append(key).append(" = ").append(value);

        query.append(" ");
        firstAdd = true;
        return this;
    }

    public DataSearch addOrSearch(String key, Object value) {

        if (firstAdd)
            query.append(" or ");
        else
            query.append(" where ");

        if (value instanceof String || value instanceof LocalDate)
            query.append(key).append(" = '").append(value).append("'");
        else
            query.append(key).append(" = ").append(value);

        query.append(" ");
        firstAdd = true;
        return this;
    }

    public DataSearch addAndSQL(String where) {

        if (firstAdd)
            query.append(" and ");
        else
            query.append(" where ");

        query.append(" ").append(where);

        query.append(" ");
        firstAdd = true;
        return this;
    }

    public DataSearch addOrSQL(String where) {

        if (firstAdd)
            query.append(" or ");
        else
            query.append(" where ");

        query.append(" ").append(where);

        query.append(" ");
        firstAdd = true;
        return this;
    }

    public Class<? extends Data> getDataClass() {
        return dataClass;
    }

    @Override
    public String toString() {
        return query.toString();
    }

}
