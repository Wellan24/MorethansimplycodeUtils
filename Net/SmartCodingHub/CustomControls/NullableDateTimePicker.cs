using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A date time picker. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class DateTimePicker : System.Windows.Forms.DateTimePicker
    {
        private DateTimePickerFormat oldFormat = DateTimePickerFormat.Long; /* The old format */
        private string oldCustomFormat = null;  /* The old custom format */
        private bool bIsNull = false;   /* true if this DateTimePicker is null */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public DateTimePicker()
            : base()
        {
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the Date/Time of the value. </summary>
        /// <value> The value. </value>
        ///--------------------------------------------------------------------------------------------------
        public new DateTime Value
        {
            get
            {
                if (bIsNull)
                    return DateTime.MinValue;
                else
                    return base.Value;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    if (bIsNull == false)
                    {
                        oldFormat = this.Format;
                        oldCustomFormat = this.CustomFormat;
                        bIsNull = true;
                    }

                    this.Format = DateTimePickerFormat.Custom;
                    this.CustomFormat = " ";
                }
                else
                {
                    if (bIsNull)
                    {
                        this.Format = oldFormat;
                        this.CustomFormat = oldCustomFormat;
                        bIsNull = false;
                    }
                    base.Value = value;
                }
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.DateTimePicker.CloseUp" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="eventargs"> Objeto <see cref="T:System.EventArgs" /> que contiene los datos del
        ///                          evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnCloseUp(EventArgs eventargs)
        {
            if (Control.MouseButtons == MouseButtons.None)
            {
                if (bIsNull)
                {
                    this.Format = oldFormat;
                    this.CustomFormat = oldCustomFormat;
                    bIsNull = false;
                }
            }
            base.OnCloseUp(eventargs);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.Control.KeyDown" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.Windows.Forms.KeyEventArgs" /> que contiene los datos
        ///                  del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Delete)
                this.Value = DateTime.MinValue;
        }
    }
}
