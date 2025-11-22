using System;

namespace Project_Sauyo
{
    public static class AppGlobals
    {
        // Current selection
        // "MATH" or "ENG"
        public static string SelectedSkill { get; set; } = "MATH";
        // "EASY" / "AVERAGE" / "DIFFICULT"
        public static string SelectedDifficulty { get; set; } = "EASY";
        public static string SelectedQuestionFile { get; set; } = string.Empty;

        // Last question image shown (used by Analytics to display picture)
        public static string LastQuestionImagePath { get; set; } = string.Empty;

        // Separate cumulative totals per subject
        public static int MathTotalCorrect { get; set; } = 0;
        public static int MathTotalQuestions { get; set; } = 0;

        public static int EngTotalCorrect { get; set; } = 0;
        public static int EngTotalQuestions { get; set; } = 0;

        // Helpers
        public static void ResetAllTotals()
        {
            MathTotalCorrect = 0;
            MathTotalQuestions = 0;
            EngTotalCorrect = 0;
            EngTotalQuestions = 0;
        }

        public static void ResetTotalsForSkill(string skill)
        {
            if (string.Equals(skill, "MATH", StringComparison.OrdinalIgnoreCase))
            {
                MathTotalCorrect = 0;
                MathTotalQuestions = 0;
            }
            else
            {
                EngTotalCorrect = 0;
                EngTotalQuestions = 0;
            }
        }
    }
}
