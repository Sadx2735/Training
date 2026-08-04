// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Solves a New York Times-style Spelling Bee puzzle.
// ------------------------------------------------------------------------------------------------

#region Program -----------------------------------------------------
/// <summary>Reads a word list file, filters valid Spelling Bee words, scores them, and displays 
/// the sorted results.</summary>
char[] letters = { 'U', 'X', 'A', 'L', 'T', 'N', 'E' };
string filePath = "WordsList.txt";
string[] words = File.ReadAllLines (filePath);
List<(int score, string word, bool isPangram)> result = [];
int totalScore = 0;
foreach (var rawWord in words) {
   string word = rawWord.Trim ().ToUpper ();
   // Go to the next word if any of the below 3 conditions are met.
   if (word.Length < 4) continue;
   if (word.Any (c => !letters.Contains (c))) continue;
   if (!word.Contains (letters[0])) continue;
   // If it's a valid word, then calculate its score.
   bool isPangram = IsPangram (word);
   int score = GetScore (word, isPangram);
   result.Add ((score, word, isPangram));
}
var sortedResult = result.OrderByDescending (item => item.score).ThenBy (item => item.word);
foreach (var (score, word, isPangram) in sortedResult) {
   totalScore += score;
   if (isPangram)
      Console.ForegroundColor = ConsoleColor.Green;
   Console.WriteLine ($"{score,2}. {word}");
   if (isPangram)
      Console.ResetColor ();
}
Console.WriteLine ("----");
Console.WriteLine ($"Total score: {totalScore}");
#endregion

#region Implementation ----------------------------------------------
/// <summary>Checks whether all required seed letters are present in the word.</summary>
/// <returns>Returns true if the word is a pangram; otherwise, false.</returns>
bool IsPangram (string word) => letters.All (ch => word.Contains (ch));

/// <summary>Calculates the word score based on length and pangram bonus status.</summary>
/// <returns>Returns the calculated score for the given word.</returns>
int GetScore (string word, bool isPangram) {
   int baseScore = (word.Length == 4) ? 1 : word.Length;
   return baseScore + (isPangram ? 7 : 0);
}
#endregion