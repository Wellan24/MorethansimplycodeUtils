using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Cartif.Extensions
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Some Extensions for Graphics class, to support rounded rectangles. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    static class GraphicsExtensions
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that generates a rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <param name="radius">    The radius. </param>
        /// <param name="filter">    Specifies the filter. </param>
        /// <returns> The rounded rectangle. </returns>
        ///--------------------------------------------------------------------------------------------------
        private static GraphicsPath GenerateRoundedRectangle(
                this Graphics graphics,
                RectangleF rectangle,
                float radius,
                RectangleEdgeFilter filter)
        {
            float diameter;
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0.0F || filter == RectangleEdgeFilter.None)
            {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }
            else
            {
                if (radius >= (Math.Min(rectangle.Width, rectangle.Height)) / 2.0)
                    return graphics.GenerateCapsule(rectangle);
                diameter = radius * 2.0F;
                SizeF sizeF = new SizeF(diameter, diameter);
                RectangleF arc = new RectangleF(rectangle.Location, sizeF);
                if ((RectangleEdgeFilter.TopLeft & filter) == RectangleEdgeFilter.TopLeft)
                    path.AddArc(arc, 180, 90);
                else
                {
                    path.AddLine(arc.X, arc.Y + arc.Height, arc.X, arc.Y);
                    path.AddLine(arc.X, arc.Y, arc.X + arc.Width, arc.Y);
                }
                arc.X = rectangle.Right - diameter;
                if ((RectangleEdgeFilter.TopRight & filter) == RectangleEdgeFilter.TopRight)
                    path.AddArc(arc, 270, 90);
                else
                {
                    path.AddLine(arc.X, arc.Y, arc.X + arc.Width, arc.Y);
                    path.AddLine(arc.X + arc.Width, arc.Y + arc.Height, arc.X + arc.Width, arc.Y);
                }
                arc.Y = rectangle.Bottom - diameter;
                if ((RectangleEdgeFilter.BottomRight & filter) == RectangleEdgeFilter.BottomRight)
                    path.AddArc(arc, 0, 90);
                else
                {
                    path.AddLine(arc.X + arc.Width, arc.Y, arc.X + arc.Width, arc.Y + arc.Height);
                    path.AddLine(arc.X, arc.Y + arc.Height, arc.X + arc.Width, arc.Y + arc.Height);
                }
                arc.X = rectangle.Left;
                if ((RectangleEdgeFilter.BottomLeft & filter) == RectangleEdgeFilter.BottomLeft)
                    path.AddArc(arc, 90, 90);
                else
                {
                    path.AddLine(arc.X + arc.Width, arc.Y + arc.Height, arc.X, arc.Y + arc.Height);
                    path.AddLine(arc.X, arc.Y + arc.Height, arc.X, arc.Y);
                }
                path.CloseFigure();
            }
            return path;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that generates a capsule. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <returns> The capsule. </returns>
        ///--------------------------------------------------------------------------------------------------
        private static GraphicsPath GenerateCapsule(
                this Graphics graphics,
                RectangleF rectangle)
        {
            float diameter;
            RectangleF arc;
            GraphicsPath path = new GraphicsPath();
            try
            {
                if (rectangle.Width > rectangle.Height)
                {
                    diameter = rectangle.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rectangle.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = rectangle.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (rectangle.Width < rectangle.Height)
                {
                    diameter = rectangle.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(rectangle.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = rectangle.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else path.AddEllipse(rectangle);
            }
            catch { path.AddEllipse(rectangle); }
            finally { path.CloseFigure(); }
            return path;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that draw rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics"> The graphics to act on. </param>
        /// <param name="pen">      The pen. </param>
        /// <param name="x">        The x coordinate. </param>
        /// <param name="y">        The y coordinate. </param>
        /// <param name="width">    The width. </param>
        /// <param name="height">   The height. </param>
        /// <param name="radius">   The radius. </param>
        /// <param name="filter">   Specifies the filter. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void DrawRoundedRectangle(
                this Graphics graphics,
                Pen pen,
                float x,
                float y,
                float width,
                float height,
                float radius,
                RectangleEdgeFilter filter)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius, filter);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(pen, path);
            graphics.SmoothingMode = old;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that draw rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics"> The graphics to act on. </param>
        /// <param name="pen">      The pen. </param>
        /// <param name="x">        The x coordinate. </param>
        /// <param name="y">        The y coordinate. </param>
        /// <param name="width">    The width. </param>
        /// <param name="height">   The height. </param>
        /// <param name="radius">   The radius. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void DrawRoundedRectangle(
                this Graphics graphics,
                Pen pen,
                float x,
                float y,
                float width,
                float height,
                float radius)
        {
            graphics.DrawRoundedRectangle(
                    pen,
                    x,
                    y,
                    width,
                    height,
                    radius,
                    RectangleEdgeFilter.All);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that draw rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics"> The graphics to act on. </param>
        /// <param name="pen">      The pen. </param>
        /// <param name="x">        The x coordinate. </param>
        /// <param name="y">        The y coordinate. </param>
        /// <param name="width">    The width. </param>
        /// <param name="height">   The height. </param>
        /// <param name="radius">   The radius. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void DrawRoundedRectangle(
                this Graphics graphics,
                Pen pen,
                int x,
                int y,
                int width,
                int height,
                int radius)
        {
            graphics.DrawRoundedRectangle(
                    pen,
                    Convert.ToSingle(x),
                    Convert.ToSingle(y),
                    Convert.ToSingle(width),
                    Convert.ToSingle(height),
                    Convert.ToSingle(radius));
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that draw rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="pen">       The pen. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <param name="radius">    The radius. </param>
        /// <param name="filter">    Specifies the filter. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            Rectangle rectangle,
            int radius,
            RectangleEdgeFilter filter)
        {
            graphics.DrawRoundedRectangle(
                pen,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                filter);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that draw rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="pen">       The pen. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <param name="radius">    The radius. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            Rectangle rectangle,
            int radius)
        {
            graphics.DrawRoundedRectangle(
                pen,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                RectangleEdgeFilter.All);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that draw rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="pen">       The pen. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <param name="radius">    The radius. </param>
        /// <param name="filter">    Specifies the filter. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            RectangleF rectangle,
            int radius,
            RectangleEdgeFilter filter)
        {
            graphics.DrawRoundedRectangle(
                pen,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                filter);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that draw rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="pen">       The pen. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <param name="radius">    The radius. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            RectangleF rectangle,
            int radius)
        {
            graphics.DrawRoundedRectangle(
                pen,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                RectangleEdgeFilter.All);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that fill rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics"> The graphics to act on. </param>
        /// <param name="brush">    The brush. </param>
        /// <param name="x">        The x coordinate. </param>
        /// <param name="y">        The y coordinate. </param>
        /// <param name="width">    The width. </param>
        /// <param name="height">   The height. </param>
        /// <param name="radius">   The radius. </param>
        /// <param name="filter">   Specifies the filter. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void FillRoundedRectangle(
                this Graphics graphics,
                Brush brush,
                float x,
                float y,
                float width,
                float height,
                float radius,
                RectangleEdgeFilter filter)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius, filter);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(brush, path);
            graphics.SmoothingMode = old;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that fill rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics"> The graphics to act on. </param>
        /// <param name="brush">    The brush. </param>
        /// <param name="x">        The x coordinate. </param>
        /// <param name="y">        The y coordinate. </param>
        /// <param name="width">    The width. </param>
        /// <param name="height">   The height. </param>
        /// <param name="radius">   The radius. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void FillRoundedRectangle(
                this Graphics graphics,
                Brush brush,
                float x,
                float y,
                float width,
                float height,
                float radius)
        {
            graphics.FillRoundedRectangle(
                    brush,
                    x,
                    y,
                    width,
                    height,
                    radius,
                    RectangleEdgeFilter.All);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that fill rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics"> The graphics to act on. </param>
        /// <param name="brush">    The brush. </param>
        /// <param name="x">        The x coordinate. </param>
        /// <param name="y">        The y coordinate. </param>
        /// <param name="width">    The width. </param>
        /// <param name="height">   The height. </param>
        /// <param name="radius">   The radius. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void FillRoundedRectangle(
                this Graphics graphics,
                Brush brush,
                int x,
                int y,
                int width,
                int height,
                int radius)
        {
            graphics.FillRoundedRectangle(
                    brush,
                    Convert.ToSingle(x),
                    Convert.ToSingle(y),
                    Convert.ToSingle(width),
                    Convert.ToSingle(height),
                    Convert.ToSingle(radius));
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that fill rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="brush">     The brush. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <param name="radius">    The radius. </param>
        /// <param name="filter">    Specifies the filter. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            Rectangle rectangle,
            int radius,
            RectangleEdgeFilter filter)
        {
            graphics.FillRoundedRectangle(
                brush,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                filter);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that fill rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="brush">     The brush. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <param name="radius">    The radius. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            Rectangle rectangle,
            int radius)
        {
            graphics.FillRoundedRectangle(
                brush,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                RectangleEdgeFilter.All);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that fill rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="brush">     The brush. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <param name="radius">    The radius. </param>
        /// <param name="filter">    Specifies the filter. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            RectangleF rectangle,
            int radius,
            RectangleEdgeFilter filter)
        {
            graphics.FillRoundedRectangle(
                brush,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                filter);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that fill rounded rectangle. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics">  The graphics to act on. </param>
        /// <param name="brush">     The brush. </param>
        /// <param name="rectangle"> The rectangle. </param>
        /// <param name="radius">    The radius. </param>
        ///--------------------------------------------------------------------------------------------------
        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            RectangleF rectangle,
            int radius)
        {
            graphics.FillRoundedRectangle(
                brush,
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                radius,
                RectangleEdgeFilter.All);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> The Graphics extension method that gets font metrics. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="graphics"> The graphics to act on. </param>
        /// <param name="font">     The font. </param>
        /// <returns> The font metrics. </returns>
        ///--------------------------------------------------------------------------------------------------
        public static FontMetrics GetFontMetrics(
            this Graphics graphics,
            Font font)
        {
            return FontMetricsImpl.GetFontMetrics(graphics, font);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> A font metrics implementation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private class FontMetricsImpl : FontMetrics
        {
            ///----------------------------------------------------------------------------------------------
            /// <summary> A textmetric. </summary>
            /// <remarks> Oscvic, 2016-01-18. </remarks>
            ///----------------------------------------------------------------------------------------------
            [StructLayout(LayoutKind.Sequential)]
            public struct TEXTMETRIC
            {
                public int tmHeight;    /* Height of the time */
                public int tmAscent;    /* The time ascent */
                public int tmDescent;   /* The time descent */
                public int tmInternalLeading;   /* The time internal leading */
                public int tmExternalLeading;   /* The time external leading */
                public int tmAveCharWidth;  /* Width of the time ave character */
                public int tmMaxCharWidth;  /* Width of the time maximum character */
                public int tmWeight;    /* The time weight */
                public int tmOverhang;  /* The time overhang */
                public int tmDigitizedAspectX;  /* The time digitized aspect x coordinate */
                public int tmDigitizedAspectY;  /* The time digitized aspect y coordinate */
                public char tmFirstChar;    /* The time first character */
                public char tmLastChar; /* The time last character */
                public char tmDefaultChar;  /* The time default character */
                public char tmBreakChar;    /* The time break character */
                public byte tmItalic;   /* The time italic */
                public byte tmUnderlined;   /* The time underlined */
                public byte tmStruckOut;    /* The time struck out */
                public byte tmPitchAndFamily;   /* The time pitch and family */
                public byte tmCharSet;  /* Set the time character belongs to */
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Select object. </summary>
            /// <remarks> Oscvic, 2016-01-18. </remarks>
            /// <param name="hdc">     The hdc. </param>
            /// <param name="hgdiobj"> The hgdiobj. </param>
            /// <returns> An IntPtr. </returns>
            ///----------------------------------------------------------------------------------------------
            [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets text metrics. </summary>
            /// <remarks> Oscvic, 2016-01-18. </remarks>
            /// <param name="hdc">  The hdc. </param>
            /// <param name="lptm"> [out] The lptm. </param>
            /// <returns> true if it succeeds, false if it fails. </returns>
            ///----------------------------------------------------------------------------------------------
            [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
            public static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm);

            ///----------------------------------------------------------------------------------------------
            /// <summary> Deletes the object described by hdc. </summary>
            /// <remarks> Oscvic, 2016-01-18. </remarks>
            /// <param name="hdc"> The hdc. </param>
            /// <returns> true if it succeeds, false if it fails. </returns>
            ///----------------------------------------------------------------------------------------------
            [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
            public static extern bool DeleteObject(IntPtr hdc);

            ///----------------------------------------------------------------------------------------------
            /// <summary> Generates a text metrics. </summary>
            /// <remarks> Oscvic, 2016-01-18. </remarks>
            /// <param name="graphics"> The graphics. </param>
            /// <param name="font">     The font. </param>
            /// <returns> The text metrics. </returns>
            ///----------------------------------------------------------------------------------------------
            private TEXTMETRIC GenerateTextMetrics(
                Graphics graphics,
                Font font)
            {
                IntPtr hDC = IntPtr.Zero;
                TEXTMETRIC textMetric;
                IntPtr hFont = IntPtr.Zero;
                try
                {
                    hDC = graphics.GetHdc();
                    hFont = font.ToHfont();
                    IntPtr hFontDefault = SelectObject(hDC, hFont);
                    bool result = GetTextMetrics(hDC, out textMetric);
                    SelectObject(hDC, hFontDefault);
                }
                finally
                {
                    if (hFont != IntPtr.Zero) DeleteObject(hFont);
                    if (hDC != IntPtr.Zero) graphics.ReleaseHdc(hDC);
                }
                return textMetric;
            }
            private TEXTMETRIC metrics; /* The metrics */

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the height. </summary>
            /// <value> The height. </value>
            ///----------------------------------------------------------------------------------------------
            public override int Height { get { return this.metrics.tmHeight; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the ascent. </summary>
            /// <value> The ascent. </value>
            ///----------------------------------------------------------------------------------------------
            public override int Ascent { get { return this.metrics.tmAscent; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the descent. </summary>
            /// <value> The descent. </value>
            ///----------------------------------------------------------------------------------------------
            public override int Descent { get { return this.metrics.tmDescent; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the internal leading. </summary>
            /// <value> The internal leading. </value>
            ///----------------------------------------------------------------------------------------------
            public override int InternalLeading { get { return this.metrics.tmInternalLeading; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the external leading. </summary>
            /// <value> The external leading. </value>
            ///----------------------------------------------------------------------------------------------
            public override int ExternalLeading { get { return this.metrics.tmExternalLeading; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the width of the average character. </summary>
            /// <value> The width of the average character. </value>
            ///----------------------------------------------------------------------------------------------
            public override int AverageCharacterWidth { get { return this.metrics.tmAveCharWidth; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the width of the maximum character. </summary>
            /// <value> The width of the maximum character. </value>
            ///----------------------------------------------------------------------------------------------
            public override int MaximumCharacterWidth { get { return this.metrics.tmMaxCharWidth; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the weight. </summary>
            /// <value> The weight. </value>
            ///----------------------------------------------------------------------------------------------
            public override int Weight { get { return this.metrics.tmWeight; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the overhang. </summary>
            /// <value> The overhang. </value>
            ///----------------------------------------------------------------------------------------------
            public override int Overhang { get { return this.metrics.tmOverhang; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the digitized aspect x coordinate. </summary>
            /// <value> The digitized aspect x coordinate. </value>
            ///----------------------------------------------------------------------------------------------
            public override int DigitizedAspectX { get { return this.metrics.tmDigitizedAspectX; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets the digitized aspect y coordinate. </summary>
            /// <value> The digitized aspect y coordinate. </value>
            ///----------------------------------------------------------------------------------------------
            public override int DigitizedAspectY { get { return this.metrics.tmDigitizedAspectY; } }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Constructor. </summary>
            /// <remarks> Oscvic, 2016-01-18. </remarks>
            /// <param name="graphics"> The graphics. </param>
            /// <param name="font">     The font. </param>
            ///----------------------------------------------------------------------------------------------
            private FontMetricsImpl(Graphics graphics, Font font)
            {
                this.metrics = this.GenerateTextMetrics(graphics, font);
            }

            ///----------------------------------------------------------------------------------------------
            /// <summary> Gets font metrics. </summary>
            /// <remarks> Oscvic, 2016-01-18. </remarks>
            /// <param name="graphics"> The graphics. </param>
            /// <param name="font">     The font. </param>
            /// <returns> The font metrics. </returns>
            ///----------------------------------------------------------------------------------------------
            public static FontMetrics GetFontMetrics(
                Graphics graphics,
                Font font)
            {
                return new FontMetricsImpl(graphics, font);
            }
        }
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> Values that represent rectangle edge filters. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public enum RectangleEdgeFilter
    {
        None = 0,   /* An enum constant representing the none option */
        TopLeft = 1,    /* An enum constant representing the top left option */
        TopRight = 2,   /* An enum constant representing the top right option */
        BottomLeft = 4, /* An enum constant representing the bottom left option */
        BottomRight = 8,    /* An enum constant representing the bottom right option */
        All = TopLeft | TopRight | BottomLeft | BottomRight /* An enum constant representing all option */
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> A font metrics. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public abstract class FontMetrics
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the height. </summary>
        /// <value> The height. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int Height { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the ascent. </summary>
        /// <value> The ascent. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int Ascent { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the descent. </summary>
        /// <value> The descent. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int Descent { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the internal leading. </summary>
        /// <value> The internal leading. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int InternalLeading { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the external leading. </summary>
        /// <value> The external leading. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int ExternalLeading { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the width of the average character. </summary>
        /// <value> The width of the average character. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int AverageCharacterWidth { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the width of the maximum character. </summary>
        /// <value> The width of the maximum character. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int MaximumCharacterWidth { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the weight. </summary>
        /// <value> The weight. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int Weight { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the overhang. </summary>
        /// <value> The overhang. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int Overhang { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the digitized aspect x coordinate. </summary>
        /// <value> The digitized aspect x coordinate. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int DigitizedAspectX { get { return 0; } }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the digitized aspect y coordinate. </summary>
        /// <value> The digitized aspect y coordinate. </value>
        ///--------------------------------------------------------------------------------------------------
        public virtual int DigitizedAspectY { get { return 0; } }
    }
}
