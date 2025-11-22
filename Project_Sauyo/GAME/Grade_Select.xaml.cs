using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Project_Sauyo
{
    public partial class Grade_Select : ContentPage
    {
        private int currentIndex = 0;

        private readonly string[] monuments = { "rizalmonu.png", "intramuros.png", "edsa.png" };

        private readonly Dictionary<string, string> gradeMap = new()
        {
            { "rizalmonu.png", "GRADE 1" },
            { "intramuros.png", "GRADE 2" },
            { "edsa.png", "GRADE 3" }
        };

        public Grade_Select()
        {
            InitializeComponent();
            RegisterSwipeGestures();
            UpdateCarousel(animated: false);
        }

        // =====================================================
        // SWIPE GESTURES
        // =====================================================
        private void RegisterSwipeGestures()
        {
            var swipeLeft = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            swipeLeft.Swiped += OnSwipedLeft;

            var swipeRight = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            swipeRight.Swiped += OnSwipedRight;

            // Add gestures to the MAIN image (the center one)
            CenterImage.GestureRecognizers.Add(swipeLeft);
            CenterImage.GestureRecognizers.Add(swipeRight);

            // Also add to left + right images for convenience
            LeftImage.GestureRecognizers.Add(swipeLeft);
            LeftImage.GestureRecognizers.Add(swipeRight);
            RightImage.GestureRecognizers.Add(swipeLeft);
            RightImage.GestureRecognizers.Add(swipeRight);
        }

        private void OnSwipedLeft(object sender, SwipedEventArgs e)
        {
            SlideCarouselLeft();
        }

        private void OnSwipedRight(object sender, SwipedEventArgs e)
        {
            SlideCarouselRight();
        }

        // =====================================================
        // ANIMATED SLIDE TRANSITION
        // =====================================================
        private async void SlideCarouselLeft()
        {
            await AnimateSlide(-300); // slide left
            currentIndex = (currentIndex + 1) % monuments.Length;
            UpdateCarousel(animated: false);
            await AnimateBounce();
        }

        private async void SlideCarouselRight()
        {
            await AnimateSlide(+300); // slide right
            currentIndex = (currentIndex - 1 + monuments.Length) % monuments.Length;
            UpdateCarousel(animated: false);
            await AnimateBounce();
        }

        private Task AnimateSlide(double distance)
        {
            const uint duration = 250;

            return Task.WhenAll(
                LeftImage.TranslateTo(distance, 0, duration, Easing.CubicIn),
                CenterImage.TranslateTo(distance, 0, duration, Easing.CubicIn),
                RightImage.TranslateTo(distance, 0, duration, Easing.CubicIn)
            );
        }

        private Task AnimateBounce()
        {
            const uint duration = 150;

            // Reset positions
            LeftImage.TranslationX = 0;
            CenterImage.TranslationX = 0;
            RightImage.TranslationX = 0;

            // Slight bounce scale
            CenterImage.Scale = 0.9;

            return CenterImage.ScaleTo(1, duration, Easing.BounceOut);
        }

        // =====================================================
        // NON-ANIMATED CONTENT SWITCHING
        // =====================================================
        private void UpdateCarousel(bool animated)
        {
            int leftIndex = (currentIndex - 1 + monuments.Length) % monuments.Length;
            int centerIndex = currentIndex;
            int rightIndex = (currentIndex + 1) % monuments.Length;

            LeftImage.Source = monuments[leftIndex];
            CenterImage.Source = monuments[centerIndex];
            RightImage.Source = monuments[rightIndex];

            string gradeText = gradeMap[monuments[centerIndex]];
            GradeLabel.Text = gradeText;
            GradeLabelShadow.Text = gradeText;

            // Scale emphasis
            LeftImage.Scale = 0.7;
            CenterImage.Scale = 1.0;
            RightImage.Scale = 0.7;
        }

        // =====================================================
        // POPUP
        // =====================================================
        private async void OnImageTapped(object sender, TappedEventArgs e)
        {
            Overlay.IsVisible = true;
            PopupContainer.Opacity = 0;
            PopupContainer.IsVisible = true;

            await PopupContainer.FadeTo(1, 250);
        }

        private async void OnClosePopupClicked(object sender, EventArgs e)
        {
            await PopupContainer.FadeTo(0, 200);
            PopupContainer.IsVisible = false;
            Overlay.IsVisible = false;
        }

        // =====================================================
        // NAVIGATION BUTTONS
        // =====================================================
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
                await Navigation.PopAsync();
        }

        private async void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }

        // =====================================================
        // SUBJECT BUTTONS
        // =====================================================
        private async void OnMathClicked(object sender, EventArgs e)
        {
            AppGlobals.SelectedSkill = "MATH";
            AppGlobals.SelectedDifficulty = "Easy";
            AppGlobals.SelectedQuestionFile = null;

            await Navigation.PushAsync(new Math_Level_Select());
        }

        private async void OnEnglishClicked(object sender, EventArgs e)
        {
            AppGlobals.SelectedSkill = "ENG";
            AppGlobals.SelectedDifficulty = "Easy";
            AppGlobals.SelectedQuestionFile = null;

            await Navigation.PushAsync(new Eng_Level_Select());
        }
    }
}
