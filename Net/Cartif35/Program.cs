using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Forms;

namespace Cartif
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A program. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    static class Program
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Punto de entrada principal para la aplicación. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Test());
        }
    }
}
