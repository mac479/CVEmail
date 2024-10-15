using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CVFrontEnd
{



    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //this.ResizeMode = ResizeMode.NoResize;
            MessageBox.Foreground = Brushes.Gray;
            MessageBox.Text = "Enter your message here...";
            SubjectBox.Foreground = Brushes.Gray;
            SubjectBox.Text = "Enter subject here...";
            RecipientBox.Foreground = Brushes.Gray;
            RecipientBox.Text = "Enter recipient here...";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool errorFlag = false;
            if (MessageBox.Text == "Enter your message here...")
            {
                MessageBox.BorderBrush = Brushes.Red;
                MessageBox.Text = "Enter your message here...";
                errorFlag = true;
            }
            if (SubjectBox.Text == "Enter subject here...")
            {
                SubjectBox.BorderBrush = Brushes.Red;
                SubjectBox.Text = "Enter subject here...";
                errorFlag = true;
            }
            if (RecipientBox.Text == "Enter recipient here...")
            {
                RecipientBox.BorderBrush = Brushes.Red;
                RecipientBox.Text = "Enter recipient here...";
                errorFlag = true;
            }
            if (errorFlag)
            {
                ErrorText.Content = "Fill out all required fields before submitting.";
            }
            else
            {
                //import nessecary data from config then begin building message.
                string smtpServer = System.Configuration.ConfigurationManager.AppSettings["smtpServer"];
                string jsonEmail = JsonSerializer.Serialize(new
                {
                    smtpServer = System.Configuration.ConfigurationManager.AppSettings["smtpServer"],
                    smtpPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]),
                    smtpPassword = System.Configuration.ConfigurationManager.AppSettings["smtpPassword"],
                    smtpUsername = System.Configuration.ConfigurationManager.AppSettings["smtpUsername"],
                    to = RecipientBox.Text,
                    subject = SubjectBox.Text,
                    message = MessageBox.Text
                });

                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["apiUrl"])
                };
                HttpContent content = new StringContent(jsonEmail, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = client.PostAsync("/cvemail", content).Result;
                if (responseMessage.StatusCode == HttpStatusCode.Created)
                {
                    StatusText.Foreground = Brushes.Black;
                    StatusText.Content = "Email sent!";
                }
                else
                {
                    StatusText.Foreground = Brushes.Red;
                    StatusText.Content = "Something went wrong!";
                }
            }
        }

        private void RecipientBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RecipientBox.Text == "Enter recipient here...")
            {
                RecipientBox.Foreground = Brushes.Black;
                RecipientBox.Text = "";
            }
        }

        private void RecipientBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RecipientBox.Text == "")
            {
                RecipientBox.Foreground = Brushes.Gray;
                RecipientBox.Text = "Enter recipient here...";
            }
        }

        private void SubjectBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SubjectBox.Text == "Enter subject here...")
            {
                SubjectBox.Foreground = Brushes.Black;
                SubjectBox.Text = "";
            }
        }

        private void SubjectBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SubjectBox.Text == "")
            {
                SubjectBox.Foreground = Brushes.Gray;
                SubjectBox.Text = "Enter subject here...";
            }
        }

        private void MessageBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Text == "Enter your message here...")
            {
                MessageBox.Foreground = Brushes.Black;
                MessageBox.Text = "";
            }
        }

        private void MessageBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Text == "")
            {
                MessageBox.Foreground = Brushes.Gray;
                MessageBox.Text = "Enter your message here...";
            }
        }
    }
}