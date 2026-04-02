using Outlook = Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

public static class DomainValidator {
    private static readonly HashSet<string> AllowedDomains = LoadAllowedDomains();

    private static HashSet<string> LoadAllowedDomains() {
        string path = "allowed_domains.json";
        if (File.Exists(path)) {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<HashSet<string>>(json);
        }
        return new HashSet<string> { "test.dd", "test1.dd", "test2.dd" };
    }

    public static bool IsSafe(string email) {
        var domain = email.Split('@').Last().ToLower();
        return AllowedDomains.Contains(domain);
    }

    public static string GetSmtpAddress(Outlook.Recipient recipient) {
        try {
            var addressEntry = recipient.AddressEntry;
            if (addressEntry.Type == "EX") {
                var exchangeUser = addressEntry.GetExchangeUser();
                return exchangeUser?.PrimarySmtpAddress;
            }
            return recipient.Address;
        } catch (Exception ex) {
            Console.WriteLine($"Error resolving SMTP address: {ex.Message}");
            return null;
        }
    }

    private static List<string> cachedContactEmails = null;

    public static bool IsInContacts(string email, Outlook.Application app) {
        if (cachedContactEmails == null) {
            cachedContactEmails = new List<string>();
            try {
                var contactsFolder = app.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);
                foreach (Outlook.ContactItem contact in contactsFolder.Items) {
                    cachedContactEmails.Add(contact.Email1Address);
                    cachedContactEmails.Add(contact.Email2Address);
                    cachedContactEmails.Add(contact.Email3Address);
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error loading contacts: {ex.Message}");
            }
        }
        return cachedContactEmails.Contains(email);
    }
}
