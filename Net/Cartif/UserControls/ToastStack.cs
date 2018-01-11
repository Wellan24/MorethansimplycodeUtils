using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cartif.Extensions;

namespace Cartif.CustomControls
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> Stack of toasts. </summary>
    /// <remarks> Oscvic, 2016-01-04. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class ToastStack
    {
        private Dictionary<Form, Toast[]> toastShownByForm; /* The toast shown by form */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public ToastStack()
        {
            toastShownByForm = new Dictionary<Form, Toast[]>(2);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="capacity"> The capacity. </param>
        ///--------------------------------------------------------------------------------------------------
        public ToastStack(int capacity)
        {
            toastShownByForm = new Dictionary<Form, Toast[]>(capacity);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds a toast. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="toastToAdd"> The toast to add. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        public Boolean AddToast(Toast toastToAdd)
        {
            try
            {
                Toast[] toastShown;
                if (toastShownByForm.TryGetValue(toastToAdd.FormParent, out toastShown))
                {
                    toastShown = toastShown.InsertInEmptySpace(toastToAdd);
                    toastShownByForm[toastToAdd.FormParent] = toastShown;
                    return true;
                }
                else
                {
                    toastShown = new Toast[2];
                    toastShown[0] = toastToAdd;
                    toastShownByForm[toastToAdd.FormParent] = toastShown;
                    toastToAdd.FormParent.Disposed += FormParent_Disposed;

                    return true;
                }
            }
            catch (Exception) { return false; }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by FormParent for disposed events. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void FormParent_Disposed(object sender, EventArgs e)
        {
            Toast[] toastShown;
            Form parent = sender as Form;
            if (parent != null && toastShownByForm.TryGetValue(parent, out toastShown))
            {
                int length = toastShown.Length;
                for (int i = 0; i < length; i++)
                {
                    if (toastShown[i] != null && !toastShown[i].Disposing && !toastShown[i].IsDisposed)
                    {
                        toastShown[i].Close();
                        toastShown[i].Dispose();
                    }

                    toastShown[i] = null;
                }

                toastShownByForm.Remove(parent);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Determine if exist and reshow. </summary>
        /// <remarks> Oscvic, 2016-01-08. </remarks>
        /// <param name="parent">  The parent. </param>
        /// <param name="message"> The message. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        public Boolean CheckIfExistAndReshow(Form parent, String message)
        {
            try
            {
                Toast[] toastsShown;
                if (toastShownByForm.TryGetValue(parent, out toastsShown))
                {
                    int length = toastsShown.Length;

                    for (int i = 0; i < length; i++)
                    {
                        /* If the message is already shown, reset the opacity */
                        if (toastsShown[i] != null && toastsShown[i].Text.Equals(message))
                        {
                            toastsShown[i].ReshowToast();
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            { /* Return false */ }

            return false;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Removes the toast described by toastToRemove. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="toastToRemove"> The toast to remove. </param>
        /// <returns> true if it succeeds, false if it fails. </returns>
        ///--------------------------------------------------------------------------------------------------
        public Boolean RemoveToast(Toast toastToRemove)
        {
            try
            {
                Toast[] toastShown;
                if (toastToRemove != null && toastShownByForm.TryGetValue(toastToRemove.FormParent, out toastShown))
                {
                    int length = toastShown.Length;

                    for (int i = 0; i < length; i++)
                    {
                        if (toastShown[i] != null && toastToRemove.Equals(toastShown[i]))
                        {
                            toastShown[i] = null;
                            return true;
                        }
                    }

                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Take previous toast. </summary>
        /// <remarks> Oscvic, 2016-01-04. </remarks>
        /// <param name="toast"> The toast. </param>
        /// <returns> A Toast. </returns>
        ///--------------------------------------------------------------------------------------------------
        public Toast TakePreviousToast(Toast toast)
        {
            try
            {
                Toast[] toastShown;
                if (toast != null && toastShownByForm.TryGetValue(toast.FormParent, out toastShown))
                {
                    Toast previous = null;
                    int length = toastShown.Length;

                    for (int i = 0; i < length; i++)
                    {
                        if (toastShown[i] != null)
                        {
                            if (toastShown[i].Equals(toast))
                                return previous;
                            else
                                previous = toastShown[i];
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
