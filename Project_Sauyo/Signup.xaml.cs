namespace Project_Sauyo
{
    public partial class Signup : ContentPage
    {
        public Signup()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OnClickMeClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            string firstName = FirstNameEntry?.Text ?? "";
            string lastName = LastNameEntry?.Text ?? "";
            string nickname = NicknameEntry?.Text ?? "";
            string barangay = BarangayEntry?.Text ?? "";
            string? birthday = BirthdayPicker?.Date.ToString("MMMM dd, yyyy");

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                await DisplayAlert("Missing Info", "Please enter both first and last name.", "OK");
                return;
            }

            await DisplayAlert("Success", $"Welcome, {firstName} {lastName}!\nNickname: {nickname}\nBirthday: {birthday}\nBarangay: {barangay}", "OK");

            await Navigation.PushAsync(new Menu());
        }
    }
}
