using System.Collections.Generic;

namespace Project_Sauyo
{
    public static class FileMaps
    {
        // Math question files for each difficulty
        public static readonly Dictionary<string, string> MathFiles = new()
        {
            { "EASY", "Numeracy/NW1_4-5_Easy.txt" },
            { "AVERAGE", "Numeracy/NW1_4-5_Average.txt" },
            { "DIFFICULT", "Numeracy/NW1_4-5_Difficult.txt" }
        };

        // English question files for each difficulty
        public static readonly Dictionary<string, string> EngFiles = new()
        {
            { "EASY", "Reading/RW1EASY.txt" },
            { "AVERAGE", "Reading/RW1AVERAGE.txt" },
            { "DIFFICULT", "Reading/RW1DIFFICULT.txt" }
        };

        // Returns the file path for the current skill & difficulty
        public static string GetFileFor(string skill, string difficulty)
        {
            difficulty = difficulty.ToUpperInvariant();

            if (skill.ToUpperInvariant() == "MATH")
            {
                if (MathFiles.TryGetValue(difficulty, out var f))
                    return f;

                return MathFiles["EASY"]; // fallback
            }
            else 
            {
                if (EngFiles.TryGetValue(difficulty, out var f))
                    return f;

                return EngFiles["EASY"]; // fallback
            }
        }
    }
}
