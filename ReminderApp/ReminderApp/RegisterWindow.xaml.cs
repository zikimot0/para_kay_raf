using System;
using System.IO;
using System.Windows;

namespace ReminderApp
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            // Validate inputs
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Email and Password cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Sanitize email to create a valid file name
                string sanitizedEmail = string.Join("_", email.Split(Path.GetInvalidFileNameChars()));
                string userDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReminderApp",
                    "UserFiles"
                );

                Directory.CreateDirectory(userDirectory); // Ensure the directory exists

                string userFile = Path.Combine(userDirectory, $"{sanitizedEmail}.txt");

                // Check if the user already exists
                if (File.Exists(userFile))
                {
                    MessageBox.Show("User already registered. Please log in.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Save user data in the format "Password:<password>"
                File.WriteAllText(userFile, $"Password:{password}");
                MessageBox.Show("Registration successful! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigate back to the login window
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while registering: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}