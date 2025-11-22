using CommunityToolkit.Maui.Views;
namespace Project_Sauyo;

public partial class Soon : Popup
{
    public Soon()
    {
        InitializeComponent();

        this.BackgroundColor = Colors.Transparent;

    }
    private void OnCloseClicked(object sender, EventArgs e)
    {
        this.CloseAsync();
    }
}