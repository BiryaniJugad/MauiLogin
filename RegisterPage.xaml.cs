using System.Text.RegularExpressions;

namespace MauiLogin;

public partial class RegisterPage : ContentPage
{
    public static readonly List<string> Genders = new() { "Male", "Female", "Other" };

    private readonly List<string> _months;
    private readonly List<string> _days;
    private readonly List<string> _years;

    public RegisterPage()
    {
        InitializeComponent();

        // Gender
        GenderPicker.ItemsSource = new List<string> { "Male", "Female", "Other" };

        // Days
        _days = Enumerable.Range(1, 31)
            .Select(d => d.ToString())
            .ToList();
        DayPicker.ItemsSource = _days;

        // Months
        _months = System.Globalization.DateTimeFormatInfo
            .InvariantInfo.MonthNames
            .Where(m => !string.IsNullOrEmpty(m))
            .ToList();
        MonthPicker.ItemsSource = _months;

        // Years
        int currentYear = DateTime.Now.Year;
        _years = Enumerable.Range(1900, currentYear - 1899)
            .Reverse()
            .Select(y => y.ToString())
            .ToList();
        YearPicker.ItemsSource = _years;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string firstName = FirstNameEntry.Text?.Trim();
        string lastName = LastNameEntry.Text?.Trim();
        string email = EmailEntry.Text?.Trim();
        string address = AddressEntry.Text?.Trim();
        string gender = GenderPicker.SelectedItem as string;
        string month = MonthPicker.SelectedItem as string;
        string day = DayPicker.SelectedItem as string;
        string year = YearPicker.SelectedItem as string;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // Validation
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Error", "All fields are required.", "OK");
            return;
        }

        if (gender == null)
        {
            await DisplayAlert("Error", "Please select a gender.", "OK");
            return;
        }

        if (!IsValidEmail(email))
        {
            await DisplayAlert("Error", "Invalid email format. Example: user@example.com", "OK");
            return;
        }

        string username = $"{firstName}{lastName}";
        if (UserRepository.UserExists(username, email))
        {
            await DisplayAlert("Error", "That username or email is already registered.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        string dob = null;
        if (month != null && day != null && year != null)
        {
            dob = $"{month} {day}, {year}";
        }

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
