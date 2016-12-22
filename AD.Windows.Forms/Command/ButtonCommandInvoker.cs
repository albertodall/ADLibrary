namespace AD.Windows.Forms.Command
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Invoca un "Command" da un controllo "Button".
    /// </summary>
    public class ButtonCommandInvoker : CommandInvoker
    {
        private readonly Button _button;

        public ButtonCommandInvoker(Button button, Command command)
            : base(command)
        {
            _button = button;
            _button.Click += HandleUIEvent;
        }

        protected override void HandleEnableChangedEvent(object sender, CommandEnableChangedEventArgs e)
        {
            _button.Enabled = e.Enabled;
        }

        private void HandleUIEvent(object sender, EventArgs e)
        {
            Command.Execute();
        }

        /// <summary>
        /// Associa il comando all'oggetto grafico.
        /// </summary>
        /// <param name="button">Pulsante al quale associare il comando.</param>
        /// <param name="command">Comando da associare.</param>
        public static void AttachCommand(Button button, Command command)
        {
            new ButtonCommandInvoker(button, command);
        }
    }
}