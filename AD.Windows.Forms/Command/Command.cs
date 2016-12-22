namespace AD.Windows.Forms.Command
{
    using System;

    /// <summary>
    /// Classe che gestisce un generico comando dell'interfaccia.
    /// </summary>
    public class Command
    {
        private readonly Action _action;
        private bool _enabled = true;

        public delegate void EnableChangedEventHandler(object sender, CommandEnableChangedEventArgs e);

        public virtual event EnableChangedEventHandler EnabledChanged;

        public Command(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action();
        }

        public bool Enabled
        {
            get { return _enabled; }
            set 
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    OnEnableChanged(new CommandEnableChangedEventArgs(_enabled));	
                }
            }
        }

        private void OnEnableChanged(CommandEnableChangedEventArgs e)
        {
            EnabledChanged?.Invoke(this, e);
        }
    }
}
