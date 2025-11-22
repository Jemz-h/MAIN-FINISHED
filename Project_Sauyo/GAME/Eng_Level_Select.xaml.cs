namespace Project_Sauyo
{
    public partial class Eng_Level_Select : ContentPage
    {
        public Eng_Level_Select()
        {
            InitializeComponent();
        }

        // Play button always starts at Easy per Option B
        private async void OnPlayClicked(object sender, EventArgs e)
        {
            AppGlobals.SelectedSkill = "ENG";
            AppGlobals.SelectedDifficulty = "EASY";
            AppGlobals.SelectedQuestionFile = FileMaps.GetFileFor(AppGlobals.SelectedSkill, AppGlobals.SelectedDifficulty);

            await Navigation.PushAsync(new Word_Question());
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
    }
}
