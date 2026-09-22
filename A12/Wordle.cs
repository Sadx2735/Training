// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Wordle.cs
// Controls the Game display by processing the user input.
// ------------------------------------------------------------------------------------------------

using static System.Console;

namespace WordleGame;

#region Class Wordle ------------------------------------------------------------------------------
/// <summary>Implements the wordle game.</summary>
class Wordle {
   #region Constructors ---------------------------------------------
   /// <summary>initializes the wordBank.</summary>
   /// <param name="bank">Object of WordBank.</param>
   public Wordle (WordBank bank) => mWordBank = bank;

   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the Wordle game till the condition is met</summary>
   public void Run () {
      ClearScreen ();
      SelectWord ();
      DisplayBoard ();
      while (!iGameOver) {
         ConsoleKeyInfo key = Console.ReadKey (true);
         UpdateGameState (key);
         DisplayBoard ();
      }
      PrintResult ();
   }
   #endregion

   #region Implementation -------------------------------------------
   // Clears the console window
   void ClearScreen () => Clear ();
   // Randomly selects a word.
   void SelectWord () => mExpected = "RIVER"; // wordBank.GetRandomWord ();
   // Processes the given key for the game.
   void UpdateGameState (ConsoleKeyInfo key) {
      mStatusMessage = "";
      if (key.Key is >= ConsoleKey.A and <= ConsoleKey.Z
                     && mCursor >= 0 && mCursor < (mRow + 1) * WORDSIZE) {
         mMemBuffer[mCursor] = char.ToUpper (key.KeyChar);
         mCursor++;
      } else if (key.Key is ConsoleKey.Backspace && mCursor > mRow * WORDSIZE) {
         mCursor--;
         mMemBuffer[mCursor] = default;
      } else if (key.Key is ConsoleKey.Enter && mCursor == (mRow + 1) * WORDSIZE) {
         ProcessGuess ();
      }
   }

   void ProcessGuess () {
      string guessed = new(mMemBuffer, mRow * WORDSIZE, WORDSIZE);
      if (!mWordBank.IsValidWord (guessed)) {
         mStatusMessage = $"{guessed} is not a word";
         return;
      }
      CalculateColors (guessed); mRow++;
      if (guessed == mExpected) { iHasWon = true; iGameOver = true; } 
      else if (mRow >= TRIES) { iGameOver = true; }
   }

   void CalculateColors (string guessed) {
      HashSet<char> Seen = [];
      int[] cBuffer = Enumerable.Repeat (1, WORDSIZE).ToArray ();
      int rowOffset = mRow * WORDSIZE;

      for (int i = 0; i < WORDSIZE; i++) {
         if (mMemBuffer[rowOffset + i] == mExpected[i]) {
            cBuffer[i] = 3;
            Seen.Add (mMemBuffer[rowOffset + i]);
         }
      }


      for (int i = 0; i < WORDSIZE; i++) {
         if (cBuffer[i] != 3 && !Seen.Contains (mMemBuffer[rowOffset + i])) {
            cBuffer[i] = mExpected.Contains (mMemBuffer[rowOffset + i]) ? 2 : 1;
            Seen.Add (mMemBuffer[rowOffset + i]);
         }
      }

      for (int idx = 0; idx < WORDSIZE; idx++) {
         mMemBufferColor[rowOffset + idx] = cBuffer[idx];
         int keyIndex = mMemBuffer[rowOffset + idx] - 'A';
         mKeyBuffer[keyIndex] = Math.Max (mKeyBuffer[keyIndex], cBuffer[idx]);
      }
   }

   void DisplayBoard () {
      ClearScreen ();
      for (int row = 0; row < TRIES; row++) {
         Console.SetCursorPosition (mGridStart, Console.CursorTop);
         for (int col = 0; col < WORDSIZE; col++) {
            if (mCursor / WORDSIZE == row && mCursor % WORDSIZE == col && mCursor < ((mRow + 1) * WORDSIZE))
               DrawCell ('◌', ConsoleColor.White);
            else if (mMemBuffer[row * WORDSIZE + col] == default)
               DrawCell ('·', ConsoleColor.White);
            else
               DrawAllocatedCell (row, col);
         }
         WriteLine ("\n");
      }

      Console.SetCursorPosition (mGridStart, Console.CursorTop);
      Console.WriteLine (string.Join ("*", Enumerable.Repeat ("-", 12)));
      Console.Write ('\n');

      Console.SetCursorPosition (mKeyStart, Console.CursorTop);
      for (int i = 1; i <= 26; i++) {
         ConsoleColor kbColor = mKeyBuffer[i - 1] switch {
            1 => ConsoleColor.Red,
            2 => ConsoleColor.Blue,
            3 => ConsoleColor.Green,
            _ => ConsoleColor.White
         };

         Console.ForegroundColor = kbColor;
         Console.Write ($"{(char)(i + 64),-5}");
         Console.ResetColor ();

         if (i % KEYPERROW == 0) {
            Console.Write ("\n\n");
            Console.SetCursorPosition (mKeyStart, Console.CursorTop);
         }
      }
      Console.WriteLine ();

      if (!string.IsNullOrEmpty (mStatusMessage)) {
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.WriteLine ('\n');
         int MesStart = Math.Max (0, (Console.WindowWidth - mStatusMessage.Length) / 2);
         Console.SetCursorPosition (MesStart, Console.CursorTop);
         Console.WriteLine (mStatusMessage);
         Console.ResetColor ();
      }
   }

   void DrawAllocatedCell (int row, int col) {
      ConsoleColor color = ConsoleColor.White;
      if (row < mRow) {
         color = mMemBufferColor[row * WORDSIZE + col] switch {
            1 => ConsoleColor.Red,
            2 => ConsoleColor.Blue,
            3 => ConsoleColor.Green,
            _ => ConsoleColor.White
         };
      }
      DrawCell (mMemBuffer[row * WORDSIZE + col], color);
   }

   void DrawCell (char character, ConsoleColor color) {
      ForegroundColor = color;
      Write ($"{character,-SPACING}");
      ResetColor ();
   }

   void PrintResult () {
      WriteLine ('\n');
      string resultMsg = iHasWon ? "YOU GUESSED IT CORRECTLY!"
                                : $"{mExpected} IS THE WORD! PLEASE TRY AGAIN!";
      ForegroundColor = iHasWon ? ConsoleColor.Green : ConsoleColor.Red;
      int MesStart = Math.Max (0, (WindowWidth - resultMsg.Length) / 2);
      SetCursorPosition (MesStart, CursorTop);
      WriteLine (resultMsg);
      ResetColor ();
      ReadKey (true);
   }
   #endregion

   #region Fields ---------------------------------------------------
   int mCursor = 0, mRow = 0;
   bool iGameOver = false, iHasWon = false;
   string mExpected = "", mStatusMessage = "";
   WordBank mWordBank;
   int[] mKeyBuffer = new int[26];
   int mGridStart = (WindowWidth - WORDSPACE) / 2, mKeyStart = (WindowWidth - KEYSPACE) / 2;
   char[] mMemBuffer = new char[TRIES * WORDSIZE];
   int[] mMemBufferColor = new int[TRIES * WORDSIZE];
   #endregion

   #region Constants ------------------------------------------------
   const int KEYPERROW = 8, SPACING = 5, TRIES = 6, WORDSIZE = 5;
   const int KEYSPACE = ((KEYPERROW - 1) * SPACING) + 1;
   const int WORDSPACE = ((WORDSIZE - 1) * SPACING) + 1;
   #endregion
}
#endregion