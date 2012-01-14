using System;
using System.Windows.Forms;

namespace AD.Windows.Forms.Command
{
    /// <summary>
    /// Invoca un "Command" da un controllo ToolStrip (menù o toolbar)
    /// </summary>
    public class ToolStripItemCommandInvoker : CommandInvoker
    {
        private readonly ToolStripItem _toolStripItem;

        public ToolStripItemCommandInvoker(ToolStripItem toolStripItem, Command command) 
            : base(command)
        {
            _toolStripItem = toolStripItem;
            _toolStripItem.Click += HandleUIEvent;
        }

        protected override void HandleEnableChangedEvent(object sender, CommandEnableChangedEventArgs e)
        {
            _toolStripItem.Enabled = e.Enabled;
        }

        private void HandleUIEvent(object sender, EventArgs e)
        {
            Command.Execute();
        }

        /// <summary>
        /// Associa il comando all'oggetto grafico.
        /// </summary>
        /// <param name="toolStripItem">Oggetto ToolStrip al quale associare il comando.</param>
        /// <param name="command">Oggetto "Command" da associare.</param>
        public static void AttachCommand(ToolStripItem toolStripItem, Command command)
        {
            new ToolStripItemCommandInvoker(toolStripItem, command);
        }
    }
}