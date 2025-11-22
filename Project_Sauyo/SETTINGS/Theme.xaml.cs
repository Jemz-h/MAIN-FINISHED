using CommunityToolkit.Maui.Views;

namespace Project_Sauyo.SETTINGS
{
    public enum ThemeType
    {
        Classroom,
        Nature,
        Space
    }

    public partial class Theme : Popup
    {
        private ThemeType selectedTheme = ThemeType.Classroom;

        public Theme()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            CloseAsync();
        }

        private void OnClassroomTapped(object sender, EventArgs e)
        {
            selectedTheme = ThemeType.Classroom;

            ClassroomOverlay.IsVisible = true;
            ClassroomCheck.IsVisible = true;

            NatureOverlay.IsVisible = false;
            NatureCheck.IsVisible = false;

            SpaceOverlay.IsVisible = false;
            SpaceCheck.IsVisible = false;
        }

        private void OnNatureTapped(object sender, EventArgs e)
        {
            selectedTheme = ThemeType.Nature;

            ClassroomOverlay.IsVisible = false;
            ClassroomCheck.IsVisible = false;

            NatureOverlay.IsVisible = true;
            NatureCheck.IsVisible = true;

            SpaceOverlay.IsVisible = false;
            SpaceCheck.IsVisible = false;
        }

        private void OnSpaceTapped(object sender, EventArgs e)
        {
            selectedTheme = ThemeType.Space;

            ClassroomOverlay.IsVisible = false;
            ClassroomCheck.IsVisible = false;

            NatureOverlay.IsVisible = false;
            NatureCheck.IsVisible = false;

            SpaceOverlay.IsVisible = true;
            SpaceCheck.IsVisible = true;
        }

        private void OnConfirmClicked(object sender, EventArgs e)
        {
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {

        }
    }
}
