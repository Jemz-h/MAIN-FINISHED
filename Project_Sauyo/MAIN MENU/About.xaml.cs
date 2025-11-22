namespace Project_Sauyo
{
    public partial class About : ContentPage
    {
        public About()
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

