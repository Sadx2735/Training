// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// A program to help solve a New-York Times style Spelling Bee.

#region Program -----------------------------------------------------------------------------------

/// <summary>
/// Opens the file and iterates through every word. Criteria checked:
/// 1. Word length is at least 4 letters.
/// 2. All characters in the word belong to the allowed seed letters.
/// 3. The word contains the mandatory first seed letter (U).
/// Valid words are scored, categorized, and sorted before printing.
/// </summary>

char[] letters = { 'U', 'X', 'A', 'L', 'T', 'N', 'E' };
string filePath = @"C:\Users\msara\Downloads\words-List.txt";

if (!File.Exists (filePath)) {
   Console.WriteLine ($"Error: Word list file not found at {filePath}");
   return;
}

string[] words = File.ReadAllLines (filePath);
List<(int score, string word, bool isPangram)> result = new ();
int totalScore = 0;

foreach (var rawWord in words) {
   string word = rawWord.Trim ().ToUpper ();
   if (word.Length < 4) continue;
   if (word.Any (c => !letters.Contains (c))) continue;
   if (!word.Contains (letters[0])) continue;

   bool isPangram = IsPangram (word);
   int score = GetScore (word, isPangram);
   result.Add ((score, word, isPangram));
}

var sortedResult = result.OrderByDescending (item => item.score).ThenBy (item => item.word);

foreach (var item in sortedResult) {
   totalScore += item.score;

   if (item.isPangram) 
      Console.ForegroundColor = ConsoleColor.Green;

   Console.WriteLine ($"{item.score,2}. {item.word}");
   
   if (item.isPangram) 
      Console.ResetColor ();
}

Console.WriteLine ("----");
Console.WriteLine ($"{totalScore} is the total score");

#region Implementations ---------------------------------------------------------------------------

/// <summary>
/// Iterates through every character in the letters array to check if all of them are present in the word.
/// </summary>
bool IsPangram (string word) {
   return letters.All (character => word.Contains (character));
}

/// <summary>
/// Assigns a score: 4-letter words score 1 point, longer words score their exact length.
/// Additionally, if it is a pangram, 7 bonus points are added.
/// </summary>
int GetScore (string word, bool isPangram) {
   int baseScore = (word.Length == 4) ? 1 : word.Length;
   return baseScore + (isPangram ? 7 : 0);
}

#endregion
#endregion