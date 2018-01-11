using Cartif.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Windows.Forms;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Some Extensions for Controls, like DoubleBuffering or Styles. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public static class ControlExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> A Control extension method that sets double buffered. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="control">          The control to act on. </param>
        /// <param name="isDoubleBuffered"> true if this ControlExtensions is double buffered. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Boolean SetDoubleBuffered(this Control control, Boolean isDoubleBuffered)
        {
            PropertyInfo property = control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (property != null)
            {
                property.SetValue(control, isDoubleBuffered, null);
                return true;
            }

            return false;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A Control extension method that sets styles of control. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="control"> The control to act on. </param>
        /// <param name="styles">  The styles. </param>
        /// <param name="value">   true to value. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Boolean SetStylesOfControl(this Control control, ControlStyles styles, Boolean value)
        {
            MethodInfo method = control.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);

            if (method != null)
            {
                method.Invoke(control, new Object[] { styles, value });
                return true;
            }

            return false;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A Control extension method that executes the hover color action. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="control"> The control to act on. </param>
        /// <param name="onHover"> The on hover. </param>
        /// <param name="normal">  The normal. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void OnHoverColor(this Control control, Color onHover, Color normal)
        {
            control.MouseEnter += (sender, e) => control.BackColor = onHover;
            control.MouseLeave += (sender, e) => control.BackColor = normal;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A Control extension method that gets true back color. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="c"> The c to act on. </param>
        /// <returns> The true back color. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Color GetTrueBackColor(this Control c)
        {
            return (c.BackColor == Color.Transparent) ? ControlUtils.GetNonTransparentColorFromParent(c) : c.BackColor;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A Control extension method that gets the parent of this item. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="c"> The c to act on. </param>
        /// <returns> The non transparent color from parent. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static Color GetNonTransparentColorFromParent(this Control c)
        {
            return ControlUtils.GetNonTransparentColorFromParent(c);
        }
    }
}
