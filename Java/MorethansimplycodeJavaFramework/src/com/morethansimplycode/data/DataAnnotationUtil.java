/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.data;

import java.lang.annotation.Annotation;

/**
 *
 * @author Oscar
 */
public class DataAnnotationUtil {

    private DataAnnotationUtil() {
    }

    /**
     * Recover the fields names from DataBInfo. If there's no anntotation,
     * returns an empty Array.
     *
     * @param d The class for which table name wants to recover
     * @return The columns or an empty array
     */
    public static String[] recoverDBInfoColumns(Class<? extends Data> d) {

        Annotation a = d.getAnnotation(DataDBInfo.class);
        if (a != null) {

            String[] columns = ((DataDBInfo) a).columns();
            return (columns != null) ? columns : new String[]{};
        }

        return new String[]{};
    }

    /**
     * Recover the fields names from DataBInfo. If there's no anntotation,
     * returns the name of the Class.
     *
     * @param d The class for which table name wants to recover
     * @return The tableName or the Class name if tableName is null
     */
    public static String recoverDBInfoTableName(Class<? extends Data> d) {

        Annotation a = d.getAnnotation(DataDBInfo.class);
        if (a != null) {

            String tableName = ((DataDBInfo) a).tableName();
            return (tableName != null && !tableName.equals("NONE")) ? tableName : d.getSimpleName();
        }

        return d.getSimpleName();
    }

    /**
     * Recover the fields names from DataBInfo. If there's no anntotation,
     * returns an empty Array.
     *
     * @param d The class for which table name wants to recover
     * @return The tableName or an empty array
     */
    public static String[] recoverDBInfoPrimaryKeys(Class<? extends Data> d) {

        Annotation a = d.getAnnotation(DataDBInfo.class);
        if (a != null) {

            String[] primarykeys = ((DataDBInfo) a).primaryKey();
            return (primarykeys != null) ? primarykeys : new String[]{};
        }

        return new String[]{};
    }

    /**
     * Recover the autoNumKey names from DataBInfo. If there's no anntotation,
     * returns an empty Array.
     *
     * @param d The class for which table name wants to recover
     * @return The autoNumKeys or an empty array
     */
    public static String[] recoverDBInfoAutoNumKeys(Class<? extends Data> d) {

        Annotation a = d.getAnnotation(DataDBInfo.class);
        if (a != null) {

            String[] autoNumKeys = ((DataDBInfo) a).autoNumKeys();
            return (autoNumKeys != null) ? autoNumKeys : new String[]{};
        }

        return new String[]{};
    }

    /**
     * Recover the fields names to show in the Detail Section from
     * DataDetailInfo. If there's no anntotation, returns an empty Array.
     *
     * @param d The class for which table name wants to recover
     * @return The names or an empty array
     */
    public static String[] recoverDetailInfoFields(Class<? extends Data> d) {

        Annotation a = d.getAnnotation(DataDetailInfo.class);
        if (a != null) {

            String[] names = ((DataDetailInfo) a).fields();
            return (names != null) ? names : new String[]{};
        }

        return new String[]{};
    }
    
    /**
     * Recover the fields names to show in the Detail Section from
     * DataDetailInfo. If there's no anntotation, returns an empty Array.
     *
     * @param d The class for which table name wants to recover
     * @return The names or an empty array
     */
    public static String[] recoverDetailInfoNamesShow(Class<? extends Data> d) {

        Annotation a = d.getAnnotation(DataDetailInfo.class);
        if (a != null) {

            String[] names = ((DataDetailInfo) a).namesShow();
            return (names != null) ? names : new String[]{};
        }

        return new String[]{};
    }

    /**
     * Recover the columns names from DataTableInfo. If there's no anntotation,
     * returns an empty Array.
     *
     * @param d The class for which table name wants to recover
     * @return The columns or an empty array
     */
    public static String[] recoverTableInfoFields(Class<? extends Data> d) {

        Annotation a = d.getAnnotation(DataTableInfo.class);
        if (a != null) {

            String[] columns = ((DataTableInfo) a).fields();
            return (columns != null) ? columns : new String[]{};
        }

        return new String[]{};
    }

    /**
     * Recover the classes from DataTableInfo. If there's no anntotation,
     * returns an empty Array.
     *
     * @param d The class for which classes wants to recover
     * @return The classes or an empty array
     */
    public static Class[] recoverTableInfoAutoClasses(Class<? extends Data> d) {

        Annotation a = d.getAnnotation(DataTableInfo.class);
        if (a != null) {

            Class[] classes = ((DataTableInfo) a).classes();
            return (classes != null) ? classes : new Class[]{};
        }

        return new Class[]{};
    }

    /**
     * Recover the classes from DataTableInfo. If there's no anntotation,
     * returns an empty Array.
     *
     * @param d The class for which fields name wants to recover
     * @return The classes or an empty array
     */
    public static String[] recoverTableInfoColumnNames(Class<? extends Data> d) {

        Annotation a = d.getAnnotation(DataTableInfo.class);
        if (a != null) {

            String[] names = ((DataTableInfo) a).columnNames();
            return (names != null) ? names : new String[]{};
        }

        return new String[]{};
    }
}
