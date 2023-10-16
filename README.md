# IVSoftware.Winforms.HotHeyManagement

One way to make some hot key shortcuts is to implement `IMessageFilter` which installs the `PreFilterMessage` hook for the entire form regardless of which control has focus.

[![Hot key demo][1]][1]

___
In the constructor, add the message filter and set it to remove when the form disposes.

```
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
    ...
}
```

___

In the method, screen for the Win32 key down message.

```
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
```

___

Then implement a custom hot key detector for the shortcuts that are of interest to you.

```    

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
```

  [1]: https://i.stack.imgur.com/3FmRb.png