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
        var checkedDomains = new HashSet<string>(); // To store unique domains
        var unverifiedDomains = new HashSet<string>(); // To accumulate unverified domains

        // Iterate through all recipients
        foreach (Outlook.Recipient recipient in mail.Recipients)
        {
            // Get the SMTP address of the recipient
            var address = DomainValidator.GetSmtpAddress(recipient);
            if (string.IsNullOrEmpty(address))
                continue;

            // Extract the domain from the email address
            var domain = address.Split('@').Last();
            if (!checkedDomains.Add(domain)) // Skip if domain is already processed
                continue;

            // Check if the domain is verified (safe or present in contacts)
            if (DomainValidator.IsSafe(domain) || DomainValidator.IsInContacts(address, Application))
                continue;

            // Add unverified domain
            unverifiedDomains.Add(domain);
        }

        // If there are unverified domains, show a single prompt
        if (unverifiedDomains.Any())
        {
            using (var form = new PromptForm(unverifiedDomains.ToList()))
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    Cancel = true; // Cancel sending the email if user cancels
                    return;
                }
            }
        }
    }
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