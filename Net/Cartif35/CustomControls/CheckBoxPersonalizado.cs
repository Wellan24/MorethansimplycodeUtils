using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Cartif.CustomControls
{
    //[sourcecode language="csharp"]

    ///------------------------------------------------------------------------------------------------------
    /// <summary> A custom check box. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class CustomCheckBox : CheckBox
    {
        private CustomCheckBoxUIAppearance _customAppearance;   /* The custom appearance */
        private bool _enableCustomUIAppearance; /* true to enable, false to disable the custom user interface appearance */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether the custom user interface appearance is
        ///           enabled. </summary>
        /// <value> true if enable custom user interface appearance, false if not. </value>
        ///--------------------------------------------------------------------------------------------------
        public bool EnableCustomUIAppearance
        {
            get { return _enableCustomUIAppearance; }
            set
            {
                if (_enableCustomUIAppearance != value)
                {
                    _enableCustomUIAppearance = value;
                    if (AutoSize)
                    {
                        AutoSize = false;
                    }
                    Invalidate();
                }
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the custom appearance. </summary>
        /// <value> The custom appearance. </value>
        ///--------------------------------------------------------------------------------------------------
        public CustomCheckBoxUIAppearance CustomAppearance
        {
            get { return _customAppearance; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public CustomCheckBox()
        {
            _enableCustomUIAppearance = true;
            _customAppearance = new CustomCheckBoxUIAppearance(this);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento
        ///           <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="pevent"> Objeto <see cref="T:System.Windows.Forms.PaintEventArgs" /> que contiene los
        ///                       datos del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs pevent)
        {
            // Call base class’ OnPaint
            base.OnPaint(pevent);
            if (!_enableCustomUIAppearance)
            {
                return;
            }
            // padding of the standard CheckBox
            int offset = 2;
            // distance betwen CechBox area and included label
            int distance = 5;
            // tick’s part height placed above the border
            int tickOffset = 6;
            // both faces of check box square are 11 pixels
            int checkBoxWidth = 11;
            Graphics graphics = pevent.Graphics;
            graphics.Clear(BackColor);
            // get Text measure according to selected Font
            SizeF stringMeasure = graphics.MeasureString(Text, Font);
            // Set graphics object to paint nice using antialias.
            if (_customAppearance.EnableAntiAlias)
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            }
            // calculate offsets
            int leftOffset = offset + Padding.Left;
            int topOffset = (int)(ClientRectangle.Height - stringMeasure.Height) / 2;
            if (topOffset < 0)
            {
                topOffset = offset + Padding.Top;
            }
            else
            {
                topOffset += Padding.Top;
            }
            if (Checked)
            {
                // Fill CheckBox's rectangle
                graphics.FillRectangle(new SolidBrush(_customAppearance.CheckedBackColor), leftOffset, topOffset + tickOffset, checkBoxWidth, checkBoxWidth);
                // Draw Checkox's rectangle
                graphics.DrawRectangle(new Pen(_customAppearance.CheckedBorderColor, _customAppearance.BorderThicness), leftOffset, topOffset + tickOffset, checkBoxWidth, checkBoxWidth);
                // Draw tick
                Point[] points = new Point[]
{
new Point(leftOffset, topOffset + tickOffset + _customAppearance.TickThickness),
new Point(leftOffset + (int)(checkBoxWidth / 2), topOffset + (checkBoxWidth - _customAppearance.TickThickness) + tickOffset),
new Point(leftOffset + checkBoxWidth + 1, topOffset)
};
                graphics.DrawLines(new Pen(_customAppearance.TickColor, _customAppearance.TickThickness), points);
            }
            else
            {
                // Fill CheckBox's rectangle
                graphics.FillRectangle(new SolidBrush(_customAppearance.BackColor), leftOffset, topOffset + tickOffset, checkBoxWidth, checkBoxWidth);
                // Draw Checkox's rectangle
                graphics.DrawRectangle(new Pen(_customAppearance.BorderColor, _customAppearance.BorderThicness), leftOffset, topOffset + tickOffset, checkBoxWidth, checkBoxWidth);
            }
            graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new Point(leftOffset + checkBoxWidth + distance, topOffset + tickOffset));
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Genera el evento <see cref="E:System.Windows.Forms.Control.AutoSizeChanged" />. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="e"> Objeto <see cref="T:System.EventArgs" /> que contiene los datos del evento. </param>
        ///--------------------------------------------------------------------------------------------------
        protected override void OnAutoSizeChanged(EventArgs e)
        {
            if (_enableCustomUIAppearance)
            {
                if (AutoSize)
                {
                    AutoSize = false;
                }
            }
            base.OnAutoSizeChanged(e);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> [sourcecode language="csharp"]
        ///           [TypeConverter(typeof(CustomBaseUIAppearanceTypeConverter))]. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public class CustomBaseUIAppearance
        {
            private Control _owner; /* The owner */
            private Color _borderColor; /* The border color */
            private Color _backColor;   /* The back color */
            private int _borderThicness;    /* The border thicness */
            private bool _enableAntiAlias;  /* true to enable, false to disable the anti alias */

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the owner. </summary>
            /// <value> The owner. </value>
            ///----------------------------------------------------------------------------------------------
            protected Control Owner
            {
                get
                {
                    return _owner;
                }
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets or Sets control's border color. It can not be set to Color.Transparent. </summary>
            /// <value> The color of the border. </value>
            ///----------------------------------------------------------------------------------------------
            [Description("Gets or Sets control's border color. It can not be set to Color.Transparent."), Category("Custom Appearance"), DefaultValue(typeof(Color), "Black")]
            public Color BorderColor
            {
                get { return _borderColor; }
                set
                {
                    //if (value.Equals(Color.Transparent))
                    //{
                    //    throw new ArgumentOutOfRangeException("BorderColor", "Parameter cannot be set to Color.Transparent");
                    //}
                    if (!_borderColor.Equals(value))
                    {
                        _borderColor = value;
                        _owner.Invalidate();
                    }
                }
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets or Sets control's background color. It can not be set to Color.Transparent. </summary>
            /// <value> The color of the back. </value>
            ///----------------------------------------------------------------------------------------------
            [Description("Gets or Sets control's background color. It can not be set to Color.Transparent."), Category("Custom Appearance"), DefaultValue(typeof(Color), "White")]
            public Color BackColor
            {
                get { return _backColor; }
                set
                {
                    //if (value.Equals(Color.Transparent))
                    //{
                    //    throw new ArgumentOutOfRangeException("BackColor", "Parameter cannot be set to Color.Transparent");
                    //}
                    if (!_backColor.Equals(value))
                    {
                        _backColor = value;
                        _owner.Invalidate();
                    }
                }
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets or Sets control's border thickness. </summary>
            /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside the
            ///                                                required range. </exception>
            /// <value> The border thicness. </value>
            ///----------------------------------------------------------------------------------------------
            [Description("Gets or Sets control's border thickness."), Category("Custom Appearance"), DefaultValue(1)]
            public int BorderThicness
            {
                get { return _borderThicness; }
                set
                {
                    if (value < 1)
                    {
                        throw new ArgumentOutOfRangeException("BorderThicness", "Parameter must be set to a valid integer value hihger than 0.");
                    }
                    if (_borderThicness != value)
                    {
                        _borderThicness = value;
                        _owner.Invalidate();
                    }
                }
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> If true control's is drawn using antialiasing, otherwise antialiasing is not enabled. </summary>
            /// <value> true if enable anti alias, false if not. </value>
            ///----------------------------------------------------------------------------------------------
            [Description("If true control's is drawn using antialiasing, otherwise antialiasing is not enabled."), Category("Custom Appearance"), DefaultValue(true)]
            public bool EnableAntiAlias
            {
                get { return _enableAntiAlias; }
                set
                {
                    if (_enableAntiAlias != value)
                    {
                        _enableAntiAlias = value;
                        _owner.Invalidate();
                    }
                }
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Constructor. </summary>
            /// <remarks> Oscvic, 2016-01-18. </remarks>
            /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
            /// <param name="owner"> The owner. </param>
            ///----------------------------------------------------------------------------------------------
            public CustomBaseUIAppearance(Control owner)
            {
                if (owner == null)
                {
                    throw new ArgumentNullException("owner");
                }
                _owner = owner;
                _borderColor = Color.Black;
                _backColor = Color.White;
                _borderThicness = 1;
                _enableAntiAlias = true;
            }
        }
        //[/sourcecode]

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A custom check box user interface appearance. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public class CustomCheckBoxUIAppearance : CustomBaseUIAppearance
        {
            private Color _checkedBorderColor;  /* The checked border color */
            private Color _checkedBackColor;    /* The checked back color */
            private Color _tickColor;   /* The tick color */
            private int _tickThickness; /* The tick thickness */

            ///----------------------------------------------------------------------------------------------
            /// <summary> [Description("Gets or Sets check box's border color when checked. It can not be set to
            ///           Color.Transparent."), Category("Custom Appearance"), DefaultValue(typeof(Color),
            ///           "Black")]. </summary>
            /// <value> The color of the checked border. </value>
            ///----------------------------------------------------------------------------------------------
            public Color CheckedBorderColor
            {
                get { return _checkedBorderColor; }
                set
                {
                    //if (value.Equals(Color.Transparent))
                    //{
                    //    throw new ArgumentOutOfRangeException("CheckedBorderColor", "Parameter cannot be set to Color.Transparent");
                    //}
                    if (!_checkedBorderColor.Equals(value))
                    {
                        _checkedBorderColor = value;
                        Owner.Invalidate();
                    }
                }
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets or sets the color of the checked back. </summary>
            /// <value> The color of the checked back. </value>
            ///----------------------------------------------------------------------------------------------
            public Color CheckedBackColor
            {
                get { return _checkedBackColor; }
                set
                {
                    //if (value.Equals(Color.Transparent))
                    //{
                    //    throw new ArgumentOutOfRangeException("CheckedBackColor", "Parameter cannot be set to Color.Transparent");
                    //}
                    if (!_checkedBackColor.Equals(value))
                    {
                        _checkedBackColor = value;
                        Owner.Invalidate();
                    }
                }
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets or sets the color of the tick. </summary>
            /// <value> The color of the tick. </value>
            ///----------------------------------------------------------------------------------------------
            public Color TickColor
            {
                get { return _tickColor; }
                set
                {
                    //if (value.Equals(Color.Transparent))
                    //{
                    //    throw new ArgumentOutOfRangeException("TickColor", "Parameter cannot be set to Color.Transparent");
                    //}
                    if (!_tickColor.Equals(value))
                    {
                        _tickColor = value;
                        Owner.Invalidate();
                    }
                }
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets or sets the tick thickness. </summary>
            /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside the
            ///                                                required range. </exception>
            /// <value> The tick thickness. </value>
            ///----------------------------------------------------------------------------------------------
            public int TickThickness
            {
                get { return _tickThickness; }
                set
                {
                    if (value < 1)
                    {
                        throw new ArgumentOutOfRangeException("TickThickness", "Parameter must be set to a valid integer value hihger than 0.");
                    }
                    if (_tickThickness != value)
                    {
                        _tickThickness = value;
                        Owner.Invalidate();
                    }
                }
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Constructor. </summary>
            /// <remarks> Oscvic, 2016-01-18. </remarks>
            /// <param name="owner"> The owner. </param>
            ///----------------------------------------------------------------------------------------------
            public CustomCheckBoxUIAppearance(Control owner)
                : base(owner)
            {
                //_checkedBorderColor = Color.Black;
                //_checkedBackColor = Color.White;
                //_tickColor = Color.Black;
                //_tickThickness = 1;
                _checkedBorderColor = Color.Gray;
                _checkedBackColor = Color.White;
                _tickColor = SystemColors.MenuHighlight;
                _tickThickness = 3;
            }
        }


    }
}
