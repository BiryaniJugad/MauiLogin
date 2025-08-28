using System.Text.RegularExpressions;

namespace MauiLogin;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        GenderPicker.SelectedIndex = 0;

        LoadPickers();
    }
    private void LoadPickers()
    {
        // Months (Jan–Dec)
        MonthPicker.ItemsSource = System.Globalization.DateTimeFormatInfo
            .InvariantInfo.MonthNames
            .Where(m => !string.IsNullOrEmpty(m))
            .ToList();

        // Days 1–31
        DayPicker.ItemsSource = Enumerable.Range(1, 31)
            .Select(d => d.ToString())
            .ToList();

        // Years 1900–current (descending)
        int currentYear = DateTime.Now.Year;
        YearPicker.ItemsSource = Enumerable.Range(1900, currentYear - 1899)
            .Reverse()
            .Select(y => y.ToString())
            .ToList();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string firstName = FirstNameEntry.Text?.Trim();
        string lastName = LastNameEntry.Text?.Trim();
        string email = EmailEntry.Text?.Trim();
        string address = AddressEntry.Text?.Trim();
        string gender = GenderPicker.SelectedItem?.ToString();
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // Validate gender (must be selected)
        if (GenderPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Error", "Please select a gender.", "OK");
            return;
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Error", "All fields are required.", "OK");
            return;
        }

        // Validate email format
        if (!IsValidEmail(email))
        {
            await DisplayAlert("Error", "Invalid email format. Example: user@example.com", "OK");
            return;
        }

        // Check if user already exists (username = firstname+lastname here, or use email)
        string username = $"{firstName}{lastName}";
        if (UserRepository.UserExists(username, email))
        {
            await DisplayAlert("Error", "That username or email is already registered.", "OK");
            return;
        }

        // Check password match
        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        // Get DOB string
        string dob = null;
        if (MonthPicker.SelectedIndex >= 0 && DayPicker.SelectedIndex >= 0 && YearPicker.SelectedIndex >= 0)
        {
            dob = $"{MonthPicker.SelectedItem} {DayPicker.SelectedItem}, {YearPicker.SelectedItem}";
        }

        // Save user
        UserRepository.AddUser(new User
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Address = address,
            Gender = gender,
            DateOfBirth = dob,
            Password = password
        });

        await DisplayAlert("Success", "Registration successful!", "OK");
        await Navigation.PopAsync();
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
