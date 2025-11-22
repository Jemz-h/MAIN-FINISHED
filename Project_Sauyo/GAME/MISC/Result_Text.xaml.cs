using CommunityToolkit.Maui.Views;

namespace Project_Sauyo.GAME.MISC;

public partial class Result_Text : Popup
{
    public Result_Text()
    {
        InitializeComponent();
    }

    private void OnContinueClicked(object sender, EventArgs e)
    {
        this.CloseAsync();
    }

    private async void OnMegaphoneClicked(object sender, EventArgs e)
    {
        ImageButton? button = sender as ImageButton;

        if (button == null)
            return;

        // 🔊 TODO: add audio or speech here later

        // 🎬 ANIMATION: bounce + shake
        await button.ScaleTo(1.2, 120, Easing.CubicOut);      // enlarge
        await button.ScaleTo(0.9, 120, Easing.CubicIn);       // shrink

        await button.RotateTo(-10, 60);
        await button.RotateTo(10, 60);
        await button.RotateTo(-5, 60);
        await button.RotateTo(0, 60);

        await button.ScaleTo(1.0, 100);                       // return to normal
    }
}
