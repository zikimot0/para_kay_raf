using System;
using System.IO;
using System.Windows;

namespace ReminderApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string userEmail = EmailTextBox.Text;
            string password = PasswordBox.Password;

            // 👇 Only enforce password if it's YOU (admin)
            if (userEmail == "admin@gwapo.com" && password != "admingwapo")
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "Wrong password for admin.";
                return;
            }

            // Log the login
            string logPath = "login_logs.txt";
            string logEntry = $"{DateTime.Now:G} - {userEmail} logged in{Environment.NewLine}";
            File.AppendAllText(logPath, logEntry);

            // Open MainWindow and pass current user email
            MainWindow mainWindow = new MainWindow(userEmail);
            mainWindow.Show();
            this.Close();
        }


    }
}
