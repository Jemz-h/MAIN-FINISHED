using System.Text;


namespace Project_Sauyo
{
    public class Question
    {
        // ID (optional), image filename (relative to Resources/Images or same folder), text, 4 choices, answer index (1..4), explanation
        public string ID { get; set; } = "";
        public string ImageFile { get; set; } = "";
        public string Text { get; set; } = "";
        public string Choice1 { get; set; } = "";
        public string Choice2 { get; set; } = "";
        public string Choice3 { get; set; } = "";
        public string Choice4 { get; set; } = "";
        public int Answer { get; set; } = 1;
        public string Explanation { get; set; } = "";

        public string GetCorrectAnswer()
        {
            return Answer switch
            {
                1 => Choice1,
                2 => Choice2,
                3 => Choice3,
                4 => Choice4,
                _ => Choice1
            };
        }
    }

    public static class QuestionLoader
    {
        /// <summary>
        /// Load questions from an embedded MAUI asset text file.
        /// Supports blocks of 7..9 lines:
        /// - 9-line: ID, Image, Text, C1, C2, C3, C4, Answer, Explanation
        /// - 8-line: Image, Text, C1, C2, C3, C4, Answer, Explanation
        /// - 7-line: Text, C1, C2, C3, C4, Answer, Explanation (no image)
        /// Returns empty list on failure (logs to Debug).
        /// </summary>
        public static async Task<List<Question>> LoadFromFile(string filename)
        {
            var questions = new List<Question>();
            try
            {
                System.Diagnostics.Debug.WriteLine($"📗 Loading questions from: {filename}");

                using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
                using var reader = new StreamReader(stream, Encoding.UTF8);

                var allText = await reader.ReadToEndAsync();

                // Normalize endings and split blocks by blank line(s)
                var normalized = allText.Replace("\r\n", "\n").Replace("\r", "\n");
                var blocks = normalized
                    .Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(b => b.Trim())
                    .Where(b => !string.IsNullOrWhiteSpace(b))
                    .ToArray();

                System.Diagnostics.Debug.WriteLine($"📄 Total question blocks found: {blocks.Length}");

                foreach (var block in blocks)
                {
                    var lines = block
                        .Split('\n')
                        .Select(l => l.Trim())
                        .Where(l => l.Length > 0)
                        .ToArray();

                    try
                    {
                        if (lines.Length >= 9)
                        {
                            // ID, Image, Text, C1, C2, C3, C4, Answer, Explanation
                            var choices = new[] { lines[3], lines[4], lines[5], lines[6] };
                            var answerIndex = DetermineAnswerIndex(lines[7], choices);
                            var q = new Question
                            {
                                ID = lines[0],
                                ImageFile = lines[1],
                                Text = lines[2],
                                Choice1 = choices[0],
                                Choice2 = choices[1],
                                Choice3 = choices[2],
                                Choice4 = choices[3],
                                Answer = answerIndex,
                                Explanation = lines[8]
                            };
                            questions.Add(q);
                        }
                        else if (lines.Length == 8)
                        {
                            // Image, Text, C1, C2, C3, C4, Answer, Explanation
                            var choices = new[] { lines[2], lines[3], lines[4], lines[5] };
                            var answerIndex = DetermineAnswerIndex(lines[6], choices);
                            var q = new Question
                            {
                                ImageFile = lines[0],
                                Text = lines[1],
                                Choice1 = choices[0],
                                Choice2 = choices[1],
                                Choice3 = choices[2],
                                Choice4 = choices[3],
                                Answer = answerIndex,
                                Explanation = lines[7]
                            };
                            questions.Add(q);
                        }
                        else if (lines.Length == 7)
                        {
                            // Text, C1, C2, C3, C4, Answer, Explanation (no image)
                            var choices = new[] { lines[1], lines[2], lines[3], lines[4] };
                            var answerIndex = DetermineAnswerIndex(lines[5], choices);
                            var q = new Question
                            {
                                ImageFile = string.Empty,
                                Text = lines[0],
                                Choice1 = choices[0],
                                Choice2 = choices[1],
                                Choice3 = choices[2],
                                Choice4 = choices[3],
                                Answer = answerIndex,
                                Explanation = lines[6]
                            };
                            questions.Add(q);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"⚠️ Skipping block with unexpected line count: {lines.Length}");
                        }
                    }
                    catch (Exception exBlock)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Failed parsing block: {exBlock.Message}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"✅ LOADED {questions.Count} QUESTIONS.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ FAILED TO LOAD FILE");
                System.Diagnostics.Debug.WriteLine($"   File: {filename}");
                System.Diagnostics.Debug.WriteLine($"   ERROR: {ex.Message}");
            }

            return questions;
        }
        static int DetermineAnswerIndex(string answerString, string[] choices)
        {
            if (string.IsNullOrWhiteSpace(answerString))
                return 1;

            var s = answerString.Trim();

            // 1) try integer
            if (int.TryParse(s, out var n))
                return Math.Clamp(n, 1, 4);

            // 2) single letter A-D
            if (s.Length == 1)
            {
                char c = char.ToUpperInvariant(s[0]);
                if (c >= 'A' && c <= 'D')
                    return (c - 'A') + 1;
            }

            // 3) try to match to one of the choices (case-insensitive)
            if (choices != null && choices.Length >= 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (string.Equals(choices[i]?.Trim(), s, StringComparison.OrdinalIgnoreCase))
                        return i + 1;
                }
            }

            // 4) if answer string is like "Choice 2" / "choice2" / "choice 2"
            var low = s.ToLowerInvariant().Replace(" ", "");
            if (low.StartsWith("choice") && low.Length > 6)
            {
                var tail = low.Substring(6);
                if (int.TryParse(tail, out var m))
                    return Math.Clamp(m, 1, 4);
            }

            // fallback
            return 1;
        }

        static int ParseAnswer(string s)
        {
            // kept for backward compatibility — prefer DetermineAnswerIndex where choices are available
            if (string.IsNullOrWhiteSpace(s)) return 1;
            s = s.Trim();

            if (int.TryParse(s, out var n))
                return Math.Clamp(n, 1, 4);

            var low = s.ToLowerInvariant();
            if (low.StartsWith("choice") && low.Length >= 7)
            {
                if (int.TryParse(low.Substring(6).Trim(), out var m))
                    return Math.Clamp(m, 1, 4);
            }

            // letters
            if (s.Length == 1)
            {
                char c = char.ToUpperInvariant(s[0]);
                if (c >= 'A' && c <= 'D')
                    return (c - 'A') + 1;
            }

            return 1;
        }

        // Fisher-Yates shuffle extension (returns new list)
        public static List<T> Shuffle<T>(this List<T> source, Random rng = null)
        {
            if (source == null) return new List<T>();
            rng ??= new Random();
            var arr = source.ToArray();
            for (int i = arr.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = arr[i];
                arr[i] = arr[j];
                arr[j] = tmp;
            }
            return arr.ToList();
        }

        public static Question GetNext(List<Question> list, int index)
        {
            if (list == null || index < 0 || index >= list.Count) return null;
            return list[index];
        }
    }
}
