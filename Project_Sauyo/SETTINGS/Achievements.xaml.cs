namespace Project_Sauyo
{
    public partial class Achievements : ContentPage
    {
        public Achievements()
        {
            InitializeComponent();
        }
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
                await Navigation.PopAsync();
        }
    }
}
