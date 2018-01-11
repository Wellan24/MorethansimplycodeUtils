using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Cartif.Util
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> -----------------------------------------------------------------
    ///             Namespace:      Cartif.Util Class:          Stack Description:    Provides a stack
    ///             with a slightly different behavior than the standard Author:         Oscar - Cartif
    ///             Date: 14-09-2015 Notes: Revision History: Name:           Date:        Description:
    ///           -----------------------------------------------------------------. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    /// <typeparam name="TItem"> The type of the item. </typeparam>
    ///------------------------------------------------------------------------------------------------------
    [DebuggerDisplay("{Item}, {ActualStack.Count, nq} levels")]
    public class Stack<TItem>
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the actual stack. </summary>
        /// <value> The actual stack. </value>
        ///--------------------------------------------------------------------------------------------------
        private Stack<Stack<TItem>> ActualStack { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the parent node of the current node. </summary>
        /// <value> The parent. </value>
        ///--------------------------------------------------------------------------------------------------
        public Stack<TItem> ParentNode { get; private set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the item in the current node. </summary>
        /// <value> The item. </value>
        ///--------------------------------------------------------------------------------------------------
        public TItem Item { get; private set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets a value indicating whether this instance is root. </summary>
        /// <value> <c>true</c> if this instance is root; otherwise, <c>false</c>. </value>
        ///--------------------------------------------------------------------------------------------------
        private bool HasItem { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the <see cref="Stack&lt;TItem&gt;"/> class. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public Stack()
        {
            this.ActualStack = new Stack<Stack<TItem>>();
            this.HasItem = false;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Initializes a new instance of the <see cref="Stack&lt;TItem&gt;"/> class. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="item"> The item. </param>
        ///--------------------------------------------------------------------------------------------------
        private Stack(TItem item)
        {
            this.Item = item;
            this.HasItem = true;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets the element at the bottom of the stack. </summary>
        /// <value> The top. </value>
        ///--------------------------------------------------------------------------------------------------
        public Stack<TItem> Root
        {
            get
            {
                Stack<TItem> potentialRoot = this;
                while (potentialRoot.ParentNode != null)
                {
                    potentialRoot = potentialRoot.ParentNode;
                }

                return potentialRoot;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Pushes an item on the stack and sets the current node as the new node. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="item"> The item. </param>
        ///--------------------------------------------------------------------------------------------------
        public void Push(TItem item)
        {
            Stack<TItem> newParent = null;

            // Set the parent of the new element to the current element (if the current element exists)
            if (this.HasItem)
            {
                newParent = new Stack<TItem>(this.Item);
                newParent.ParentNode = this.ParentNode;
                this.ActualStack.Push(newParent);
            }
            else
            {
                this.HasItem = true;
            }

            // Replace the content of the current element (which is always the last level) with the new content
            this.Item = item;

            // Set the parent of the current element with the new parent
            this.ParentNode = newParent;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Removes the element at the top of the stack and returns its content. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <returns> The content of the popped element. </returns>
        ///--------------------------------------------------------------------------------------------------
        public TItem Pop()
        {
            TItem currentItem = this.Item;

            // Pop the parent of the current element (it will become the new current element)
            Stack<TItem> secondToLastElement = this.ActualStack.Pop();

            this.Item = secondToLastElement.Item;
            this.ParentNode = secondToLastElement.ParentNode;

            return currentItem;
        }
    }
}
