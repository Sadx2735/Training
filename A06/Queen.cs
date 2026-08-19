// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Queen.cs
// Implements all the core operations for the N-Queens solver.
// ------------------------------------------------------------------------------------------------
using System.Text;
using static System.Console;

namespace NQueenSolver;

#region Class NQueensSolver -----------------------------------------------------------------------
public class NQueensSolver {
   #region Constructors ---------------------------------------------
   /// <summary>Gets board size and initializes solver buffers.</summary>
   public NQueensSolver (int boardSize) {
      mBoardSize = boardSize;
      mUsedCols = new bool[boardSize];
      var diagCount = (2 * boardSize) - 1;
      (mDiag1, mDiag2) = (new bool[diagCount], new bool[diagCount]);
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the main user prompt and solution visualizer loop.</summary>
   public void Run () {
      OutputEncoding = Encoding.UTF8;
      CursorVisible = false;
      if (!IsConsoleSizeEnough ()) return;
      // Displaying a temporary message
      WriteLine ("Processing...");
      // Clearing the line once the processing is done
      SetCursorPosition (0, 1);
      List<int[]> allSolns = Solve ();
      if (allSolns.Count == 0) {
         WriteLine ("No solutions exist; queens attack each other.\n");
         return;
      }
      List<int[]> uniqueSolns = FilterUniqueSolns (allSolns);
      var solType = PromptDisplayMode ();
      Clear (); SetCursorPosition (0, 0);
      bool showAllSolns = solType == ESolType.AllSolns;
      List<int[]> targetSolns = showAllSolns ? allSolns : uniqueSolns;
      int currentIndex = 0, allCount = allSolns.Count, uniqueCount = uniqueSolns.Count,
      targetCount = showAllSolns ? allCount : uniqueCount;
      while (true) {
         SetCursorPosition (0, 0);
         if (!IsConsoleSizeEnough ()) return;
         WriteLine ($"Total solutions found: {allCount}");
         WriteLine ($"Unique solutions (excluding symmetries): {uniqueCount}");
         WriteLine ($"Showing {(showAllSolns ? "all" : "unique")}" +
            $" solution {currentIndex + 1} of {targetSolns.Count}\n");
         DrawBoard (targetSolns[currentIndex]);
         WriteLine ("\nControls: [→] Next | [←] Previous | [ESC] Exit");
         switch (ReadKey (true).Key) {
            case ConsoleKey.RightArrow when currentIndex < targetCount - 1: currentIndex++; break;
            case ConsoleKey.LeftArrow when currentIndex > 0: currentIndex--; break;
            case ConsoleKey.Escape: CursorVisible = true; return;
         }
      }
   }
   #endregion

   #region Implementation -------------------------------------------
   // Draws the board configuration to the console.
   void DrawBoard (int[] map) {
      PrintOuter (TOPPATTERN);
      for (int r = 0; r < mBoardSize; r++) {
         PrintInternal (r, map);
         if (r != mBoardSize - 1) PrintOuter (MIDPATTERN);
      }
      PrintOuter (BOTPATTERN);

      // Helper .....................................................
      // Prints cell contents based on the presence of a queen.
      void PrintInternal (int rowId, int[] map) {
         var cell = Enumerable.Range (0, mBoardSize).Select (c => map[rowId] == c ? QUEEN : EMPTY);
         WriteLine ($"{VERTLINE}{string.Join (VERTLINE, cell)}{VERTLINE}");
      }

      // Prints the top, middle, and bottom outer borders.
      void PrintOuter (string pattern)
          => WriteLine ($"{pattern[0]}{string.Join (pattern[1],
          Enumerable.Repeat (HORILINE, mBoardSize))}{pattern[2]}");
   }

   // Filters out symmetrical solutions to leave only unique solutions.
   List<int[]> FilterUniqueSolns (List<int[]> solns) {
      List<int[]> uniqueSolns = [];
      HashSet<string> knownSymmetries = [];
      foreach (var map in solns) {
         if (knownSymmetries.Contains (MapToString (map))) continue;
         uniqueSolns.Add (map);
         int[] current = map;
         for (int i = 0; i < 4; i++) {
            knownSymmetries.Add (MapToString (current));
            knownSymmetries.Add (MapToString (FlipHorizontally (current)));
            current = Rotate90Degrees (current);
         }
      }
      return uniqueSolns;

      // Helper .....................................................
      // Returns a horizontally flipped version of the input board map.
      int[] FlipHorizontally (int[] map) {
         int[] flipped = new int[map.Length];
         for (int r = 0; r < map.Length; r++)
            flipped[r] = mBoardSize - 1 - map[r];
         return flipped;
      }

      // Returns a string representation of the array.
      string MapToString (int[] map) => string.Join (",", map);

      // Returns a 90-degree rotated version of the input board map.
      int[] Rotate90Degrees (int[] map) {
         int[] rotated = new int[map.Length];
         for (int r = 0; r < map.Length; r++)
            rotated[map[r]] = mBoardSize - 1 - r;
         return rotated;
      }
   }

   // Checks if the current console window is large enough to fit a given board size,
   bool IsConsoleSizeEnough () {
      var (reqHeight, reqWidth) = ((2 * mBoardSize) + 8, Math.Max (46, (5 * mBoardSize) + 1));
      if (WindowHeight < reqHeight || WindowWidth < reqWidth) {
         Clear ();
         WriteLine ("Console window is too small to display the board.");
         WriteLine ("Please enlarge the console window and run the program again.");
         return false;
      }
      return true;
   }

   // Prompts the user to select the solution display mode (All or Unique).
   ESolType PromptDisplayMode () {
      while (true) {
         WriteLine ("Press key to choose (A)ll or (U)nique solutions: ");
         var key = ReadKey (true);
         if (key.Key is ConsoleKey.A or ConsoleKey.U) {
            WriteLine (key.KeyChar.ToString ().ToUpper ());
            return key.Key == ConsoleKey.A ? ESolType.AllSolns : ESolType.UniqueOnly;
         }
      }
   }

   // Recursively solves the N-Queens problem.
   List<int[]> Solve () {
      mAllSolns.Clear ();
      SolveRecursive (0, new int[mBoardSize]);
      return mAllSolns;

      // Helper .....................................................
      // Recursively attempts to place a queen in every row and saves valid configurations.
      void SolveRecursive (int row, int[] boardMap) {
         if (row == mBoardSize) {
            // If queens are placed across all rows, record this solution.
            mAllSolns.Add ((int[])boardMap.Clone ()); return;
         }
         // Check if placing a queen in the current row and column is valid.
         for (int col = 0; col < mBoardSize; col++) {
            int slashDiag = row + col;
            int backslashDiag = row - col + mBoardSize - 1;
            if (!mUsedCols[col] && !mDiag2[slashDiag] && !mDiag1[backslashDiag]) {
               // Mark the column and diagonals as occupied by a queen.
               mUsedCols[col] = mDiag2[slashDiag] = mDiag1[backslashDiag] = true;
               boardMap[row] = col;
               SolveRecursive (row + 1, boardMap);
               // removing the queen to explore other configurations.
               mUsedCols[col] = mDiag2[slashDiag] = mDiag1[backslashDiag] = false;
            }
         }
      }
   }
   #endregion

   #region Constants ------------------------------------------------
   const string TOPPATTERN = "┌┬┐";
   const string MIDPATTERN = "├┼┤";
   const string BOTPATTERN = "└┴┘";
   const string HORILINE = "────";
   const string VERTLINE = "│";
   const string QUEEN = " ♕  ";
   const string EMPTY = "    ";
   #endregion

   #region Fields ---------------------------------------------------
   readonly int mBoardSize;
   readonly bool[] mUsedCols;
   readonly bool[] mDiag2;
   readonly bool[] mDiag1;
   readonly List<int[]> mAllSolns = [];
   #endregion

   #region Enumerations ---------------------------------------------
   public enum ESolType {
      AllSolns, UniqueOnly
   }
   #endregion
}
#endregion