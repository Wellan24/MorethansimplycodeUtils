using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A file extensions. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class FileExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> A FileInfo extension method that count lines. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="file"> The file to act on. </param>
        /// <returns> The total number of lines. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static long CountLines(this FileInfo file)
        {
            try
            {
                using (StreamReader reader = file.OpenText())
                {
                    int count = 0;

                    while (reader.ReadLine() != null)
                        count++;

                    return count;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }
    }
}
