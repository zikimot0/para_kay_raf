using System;
using System.IO;
using System.Windows;

namespace ReminderApp
{
    public partial class ReminderWindow : Window
    {
        private readonly string _userEmail;

        public ReminderWindow(string userEmail)
        {
            InitializeComponent();
            _userEmail = userEmail; // Pass the logged-in user's email
        }

        private void SaveReminderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get input values
                DateTime selectedDate = ReminderCalendar.SelectedDate ?? DateTime.Now;
                string time = TimeTextBox.Text.Trim();
                string subject = SubjectTextBox.Text.Trim();
                string description = DescriptionTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(time) || string.IsNullOrWhiteSpace(subject))
                {
                    MessageBox.Show("Time and Subject are required!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Parse time input
                if (!TimeSpan.TryParse(time, out TimeSpan reminderTime))
                {
                    MessageBox.Show("Invalid time format. Use HH:mm (e.g., 14:30).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DateTime reminderDateTime = selectedDate.Date + reminderTime;

                // Save reminder to user folder
                string userFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReminderApp",
                    "UserFiles",
                    _userEmail.Replace("@", "_").Replace(".", "_")
                );

                Directory.CreateDirectory(userFolder);

                string reminderFile = Path.Combine(userFolder, "reminders.txt");
                string reminderEntry = $"{reminderDateTime:G}|{subject}|{description}{Environment.NewLine}";
                File.AppendAllText(reminderFile, reminderEntry);

                MessageBox.Show("Reminder saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}