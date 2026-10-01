// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Words.cs
// Helps us with accessing words from the embedded word lists.
// ------------------------------------------------------------------------------------------------

using System.Reflection;

namespace WordleGame;

#region Class WordBank ----------------------------------------------------------------------------
/// <summary>Gives access to the dictionary of valid guesses.</summary>
class WordBank {
   #region Constructors ---------------------------------------------
   /// <summary>Loads the dictionary from the embedded resources.</summary>
   public WordBank () => mDictWords = LoadStrings ("dict-5.txt");
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Checks if the given word is in the dictionary.</summary>
   /// <param name="word">Word guessed by the user.</param>
   /// <returns>True if the word exists, else false.</returns>
   public bool IsValidWord (string word) => mDictWords.Contains (word);
   #endregion

   #region Implementation -------------------------------------------
   // Reads an embedded resource file and returns its lines (trimmed, without empty lines).
   string[] LoadStrings (string file) {
      using var reader = new StreamReader (Assembly.GetExecutingAssembly ()
         .GetManifestResourceStream ($"A14._2.data.{file}")!);
      return reader.ReadToEnd ()
         .Split ('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
   }
   #endregion

   #region Fields ---------------------------------------------------
   string[] mDictWords;
   #endregion
}
#endregion