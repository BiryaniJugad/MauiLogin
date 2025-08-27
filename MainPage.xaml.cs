namespace MauiLogin
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string input = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter username/email and password.", "OK");
                return;
            }

            var user = UserRepository.GetUser(input, password);

            if (user != null)
            {
                await Navigation.PushAsync(new Dashboard(user));
            }
            else
            {
                await DisplayAlert("Error", "Invalid credentials.", "OK");
            }

        }
        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            UsernameEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
        }
    }
}
