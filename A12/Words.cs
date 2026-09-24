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
   public WordBank () {
      mWords = File.ReadAllLines ("puzzle-5.txt");
      mDictWords = File.ReadAllLines ("dict-5.txt");
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Gives all the words that are present in the dictionary</summary>
   /// <returns>Returns array of all words in the dictionary.</returns>
   public string[] GetAllWords () => mDictWords;

   /// <summary>Generates a random word from the dictionary</summary>
   /// <returns>The generated random word</returns>
   public string GetRandomWord () => mWords[Random.Shared.Next (mWords.Length)];

   /// <summary>Tells if the particular user input is valid or not</summary>
   /// <param name="word">given by the user</param>
   /// <returns>true if the word exist , false if it doesn't exist.</returns>
   public bool IsValidWord (string word) => mDictWords.Contains (word);
   #endregion

   #region Fields ---------------------------------------------------
   string[] mWords;
   string[] mDictWords;
   #endregion
}
#endregion