using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace ReminderApp
{
    public partial class MainWindow : Window
    {
        private readonly string _userEmail;
        private List<Reminder> _reminders;

        public MainWindow(string userEmail)
        {
            InitializeComponent();
            _userEmail = userEmail;

            // Show the "Login Logs" button if the user is an admin
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
                // Get the user's reminder file path
                string userFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReminderApp",
                    "UserFiles",
                    _userEmail.Replace("@", "_").Replace(".", "_")
                );

                string reminderFile = Path.Combine(userFolder, "reminders.txt");

                _reminders = new List<Reminder>();

                if (File.Exists(reminderFile))
                {
                    string[] lines = File.ReadAllLines(reminderFile);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 3)
                        {
                            _reminders.Add(new Reminder
                            {
                                DateTime = DateTime.Parse(parts[0]),
                                Subject = parts[1],
                                Description = parts[2]
                            });
                        }
                    }
                }

                ReminderListBox.ItemsSource = null;
                ReminderListBox.ItemsSource = _reminders;
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and show an error message
                MessageBox.Show($"Error loading reminders: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveReminders()
        {
            try
            {
                // Get the user's reminder file path
                string userFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReminderApp",
                    "UserFiles",
                    _userEmail.Replace("@", "_").Replace(".", "_")
                );

                string reminderFile = Path.Combine(userFolder, "reminders.txt");

                Directory.CreateDirectory(userFolder);

                // Save reminders back to the file
                File.WriteAllLines(reminderFile, _reminders.Select(r => $"{r.DateTime}|{r.Subject}|{r.Description}"));
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and show an error message
                MessageBox.Show($"Error saving reminders: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddReminderButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the ReminderWindow for adding a new reminder
            ReminderWindow reminderWindow = new ReminderWindow(_userEmail);
            reminderWindow.ShowDialog(); // Use ShowDialog to wait for the window to close
            LoadReminders(); // Reload reminders after adding a new one
        }

        private void DeleteReminderButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected reminder
            Reminder selectedReminder = (Reminder)ReminderListBox.SelectedItem;

            if (selectedReminder == null)
            {
                MessageBox.Show("Please select a reminder to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm deletion
            var result = MessageBox.Show($"Are you sure you want to delete the reminder for '{selectedReminder.Subject}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Remove the selected reminder and save the updated list
                _reminders.Remove(selectedReminder);
                SaveReminders();
                LoadReminders(); // Refresh the ListBox
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Log out the user and return to the LoginWindow
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
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

                // Display the login logs in a message box or another window
                string logs = File.ReadAllText(logFilePath);
                MessageBox.Show(logs, "Login Logs", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Show a friendly error message if logs cannot be loaded
                MessageBox.Show($"Error loading login logs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class Reminder
    {
        public DateTime DateTime { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
    }
}