using CommunityToolkit.Maui.Extensions;
using Project_Sauyo.SETTINGS;

namespace Project_Sauyo
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void MuteTapped(object sender, TappedEventArgs e)
        {
        }

        private void MusicTapped(object sender, TappedEventArgs e)
        {
        }

        private async void BackTapped(object sender, TappedEventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }

        private async void ThemeClicked(object sender, EventArgs e)
        {
            var popup = new Theme();
            await this.ShowPopupAsync(popup);
        }

        private async void AchievementsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Achievements());
        }

        private void SignOutClicked(object sender, EventArgs e)
        {
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
                await Navigation.PopAsync();
        }
    }
}
