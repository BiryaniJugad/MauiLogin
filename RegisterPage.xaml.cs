using System.Text.RegularExpressions;

namespace MauiLogin;

public partial class RegisterPage : ContentPage
{
    private readonly List<string> _genders = new() { "Male", "Female", "Other" };
    private readonly List<string> _months;
    private readonly List<string> _days;
    private readonly List<string> _years;

    public RegisterPage()
    {
        InitializeComponent();

        // Prepare dropdown data
        _months = System.Globalization.DateTimeFormatInfo
            .InvariantInfo.MonthNames
            .Where(m => !string.IsNullOrEmpty(m))
            .ToList();

        _days = Enumerable.Range(1, 31)
            .Select(d => d.ToString())
            .ToList();

        int currentYear = DateTime.Now.Year;
        _years = Enumerable.Range(1900, currentYear - 1899)
            .Reverse()
            .Select(y => y.ToString())
            .ToList();

        // Bind data
        GenderDropdownCollection.ItemsSource = _genders;
        MonthDropdownCollection.ItemsSource = _months;
        DayDropdownCollection.ItemsSource = _days;
        YearDropdownCollection.ItemsSource = _years;

        // Toggle dropdowns
        GenderDropdownFrame.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => ToggleDropdown(GenderDropdownListFrame))
        });

        MonthDropdownFrame.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => ToggleDropdown(MonthDropdownListFrame))
        });

        DayDropdownFrame.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => ToggleDropdown(DayDropdownListFrame))
        });

        YearDropdownFrame.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => ToggleDropdown(YearDropdownListFrame))
        });
    }

    private void ToggleDropdown(Frame listFrame)
    {
        // Hide all dropdowns first
        GenderDropdownListFrame.IsVisible = false;
        MonthDropdownListFrame.IsVisible = false;
        DayDropdownListFrame.IsVisible = false;
        YearDropdownListFrame.IsVisible = false;

        // Show the one tapped
        listFrame.IsVisible = true;
    }

    // Gender item tapped
    private void OnGenderItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is string gender)
        {
            GenderSelectedLabel.Text = gender;
            GenderDropdownListFrame.IsVisible = false;
        }
    }

    // Month item tapped
    private void OnMonthItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is string month)
        {
            MonthSelectedLabel.Text = month;
            MonthDropdownListFrame.IsVisible = false;
        }
    }

    // Day item tapped
    private void OnDayItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is string day)
        {
            DaySelectedLabel.Text = day;
            DayDropdownListFrame.IsVisible = false;
        }
    }

    // Year item tapped
    private void OnYearItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is string year)
        {
            YearSelectedLabel.Text = year;
            YearDropdownListFrame.IsVisible = false;
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string firstName = FirstNameEntry.Text?.Trim();
        string lastName = LastNameEntry.Text?.Trim();
        string email = EmailEntry.Text?.Trim();
        string address = AddressEntry.Text?.Trim();
        string gender = GenderSelectedLabel.Text != "Select Gender" ? GenderSelectedLabel.Text : null;
        string month = MonthSelectedLabel.Text != "Month" ? MonthSelectedLabel.Text : null;
        string day = DaySelectedLabel.Text != "Day" ? DaySelectedLabel.Text : null;
        string year = YearSelectedLabel.Text != "Year" ? YearSelectedLabel.Text : null;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

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

        // Validate gender
        if (gender == null)
        {
            await DisplayAlert("Error", "Please select a gender.", "OK");
            return;
        }

        // Validate email format
        if (!IsValidEmail(email))
        {
            await DisplayAlert("Error", "Invalid email format. Example: user@example.com", "OK");
            return;
        }

        // Check if user already exists
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
        if (month != null && day != null && year != null)
        {
            dob = $"{month} {day}, {year}";
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