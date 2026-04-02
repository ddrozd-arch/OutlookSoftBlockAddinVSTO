using System.Windows.Forms;

public class PromptForm : Form
{
    public PromptForm(string email)
    {
        Text = "Ostrzeżenie";
        Width = 400;
        Height = 180;

        var label = new Label()
        {
            Text = $"Wysyłasz do zewnętrznego adresu:\n{email}\nCzy kontynuować?",
            Dock = DockStyle.Top,
            Height = 80
        };

        var btnOk = new Button()
        {
            Text = "Wyślij",
            DialogResult = DialogResult.OK,
            Left = 80,
            Top = 90
        };

        var btnCancel = new Button()
        {
            Text = "Anuluj",
            DialogResult = DialogResult.Cancel,
            Left = 200,
            Top = 90
        };

        Controls.Add(label);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);

        AcceptButton = btnOk;
        CancelButton = btnCancel;
    }
}
