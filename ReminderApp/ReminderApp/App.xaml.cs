using System;
using System.IO;
using System.Linq;
using System.Media; // For playing sound
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

                        // If the current time is within 1 minute of the reminder time
                        if (DateTime.Now >= reminderDateTime && DateTime.Now <= reminderDateTime.AddMinutes(1))
                        {
                            // Show a message box
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                MessageBox.Show($"Reminder: {subject}", "Reminder", MessageBoxButton.OK, MessageBoxImage.Information);
                            });

                            // Play a beep or sound
                            PlayReminderSound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
            }
        }

        private void PlayReminderSound()
        {
            try
            {
                // Use the System.Media.SoundPlayer to play a beep or sound
                string soundPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "alarm.wav" // Replace with your custom sound file
                );

                if (File.Exists(soundPath))
                {
                    SoundPlayer player = new SoundPlayer(soundPath);
                    player.Play(); // Play the sound asynchronously
                }
                else
                {
                    // Fallback to system beep if no sound file is found
                    Console.Beep();
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Error playing sound: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
    }
}