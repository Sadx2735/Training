// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Entry point for the wordle game.
// ------------------------------------------------------------------------------------------------

using System.Text;

namespace WordleGame;

class Program {
   static void Main () {
      Console.OutputEncoding = Encoding.UTF8;
      Console.CursorVisible = false;

      string puzzlePath = @"C:\Work\Training\A12\puzzle-5.txt";
      string dictPath = @"C:\Work\Training\A12\dict-5.txt";

      try {
         WordBank bank = new WordBank (puzzlePath, dictPath);
         var game = new Wordle (bank);
         game.Run ();
      } catch (Exception ex) {
         Console.WriteLine ($"Error loading files: {ex.Message}");
      }
   }
}