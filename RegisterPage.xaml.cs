using System.Text.RegularExpressions;

namespace MauiLogin;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string username = RegUsernameEntry.Text;
        string email = RegEmailEntry.Text;
        string password = RegPasswordEntry.Text;
        string confirmPassword = RegConfirmPasswordEntry.Text;
        string gender = GenderPicker.SelectedItem?.ToString();

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword) ||
            string.IsNullOrWhiteSpace(gender))
        {
            await DisplayAlert("Error", "All fields are required.", "OK");
            return;
        }

        if (!IsValidEmail(email))
        {
            await DisplayAlert("Error", "Invalid email format. Example: user@example.com", "OK");
            return;
        }

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

        UserRepository.AddUser(new User
        {
            Username = username,
            Email = email,
            Password = password,
            Gender = gender
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