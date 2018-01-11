using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Provides additional methods to help working with strings. </summary>
    /// <remarks> Oscvic, 2016-01-08. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class StringExtension
    {
        #region Casing methods

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a copy of this <c>System.String</c> converted to camel case. The first word of an
        ///           identifier is lowercase and the first letter of each subsequent concatenated word is
        ///           capitalized. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="theString"> The string. </param>
        /// <returns> A <c>System.String</c> in camelCase. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToCamelCase(this string theString)
        {
            string camelCaseVersion = "";

            int index = 0;
            foreach (string segment in theString.SplitWords())
            {
                bool isFirstSegment = index == 0;

                if (isFirstSegment)
                {
                    camelCaseVersion += segment.ToLowerInvariant();
                }
                else
                {
                    camelCaseVersion += segment.Capitalize();
                }

                index++;
            }

            return camelCaseVersion;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a copy of this <c>System.String</c> converted to pascal case. The first letter of
        ///           each concatenated word are capitalized. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="theString"> The string. </param>
        /// <returns> A <c>System.String</c> in PascalCase. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToPascalCase(this string theString)
        {
            string pascalCaseVersion = "";

            foreach (string segment in theString.SplitWords())
            {
                pascalCaseVersion += segment.Capitalize();
            }

            return pascalCaseVersion;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a copy of this <c>System.String</c> converted to title case. The first letter of
        ///           each word are capitalized and all the words are separated by a space. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="theString"> The string. </param>
        /// <returns> A <c>System.String</c> in Title Case. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToTitleCase(this string theString)
        {
            string titleCaseVersion = "";

            foreach (string segment in theString.SplitWords())
            {
                titleCaseVersion += segment.Capitalize() + " ";
            }

            // Remove the trailing space
            if (titleCaseVersion.Length > 0)
            {
                titleCaseVersion = titleCaseVersion.Substring(0, titleCaseVersion.Length - 1);
            }

            return titleCaseVersion;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a copy of this <c>System.String</c> converted to plain english. All the words are
        ///           in lowercase and are separated by a space. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="theString"> The string. </param>
        /// <returns> A <c>System.String</c> in plain english. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToPlainEnglish(this string theString)
        {
            return theString.ToPlainEnglish(false);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a copy of this <c>System.String</c> converted to plain english. All the words are
        ///           in lowercase and are separated by a space. The first word will be capitalized if
        ///           requested. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="theString">           The string. </param>
        /// <param name="capitalizeFirstWord"> if set to <c>true</c> capitalizes the first word. </param>
        /// <returns> A <c>System.String</c> in Plain english. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToPlainEnglish(this string theString, bool capitalizeFirstWord)
        {
            string plainEnglishVersion = "";

            int index = 0;
            foreach (string word in theString.SplitWords())
            {
                bool isFirstWord = index == 0;

                plainEnglishVersion += (isFirstWord && capitalizeFirstWord ? word.Capitalize() : word) + " ";

                index++;
            }

            // Remove the trailing space
            if (plainEnglishVersion.Length > 0)
            {
                plainEnglishVersion = plainEnglishVersion.Substring(0, plainEnglishVersion.Length - 1);
            }

            return plainEnglishVersion;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a copy of this <c>System.String</c> with the first letter in uppercase. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <exception name="ArgumentException"> Thrown when one or more arguments have unsupported or illegal
        ///                                      values. </exception>
        /// <param name="theString"> The string. </param>
        /// <returns> A <c>System.String</c> with the first letter in uppercase. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string Capitalize(this string theString)
        {
            if (String.IsNullOrEmpty(theString))
            {
                throw new ArgumentException("Can't capitalize an empty string.");
            }

            bool hasMoreThanOneCharacter = theString.Length > 1;
            return char.ToUpperInvariant(theString[0]) + (hasMoreThanOneCharacter ? theString.Substring(1) : "");
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Returns a string array that contains the words in the string delimited by a space or a
        ///           group of capital letters (such as an acronym). </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="theString"> The string. </param>
        /// <returns> An array of <c>System.String</c> in lowercase. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string[] SplitWords(this string theString)
        {
            // Add a space before every word (a word starts by a capital letter or at the first letter after a space)
            // A group of uppercase characters will be considered a word because it probably is an acronym
            // ex: VariableName -> Variable Name, IPAddress -> IP Address
            bool lastCharWasUpper = false;
            for (int index = 0; index < theString.Length; index++)
            {
                bool charIsUpper = char.IsUpper(theString, index);
                bool nextCharIsLower = index != theString.Length - 1 ? char.IsLower(theString, index + 1) : false;

                if (charIsUpper && (!lastCharWasUpper || nextCharIsLower))
                {
                    // Insert a space before the char (and update the index to stay on the char)
                    theString = theString.Insert(index, " ");
                    index++;
                }

                lastCharWasUpper = charIsUpper;
            }

            // Split the string into words
            string[] segments = theString.Split(new char[] { ' ' },  StringSplitOptions.RemoveEmptyEntries);

            // Lowercase each word (except words that are all uppercase, which are most probably acronyms) 
            for (int index = 0; index < segments.Length; index++)
            {
                bool isAllUppercase = segments[index] == segments[index].ToUpperInvariant();

                if (!isAllUppercase)
                {
                    segments[index] = segments[index].ToLowerInvariant();
                }
            }

            return segments;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Splits the lines. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="theString"> The string. </param>
        /// <returns> A string[]. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string[] SplitLines(this string theString)
        {
            return theString.SplitLines(StringSplitOptions.None);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Splits the lines. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="theString"> The string. </param>
        /// <param name="options">   The options. </param>
        /// <returns> A string[]. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string[] SplitLines(this string theString, StringSplitOptions options)
        {
            return theString.Split(new string[] { "\r\n", "\n", "\r" }, options);
        }

        #endregion // Casing methods

        #region Cardinality methods

        #endregion // Cardinality methods

        #region Conversion methods

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Fluent interface for <c>int.Parse(value)</c>. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> The integer representation of the value. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int ToInt(this string value)
        {
            return int.Parse(value);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Fluent interface for <c>long.Parse(value)</c>. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> The longeger representation of the value. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static long ToLong(this string value)
        {
            return long.Parse(value);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Fluent interface for <c>BooleanSharp.Parse(value)</c>. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The value to convert. </param>
        /// <returns> true if value is accepted true string; otherwise, false. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool ToBool(this string value)
        {
            return value.ToBool(false);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Converts the value using either <c>BooleanSharp.Parse(value)</c> or
        ///           <c>bool.Parse(value)</c> depending on the value of <c>useStandardBooleanParser</c>. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value">                    The value to convert. </param>
        /// <param name="useStandardBooleanParser"> if set to <c>true</c> use the standard boolean parser
        ///                                         (<c>bool.Parse(value)</c>). </param>
        /// <returns> true if value is an accepted true string; otherwise, false. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool ToBool(this string value, bool useStandardBooleanParser)
        {
            if (useStandardBooleanParser)
            {
                return bool.Parse(value);
            }
            else
            {
                return BooleanExtensions.Parse(value);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Converts the string representation of the name or numeric value of one or more enumerated
        ///           constants to an equivalent enumerated object. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="TEnum"> The type of the enum. </typeparam>
        /// <param name="value"> The value. </param>
        /// <returns> An object of type TEnum whose value is represented by value. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static TEnum ToEnum<TEnum>(this string value)
            where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }

        #endregion // Conversion methods

        #region FormatWith

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Formats a string with one literal placeholder. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="text"> The extension text. </param>
        /// <param name="arg0"> Argument 0. </param>
        /// <returns> The formatted string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string FormatWith(this string text, object arg0)
        {
            return string.Format(text, arg0);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Formats a string with two literal placeholders. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="text"> The extension text. </param>
        /// <param name="arg0"> Argument 0. </param>
        /// <param name="arg1"> Argument 1. </param>
        /// <returns> The formatted string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string FormatWith(this string text, object arg0, object arg1)
        {
            return string.Format(text, arg0, arg1);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Formats a string with tree literal placeholders. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="text"> The extension text. </param>
        /// <param name="arg0"> Argument 0. </param>
        /// <param name="arg1"> Argument 1. </param>
        /// <param name="arg2"> Argument 2. </param>
        /// <returns> The formatted string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string FormatWith(this string text, object arg0, object arg1, object arg2)
        {
            return string.Format(text, arg0, arg1, arg2);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Formats a string with a list of literal placeholders. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="text"> The extension text. </param>
        /// <param name="args"> The argument list. </param>
        /// <returns> The formatted string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string FormatWith(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Formats a string with a list of literal placeholders. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="text">     The extension text. </param>
        /// <param name="provider"> The format provider. </param>
        /// <param name="args">     The argument list. </param>
        /// <returns> The formatted string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string FormatWith(this string text, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, text, args);
        }
        #endregion

        #region XmlSerialize XmlDeserialize

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Serialises an object of type T in to an xml string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Any class type. </typeparam>
        /// <param name="objectToSerialise"> Object to serialise. </param>
        /// <returns> A string that represents Xml, empty oterwise. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string XmlSerialize<T>(this T objectToSerialise) where T : class
        {
            var serialiser = new XmlSerializer(typeof(T));
            string xml;
            using (var memStream = new MemoryStream())
            {
                using (var xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8))
                {
                    serialiser.Serialize(xmlWriter, objectToSerialise);
                    xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                }
            }

            // ascii 60 = '<' and ascii 62 = '>'
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            return xml;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Deserialises an xml string in to an object of Type T. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> Any class type. </typeparam>
        /// <param name="xml"> Xml as string to deserialise from. </param>
        /// <returns> A new object of type T is successful, null if failed. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static T XmlDeserialize<T>(this string xml) where T : class
        {
            var serialiser = new XmlSerializer(typeof(T));
            T newObject;

            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    try
                    {
                        newObject = serialiser.Deserialize(xmlReader) as T;
                    }
                    catch (InvalidOperationException) // String passed is not Xml, return null
                    {
                        return null;
                    }

                }
            }

            return newObject;
        }
        #endregion

        #region To X conversions

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Parses a string into an Enum. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <typeparam name="T"> The type of the Enum. </typeparam>
        /// <param name="value"> String value to parse. </param>
        /// <returns> The Enum corresponding to the stringExtensions. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static T ToEnumChecked<T>(this string value)
        {
            return ToEnumChecked<T>(value, false);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Parses a string into an Enum. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <exception name="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <exception name="ArgumentException">     Thrown when one or more arguments have unsupported or
        ///                                          illegal values. </exception>
        /// <typeparam name="T"> The type of the Enum. </typeparam>
        /// <param name="value">      String value to parse. </param>
        /// <param name="ignorecase"> Ignore the case of the string being parsed. </param>
        /// <returns> The Enum corresponding to the stringExtensions. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static T ToEnumChecked<T>(this string value, bool ignorecase)
        {
            if (value == null)
                throw new ArgumentNullException("Value");

            value = value.Trim();

            if (value.Length == 0)
                throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");

            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            return (T)Enum.Parse(t, value, ignorecase);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Toes the integer. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value">        The value. </param>
        /// <param name="defaultvalue"> The defaultvalue. </param>
        /// <returns> The given data converted to an int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int ToInteger(this string value, int defaultvalue)
        {
            return (int)ToDouble(value, defaultvalue);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Toes the integer. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> value as an int. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static int ToInteger(this string value)
        {
            return ToInteger(value, 0);
        }

        ///// <summary>
        ///// Toes the U long.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <returns></returns>
        //public static ulong ToULong(this string value)
        //{
        //    ulong def = 0;
        //    return value.ToULong(def);
        //}
        ///// <summary>
        ///// Toes the U long.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <param name="defaultvalue">The defaultvalue.</param>
        ///// <returns></returns>
        //public static ulong ToULong(this string value, ulong defaultvalue)
        //{
        //    return (ulong)ToDouble(value, defaultvalue);
        //}

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Toes the double. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value">        The value. </param>
        /// <param name="defaultvalue"> The defaultvalue. </param>
        /// <returns> The given data converted to a double. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static double ToDouble(this string value, double defaultvalue)
        {
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            else return defaultvalue;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Toes the double. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> value as a double. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static double ToDouble(this string value)
        {
            return ToDouble(value, 0);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Toes the date time. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value">        The value. </param>
        /// <param name="defaultvalue"> The defaultvalue. </param>
        /// <returns> The given data converted to a DateTime? </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime? ToDateTime(this string value, DateTime? defaultvalue)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                return result;
            }
            else return defaultvalue;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Toes the date time. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> value as a DateTime? </returns>
        ///--------------------------------------------------------------------------------------------------
        public static DateTime? ToDateTime(this string value)
        {
            return ToDateTime(value, null);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Converts a string value to bool value, supports "T" and "F" conversions. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The string value. </param>
        /// <returns> A bool based on the string value. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool? ToBoolean(this string value)
        {
            if (string.Compare("T", value, true) == 0)
            {
                return true;
            }
            if (string.Compare("F", value, true) == 0)
            {
                return false;
            }
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            else return null;
        }
        #endregion

        #region ValueOrDefault

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A string extension method that gets value or empty. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The value. </param>
        /// <returns> The value or empty. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string GetValueOrEmpty(this string value)
        {
            return GetValueOrDefault(value, string.Empty);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A string extension method that gets value or default. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value">        The value. </param>
        /// <param name="defaultvalue"> The defaultvalue. </param>
        /// <returns> The value or default. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string GetValueOrDefault(this string value, string defaultvalue)
        {
            if (value != null) return value;
            return defaultvalue;
        }

        #endregion

        #region ToUpperLowerNameVariant

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Converts string to a Name-Format where each first letter is Uppercase. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="value"> The string value. </param>
        /// <returns> value as a string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string ToUpperLowerNameVariant(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            char[] valuearray = value.ToLower().ToCharArray();
            bool nextupper = true;
            for (int i = 0; i < (valuearray.Count() - 1); i++)
            {
                if (nextupper)
                {
                    valuearray[i] = char.Parse(valuearray[i].ToString().ToUpper());
                    nextupper = false;
                }
                else
                {
                    switch (valuearray[i])
                    {
                        case ' ':
                        case '-':
                        case '.':
                        case ':':
                        case '\n':
                            nextupper = true;
                            break;
                        default:
                            nextupper = false;
                            break;
                    }
                }
            }
            return new string(valuearray);
        }
        #endregion

        #region Encrypt Decrypt

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Encryptes a string using the supplied key. Encoding is done using RSA encryption. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <exception name="ArgumentException"> Occurs when stringToEncrypt or key is null or empty. </exception>
        /// <param name="stringToEncrypt"> String that must be encrypted. </param>
        /// <param name="key">             Encryptionkey. </param>
        /// <returns> A string representing a byte array separated by a minus sign. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string Encrypt(this string stringToEncrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
            }

            System.Security.Cryptography.CspParameters cspp = new System.Security.Cryptography.CspParameters();
            cspp.KeyContainerName = key;

            System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(cspp);
            rsa.PersistKeyInCsp = true;

            byte[] bytes = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(stringToEncrypt), true);

            return BitConverter.ToString(bytes);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Decryptes a string using the supplied key. Decoding is done using RSA encryption. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <exception name="ArgumentException"> Occurs when stringToDecrypt or key is null or empty. </exception>
        /// <param name="stringToDecrypt"> The stringToDecrypt to act on. </param>
        /// <param name="key">             Decryptionkey. </param>
        /// <returns> The decrypted string or null if decryption failed. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string Decrypt(this string stringToDecrypt, string key)
        {
            string result = null;

            if (string.IsNullOrEmpty(stringToDecrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
            }

            try
            {
                System.Security.Cryptography.CspParameters cspp = new System.Security.Cryptography.CspParameters();
                cspp.KeyContainerName = key;

                System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(cspp);
                rsa.PersistKeyInCsp = true;

                string[] decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
                byte[] decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));


                byte[] bytes = rsa.Decrypt(decryptByteArray, true);

                result = System.Text.UTF8Encoding.UTF8.GetString(bytes);

            }
            finally
            {
                // no need for further processing
            }

            return result;
        }
        #endregion

        #region IsValidUrl

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Determines whether it is a valid URL. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="text"> The extension text. </param>
        /// <returns> <c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool IsValidUrl(this string text)
        {
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(text);
        }
        #endregion

        #region IsValidEmailAddress

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Determines whether it is a valid email address. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="email"> The email to act on. </param>
        /// <returns> <c>true</c> if [is valid email address] [the specified s]; otherwise, <c>false</c>. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool IsValidEmailAddress(this string email)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email);
        }
        #endregion

        #region Email

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Send an email using the supplied string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <exception name="Exception"> Thrown when an exception error condition occurs. </exception>
        /// <param name="body">      String that will be used i the body of the email. </param>
        /// <param name="subject">   Subject of the email. </param>
        /// <param name="sender">    The email address from which the message was sent. </param>
        /// <param name="recipient"> The receiver of the email. </param>
        /// <param name="server">    The server from which the email will be sent. </param>
        /// <returns> A boolean value indicating the success of the email send. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool Email(this string body, string subject, string sender, string recipient, string server)
        {
            try
            {
                // To
                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(recipient);

                // From
                MailAddress mailAddress = new MailAddress(sender);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.Body = body;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(server);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not send mail from: " + sender + " to: " + recipient + " thru smtp server: " + server + "\n\n" + ex.Message, ex);
            }

            return true;
        }
        #endregion

        #region Truncate

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Truncates the string to a specified length and replace the truncated to a ... </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="text">      The extension text. </param>
        /// <param name="maxLength"> total length of characters to maintain before the truncate happens. </param>
        /// <returns> truncated string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string Truncate(this string text, int maxLength)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            string truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            int strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }
        #endregion

        #region HTMLHelper

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Converts to a HTML-encoded string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="data"> The data. </param>
        /// <returns> A string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string HtmlEncode(this string data)
        {
            return System.Web.HttpUtility.HtmlEncode(data);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Converts the HTML-encoded string into a decoded string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="data"> The data. </param>
        /// <returns> A string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string HtmlDecode(this string data)
        {
            return System.Web.HttpUtility.HtmlDecode(data);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Parses a query string into a System.Collections.Specialized.NameValueCollection using
        ///           System.Text.Encoding.UTF8 encoding. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="query"> The query to act on. </param>
        /// <returns> A list of. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static System.Collections.Specialized.NameValueCollection ParseQueryString(this string query)
        {
            return System.Web.HttpUtility.ParseQueryString(query);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Encode an Url string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="url"> The URL to act on. </param>
        /// <returns> A string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string UrlEncode(this string url)
        {
            return System.Web.HttpUtility.UrlEncode(url);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Converts a string that has been encoded for transmission in a URL into a decoded string. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="url"> The URL to act on. </param>
        /// <returns> A string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string UrlDecode(this string url)
        {
            return System.Web.HttpUtility.UrlDecode(url);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Encodes the path portion of a URL string for reliable HTTP transmission from the Web
        ///           server to a client. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="url"> The URL to act on. </param>
        /// <returns> A string. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string UrlPathEncode(this string url)
        {
            return System.Web.HttpUtility.UrlPathEncode(url);
        }
        #endregion

        #region Format

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Replaces the format item in a specified System.String with the text equivalent of the
        ///           value of a specified System.Object instance. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="format">         The format to act on. </param>
        /// <param name="arg">            The arg. </param>
        /// <param name="additionalArgs"> The additional args. </param>
        /// <returns> The formatted value. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string Format(this string format, object arg, params object[] additionalArgs)
        {
            if (additionalArgs == null || additionalArgs.Length == 0)
            {
                return string.Format(format, arg);
            }
            else
            {
                return string.Format(format, new object[] { arg }.Concat(additionalArgs).ToArray());
            }
        }
        #endregion

        #region IsNullOrEmpty

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Determines whether [is not null or empty] [the specified input]. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="input"> The input to act on. </param>
        /// <returns> <c>true</c> if [is not null or empty] [the specified input]; otherwise, <c>false</c>. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool IsNotNullOrEmpty(this string input)
        {
            return !String.IsNullOrEmpty(input);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Determines whether [is not null or empty] [the specified input]. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="input"> The input to act on. </param>
        /// <returns> <c>true</c> if [is not null or empty] [the specified input]; otherwise, <c>false</c>. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static bool IsNullOrEmpty(this string input)
        {
            return String.IsNullOrEmpty(input);
        }

        public static String EmptyToNull(this string input)
        {
            return String.IsNullOrEmpty(input) ? null : input;
        }
        #endregion

    }
}
