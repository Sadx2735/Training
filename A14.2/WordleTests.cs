// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// WordleTests.cs
// Tests the Wordle game by simulating key presses and comparing the saved boards.
// ------------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.Xml.Linq;
namespace WordleGame;

[TestClass]
public class WordleTest {

   [TestMethod]
   public void TestKeystrokes () {
      // TEST FOR COLOR CHECK & CORRECT PLAY.
      Play ("APPLE\nMANGO\nJUICE\nSTORM\nDRAWN\nSTRAW\n", "REFERENCE-1.txt", "STRAW");
      // TEST FOR COLOR CHECK & WRONG PLAY.
      Play ("TRASH\nTARTS\nSTARS\nDRIVE\nCHILD\nFEVER\n", "REFERENCE-2.txt", "STRAW");
      // TEST FOR SOMEOTHER WORD & INTERMEDIATE TERMINATION.
      Play ("APPLE\nMANGO\n", "REFERENCE-3.txt", "BUILD");
      // TEST FOR INTERMEDIATE EXIT.
      Play ("STORM\nSTRAW\n", "REFERENCE-4.txt", "STRAW");
      // TEST FOR SOMEOTHER WORD & BACKSPACE.
      Play ("DRIVE\nDROVE\nCOAST\nCOST<<<<TOAST\nBUILD\n", "REFERENCE-5.txt", "BUILD");
      // Invalid word shows the message, then a valid word on the same row.
      Play ("APPLE\nZZZZZ\n<<<<<TRASH\n", "REFERENCE-6.txt", "STRAW");
      // Enter on an incomplete row is ignored, and a 6th letter is ignored.
      Play ("APP\nLES\n", "REFERENCE-7.txt", "STRAW");
      // Backspace at the start of a new row must not touch the previous row.
      Play ("APPLE\n<<MANGO\n", "REFERENCE-8.txt", "STRAW");
   }

   static void Play (string keys, string reference, string expected) {
      string output = $"OUTPUT.txt";
      var game = new Wordle (new WordBank (), expected);
      File.Delete (output);
      foreach (char ch in keys) {
         game.UpdateGameState (ToKey (ch));
         game.SaveBoard (output);
      }
      Assert.IsTrue (CheckTextFilesEqual (reference, output));
   }

   static bool CheckTextFilesEqual (string f1, string f2) {
      if (File.Exists (f2) && File.ReadAllText (f1) == File.ReadAllText (f2)) return true;
      Process.Start (@"C:\Program Files\WinMerge\WinMergeU.exe", $"\"{f1}\" \"{f2}\"")?.WaitForExit ();
      return false;
   }

   // Converts a character into a key press: '\n' = Enter, '<' = Backspace, else a letter.
   static ConsoleKeyInfo ToKey (char ch) => ch switch {
      '\n' => new ('\r', ConsoleKey.Enter, false, false, false),
      '<' => new ('\b', ConsoleKey.Backspace, false, false, false),
      _ => new (ch, (ConsoleKey)ch, false, false, false)
   };
}