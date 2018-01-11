using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> An advanced menu strip. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    class AdvancedMenuStrip : ToolStripProfessionalRenderer
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public AdvancedMenuStrip()
            : base(new MyColors())
        {
            this.RoundedEdges = false;
        }
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> my colors. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    class MyColors : ProfessionalColorTable
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> menustrip. </summary>
        /// <value> The menu item selected. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color MenuItemSelected
        {
            get { return ColorTranslator.FromHtml("#8CC63E"); }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el color de inicio del degradado utilizado cuando se selecciona el
        ///           <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. </summary>
        /// <value> <see cref="T:System.Drawing.Color" /> que es el color de inicio del degradado utilizado
        ///         cuando se selecciona el <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color MenuItemSelectedGradientBegin
        {
            get { return ColorTranslator.FromHtml("#5B9E3D"); }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el color final del degradado utilizado cuando se selecciona el
        ///           <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. </summary>
        /// <value> <see cref="T:System.Drawing.Color" /> que es el color final del degradado utilizado cuando
        ///         se selecciona el <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color MenuItemSelectedGradientEnd
        {
            get { return ColorTranslator.FromHtml("#8CC63E"); }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el color de borde que se va a utilizar con
        ///           <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. </summary>
        /// <value> <see cref="T:System.Drawing.Color" /> que es el color de borde que se va a utilizar con
        ///         <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color MenuItemBorder
        {
            get { return ColorTranslator.FromHtml("#8CC63E"); }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> toolstrip buttons. </summary>
        /// <value> The button selected gradient begin. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ButtonSelectedGradientBegin
        {
            get
            {
                //5B9E3D
                return ColorTranslator.FromHtml("#8CC63E");
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el color final del degradado utilizado cuando se selecciona el botón. </summary>
        /// <value> <see cref="T:System.Drawing.Color" /> que es el color final del degradado utilizado cuando
        ///         se selecciona el botón. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ButtonSelectedGradientEnd
        {
            get
            {
                return ColorTranslator.FromHtml("#8CC63E");
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el color de borde que se va a utilizar con los colores
        ///           <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin" />,
        ///           <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle" />
        ///           y <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd" />
        ///           . </summary>
        /// <value> <see cref="T:System.Drawing.Color" /> que es el color de borde que se va a utilizar con
        ///         los colores
        ///         <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin" />,
        ///         <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle" />
        ///         y <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd" />. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ButtonSelectedBorder
        {
            get
            {
                return ColorTranslator.FromHtml("#8CC63E");
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el color de inicio del degradado utilizado cuando se presiona el botón. </summary>
        /// <value> <see cref="T:System.Drawing.Color" /> que es el color de inicio del degradado utilizado
        ///         cuando se presiona el botón. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ButtonPressedGradientBegin
        {
            get
            {
                return ColorTranslator.FromHtml("#5B9E3D");
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el color final del degradado utilizado cuando se presiona el botón. </summary>
        /// <value> <see cref="T:System.Drawing.Color" /> que es el color final del degradado utilizado cuando
        ///         se presiona el botón. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ButtonPressedGradientEnd
        {
            get
            {
                return ColorTranslator.FromHtml("#5B9E3D");
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Obtiene el color de borde que se va a utilizar con los colores
        ///           <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin" />,
        ///           <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle" />
        ///           y <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd" />.</summary>
        /// <value> <see cref="T:System.Drawing.Color" /> que es el color de borde que se va a utilizar con
        ///         los colores
        ///         <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin" />,
        ///         <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle" /> y
        ///         <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd" />. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ButtonPressedBorder
        {
            get
            {
                return ColorTranslator.FromHtml("#8CC63E");
            }
        }
    }

}
