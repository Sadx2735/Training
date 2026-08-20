// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Main entry point file that invokes the N-Queens solver.
// ------------------------------------------------------------------------------------------------
using static System.Console;
using NQueenSolver;

#region Class Program -----------------------------------------------------------------------------
class Program {
   #region Methods --------------------------------------------------
   /// <summary>Solves N-Queens for the board size entered by the user.</summary>
   static void Main () => new NQueensSolver (PromptBoardSize ()).Run ();

   /// <summary>Asks the user repeatedly to provide a board size.</summary>
   /// <returns>The specified board size.</returns>
   static int PromptBoardSize () {
      while (true) {
         Write ("Enter your board size: ");
         if (int.TryParse (ReadLine (), out int result) && result > 0) return result;
         WriteLine ("Please enter a valid positive number.");
      }
   }
   #endregion
}
#endregion