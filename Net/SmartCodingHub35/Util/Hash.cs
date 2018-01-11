using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace Cartif.Util
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> -----------------------------------------------------------------
    ///             Namespace:      Cartif.Util Class:          Hash Description:    Provides methods
    ///             for hashing string Author:         Oscar - Cartif       Date: 14-09-2015 Notes:
    ///             Revision History: Name:           Date:        Description:
    ///           -----------------------------------------------------------------. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class Hash
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets md 5. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="input"> The input. </param>
        /// <returns> The md 5. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static string GetMD5(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
    }
}
