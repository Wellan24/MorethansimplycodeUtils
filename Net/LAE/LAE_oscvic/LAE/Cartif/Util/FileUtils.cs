using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cartif.Extensions;

namespace Cartif.Util
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A file utilities. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class FileUtils
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Count lines. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="path"> Full pathname of the file. </param>
        /// <returns> The total number of lines. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static long CountLines(String path)
        {
            try
            {
                return new FileInfo(path).CountLines();
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
