using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A cartif table color. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class CartifTableColor
    {
        private static readonly CartifTableColor instance = new CartifTableColor(); /* The instance */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the instance. </summary>
        /// <value> The instance. </value>
        ///--------------------------------------------------------------------------------------------------
        public static CartifTableColor INSTANCE { get { return instance; } }

        #region Toast

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the toast border. </summary>
        /// <value> The toast border. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual Color ToastBorder
        {
            get { return SystemColors.MenuHighlight; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the color of the toast back. </summary>
        /// <value> The color of the toast back. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual Color ToastBackColor
        {
            get { return Color.White; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the color of the toast foreground. </summary>
        /// <value> The color of the toast foreground. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual Color ToastForeColor
        {
            get { return Color.Black; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the toast check. </summary>
        /// <value> The toast check. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual Color ToastCheck
        {
            get { return SystemColors.HotTrack; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the toast check border. </summary>
        /// <value> The toast check border. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual Color ToastCheckBorder
        {
            get { return Color.Gray; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the color of the toast check back. </summary>
        /// <value> The color of the toast check back. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual Color ToastCheckBackColor
        {
            get { return Color.White; }
        }
        #endregion
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> A default error table color. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class DefaultErrorTableColor : CartifTableColor
    {
        private static readonly DefaultErrorTableColor instance = new DefaultErrorTableColor(); /* The instance */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the instance. </summary>
        /// <value> The instance. </value>
        ///--------------------------------------------------------------------------------------------------
        public new static CartifTableColor INSTANCE { get { return instance; } }

        #region Toast

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the toast border. </summary>
        /// <value> The toast border. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ToastBorder
        {
            get { return Color.Red; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the color of the toast back. </summary>
        /// <value> The color of the toast back. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ToastBackColor
        {
            get { return Color.White; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the color of the toast foreground. </summary>
        /// <value> The color of the toast foreground. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ToastForeColor
        {
            get { return Color.Black; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the toast check. </summary>
        /// <value> The toast check. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ToastCheck
        {
            get { return Color.Red; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the toast check border. </summary>
        /// <value> The toast check border. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ToastCheckBorder
        {
            get { return Color.Gray; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the color of the toast check back. </summary>
        /// <value> The color of the toast check back. </value>
        ///--------------------------------------------------------------------------------------------------
        public override Color ToastCheckBackColor
        {
            get { return Color.White; }
        }
        #endregion
    }
}