using CommunityToolkit.Maui.Views;

namespace Project_Sauyo
{
    public partial class Answer : Popup
    {
        public Answer()
        {
            InitializeComponent();
        }

        // Method to set the answer details
        public void SetAnswerDetails(string result, string answerText, string explanation)
        {
            Result_Response.Text = result;           // "CORRECT! 🎉" or "NICE TRY! ❌"
            AnswerLabel.Text = answerText;           // "The answer is: Dog"
            Factlbl.Text = "FUN FACT! 💡";          // Always "FUN FACT!"
            Explanationlbl.Text = explanation;       // "Dogs are loyal pets!"
        }

        private async void OnContinueClicked(object sender, EventArgs e)
        {
            await this.CloseAsync();
        }
    }
}