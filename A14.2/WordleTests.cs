// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// WordleTests.cs
// Tests the Wordle game by simulating key presses and comparing the saved boards.
// ------------------------------------------------------------------------------------------------

using System.Diagnostics;

namespace WordleGame;

#region Class WordleTest --------------------------------------------------------------------------
/// <summary>Plays Wordle with a fixed secret word and compares the saved boards.</summary>
[TestClass]
public class WordleTest {
   #region Tests ----------------------------------------------------
   /// <summary>Normal gameplay where the user wins the game.</summary>
   [TestMethod]
   public void Test1 () {
      Play ("APPLE\nMANGO\nJUICE\nSTORM\nDRAWN\nSTRAW\n", "REFERENCE-1", "STRAW");
   }

   /// <summary>Normal gameplay where the user is unable to guess the word.</summary>
   [TestMethod]
   public void Test2 () {
      Play ("TRASH\nTARTS\nSTARS\nDRIVE\nCHILD\nFEVER\n", "REFERENCE-2", "STRAW");
   }

   /// <summary>To check if the states are maintained when game still in progress.</summary>
   [TestMethod]
   public void Test3 () => Play ("APPLE\nMANGO\n", "REFERENCE-3", "BUILD");

   /// <summary>To check if the states remain the same when the game ends midway.</summary>
   [TestMethod]
   public void Test4 () => Play ("STORM\nSTRAW\n", "REFERENCE-4", "STRAW");

   /// <summary>To check if the game works correctly when backspace is used.</summary>
   [TestMethod]
   public void Test5 () {
      Play ("DRIVE\nDROVE\nCOAST\nCOST<<<<TOAST\nBUILD\n", "REFERENCE-5", "BUILD");
   }

   /// <summary>To check if the invalid word message and backspace work correctly.</summary>
   [TestMethod]
   public void Test6 () {
      Play ("APPLE\nZZZZZ\n<<<<<TRASH\n", "REFERENCE-6", "STRAW");
   }

   /// <summary>To check if program does not process the guess when word is incomplete.</summary>
   [TestMethod]
   public void Test7 () => Play ("APP\nLES\n", "REFERENCE-7", "STRAW");

   /// <summary>To check if backspace works correctly when there are no letters.</summary>
   [TestMethod]
   public void Test8 () => Play ("APPLE\n<<MANGO\n", "REFERENCE-8", "STRAW");
   #endregion

   #region Implementation -------------------------------------------
   // Returns true if both files are equal, else opens WinMerge to show the difference.
   static bool CheckTextFilesEqual (string reference, string output) {
      if (File.Exists (reference) && File.ReadAllText (reference) == File.ReadAllText (output))
         return true;
      Process.Start (@"C:\Program Files\WinMerge\WinMergeU.exe", $"\"{reference}\" \"{output}\"")?
                                                                                   .WaitForExit ();
      return false;
   }

   // Plays the keys with the given secret word and compares the saved boards with the reference.
   static void Play (string keys, string name, string secret) {
      string output = $"{name}-OUTPUT.txt", reference = $"{name}.txt";
      File.Delete (output);
      var game = new Wordle (new WordBank (), secret);
      foreach (char ch in keys) {
         game.UpdateGameState (ToKey (ch));
         game.SaveBoard (output);
      }
      Assert.IsTrue (CheckTextFilesEqual (reference, output), $"{name} does not match");
   }

   // Converts a character into a key press: '\n' = Enter, '<' = Backspace, else a letter.
   static ConsoleKeyInfo ToKey (char ch) => ch switch {
      '\n' => new ('\r', ConsoleKey.Enter, false, false, false),
      '<' => new ('\b', ConsoleKey.Backspace, false, false, false),
      _ => new (ch, (ConsoleKey)ch, false, false, false)
   };
   #endregion
}
#endregion