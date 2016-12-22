namespace AD.Windows.Forms.Command
{
    using System;

    /// <summary>
    /// Gestione dello stato di abilitazione/disabilitazione di un comando.
    /// </summary>
    public class CommandEnableChangedEventArgs : EventArgs
    {
        private readonly bool _enabled;
        
        public CommandEnableChangedEventArgs(bool enabled)
        {
            _enabled = enabled;
        }

        public bool Enabled
        {
            get { return _enabled; }
        }
    }
}