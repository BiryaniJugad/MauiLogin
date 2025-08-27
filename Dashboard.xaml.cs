namespace MauiLogin;

public partial class Dashboard : ContentPage
{
    private User _user;
    private bool _isAdmin;

    public Dashboard(User user)
    {
        InitializeComponent();
        _user = user;
        _isAdmin = user.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
                   user.Email.Equals("admin@system.com", StringComparison.OrdinalIgnoreCase);

        if (_isAdmin)
        {
            AdminPanel.IsVisible = true;
            UsersList.ItemsSource = UserRepository.GetAllUsers();
        }
        else
        {
            UserPanel.IsVisible = true;
            EditGender.ItemsSource = new[] { "Male", "Female", "Other" };
            LoadUserProfile();
        }
    }

    private void LoadUserProfile()
    {
        EditUsername.Text = _user.Username;
        EditEmail.Text = _user.Email;
        EditPassword.Text = string.Empty; // empty on load
        EditGender.SelectedItem = _user.Gender;
    }

    private async void OnSaveUserChanges(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(EditPassword.Text))
            _user.Password = EditPassword.Text;

        _user.Gender = EditGender.SelectedItem?.ToString();

        UserRepository.UpdateUser(_user);
        await DisplayAlert("Success", "Profile updated successfully!", "OK");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}