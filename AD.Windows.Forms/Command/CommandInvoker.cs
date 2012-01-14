namespace AD.Windows.Forms.Command
{
	/// <summary>
	/// Classe base per invocare un "Command" <see cref="Forms.Command.Command"/>.
	/// </summary>
	public abstract class CommandInvoker
	{
		protected Command Command;
		protected abstract void HandleEnableChangedEvent(object sender, CommandEnableChangedEventArgs e);

		protected CommandInvoker(Command command)
		{
			Command = command;
			Command.EnabledChanged += HandleEnableChangedEvent;
		}
	}
}
