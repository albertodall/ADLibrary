using System.Windows.Forms;
using AD.Windows.Forms.Command;

namespace AD.Windows.Forms.Controls
{
    public partial class TestCommand : Form
    {
        private readonly Command.Command _clickCommand;

        public TestCommand()
        {
            InitializeComponent();

            _clickCommand = new Command.Command(Click_Action);
            ButtonCommandInvoker.AttachCommand(button1, _clickCommand);
        }

        private void Click_Action()
        {
            MessageBox.Show(@"Clicked!");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {

        }
    }
}
