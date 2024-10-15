using CVEmailLib;
using System.Configuration;

//Test application for the library.

Console.WriteLine("Trying to send an email");
CVCredentials cred = new CVCredentials()
{
    smtpServer = ConfigurationManager.AppSettings["smtpServer"] ?? "",
    smtpPort = Int32.Parse(ConfigurationManager.AppSettings["smtpPort"] ?? ""),
    smtpPassword = ConfigurationManager.AppSettings["smtpPassword"] ?? "",
    smtpUsername = ConfigurationManager.AppSettings["smtpUsername"] ?? "",
};
CVEmailer email = new CVEmailer(cred, 100, 3);
email.SendEmail("Test Email", "Testing", "myles26tbg@gmail.com");
