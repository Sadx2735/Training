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
      Play ("APPLE\nAPPLE\nAPPLE\nAPPLE\nAPPLE\nAPPLE\n");
   }

   static void Play (string keys) {
      string output = $"OUTPUT.txt";
      var game = new Wordle (new WordBank ());
      File.Delete (output);
      foreach (char ch in keys) {
         game.UpdateGameState (ToKey (ch));
         game.SaveBoard (output);
      }
      string f1 = "TESTCASE.txt";
      Assert.IsTrue(CheckTextFilesEqual(f1,output));
   }

   static bool CheckTextFilesEqual (string f1, string f2) {
      if (File.Exists (f2) && File.ReadAllText (f1) == File.ReadAllText (f2)) return true;
      Process.Start (@"C:\Program Files\WinMerge\WinMergeU.exe", $"\"{f1}\" \"{f2}\"")?.WaitForExit ();
      return false;
   }

   // Converts a character into a key press: '\n' = Enter, '<' = Backspace, else a letter.
   static ConsoleKeyInfo ToKey (char ch) => ch switch {
      '\n' => new ('\r', ConsoleKey.Enter, false, false, false),
      '-' => new ('\b', ConsoleKey.Backspace, false, false, false),
      _ => new (ch, (ConsoleKey)ch, false, false, false)
   };
}