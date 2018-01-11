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
    /// <summary> A tab. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public partial class Tab : UserControl
    {
        private TabItem selectedItem;   /* The selected item */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the height of the item. </summary>
        /// <value> The height of the item. </value>
        ///--------------------------------------------------------------------------------------------------
        public int ItemHeight { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the width of the item. </summary>
        /// <value> The width of the item. </value>
        ///--------------------------------------------------------------------------------------------------
        public int ItemWidth { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the Date/Time of the time. </summary>
        /// <value> The time. </value>
        ///--------------------------------------------------------------------------------------------------
        public DateTime Time { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Delegado para el click en TabItem. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="selected"> El Item seleccionado. </param>
        ///--------------------------------------------------------------------------------------------------
        public delegate void OnTabItemClick(int selected);

        [Category("ClickCustomEvents")]
        [Description("Fired when clicked on an item")]
        public event OnTabItemClick onTabItemClick; /* Event queue for all listeners interested in onTabItemClick events. */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Executes the tab item click handler action. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="selected"> The selected. </param>
        ///--------------------------------------------------------------------------------------------------
        protected virtual void OnTabItemClickHandler(int selected)
        {
            if (onTabItemClick != null)
                onTabItemClick(selected);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the selected item. </summary>
        /// <value> The selected item. </value>
        ///--------------------------------------------------------------------------------------------------
        public TabItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value != null)
                {
                    selectedItem = value;
                    selectedIndex = panel.Controls.GetChildIndex(value, false);
                    ChangeSelection(selectedIndex);
                }

            }
        }

        private int selectedIndex;  /* The selected index */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the selected index. </summary>
        /// <value> The selected index. </value>
        ///--------------------------------------------------------------------------------------------------
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if (selectedIndex >= 0 && selectedIndex <= panel.Controls.Count && panel.Controls.Count != 0)
                    ChangeSelection(selectedIndex);
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Default constructor. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public Tab()
        {
            InitializeComponent();
            panel.Height = this.Height;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Adds a tab item. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="item"> The item. </param>
        ///--------------------------------------------------------------------------------------------------
        public void AddTabItem(TabItem item)
        {
            item.SetWidth(ItemWidth);
            item.SetHeight(ItemHeight);
            item.OnInnerTabItemClickEvent += item_OnInnerTabItemClickEvent;
            panel.Width += ItemWidth;
            panel.Width += 6;
            panel.Controls.Add(item);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Item on inner tab item click event. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="selected"> The selected. </param>
        ///--------------------------------------------------------------------------------------------------
        void item_OnInnerTabItemClickEvent(TabItem selected)
        {
            //EnableTabItems(false);

            selectedIndex = panel.Controls.GetChildIndex(selected, false);
            ChangeSelection(selectedIndex);

            //EnableTabItems(true);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Enables the tab items. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="enabled"> true to enable, false to disable. </param>
        ///--------------------------------------------------------------------------------------------------
        private void EnableTabItems(Boolean enabled)
        {
            TabItem[] Control = Controls.OfType<TabItem>().ToArray();
            foreach (TabItem item in Control)
            {
                item.Enabled = enabled;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets tab item. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="position"> The position. </param>
        /// <returns> The tab item. </returns>
        ///--------------------------------------------------------------------------------------------------
        public TabItem GetTabItem(int position)
        {
            if (panel.Controls.Count != 0)
                return (TabItem)panel.Controls[position];
            else
                return null;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets value at. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="position"> The position. </param>
        /// <returns> The value at. </returns>
        ///--------------------------------------------------------------------------------------------------
        public Object GetValueAt(int position)
        {
            if (selectedIndex >= 0 && selectedIndex <= panel.Controls.Count && panel.Controls.Count != 0)
                return ((TabItem)panel.Controls[position]).Value;
            else
                return null;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by tLeft for tick events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void tLeft_Tick(object sender, EventArgs e)
        {
            if (panel.Left < 0)
            {
                panel.Left = panel.Left + 5;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by tRight for tick events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void tRight_Tick(object sender, EventArgs e)
        {
            if (panel.Width >= Math.Abs(panel.Left) + container.Width)
            {
                panel.Left = panel.Left - 5;
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by bRight for mouse up events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Mouse event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void bRight_MouseUp(object sender, MouseEventArgs e)
        {
            tRight.Stop();
            if ((DateTime.Now - Time).TotalMilliseconds < 100)
            {
                click_bRight();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by bRight for mouse down events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Mouse event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void bRight_MouseDown(object sender, MouseEventArgs e)
        {
            tRight.Start();
            Time = DateTime.Now;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by bLeft for mouse up events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Mouse event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void bLeft_MouseUp(object sender, MouseEventArgs e)
        {
            tLeft.Stop();
            if ((DateTime.Now - Time).TotalMilliseconds < 100)
                click_bLeft();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by bLeft for mouse down events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Mouse event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void bLeft_MouseDown(object sender, MouseEventArgs e)
        {
            tLeft.Start();
            Time = DateTime.Now;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Call listener. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void CallListener()
        {
            OnTabItemClickHandler(selectedIndex);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Click b right. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void click_bRight()
        {
            ButtonsEnabled(false);

            if (selectedIndex < panel.Controls.Count - 1)
                selectedIndex++;

            ChangeSelection(selectedIndex);

            ButtonsEnabled(true);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Click b left. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        private void click_bLeft()
        {
            ButtonsEnabled(false);

            if (selectedIndex > 0)
                selectedIndex--;

            ChangeSelection(selectedIndex);

            ButtonsEnabled(true);
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Buttons enabled. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="isEnabled"> true if this Tab is enabled. </param>
        ///--------------------------------------------------------------------------------------------------
        private void ButtonsEnabled(Boolean isEnabled)
        {
            bLeft.Enabled = isEnabled;
            bRight.Enabled = isEnabled;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Change selection. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="newIndex"> Zero-based index of the new. </param>
        ///--------------------------------------------------------------------------------------------------
        private void ChangeSelection(int newIndex)
        {
            if (selectedIndex >= 0 && selectedIndex <= panel.Controls.Count || panel.Controls.Count != 0)
            {
                if (selectedItem != null)
                    selectedItem.Deseleccionar();

                selectedItem = GetTabItem(selectedIndex);
                if (selectedItem != null)
                {
                    selectedItem.Seleccionar();
                    if (selectedItem.Left + selectedItem.Width > container.Width)
                        panel.Left = -selectedIndex * (ItemWidth + 6) - 2;
                }
                timer.Stop();
                timer.Start();
            }
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Sets an activado. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="activo"> true to activo. </param>
        ///--------------------------------------------------------------------------------------------------
        public void SetActivado(bool activo)
        {
            bLeft.Enabled = activo;
            bRight.Enabled = activo;
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Clears the tab items. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        ///--------------------------------------------------------------------------------------------------
        public void ClearTabItems()
        {
            panel.Controls.Clear();
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Event handler. Called by timer for tick events. </summary>
        /// <remarks> Oscvic, 2016-01-18. </remarks>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        ///--------------------------------------------------------------------------------------------------
        private void timer_Tick(object sender, EventArgs e)
        {
            CallListener();
            timer.Stop();
        }
    }

    ///------------------------------------------------------------------------------------------------------
    /// <summary> A context menu action. </summary>
    /// <remarks> Oscvic, 2016-01-18. </remarks>
    ///------------------------------------------------------------------------------------------------------
    class ContextMenuAction
    {
        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the text. </summary>
        /// <value> The text. </value>
        ///--------------------------------------------------------------------------------------------------
        public String Text { get; set; }
    }
}
