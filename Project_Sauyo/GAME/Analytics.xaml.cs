namespace Project_Sauyo
{
    public partial class Analytics : ContentPage
    {
        private int finalScore;
        private int totalQuestions;
        private string finalImagePath;

        public Analytics()
        {
            InitializeComponent();

            // Load results from AppGlobals for the currently selected skill
            LoadResults();
        }

        private void LoadResults()
        {
            if (string.Equals(AppGlobals.SelectedSkill, "MATH", StringComparison.OrdinalIgnoreCase))
            {
                finalScore = AppGlobals.MathTotalCorrect;
                totalQuestions = AppGlobals.MathTotalQuestions;
            }
            else
            {
                finalScore = AppGlobals.EngTotalCorrect;
                totalQuestions = AppGlobals.EngTotalQuestions;
            }

            finalImagePath = AppGlobals.LastQuestionImagePath;

            var scoreLabel = this.FindByName<Label>("ScoreLabel");
            if (scoreLabel != null) scoreLabel.Text = $"{finalScore}/{totalQuestions}";

            var diffLabel = this.FindByName<Label>("DifficultyLabel");
            if (diffLabel != null) diffLabel.Text = (AppGlobals.SelectedDifficulty ?? "EASY").ToUpperInvariant();

            // Determine medal by percentage
            var coin = this.FindByName<Image>("Coin");
            double percentage = totalQuestions > 0 ? (finalScore * 100.0) / totalQuestions : 0;

            if (coin != null)
            {
                if (percentage == 100)
                    coin.Source = "gold.png";
                else if (percentage >= 60)
                    coin.Source = "silver.png";
                else
                    coin.Source = "bronze.png";
            }

            var achievement = this.FindByName<Label>("achievementText");
            if (achievement != null)
            {
                if (percentage == 100) achievement.Text = "PERFECT!";
                else if (percentage >= 60) achievement.Text = "WELL DONE!";
                else achievement.Text = "GOOD TRY!";
            }

            var pic = this.FindByName<Image>("picHold");
            if (pic != null && !string.IsNullOrWhiteSpace(finalImagePath))
            {
                try { pic.Source = ImageSource.FromFile(finalImagePath); }
                catch { pic.Source = "draft01.png"; }
            }

            // NEXT LEVEL button reference
            var nextLevel = this.FindByName<Border>("NextLevelBtn");
            if (nextLevel != null)
            {
                // Lock next level if below threshold OR if already at maximum difficulty
                if (percentage < 60 || QuestionsLogic.IsMaxDifficulty(AppGlobals.SelectedDifficulty))
                {
                    nextLevel.Opacity = 0.4;
                    nextLevel.IsEnabled = false;
                }
                else
                {
                    nextLevel.Opacity = 1.0;
                    nextLevel.IsEnabled = true;
                }
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try { await AnimateMascotsAsync(); }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Animation error: " + ex.Message); }
        }

        private async Task AnimateMascotsAsync()
        {
            var sisa = this.FindByName<Image>("showSisa");
            var oyo = this.FindByName<Image>("showOyo");
            var coin = this.FindByName<Image>("Coin");

            if (sisa != null) sisa.TranslationX = -200;
            if (oyo != null) oyo.TranslationX = 200;
            if (coin != null) { coin.TranslationY = 100; coin.Scale = 0; }

            var tasks = new System.Collections.Generic.List<Task>();

            if (sisa != null) tasks.Add(sisa.TranslateTo(0, 0, 600, Easing.CubicOut));
            if (oyo != null) tasks.Add(oyo.TranslateTo(0, 0, 600, Easing.CubicOut));
            if (coin != null) tasks.Add(coin.TranslateTo(0, 0, 400, Easing.BounceOut));
            if (coin != null) tasks.Add(coin.ScaleTo(1, 400, Easing.BounceOut));
            await Task.WhenAll(tasks);
        }

        private async Task AnimateButtonPress(VisualElement element)
        {
            if (element == null) return;
            await element.ScaleTo(0.92, 80, Easing.CubicIn);
            await element.ScaleTo(1, 120, Easing.CubicOut);
        }

        private async void OnNextLevelClicked(object sender, EventArgs e)
        {
            double percentage = totalQuestions > 0 ? (finalScore * 100.0) / totalQuestions : 0;

            if (percentage >= 60)
            {
                // Move to the next difficulty
                AppGlobals.SelectedDifficulty = QuestionsLogic.GetNextDifficulty(AppGlobals.SelectedDifficulty);
            }

            // Re-map the file for the new difficulty (FileMaps assumed present in project)
            AppGlobals.SelectedQuestionFile = FileMaps.GetFileFor(AppGlobals.SelectedSkill, AppGlobals.SelectedDifficulty);

            // Launch appropriate quiz
            if (string.Equals(AppGlobals.SelectedSkill, "MATH", StringComparison.OrdinalIgnoreCase))
                await Navigation.PushAsync(new Picture_Question());
            else
                await Navigation.PushAsync(new Word_Question());
        }

        // Restart level: reset only current skill cumulative totals and re-launch
        private async void OnRestartLevelClicked(object sender, EventArgs e)
        {
            await AnimateButtonPress(sender as VisualElement);

            AppGlobals.ResetTotalsForSkill(AppGlobals.SelectedSkill);

            // Re-launch the same difficulty & file
            AppGlobals.SelectedQuestionFile = FileMaps.GetFileFor(AppGlobals.SelectedSkill, AppGlobals.SelectedDifficulty);

            if (string.Equals(AppGlobals.SelectedSkill, "MATH", StringComparison.OrdinalIgnoreCase))
                await Navigation.PushAsync(new Picture_Question());
            else
                await Navigation.PushAsync(new Word_Question());
        }

        private async void OnReturnMapClicked(object sender, EventArgs e)
        {
            await AnimateButtonPress(sender as VisualElement);

            // Reset cumulative scoring for both subjects
            AppGlobals.ResetAllTotals();

            if (string.Equals(AppGlobals.SelectedSkill, "MATH", StringComparison.OrdinalIgnoreCase))
                await Navigation.PushAsync(new Math_Level_Select());
            else
                await Navigation.PushAsync(new Eng_Level_Select());
        }

        private async void OnReturnHomeClicked(object sender, EventArgs e)
        {
            await AnimateButtonPress(sender as VisualElement);

            // Reset cumulative scoring for both subjects
            AppGlobals.ResetAllTotals();

            Application.Current.MainPage = new NavigationPage(new Title());
        }
    }
}
