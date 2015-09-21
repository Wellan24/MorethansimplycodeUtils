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
public class StringFormatter {

    StringBuilder stringBuilder;

    public StringFormatter() {

        stringBuilder = new StringBuilder();
    }

    public StringFormatter append(Object obj) {
        stringBuilder.append(obj);
        return this;
    }

    public StringFormatter append(String str) {
        stringBuilder.append(str);
        return this;
    }

    public StringFormatter append(StringBuffer sb) {
        stringBuilder.append(sb);
        return this;
    }

    public StringFormatter append(CharSequence s) {
        stringBuilder.append(s);
        return this;
    }

    public StringFormatter append(CharSequence s, int start, int end) {
        stringBuilder.append(s, start, end);
        return this;
    }

    public StringFormatter append(char[] str) {
        stringBuilder.append(str);
        return this;
    }

    public StringFormatter append(char[] str, int offset, int len) {
        stringBuilder.append(str, offset, len);
        return this;
    }

    public StringFormatter append(boolean b) {
        stringBuilder.append(b);
        return this;
    }

    public StringFormatter append(char c) {
        stringBuilder.append(c);
        return this;
    }

    public StringFormatter append(int i) {
        stringBuilder.append(i);
        return this;
    }

    public StringFormatter append(long lng) {
        stringBuilder.append(lng);
        return this;
    }

    public StringFormatter append(float f) {
        stringBuilder.append(f);
        return this;
    }

    public StringFormatter append(double d) {
        stringBuilder.append(d);
        return this;
    }

    public StringFormatter appendCodePoint(int codePoint) {
        stringBuilder.appendCodePoint(codePoint);
        return this;
    }

    public StringFormatter delete(int start, int end) {
        stringBuilder.delete(start, end);
        return this;
    }

    public StringFormatter deleteCharAt(int index) {
        stringBuilder.deleteCharAt(index);
        return this;
    }

    public StringFormatter replace(int start, int end, String str) {
        stringBuilder.replace(start, end, str);
        return this;
    }

    public StringFormatter insert(int index, char[] str, int offset, int len) {
        stringBuilder.insert(index, str, offset, len);
        return this;
    }

    public StringFormatter insert(int offset, Object obj) {
        stringBuilder.insert(offset, obj);
        return this;
    }

    public StringFormatter insert(int offset, String str) {
        stringBuilder.insert(offset, str);
        return this;
    }

    public StringFormatter insert(int offset, char[] str) {
        stringBuilder.insert(offset, str);
        return this;
    }

    public StringFormatter insert(int dstOffset, CharSequence s) {
        stringBuilder.insert(dstOffset, s);
        return this;
    }

    public StringFormatter insert(int dstOffset, CharSequence s, int start, int end) {
        stringBuilder.insert(dstOffset, s, start, end);
        return this;
    }

    public StringFormatter insert(int offset, boolean b) {
        stringBuilder.insert(offset, b);
        return this;
    }

    public StringFormatter insert(int offset, char c) {
        stringBuilder.insert(offset, c);
        return this;
    }

    public StringFormatter insert(int offset, int i) {
        stringBuilder.insert(offset, i);
        return this;
    }

    public StringFormatter insert(int offset, long l) {
        stringBuilder.insert(offset, l);
        return this;
    }

    public StringFormatter insert(int offset, float f) {
        stringBuilder.insert(offset, f);
        return this;
    }

    public StringFormatter insert(int offset, double d) {
        stringBuilder.insert(offset, d);
        return this;
    }

    public int indexOf(String str) {
        return stringBuilder.indexOf(str);
    }

    public int indexOf(String str, int fromIndex) {
        return stringBuilder.indexOf(str, fromIndex);
    }

    public int lastIndexOf(String str) {
        return stringBuilder.lastIndexOf(str);
    }

    public int lastIndexOf(String str, int fromIndex) {
        return stringBuilder.lastIndexOf(str, fromIndex);
    }

    public StringFormatter reverse() {
        stringBuilder.reverse();
        return this;
    }

    @Override
    public String toString() {
        return stringBuilder.toString();
    }

    public int length() {
        return stringBuilder.length();
    }

    public int capacity() {
        return stringBuilder.capacity();
    }

    public void ensureCapacity(int minimumCapacity) {
        stringBuilder.ensureCapacity(minimumCapacity);
    }

    public void trimToSize() {
        stringBuilder.trimToSize();
    }

    public void setLength(int newLength) {
        stringBuilder.setLength(newLength);
    }

    public char charAt(int index) {
        return stringBuilder.charAt(index);
    }

    public int codePointAt(int index) {
        return stringBuilder.codePointAt(index);
    }

    public int codePointBefore(int index) {
        return stringBuilder.codePointBefore(index);
    }

    public int codePointCount(int beginIndex, int endIndex) {
        return stringBuilder.codePointCount(beginIndex, endIndex);
    }

    public int offsetByCodePoints(int index, int codePointOffset) {
        return stringBuilder.offsetByCodePoints(index, codePointOffset);
    }

    public void getChars(int srcBegin, int srcEnd, char[] dst, int dstBegin) {
        stringBuilder.getChars(srcBegin, srcEnd, dst, dstBegin);
    }

    public void setCharAt(int index, char ch) {
        stringBuilder.setCharAt(index, ch);
    }

    public String substring(int start) {
        return stringBuilder.substring(start);
    }

    public CharSequence subSequence(int start, int end) {
        return stringBuilder.subSequence(start, end);
    }

    public String substring(int start, int end) {
        return stringBuilder.substring(start, end);
    }

    // TODO Añadir un método static que aproveche esta funcionalidad
    public StringFormatter appendFormat(String format, Object... args) throws StringFormatterException {

        if (format == null || args == null)
            throw new StringFormatterException((format == null) ? "format" : "args");

        int pos = 0;
        int len = format.length();
        char ch = '0';

        while (true) {
            int p = pos;
            int i = pos;
            while (pos < len) {
                ch = format.charAt(pos);

                pos++;
                if (ch == '}') {
                    if (pos < len && format.charAt(pos) == '}') // Treat as escape character for }} 
                        pos++;
                    else
                        formatError();
                }

                if (ch == '{') {
                    if (pos < len && format.charAt(pos) == '{') // Treat as escape character for {{
                        pos++;
                    else {
                        pos--;
                        break;
                    }
                }

                append(ch);
            }

            if (pos == len)
                break;

            pos++;

            if (pos == len || (ch = format.charAt(pos)) < '0' || ch > '9')
                formatError();

            int index = 0;

            do {
                index = index * 10 + ch - '0';
                pos++;
                if (pos == len)
                    formatError();
                ch = format.charAt(pos);
            } while (ch >= '0' && ch <= '9' && index < 1000000);

            if (index >= args.length)
                formatError();

            while (pos < len && (ch = format.charAt(pos)) == ' ')
                pos++;

            boolean leftJustify = false;
            int width = 0;

            if (ch == ',') {
                pos++;
                while (pos < len && format.charAt(pos) == ' ')
                    pos++;

                if (pos == len)
                    formatError();

                ch = format.charAt(pos);

                if (ch == '-') {
                    leftJustify = true;
                    pos++;
                    if (pos == len)
                        formatError();
                    ch = format.charAt(pos);
                }

                if (ch < '0' || ch > '9')
                    formatError();

                do {
                    width = width * 10 + ch - '0';
                    pos++;
                    if (pos == len)
                        formatError();
                    ch = format.charAt(pos);
                } while (ch >= '0' && ch <= '9' && width < 1000000);

            }

            while (pos < len && (ch = format.charAt(pos)) == ' ')
                pos++;

            Object arg = args[index];

            StringBuilder fmt = null;

            if (ch == ':') {
                pos++;
                p = pos;
                i = pos;
                while (true) {

                    if (pos == len)
                        formatError();

                    ch = format.charAt(pos);
                    pos++;

                    if (ch == '{') {

                        if (pos < len && format.charAt(pos) == '{')  // Treat as escape character for {{
                            pos++;
                        else
                            formatError();

                    } else if (ch == '}') {

                        if (pos < len && format.charAt(pos) == '}')  // Treat as escape character for }} 
                            pos++;
                        else {
                            pos--;
                            break;
                        }
                    }

                    if (fmt == null) {
                        fmt = new StringBuilder();
                    }
                    fmt.append(ch);
                }
            }
            if (ch != '}')
                formatError();

            pos++;
            String sFmt = null;
            String s = null;

            if (s == null) {

                Formattable formattableArg = (arg instanceof Formattable) ? (Formattable) arg : null;

                if (formattableArg != null) {

                    if (sFmt == null && fmt != null)
                        sFmt = fmt.toString();

                    s = formattableArg.toString(sFmt);

                } else if (arg != null) {

                    s = arg.toString();
                }
            }

            if (s == null)
                s = "";

            int pad = width - s.length();

            appendWithPad(s, leftJustify, pad);
        }

        return this;
    }

    public StringFormatter append(char value, int repeatCount) throws StringFormatterException {

        if (repeatCount < 0)
            formatError();

        if (repeatCount == 0)
            return this;

        for (int i = repeatCount; i > 0; i--) {

            ensureCapacity(stringBuilder.length() + 1);
            stringBuilder.append(' ');
        }

        return this;
    }

    public StringFormatter appendWithPad(String value, boolean isLeftJustified, int pad) throws StringFormatterException {

        if (!isLeftJustified && pad > 0)
            append(' ', pad);

        append(value);

        if (isLeftJustified && pad > 0)
            append(' ', pad);

        return this;
    }

    public static String format(String format, Object... args) throws StringFormatterException {

        return new StringFormatter().appendFormat(format, args).toString();
    }

    private static void formatError() throws StringFormatterException {
        throw new StringFormatterException("Error formatting the given string in StringFormatter");
    }
}
