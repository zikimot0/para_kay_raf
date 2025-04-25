using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;

namespace ReminderApp
{
    public partial class App : Application
    {
        private Timer _reminderTimer;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _reminderTimer = new Timer(60000); // Check every 60 seconds
            _reminderTimer.Elapsed += CheckReminders;
            _reminderTimer.Start();
        }

        private void CheckReminders(object sender, ElapsedEventArgs e)
        {
            try
            {
                string userFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReminderApp",
                    "UserFiles"
                );

                if (!Directory.Exists(userFolder)) return;

                foreach (var userFile in Directory.GetDirectories(userFolder))
                {
                    string reminderFile = Path.Combine(userFile, "reminders.txt");
                    if (!File.Exists(reminderFile)) continue;

                    string[] reminders = File.ReadAllLines(reminderFile);
                    foreach (string reminder in reminders)
                    {
                        string[] parts = reminder.Split('|');
                        if (parts.Length < 3) continue;

                        DateTime reminderDateTime = DateTime.Parse(parts[0]);
                        string subject = parts[1];

                        if (DateTime.Now >= reminderDateTime && DateTime.Now <= reminderDateTime.AddMinutes(1))
                        {
                            MessageBox.Show($"Reminder: {subject}", "Reminder", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
            }
        }
    }
}