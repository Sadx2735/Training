// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Entry point for the wordle game.
// ------------------------------------------------------------------------------------------------

using System.Text;
using static System.Console;

namespace WordleGame;

class Program {
   static void Main () {
      OutputEncoding = Encoding.UTF8;
      CursorVisible = false;
      try {
         var bank = new WordBank ();
         var game = new Wordle (bank);
         game.Run ();
      } catch (Exception ex) {
         WriteLine ($"Error loading files: {ex.Message}");
      }
   }
}