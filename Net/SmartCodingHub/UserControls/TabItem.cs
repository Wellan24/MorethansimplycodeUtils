using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;

namespace Cartif.UserControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A tab item. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public partial class TabItem : UserControl
    {
        private Color selectedColor = Color.Gray;   /* The selected color */
        private Color selectedFontColor = Color.White;  /* The selected font color */

        private Color deselectedColor = Color.White;    /* The deselected color */
        private Color deselectedFontColor = Color.Black;    /* The deselected font color */

        private Object value;   /* The value */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the value. </summary>
        /// <value> The value. </value>
        ///--------------------------------------------------------------------------------------------------
        public Object Value
        {
            get { return value; }
            set
            {
                this.value = value;
                RefreshText();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the pattern. </summary>
        /// <value> The pattern. </value>
        ///--------------------------------------------------------------------------------------------------
        public String Pattern { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Delegado para el click en TabItem. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="selected"> El Item seleccionado. </param>
        ///--------------------------------------------------------------------------------------------------
        public delegate void OnInnerTabItemClick(TabItem selected);

        [Category("ClickCustomEvents")]
        [Description("Fired when this is clicked")]
        public event OnInnerTabItemClick OnInnerTabItemClickEvent;  /* Event queue for all listeners interested in onInnerTabItemClick events. */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value"> The value. </param>
        ///--------------------------------------------------------------------------------------------------
        public TabItem(Object value)
        {
            InitializeComponent();
            this.value = value;
            RefreshText();
            button.Click += ItemClick;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="value">   The value. </param>
        /// <param name="pattern"> Specifies the pattern. </param>
        ///--------------------------------------------------------------------------------------------------
        public TabItem(Object value, String pattern)
        {
            InitializeComponent();
            Pattern = pattern;
            this.value = value;
            RefreshText();
            button.Click += ItemClick;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Executes this click handler action. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        protected virtual void OnThisClickHandler()
        {
            if (OnInnerTabItemClickEvent != null)
                OnInnerTabItemClickEvent(this);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Refresh text. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void RefreshText()
        {
            if (Pattern != null && value.GetType().Equals(typeof(DateTime)))
            {
                DateTime date = (DateTime)(value ?? DateTime.MinValue);
                button.Text = date.ToString(Pattern);
            }
            else
                button.Text = (value != null) ? value.ToString() : "";

        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Item click. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="args">   Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void ItemClick(object sender, EventArgs args)
        {
            OnThisClickHandler();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Seleccionars this TabItem. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void Seleccionar()
        {
            button.BackColor = selectedColor;
            button.ForeColor = selectedFontColor;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Deseleccionars this TabItem. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void Deseleccionar()
        {
            button.BackColor = deselectedColor;
            button.ForeColor = deselectedFontColor;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Sets a height. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="newHeight"> Height of the new. </param>
        ///--------------------------------------------------------------------------------------------------
        public void SetHeight(int newHeight)
        {
            this.Height = newHeight;
            button.Height = newHeight;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Sets a width. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="newWidth"> Width of the new. </param>
        ///--------------------------------------------------------------------------------------------------
        public void SetWidth(int newWidth)
        {
            this.Width = newWidth;
            button.Width = newWidth;
        }

    }
}
