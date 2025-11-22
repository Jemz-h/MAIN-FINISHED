namespace Project_Sauyo
{
    public partial class Math_Level_Select : ContentPage
    {
        public Math_Level_Select()
        {
            InitializeComponent();
        }

        private async void OnPlayClicked(object sender, EventArgs e)
        {
            AppGlobals.SelectedSkill = "MATH";
            AppGlobals.SelectedDifficulty = "EASY"; // always starts at easy on Play
            AppGlobals.SelectedQuestionFile = FileMaps.GetFileFor(AppGlobals.SelectedSkill, AppGlobals.SelectedDifficulty);

            await Navigation.PushAsync(new Picture_Question());
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
                await Navigation.PopAsync();
        }

        private async void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }

        private async void OnEasyTapped(object sender, EventArgs e)
        {
            AppGlobals.SelectedSkill = "MATH";
            AppGlobals.SelectedDifficulty = "EASY";
            AppGlobals.SelectedQuestionFile = FileMaps.GetFileFor(AppGlobals.SelectedSkill, AppGlobals.SelectedDifficulty);
            await Navigation.PushAsync(new Picture_Question());
        }
    }
}
