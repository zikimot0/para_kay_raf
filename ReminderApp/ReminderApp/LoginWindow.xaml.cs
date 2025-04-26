using System;
using System.IO;
using System.Windows;

namespace ReminderApp
{
    public partial class LoginWindow : Window
    {
        private const string AdminEmail = "admin@gwapo.com";
        private const string AdminPassword = "admingwapo"; // Admin's password

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the RegisterWindow
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show(); // Show the RegisterWindow
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string userEmail = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            // Validate email and password fields
            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Email and Password cannot be empty.");
                return;
            }

            // Check if it's an admin login
            if (userEmail == AdminEmail)
            {
                if (password != AdminPassword)
                {
                    ShowError("Wrong password for admin.");
                    return;
                }

                // Admin login successful
                LogLoginAttempt(userEmail, successful: true);
                OpenMainWindow(userEmail);
                return;
            }

            // Validate regular user credentials
            try
            {
                string userFile = GetUserFilePath(userEmail);

                if (!File.Exists(userFile))
                {
                    ShowError("User not found. Please register first.");
                    return;
                }

                // Read the stored password from the user file
                string[] userFileContents = File.ReadAllLines(userFile);
                string storedPassword = ExtractStoredPassword(userFileContents);

                if (storedPassword != password)
                {
                    ShowError("Incorrect password.");
                    LogLoginAttempt(userEmail, successful: false);
                    return;
                }

                // User login successful
                LogLoginAttempt(userEmail, successful: true);
                OpenMainWindow(userEmail);
            }
            catch (Exception ex)
            {
                ShowError($"An error occurred: {ex.Message}");
            }
        }

        private void OpenMainWindow(string userEmail)
        {
            MainWindow mainWindow = new MainWindow(userEmail);
            mainWindow.Show();
            this.Close();
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

        private string ExtractStoredPassword(string[] userFileContents)
        {
            foreach (string line in userFileContents)
            {
                // Look for the line that starts with "Password:" and extract the value
                if (line.StartsWith("Password:"))
                {
                    return line.Substring("Password:".Length).Trim();
                }
            }

            throw new Exception("Password not found in user file.");
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        private void LogLoginAttempt(string email, bool successful)
        {
            try
            {
                string logFilePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "ReminderApp",
                    "login_logs.txt"
                );

                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

                string logEntry = $"{DateTime.Now}: {email} - {(successful ? "Login successful" : "Login failed")}";
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Log or handle error silently. Don't crash the app if logging fails.
                Console.WriteLine($"Failed to log login attempt: {ex.Message}");
            }
        }
    }
}