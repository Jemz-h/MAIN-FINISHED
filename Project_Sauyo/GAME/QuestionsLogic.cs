namespace Project_Sauyo
{
    public static class QuestionsLogic
    {
        /// <summary>
        /// Returns how many questions to show for the given difficulty.
        /// OPTION A behaviour: every level shows 5 questions.
        /// </summary>
        public static int GetQuestionsToShow(string difficulty)
        {
            if (string.Equals(difficulty, "EASY", StringComparison.OrdinalIgnoreCase)) return 5;
            if (string.Equals(difficulty, "AVERAGE", StringComparison.OrdinalIgnoreCase)) return 5;
            if (string.Equals(difficulty, "DIFFICULT", StringComparison.OrdinalIgnoreCase)) return 5;
            // default
            return 5;
        }

        /// <summary>
        /// Returns the next difficulty string (or same if already max).
        /// </summary>
        public static string GetNextDifficulty(string difficulty)
        {
            if (string.Equals(difficulty, "EASY", StringComparison.OrdinalIgnoreCase)) return "AVERAGE";
            if (string.Equals(difficulty, "AVERAGE", StringComparison.OrdinalIgnoreCase)) return "DIFFICULT";
            return "DIFFICULT";
        }

        /// <summary>
        /// Returns whether the difficulty is max (DIFFICULT).
        /// </summary>
        public static bool IsMaxDifficulty(string difficulty)
        {
            return string.Equals(difficulty, "DIFFICULT", StringComparison.OrdinalIgnoreCase);
        }
    }
}
