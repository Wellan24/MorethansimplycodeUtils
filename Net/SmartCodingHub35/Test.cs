using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cartif.Extensions;
using Cartif.Forms;
using System.Diagnostics;

namespace Cartif
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A test. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public partial class Test : Form
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public Test()
        {
            InitializeComponent();

        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by button1 for click events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
        }
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> A prueba. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class Prueba
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the nombre prueba. </summary>
        /// <value> The nombre prueba. </value>
        ///--------------------------------------------------------------------------------------------------
        public String NOMBRE_PRUEBA { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the apellido prueba. </summary>
        /// <value> The apellido prueba. </value>
        ///--------------------------------------------------------------------------------------------------
        public String APELLIDO_PRUEBA { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the apellido prueba. </summary>
        /// <value> The apellido prueba. </value>
        ///--------------------------------------------------------------------------------------------------
        public String Apellido_Prueba { get; set; }
    }
}
