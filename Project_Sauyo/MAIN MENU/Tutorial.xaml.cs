namespace Project_Sauyo;

public partial class Tutorial : ContentPage
{
    public Tutorial()
    {
        InitializeComponent();
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.Count > 1)
            await Navigation.PopAsync();
    }
    private async void OnMegaphoneClicked(object sender, EventArgs e)
    {
        // 🔊 TODO: add audio or speech here later

        // 🎬 ANIMATION: bounce + shake
        await MegaphoneImage.ScaleTo(1.2, 120, Easing.CubicOut);   // enlarge
        await MegaphoneImage.ScaleTo(0.9, 120, Easing.CubicIn);    // shrink

        await MegaphoneImage.RotateTo(-10, 60);
        await MegaphoneImage.RotateTo(10, 60);
        await MegaphoneImage.RotateTo(-5, 60);
        await MegaphoneImage.RotateTo(0, 60);

        await MegaphoneImage.ScaleTo(1.0, 100);                    // return to normal
    }
}