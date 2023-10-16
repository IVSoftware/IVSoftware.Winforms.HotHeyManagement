using System.Windows.Forms;

namespace IVSoftware.Winforms.HotHeyManagement
{
    public partial class Form1 : Form, IMessageFilter
    {
        public Form1()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;

            // Hook the message filter here.
            Application.AddMessageFilter(this);

            // Unhook the message filter when this form disposes.
            Disposed += (sender, e) => Application.RemoveMessageFilter(this);

            button1.Click += (sender, e) => MessageBox.Show("Clicked or CTRL-M");
        }

        private const int WM_KEYDOWN = 0x0100;
        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_KEYDOWN:
                    var e = new KeyEventArgs(((Keys)m.WParam) | Control.ModifierKeys);
                    OnHotKeyPreview(FromHandle(m.HWnd), e);
                    return e.Handled;
                default:
                    break;
            }
            return false;
        }

        private void OnHotKeyPreview(Control? sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Control | Keys.M:
                    button1.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.Control | Keys.R:
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.AppendText($"{nameof(Color.Red)} {Environment.NewLine}");
                    e.Handled = true;
                    break;
                case Keys.Control | Keys.G:
                    richTextBox1.SelectionColor = Color.Green;
                    richTextBox1.AppendText($"{nameof(Color.Green)} {Environment.NewLine}");
                    e.Handled = true;
                    break;
                case Keys.Control | Keys.B:
                    richTextBox1.SelectionColor = Color.Blue;
                    richTextBox1.AppendText($"{nameof(Color.Blue)} {Environment.NewLine}");
                    e.Handled = true;
                    break;
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    richTextBox1.AppendText($"{e.KeyData} {Environment.NewLine}");
                    break;
                default:
                    break;
            }
        }
    }
}