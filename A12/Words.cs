// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Words.cs
// Helps us with accessing words from the word list.
// ------------------------------------------------------------------------------------------------

namespace WordleGame;

#region Class WordBank ----------------------------------------------------------------------------
/// <summary>Consists of the useful methods that helps us with accessing words for game.</summary>
class WordBank {
   #region Constructor ----------------------------------------------
   /// <summary>Loads all the words in the variables</summary>
   /// <param name="puzzlePath">Name of the file from with the random word be taken</param>
   /// <param name="dictPath">Name of the file in which the user guess is checked for valid</param>
   public WordBank (string puzzlePath, string dictPath) {
      availableWords = File.ReadAllLines (puzzlePath);
      dictionaryWords = File.ReadAllLines (dictPath);
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Generates a random word from the dictionary</summary>
   /// <returns>The generated random word</returns>
   public string GetRandomWord () {
      int index = randomizer.Next (availableWords.Length);
      return availableWords[index];
   }
   /// <summary>Tells if the particular user input is valid or not</summary>
   /// <param name="word">given by the user</param>
   /// <returns>true if the word exist , false if it doesn't exist.</returns>
   public bool IsValidWord (string word) {
      return dictionaryWords.Contains (word);
   }
   #endregion

   #region Fields ---------------------------------------------------
   string[] availableWords;
   string[] dictionaryWords;
   Random randomizer = new Random ();
   #endregion
}
#endregion