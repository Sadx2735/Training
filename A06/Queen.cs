// -------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -------------------------------------------------------------------------------------------------
// Queen.cs
// Implements all the core operations for the N-Queens solver.
// -------------------------------------------------------------------------------------------------
using System.Text;
using static System.Console;

namespace NQueenSolver;

#region Class NQueensSolver -----------------------------------------------------------------------
public class NQueensSolver {
   #region Constructors ---------------------------------------------
   /// <summary>Gets board size and initializes solver buffers.</summary>
   public NQueensSolver (int boardSize) {
      mBoardSize = boardSize;
      mUsedRows = new bool[boardSize];
      mDiag2 = new bool[(2 * boardSize) - 1];
      mDiag1 = new bool[(2 * boardSize) - 1];
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the main user prompt and solution visualizer loop.</summary>
   public void Run () {
      OutputEncoding = Encoding.UTF8;

      List<bool[]> allSolns = Solve ();
      if (allSolns.Count == 0) {
         WriteLine ("\nNo solutions exist; queens attack each other.\n");
         return;
      }
      List<bool[]> uniqueSolns = FilterUniqueSolutions (allSolns);
      mSolType = PromptDisplayMode ();
      List<bool[]> targetSolutions = mSolType == ESolType.AllSolutions ? allSolns : uniqueSolns;
      int currentSolutionIndex = 0;
      Clear ();

      while (true) {
         SetCursorPosition (0, 0);
         WriteLine ($"Total solutions found: {allSolns.Count}");
         WriteLine ($"Unique solutions (excluding symmetries): {uniqueSolns.Count}");
         WriteLine ($"Showing {(mSolType == ESolType.AllSolutions ? "all" : "unique")}" +
            $" solution {currentSolutionIndex + 1} of {targetSolutions.Count}\n");

         DrawBoard (targetSolutions[currentSolutionIndex]);

         WriteLine ("\nControls: [→] Next | [←] Previous | [ESC] Exit");

         switch (ReadKey (true).Key) {
            case ConsoleKey.RightArrow when currentSolutionIndex < targetSolutions.Count - 1:
               currentSolutionIndex++;
               break;

            case ConsoleKey.LeftArrow when currentSolutionIndex > 0:
               currentSolutionIndex--;
               break;

            case ConsoleKey.Escape:
               return;
         }
      }
   }

   /// <summary>Recursively solves the N-Queens problem.</summary>
   /// <returns>All valid board configurations.</returns>
   public List<bool[]> Solve () {
      SolveRecursive (0, new bool[mBoardSize * mBoardSize]);
      return mAllSolutions;

      // Recursively attempts to place a queen in every column and saves valid configurations.
      void SolveRecursive (int columnIndex, bool[] boardMap) {
         if (columnIndex == mBoardSize) {
            // If queens are placed across all columns, record this solution.
            mAllSolutions.Add ((bool[])boardMap.Clone ());
            return;
         }

         // Check if placing a queen in the current column and row is valid.
         for (int row = 0; row < mBoardSize; row++) {
            int slashDiag = row + columnIndex;
            int backslashDiag = row - columnIndex + mBoardSize - 1;

            if (!mUsedRows[row] && !mDiag2[slashDiag] && !mDiag1[backslashDiag]) {
               // Mark the row and diagonals as occupied by a queen.
               mUsedRows[row] = mDiag2[slashDiag] =
                  mDiag1[backslashDiag] = boardMap[To1D (row, columnIndex)] = true;

               SolveRecursive (columnIndex + 1, boardMap);

               // Backtrack by removing the queen to explore other configurations.
               mUsedRows[row] = mDiag2[slashDiag] = mDiag1[backslashDiag] =
                  boardMap[To1D (row, columnIndex)] = false;
            }
         }
      }
   }

   /// <summary>Prompts the user to select the solution display mode (All or Unique).</summary>
   /// <returns>The selected display mode.</returns>
   public ESolType PromptDisplayMode () {
      while (true) {
         Write ("Press key to choose (A)ll or (U)nique solutions: ");
         var key = ReadKey (true);
         if (key.Key is ConsoleKey.A or ConsoleKey.U) {
            WriteLine (key.KeyChar.ToString ().ToUpper ());
            return key.Key == ConsoleKey.A ? ESolType.AllSolutions : ESolType.UniqueOnly;
         }
      }
   }

   /// <summary>Draws the board configuration to the console.</summary>
   /// <param name="map">The board map containing queen positions.</param>
   public void DrawBoard (bool[] map) {
      PrintOuter (TOPPATTERN);
      for (int r = 0; r < mBoardSize; r++) {
         PrintInternal (r, map);
         if (r != mBoardSize - 1) PrintOuter (MIDPATTERN);
      }
      PrintOuter (BOTPATTERN);

      // Prints the top, middle, and bottom outer borders.
      void PrintOuter (string pattern) =>
      WriteLine ($"{pattern[0]}{string.Join (pattern[1], Enumerable
         .Repeat (HORILINE, mBoardSize))}{pattern[2]}");

      // Prints cell contents based on the presence of a queen.
      void PrintInternal (int rowId, bool[] map) {
         var cells = Enumerable.Range (0, mBoardSize)
             .Select (c => map[To1D (rowId, c)] ? QUEEN : EMPTY);
         WriteLine ($"{VERTLINE}{string.Join (VERTLINE, cells)}{VERTLINE}");
      }
   }

   /// <summary>Filters out symmetrical solutions to leave only unique solutions.</summary>
   /// <param name="solutions">The list of all valid solutions.</param>
   /// <returns>The list of unique board solutions.</returns>
   public List<bool[]> FilterUniqueSolutions (List<bool[]> solutions) {
      List<bool[]> uniqueSolutions = [], knownSymmetries = [];
      foreach (var map in solutions) {
         if (knownSymmetries.All (m => !m.SequenceEqual (map))) {
            uniqueSolutions.Add (map);
            bool[] current = map;
            for (int i = 0; i < 4; i++) {
               knownSymmetries.Add (current);
               knownSymmetries.Add (FlipHorizontally (current));
               current = Rotate90Degrees (current);
            }
         }
      }
      return uniqueSolutions;

      // Returns a horizontally flipped version of the input board map.
      bool[] FlipHorizontally (bool[] map) {
         bool[] flipped = new bool[map.Length];
         for (int i = 0; i < map.Length; i++) {
            var (row, col) = To2D (i);
            flipped[To1D (row, mBoardSize - 1 - col)] = map[i];
         }
         return flipped;
      }

      // Returns a 90-degree rotated version of the input board map.
      bool[] Rotate90Degrees (bool[] map) {
         bool[] rotated = new bool[map.Length];
         for (int i = 0; i < map.Length; i++) {
            var (row, col) = To2D (i);
            rotated[To1D (col, mBoardSize - 1 - row)] = map[i];
         }
         return rotated;
      }
   }
   #endregion

   #region Implementation -------------------------------------------
   // Converts a 1D index to a 2D grid position (row, column).
   (int Row, int Col) To2D (int index) => (index / mBoardSize, index % mBoardSize);

   // Converts a 2D grid position (row, column) to a 1D index in the flattened board array.
   int To1D (int row, int col) => (row * mBoardSize) + col;
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
   int mBoardSize;
   ESolType mSolType;
   readonly bool[] mUsedRows;
   readonly bool[] mDiag2;
   readonly bool[] mDiag1;
   readonly List<bool[]> mAllSolutions = [];
   #endregion

   #region Enumerations ---------------------------------------------
   public enum ESolType {
      AllSolutions,
      UniqueOnly
   }
   #endregion
}
#endregion