using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ReminderApp
{
    public partial class MainWindow : Window
    {
        private readonly string _userEmail;

        public MainWindow(string userEmail)
        {
            InitializeComponent();
            _userEmail = userEmail;

            // Check if the user is admin and show the "View Login Logs" button
            if (_userEmail == "admin@gwapo.com")
            {
                ViewLoginLogsButton.Visibility = Visibility.Visible;
            }

            LoadReminders();
        }

        private void LoadReminders()
        {
            try
            {
                // Load reminders for the user
                string userFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReminderApp",
                    "UserFiles",
                    _userEmail.Replace("@", "_").Replace(".", "_")
                );

                string reminderFile = Path.Combine(userFolder, "reminders.txt");
                if (!File.Exists(reminderFile))
                {
                    return; // No reminders to load
                }

                var reminders = new List<Reminder>();
                string[] lines = File.ReadAllLines(reminderFile);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length < 3) continue;

                    reminders.Add(new Reminder
                    {
                        DateTime = DateTime.Parse(parts[0]),
                        Subject = parts[1],
                        Description = parts[2]
                    });
                }

                ReminderListBox.ItemsSource = reminders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reminders: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddReminderButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the ReminderWindow for adding a new reminder
            ReminderWindow reminderWindow = new ReminderWindow(_userEmail);
            reminderWindow.ShowDialog(); // Use ShowDialog to wait for the window to close
            LoadReminders(); // Reload reminders after adding a new one
        }

        private void ViewLoginLogsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the login logs file path
                string logFilePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReminderApp",
                    "login_logs.txt"
                );

                if (!File.Exists(logFilePath))
                {
                    MessageBox.Show("No login logs found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Display the login logs in a message box or in a separate window
                string logs = File.ReadAllText(logFilePath);
                MessageBox.Show(logs, "Login Logs", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading login logs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Log out the user and return to the LoginWindow
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }

    public class Reminder
    {
        public DateTime DateTime { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }
}