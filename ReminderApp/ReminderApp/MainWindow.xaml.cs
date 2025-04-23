using System;
using System.IO;
using System.Windows;

namespace ReminderApp
{
    public partial class MainWindow : Window
    {
        private string currentUser;

        public MainWindow(string email)
        {
            InitializeComponent();
            currentUser = email;

            // Only show logs to admin email
            if (currentUser == "admin@gwapo.com")
            {
                ViewLogsButton.Visibility = Visibility.Visible;
            }
            else
            {
                ViewLogsButton.Visibility = Visibility.Collapsed;
            }

        }


        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void ViewLogsButton_Click(object sender, RoutedEventArgs e)
        {
            string logPath = "login_logs.txt";

            if (File.Exists(logPath))
            {
                string logs = File.ReadAllText(logPath);
                MessageBox.Show(logs, "Login Logs");
            }
            else
            {
                MessageBox.Show("No logs found.", "Login Logs");
            }
        }
    }
}
