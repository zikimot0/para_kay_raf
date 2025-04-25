using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ReminderApp
{
    public partial class RegisterWindow : Window
    {
        private const string AdminEmail = "admin@gwapo.com";
        private const int MinPasswordLength = 6; // Minimum password length for validation

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // Validate email and password
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Email and Password cannot be empty.");
                return;
            }

            if (!IsValidEmail(email))
            {
                ShowError("Invalid email format.");
                return;
            }

            if (password.Length < MinPasswordLength)
            {
                ShowError($"Password must be at least {MinPasswordLength} characters long.");
                return;
            }

            try
            {
                string userFile = GetUserFilePath(email);

                // Check if user already exists
                if (File.Exists(userFile))
                {
                    ShowError("This email is already registered.");
                    return;
                }

                // Create user directory if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(userFile));

                // Save user information to file
                File.WriteAllText(userFile, $"Welcome, {email}!\nDate Registered: {DateTime.Now}");

                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Close the registration window
            }
            catch (Exception ex)
            {
                ShowError($"An error occurred: {ex.Message}");
            }
        }

        private string GetUserFilePath(string email)
        {
            string sanitizedEmail = string.Join("_", email.Split(Path.GetInvalidFileNameChars()));

            // Use AppData folder for storing user files
            string userDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ReminderApp",
                "UserFiles"
            );

            return Path.Combine(userDirectory, $"{sanitizedEmail}.txt");
        }

        private bool IsValidEmail(string email)
        {
            // Simple email validation using regex
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}