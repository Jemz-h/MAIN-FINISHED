using CommunityToolkit.Maui.Extensions;

namespace Project_Sauyo
{
    public partial class Picture_Question : ContentPage
    {
        List<Question> allQuestions = new();
        Question currentQuestion;
        int score = 0;
        int currentQuestionIndex = 0;
        int questionsToShow = 0;
        int skipsRemaining = 3;

        public Picture_Question()
        {
            InitializeComponent();
            SkipLabel.Text = $"Available Skip: {skipsRemaining}";
            DifficultyLabel.Text = AppGlobals.SelectedDifficulty ?? "EASY";

            // Ensure this page uses MATH skill
            AppGlobals.SelectedSkill = "MATH";

            LoadQuestionsAsync();
        }

        private async void LoadQuestionsAsync()
        {
            if (string.IsNullOrEmpty(AppGlobals.SelectedQuestionFile))
            {
                QuestionTextLabel.Text = "⚠️ No question file selected!";
                DisableButtons();
                return;
            }

            allQuestions = await QuestionLoader.LoadFromFile(AppGlobals.SelectedQuestionFile);

            if (allQuestions == null || allQuestions.Count == 0)
            {
                QuestionTextLabel.Text = "⚠️ No questions found!";
                DisableButtons();
                return;
            }

            // Use central logic to decide how many questions this level should show (OPTION A: 5)
            questionsToShow = QuestionsLogic.GetQuestionsToShow(AppGlobals.SelectedDifficulty);

            // shuffle and trim
            allQuestions = QuestionLoader.Shuffle(allQuestions);
            if (allQuestions.Count > questionsToShow)
                allQuestions = allQuestions.GetRange(0, questionsToShow);

            currentQuestionIndex = 0;
            score = 0;
            ShowNextQuestion();
        }

        private async void ShowNextQuestion()
        {
            if (currentQuestionIndex >= questionsToShow || currentQuestionIndex >= allQuestions.Count)
            {
                AppGlobals.LastQuestionImagePath = currentQuestion?.ImageFile ?? string.Empty;

                // Accumulate this session's results into MATH totals (questionsToShow is correct per level)
                AppGlobals.MathTotalCorrect += score;
                AppGlobals.MathTotalQuestions += questionsToShow;

                // Navigate to analytics (reads AppGlobals)
                await Navigation.PushAsync(new Analytics());
                return;
            }

            currentQuestion = allQuestions[currentQuestionIndex];

            QuestionNumber.Text = (currentQuestionIndex + 1).ToString();
            QuestionTextLabel.Text = currentQuestion.Text;

            CBtn1.Text = currentQuestion.Choice1;
            CBtn2.Text = currentQuestion.Choice2;
            CBtn3.Text = currentQuestion.Choice3;
            CBtn4.Text = currentQuestion.Choice4;

            await LoadQuestionImage();

            currentQuestionIndex++;
            EnableButtons();
            ResetButtonColors();

            ScoreLabel.Text = $"Score: {score}/{currentQuestionIndex}";
        }

        private async Task LoadQuestionImage()
        {
            if (string.IsNullOrWhiteSpace(currentQuestion?.ImageFile))
            {
                ImageBorder.IsVisible = false;
                QuestionImage.Source = null;
                return;
            }

            ImageBorder.IsVisible = true;

            try
            {
                string folder = Path.GetDirectoryName(AppGlobals.SelectedQuestionFile) ?? "";
                string fullPath = Path.Combine(folder, currentQuestion.ImageFile).Replace("\\", "/");

                System.Diagnostics.Debug.WriteLine($"📷 Trying image: {fullPath}");

                using var fileStream = await FileSystem.OpenAppPackageFileAsync(fullPath);

                MemoryStream memStream = new MemoryStream();
                await fileStream.CopyToAsync(memStream);
                memStream.Position = 0;

                QuestionImage.Source = ImageSource.FromStream(() => memStream);
                System.Diagnostics.Debug.WriteLine("📷 Image loaded successfully!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ IMAGE LOAD FAILED: {ex.Message}");
                ImageBorder.IsVisible = false;
                QuestionImage.Source = null;
            }
        }

        private async void OnAnswerButtonClicked(object sender, EventArgs e)
        {
            if (currentQuestion == null) return;
            var btn = sender as Button;
            int selected = GetButtonNumber(btn);
            bool isCorrect = selected == currentQuestion.Answer;

            if (isCorrect)
            {
                score++;
            }

            if (btn != null)
                VisualStateManager.GoToState(btn, isCorrect ? "CorrectAnswer" : "WrongAnswer");

            DisableButtons();

            var popup = new Answer();
            popup.SetAnswerDetails(isCorrect ? "CORRECT! 🎉" : "NICE TRY! ❌",
                $"The answer is: {currentQuestion.GetCorrectAnswer()}",
                currentQuestion.Explanation);

            await this.ShowPopupAsync(popup);
            ShowNextQuestion();
        }

        private async void OnSkipButtonClicked(object sender, EventArgs e)
        {
            await Bounce(SkipButton);

            if (skipsRemaining > 0)
            {
                skipsRemaining--;
                SkipLabel.Text = $"Available Skip: {skipsRemaining}";

                ShowNextQuestion();
                return;
            }
            await Shake(SkipButton);
        }

        private int GetButtonNumber(Button btn)
        {
            if (btn == CBtn1) return 1;
            if (btn == CBtn2) return 2;
            if (btn == CBtn3) return 3;
            if (btn == CBtn4) return 4;
            return 0;
        }

        private void EnableButtons()
        {
            CBtn1.IsEnabled = true;
            CBtn2.IsEnabled = true;
            CBtn3.IsEnabled = true;
            CBtn4.IsEnabled = true;

            SkipButton.IsEnabled = true;
        }

        private void DisableButtons()
        {
            CBtn1.IsEnabled = CBtn2.IsEnabled = CBtn3.IsEnabled = CBtn4.IsEnabled = false;
        }

        private void ResetButtonColors()
        {
            VisualStateManager.GoToState(CBtn1, "Normal");
            VisualStateManager.GoToState(CBtn2, "Normal");
            VisualStateManager.GoToState(CBtn3, "Normal");
            VisualStateManager.GoToState(CBtn4, "Normal");
        }

        private async Task ButtonClickFeedback(VisualElement element)
        {
            if (element == null) return;
            await element.ScaleTo(0.92, 80, Easing.CubicIn);
            await element.ScaleTo(1.0, 120, Easing.CubicOut);
        }

        private async Task Shake(View view)
        {
            if (view == null) return;
            view.TranslationX = 0;
            view.Rotation = 0;

            const int distance = 12;
            const double angle = 8;
            const uint speed = 40;

            await Task.WhenAll(
                view.TranslateTo(-distance, 0, speed),
                view.RotateTo(-angle, speed)
            );

            await Task.WhenAll(
                view.TranslateTo(distance, 0, speed),
                view.RotateTo(angle, speed)
            );

            await Task.WhenAll(
                view.TranslateTo(-distance, 0, speed),
                view.RotateTo(-angle, speed)
            );

            await Task.WhenAll(
                view.TranslateTo(distance, 0, speed),
                view.RotateTo(angle, speed)
            );
            await Task.WhenAll(
                view.TranslateTo(0, 0, speed),
                view.RotateTo(0, speed)
            );
        }

        private async Task Bounce(View view)
        {
            if (view == null) return;

            await view.ScaleTo(0.9, 70, Easing.CubicIn);
            await view.ScaleTo(1.0, 120, Easing.CubicOut);
        }

        private async void OnCloseClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
                await Navigation.PopAsync();
            else
                await Navigation.PopToRootAsync();
        }
    }
}
