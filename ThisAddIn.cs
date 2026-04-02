using Outlook = Microsoft.Office.Interop.Outlook;
using System.Linq;

public partial class ThisAddIn
{
    private void ThisAddIn_Startup(object sender, System.EventArgs e)
    {
        this.Application.ItemSend += Application_ItemSend;
    }

    private void Application_ItemSend(object Item, ref bool Cancel)
    {
        if (Item is Outlook.MailItem mail)
        {
            var recipients = mail.Recipients;
            foreach (Outlook.Recipient recipient in recipients)
            {
                var address = DomainValidator.GetSmtpAddress(recipient);

                if (string.IsNullOrEmpty(address))
                    continue;

                if (DomainValidator.IsSafe(address))
                    continue;

                if (DomainValidator.IsInContacts(address, Application))
                    continue;

                // 🔴 Soft-block popup
                using (var form = new PromptForm(address))
                {
                    var result = form.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        Cancel = true;
                        return;
                    }
                }
            }
        }
    }

    private void ThisAddIn_Shutdown(object sender, System.EventArgs e) { }

    #region VSTO generated code
    private void InternalStartup()
    {
        this.Startup += ThisAddIn_Startup;
        this.Shutdown += ThisAddIn_Shutdown;
    }
    #endregion
}
