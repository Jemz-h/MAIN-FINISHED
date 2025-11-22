namespace Project_Sauyo
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent(); 
        }

        private void OnStartClicked(object sender, EventArgs e)
        {
            string nickname = NicknameEntry?.Text ?? "";

            if (!string.IsNullOrWhiteSpace(nickname))
            {
                greetingLabel.Text = $"Hello, {nickname}! ??";
            }
            else
            {
                greetingLabel.Text = "Please enter your nickname first!";
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Signup());
        }
    }
}
