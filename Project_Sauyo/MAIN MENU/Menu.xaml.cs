namespace Project_Sauyo
{
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();
        }

        private async void OnStartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Grade_Select());
        }

        private async void OnTutorialClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Tutorial());
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new About());
        }
    }
}
