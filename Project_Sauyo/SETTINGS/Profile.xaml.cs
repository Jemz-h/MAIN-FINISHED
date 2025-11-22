namespace Project_Sauyo
{
    public enum Gender
    {
        None,
        Male,
        Female
    }

    public partial class Profile : ContentPage
    {
        private Gender selectedGender = Gender.None;

        public Profile()
        {
            InitializeComponent();
        }

        private async void MaleClicked(object sender, EventArgs e)
        {
            selectedGender = Gender.Male;

            // Fade out female highlight/check simultaneously
            if (FemaleHighlight.IsVisible || FemaleCheck.IsVisible)
            {
                var fadeOutTasks = new Task[]
                {
            FemaleHighlight.FadeTo(0, 100),
            FemaleCheck.FadeTo(0, 100)
                };
                await Task.WhenAll(fadeOutTasks);
                FemaleHighlight.IsVisible = false;
                FemaleCheck.IsVisible = false;
            }

            // Fade in male highlight/check simultaneously
            MaleHighlight.IsVisible = true;
            MaleCheck.IsVisible = true;
            MaleHighlight.Opacity = 0;
            MaleCheck.Opacity = 0;
            var fadeInTasks = new Task[]
            {
        MaleHighlight.FadeTo(1, 150),
        MaleCheck.FadeTo(1, 150)
            };
            await Task.WhenAll(fadeInTasks);
        }

        private async void FemaleClicked(object sender, EventArgs e)
        {
            selectedGender = Gender.Female;

            if (MaleHighlight.IsVisible || MaleCheck.IsVisible)
            {
                var fadeOutTasks = new Task[]
                {
            MaleHighlight.FadeTo(0, 100),
            MaleCheck.FadeTo(0, 100)
                };
                await Task.WhenAll(fadeOutTasks);
                MaleHighlight.IsVisible = false;
                MaleCheck.IsVisible = false;
            }
       
            FemaleHighlight.IsVisible = true;
            FemaleCheck.IsVisible = true;
            FemaleHighlight.Opacity = 0;
            FemaleCheck.Opacity = 0;
            var fadeInTasks = new Task[]
            {
        FemaleHighlight.FadeTo(1, 150),
        FemaleCheck.FadeTo(1, 150)
            };
            await Task.WhenAll(fadeInTasks);
        }


        private void ConfirmClicked(object sender, EventArgs e)
        {
            // if (selectedGender == Gender.None)
            //     DisplayAlert("Warning", "Please select a gender before confirming.", "OK");
            // else
            //     DisplayAlert("Confirmed", $"You selected: {selectedGender}", "OK");
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            selectedGender = Gender.None;

            MaleCheck.IsVisible = false;
            FemaleCheck.IsVisible = false;

            MaleHighlight.IsVisible = false;
            FemaleHighlight.IsVisible = false;

            Navigation.PopAsync();
        }
    }
}
