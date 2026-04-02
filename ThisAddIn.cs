c#
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Linq;
using System.Collections.Generic;

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
            var checkedEmails = new HashSet<string>();

            foreach (Outlook.Recipient recipient in mail.Recipients)
            {
                var address = DomainValidator.GetSmtpAddress(recipient);
                if (string.IsNullOrEmpty(address) || !checkedEmails.Add(address))
                    continue;

                if (DomainValidator.IsSafe(address) || DomainValidator.IsInContacts(address, Application))
                    continue;

                using (var form = new PromptForm(address))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
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