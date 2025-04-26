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

            // Initialize the reminder timer to check every 60 seconds
            _reminderTimer = new Timer(60000);
            _reminderTimer.Elapsed += CheckReminders;
            _reminderTimer.Start();
        }

        private void CheckReminders(object sender, ElapsedEventArgs e)
        {
            try
            {
                // Get the user folder for reminders
                string userFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReminderApp",
                    "UserFiles"
                );

                if (!Directory.Exists(userFolder)) return;

                // Iterate through user directories
                foreach (var userFile in Directory.GetDirectories(userFolder))
                {
                    string reminderFile = Path.Combine(userFile, "reminders.txt");
                    if (!File.Exists(reminderFile)) continue;

                    string[] reminders = File.ReadAllLines(reminderFile);
                    foreach (string reminder in reminders)
                    {
                        string[] parts = reminder.Split('|');
                        if (parts.Length < 3) continue;

                        if (DateTime.TryParse(parts[0], out DateTime reminderDateTime))
                        {
                            string subject = parts[1];

                            // Check if the current time is within 1 minute of the reminder time
                            if (DateTime.Now >= reminderDateTime && DateTime.Now <= reminderDateTime.AddMinutes(1))
                            {
                                // Show the reminder message and play the sound
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    PlayReminderSound();
                                    MessageBox.Show($"Reminder: {subject}", "Reminder", MessageBoxButton.OK, MessageBoxImage.Information);
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the error (optional)
                Console.WriteLine($"Error checking reminders: {ex.Message}");
            }
        }

        private void PlayReminderSound()
        {
            try
            {
                // Use the System.Media.SoundPlayer to play a custom sound
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
                    PlayMarioTheme();
                }
            }
            catch (Exception ex)
            {
                // Handle sound-playing errors gracefully
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Error playing sound: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        private void PlayMarioTheme()
        {
            try
            {
                // Mario Theme melody using Console.Beep
                int[] notes = {
                    659, 659, 0, 659, 0, 523, 659, 0, 784, 0, 0, 0, 392, 0, 0, 0, // First phrase
                    523, 0, 392, 0, 330, 0, 440, 494, 466, 440, 0, 392, 659, 784, 880, 0, // Second phrase
                    698, 784, 0, 659, 523, 587, 494, 0, 523, 392, 330, 440, 494, 466, 440 // Third phrase
                };

                int[] durations = {
                    125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, // First phrase
                    125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, // Second phrase
                    125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125, 125 // Third phrase
                };

                // Play the Mario Theme
                for (int i = 0; i < notes.Length; i++)
                {
                    if (notes[i] == 0)
                    {
                        // Rest (silence)
                        System.Threading.Thread.Sleep(durations[i]);
                    }
                    else
                    {
                        // Play the note
                        Console.Beep(notes[i], durations[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors during Mario Theme playback (optional)
                Console.WriteLine($"Error playing Mario Theme: {ex.Message}");
            }
        }
    }
}