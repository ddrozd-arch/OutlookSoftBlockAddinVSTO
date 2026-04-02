using Outlook = Microsoft.Office.Interop.Outlook;
using System.Linq;

public static class DomainValidator
{
    private static readonly string[] AllowedDomains = {
        "test.dd",
        "test1.dd",
        "test2.dd"
    };

    public static bool IsSafe(string email)
    {
        var domain = email.Split('@').Last().ToLower();
        return AllowedDomains.Contains(domain);
    }

    public static string GetSmtpAddress(Outlook.Recipient recipient)
    {
        try
        {
            var addressEntry = recipient.AddressEntry;

            if (addressEntry.Type == "EX")
            {
                var exchangeUser = addressEntry.GetExchangeUser();
                return exchangeUser?.PrimarySmtpAddress;
            }

            return recipient.Address;
        }
        catch
        {
            return null;
        }
    }

    public static bool IsInContacts(string email, Outlook.Application app)
    {
        try
        {
            Outlook.MAPIFolder contactsFolder =
                app.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);

            foreach (Outlook.ContactItem contact in contactsFolder.Items)
            {
                if (contact.Email1Address == email ||
                    contact.Email2Address == email ||
                    contact.Email3Address == email)
                {
                    return true;
                }
            }
        }
        catch { }

        return false;
    }
}
