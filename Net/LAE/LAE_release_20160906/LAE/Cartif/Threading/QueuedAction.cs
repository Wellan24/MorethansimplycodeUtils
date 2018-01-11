using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartif.Threading
{
    ///------------------------------------------------------------------------------------------------------
    /// <summary> A basic class to store actions for a QueuedThread. </summary>
    /// <remarks> Oscvic, 2016-01-04. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public class QueuedAction
    {
        private Action actionToDo;  /* The action to do */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the action to do. </summary>
        /// <value> The action to do. </value>
        ///--------------------------------------------------------------------------------------------------
        public Action ActionToDo { get { return actionToDo; } set { actionToDo = value; } }

        private Action<Exception> actionWhenException;  /* The action when exception */

        ///--------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the action when exception. </summary>
        /// <value> The action when exception. </value>
        ///--------------------------------------------------------------------------------------------------
        public Action<Exception> ActionWhenException { get { return actionWhenException; } set { actionWhenException = value; } }
    }
}
