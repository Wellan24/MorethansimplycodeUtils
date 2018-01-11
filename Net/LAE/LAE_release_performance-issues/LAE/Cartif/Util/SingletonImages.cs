using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Util
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Provides the unique instance of the images included. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class SingletonImages : IDisposable
    {
        public static readonly SingletonImages ARROW_UP = new SingletonImages("./images/ui/BlackSquareArrowUp.png");    /* The arrow up */
        public static readonly SingletonImages ARROW_DOWN = new SingletonImages("./images/ui/BlackSquareArrowDown.png");    /* The arrow down */

        public static readonly SingletonImages MINIMIZEFORM = new SingletonImages("./images/ui/minimize32.png");    /* The minimizeform */
        public static readonly SingletonImages MAXIMIZEFORM = new SingletonImages("./images/ui/maximize32.png");    /* The maximizeform */
        public static readonly SingletonImages RESTOREFORM = new SingletonImages("./images/ui/restore32.png");  /* The restoreform */
        public static readonly SingletonImages CLOSEFORM = new SingletonImages("./images/ui/close.png");    /* The closeform */

        String path;    /* Full pathname of the file */
        private Bitmap image;   /* The image */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the image. </summary>
        /// <value> The image. </value>
        ///--------------------------------------------------------------------------------------------------
        public Bitmap Image
        {
            get { return image; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="image"> The image. </param>
        ///--------------------------------------------------------------------------------------------------
        public SingletonImages(String image)
        {
            this.image = new Bitmap(image);
            path = image;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Finaliser. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        ~SingletonImages() { Dispose(); }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Convert this SingletonImages into a string representation. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <returns> Cadena que representa el objeto actual. </returns>
        ///--------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return path;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting
        ///           unmanaged resources. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void Dispose()
        {
            this.image.Dispose();
            this.path = null;
            GC.SuppressFinalize(this);
        }
    }
}
