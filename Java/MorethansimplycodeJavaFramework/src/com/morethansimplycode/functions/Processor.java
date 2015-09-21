/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.morethansimplycode.functions;

import java.util.Arrays;
import java.util.Collections;
import java.util.List;

/**
 * This class can be use to process any data, check if the object is valid for it's process, then
 * process it. If you got many Processor, you can use the weigth to order them
 *
 * @author Oscar
 */
public class Processor {

    private Checkout checker;
    private Process process;
    public int weigth;

    /**
     * Default Constructor. Weigth will be set up to 0.
     */
    public Processor() {

        this.weigth = 0;
    }

    /**
     * Use this constructor to set up the weight
     *
     * @param weigth The importance against other Processor
     */
    public Processor(int weigth) {

        this.weigth = weigth;
    }

    /**
     * Use this constructor to set up the fields fast and easily. You can use lambdas.
     *
     * @param checker The checker algorithm
     * @param process The process algorithm
     * @param weigth The importance against other Processor
     */
    public Processor(Checkout checker, Process process, int weigth) {

        this.checker = checker;
        this.process = process;
        this.weigth = weigth;
    }

    /**
     * This method checks if the given Object can by processed. Use it before execute.
     *
     * @param toCheck The Object to check
     * @return True if the Object to check is valid to process
     */
    public boolean check(Object toCheck) {
        return checker.check(toCheck);
    }

    /**
     * This method executes the process. Use it after check, and if check returns true.
     *
     * @param toProcess The Object to process
     * @return The result of process the Object to process.
     */
    public Object execute(Object toProcess) {
        return process.execute(toProcess);
    }

    public void setChecker(Checkout checker) {
        this.checker = checker;
    }

    public void setProcess(Process process) {
        this.process = process;
    }

    /**
     * Use this method to sort a List of Processors, using its weight. Use desc = true to sort by
     * descending (100, 20, 1...), use desc = false to sort by ascending (1, 20, 100...)
     *
     * @param processors The processors to sort
     * @param desc The direction
     */
    public static void sort(List<Processor> processors, boolean desc) {

        if (desc)
            Collections.sort(processors, (o1, o2) -> o1.weigth < o2.weigth ? 1 : -1);
        else
            Collections.sort(processors, (o1, o2) -> o2.weigth < o1.weigth ? 1 : -1);
    }

    /**
     * Use this method to sort an Array of Processors, using its weight. Use desc = true to sort by
     * descending (100, 20, 1...), use desc = false to sort by ascending (1, 20, 100...)
     *
     * @param processors The processors to sort
     * @param desc The direction
     */
    public static void sort(Processor[] processors, boolean desc) {

        if (desc)
            Arrays.sort(processors, (o1, o2) -> o1.weigth < o2.weigth ? 1 : -1);
        else
            Arrays.sort(processors, (o1, o2) -> o2.weigth < o1.weigth ? 1 : -1);
    }
}
