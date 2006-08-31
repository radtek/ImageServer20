using System;
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.Desktop.Actions
{
    /// <summary>
    /// Abstract base class for the set of attributes that are used to declare an action.
    /// </summary>
    public abstract class ActionInitiatorAttribute : ActionAttribute
    {
        public ActionInitiatorAttribute(string actionID)
            : base(actionID)
        {
        }
    }
}
