using System.Windows.Forms;
using System.Drawing;

public class PromptForm : Form
{
    public PromptForm(string email)
    {
        Text = "Ostrzeżenie";
        Width = 400;
        Height = 180;

        var label = new Label()
        {
            Text = email.Length > 30 ? $"Wysyłasz do zewnętrznego adresu:\n{email.Substring(0, 27)}...\nCzy kontynuować?" : $"Wysyłasz do zewnętrznego adresu:\n{email}\nCzy kontynuować?",
            Dock = DockStyle.Top,
            Height = 80,
            Padding = new Padding(10)
        };

        var btnOk = new Button()
        {
            Text = "Wyślij",
            DialogResult = DialogResult.OK,
            Width = 100,
            Height = 30,
            Top = 95,
            Left = 80
        };

        var btnCancel = new Button()
        {
            Text = "Anuluj",
            DialogResult = DialogResult.Cancel,
            Width = 100,
            Height = 30,
            Top = 95,
            Left = 200
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 2,
            Padding = new Padding(10)
        };
        layout.Controls.Add(label, 0, 0);
        layout.SetColumnSpan(label, 2);
        layout.Controls.Add(btnOk, 0, 1);
        layout.Controls.Add(btnCancel, 1, 1);
        Controls.Add(layout);

        AcceptButton = btnOk;
        CancelButton = btnCancel;
    }
}