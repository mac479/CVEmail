using System.Net.Mail;
using System.Net;

namespace CVEmailLib

{
    public  class CVEmailer
    {
        //private readonly string Sender = ConfigurationManager.AppSettings["sender"];

        private string smtpServer = "";
        private int smtpPort;
        private string smtpUsername = "";
        private string smtpPassword = "";

        private int retryDelay = 0;
        private int retryMax = 3;

        private string logFile;

        public CVEmailer(CVCredentials credentials, int retryDelay, int retryMax, string logFile)
        {
            smtpServer = credentials.smtpServer;
            smtpPort = credentials.smtpPort;
            smtpPassword = credentials.smtpPassword;
            smtpUsername = credentials.smtpUsername;
            this.retryDelay = retryDelay;
            this.retryMax = retryMax;
            this.logFile = logFile;
        }
        public CVEmailer(CVCredentials credentials, int retryDelay, int retryMax)
        {
            smtpServer = credentials.smtpServer;
            smtpPort = credentials.smtpPort;
            smtpPassword = credentials.smtpPassword;
            smtpUsername = credentials.smtpUsername;
            this.retryDelay = retryDelay;
            this.retryMax = retryMax;
        }
        public CVEmailer(CVCredentials credentials, string logFile)
        {
            smtpServer = credentials.smtpServer;
            smtpPort = credentials.smtpPort;
            smtpPassword = credentials.smtpPassword;
            smtpUsername = credentials.smtpUsername;
            this.logFile = logFile;
        }
        public CVEmailer(CVCredentials credentials)
        {
            smtpServer = credentials.smtpServer;
            smtpPort = credentials.smtpPort;
            smtpPassword = credentials.smtpPassword;
            smtpUsername = credentials.smtpUsername;
        }

        public void SendEmail(string message, string subject, string to)
        {
            //Starts function as thread to make sure code calling the function isn't interrupted.
            Thread email = new Thread(() => _sendEmail(message, subject, to));
            email.Start();

        }

        //Private internal function made for use only by the thread.
        private void _sendEmail(string message, string subject, string to)
        {

            //Import settings for smtp client.
            SmtpClient client = new SmtpClient();
            client.Host = smtpServer;
            client.Port = smtpPort;
            client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;

            MailMessage email = new MailMessage()
            {
                From = new MailAddress(smtpUsername),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            email.To.Add(to);

            try
            {
                client.Send(email);
                Logger(message, subject, to, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                for (int i = 0; i < retryMax - 1; i++)
                {
                    Thread.Sleep(retryDelay);
                    try
                    {
                        client.Send(email);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                Logger(message, subject, to, false);
            }
            return;
        }

        //Store results as csv file. It would better to store it to some table but this will do for demostration purposes.
        private void Logger(string message, string subject, string recipient, bool sent)
        {
            DateTime curTime = DateTime.Now;
            //If there is no specified location for the log it choses to make one in the directory of the executable.
            if (logFile is null)
            {
                if (!File.Exists("emailLog.csv"))
                {
                    File.Create("emailLog.csv");
                }
                logFile = "emailLog.csv";
            }
            else if (!File.Exists(logFile))
            {
                File.Create(logFile);
            }

            File.AppendAllText(logFile, "\n" + smtpUsername + "," + recipient + "," + subject + "," + message.Replace('\n',' ').Replace('\r',' ') + "," + (sent ? "SENT" : "FAILED") + "," + curTime.ToString());
            return;
        }
    }
}
