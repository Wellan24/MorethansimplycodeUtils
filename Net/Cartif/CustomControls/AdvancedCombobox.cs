using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;

namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A combo box personalizado. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    class ComboBoxPersonalizado : ComboBox
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the draw mode. </summary>
        /// <value> The draw mode. </value>
        ///--------------------------------------------------------------------------------------------------
        new public DrawMode DrawMode { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the color of the highlight. </summary>
        /// <value> The color of the highlight. </value>
        ///--------------------------------------------------------------------------------------------------
        public Color HighlightColor { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public ComboBoxPersonalizado()
        {
            base.DrawMode = DrawMode.OwnerDrawFixed;
            this.HighlightColor = ColorTranslator.FromHtml("#8CC63E");
            this.DrawItem += new DrawItemEventHandler(ComboBoxPersonalizado_DrawItem);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by ComboBoxPersonalizado for draw item events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="evento"> Draw item event information. </param>
        ///--------------------------------------------------------------------------------------------------
        void ComboBoxPersonalizado_DrawItem(object sender, DrawItemEventArgs evento)
        {
            if (evento.Index < 0)
                return;

            ComboBox combo = sender as ComboBox;
            if ((evento.State & DrawItemState.Selected) == DrawItemState.Selected)
                evento.Graphics.FillRectangle(new SolidBrush(HighlightColor),evento.Bounds);
            else
                evento.Graphics.FillRectangle(new SolidBrush(combo.BackColor),evento.Bounds);

            evento.Graphics.DrawString(combo.Items[evento.Index].ToString(), evento.Font,
                                  new SolidBrush(combo.ForeColor),
                                  new Point(evento.Bounds.X, evento.Bounds.Y));

            evento.DrawFocusRectangle();
        }
    }
}
