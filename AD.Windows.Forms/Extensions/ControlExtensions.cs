using System;
using System.Windows.Forms;

namespace AD.Windows.Forms.Extensions
{
    public static class ControlExtensions
    {
        public static void ExecuteAsync<TControl>(this TControl control, Action<TControl> action) where TControl : Control
        {
            if (control.InvokeRequired)
                control.Invoke(action, control); 
            else      
                action(control);
        }
    }
}
