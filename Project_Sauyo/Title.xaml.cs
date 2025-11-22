namespace Project_Sauyo
{
    public partial class Title : ContentPage
    {
        public Title()
        {
            InitializeComponent();
        }

        private async void OnMainPageTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new Menu());
        }
    }
}
