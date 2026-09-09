// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Groups and displays anagrams from a text file.
// ------------------------------------------------------------------------------------------------

namespace Anagram;

#region Program  ----------------------------------------------------------------------------------
/// <summary>Reads a word list and prints grouped anagrams.</summary>
class Program {
   /// <summary>Groups anagrams from the word file and outputs them to the console.</summary>
   static void Main () {
      var anagrams = File.ReadAllLines ("words.txt")
         .GroupBy (word => string.Concat (word.Order ()))
         .Where (group => group.Skip (1).Any ())
         .Select (group => group.ToArray ())
         .OrderByDescending (group => group.Length);
      foreach (var group in anagrams)
         Console.WriteLine ($"{group.Length} {string.Join (" ", group)}");
   }
}
#endregion