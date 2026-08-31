// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Wordle.cs
// Controls the Game display by processing the user input.
// ------------------------------------------------------------------------------------------------

namespace WordleGame;

#region Class Wordle ------------------------------------------------------------------------------
/// <summary>Implements the wordle game.</summary>
class Wordle {
   #region Constructors ---------------------------------------------
   /// <summary>initializes the wordBank.</summary>
   /// <param name="bank">Object of WordBank.</param>
   public Wordle (WordBank bank) => wordBank = bank;
 
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the Wordle game till the condition is met</summary>
   public void Run () {
      ClearScreen ();
      SelectWord ();
      DisplayBoard ();
      while (!GameOver) {
         ConsoleKeyInfo key = Console.ReadKey (true);
         UpdateGameState (key);
         DisplayBoard ();
      }
      PrintResult ();
   }
   #endregion

   #region Implementation -------------------------------------------
   // Clears the console window
   void ClearScreen () => Console.Clear ();
   // Randomly selects a word.
   void SelectWord () => EXPECTED = wordBank.GetRandomWord ();
   // Processes the given key for the game.
   void UpdateGameState (ConsoleKeyInfo key) {
      statusMessage = "";
      // if its A to Z and if the Cursor is 
      if (key.Key is >= ConsoleKey.A and <= ConsoleKey.Z 
                                     && mCursor >= 0 && mCursor < (mRow + 1) * WORDSIZE) {
         MemBuffer[mCursor] = char.ToUpper (key.KeyChar);
         mCursor++;
      } else if (key.Key is ConsoleKey.Backspace && mCursor > mRow * WORDSIZE) {
         mCursor--;
         MemBuffer[mCursor] = default;
      } else if (key.Key is ConsoleKey.Enter && mCursor == (mRow + 1) * WORDSIZE) {
         ProcessGuess ();
      }
   }

   void ProcessGuess () {
      string guessed = new string (MemBuffer, mRow * WORDSIZE, WORDSIZE);
      if (!wordBank.IsValidWord (guessed)) {
         statusMessage = $"{guessed} is not a word";
         return;
      }
      CalculateColors (guessed);
      mRow++;
      if (guessed == EXPECTED) {
         HasWon = true;
         GameOver = true;
      } else {
         if (mRow >= TRIES) {
            GameOver = true;
         }
      }
   }

   void CalculateColors (string guessed) {
      HashSet<char> Seen = new HashSet<char> ();
      int[] cBuffer = Enumerable.Repeat (1, WORDSIZE).ToArray ();
      int rowOffset = mRow * WORDSIZE;

      for (int i = 0; i < WORDSIZE; i++) {
         if (MemBuffer[rowOffset+i] == EXPECTED[i]) {
            cBuffer[i] = 3;
            Seen.Add (MemBuffer[rowOffset+i]);
         }
      }


      for (int i = 0; i < WORDSIZE; i++) {
         if (cBuffer[i] != 3 && !Seen.Contains (MemBuffer[rowOffset+i])) {
            cBuffer[i] = EXPECTED.Contains (MemBuffer[rowOffset+i]) ? 2 : 1;
            Seen.Add (MemBuffer[rowOffset+i]);
         }
      }

      for (int idx = 0; idx < WORDSIZE; idx++) {
         MemBufferColor[rowOffset + idx] = cBuffer[idx];
         int keyIndex = MemBuffer[rowOffset + idx] - 'A';
         KeyBuffer[keyIndex] = Math.Max (KeyBuffer[keyIndex], cBuffer[idx]);
      }
   }

   void DisplayBoard () {
      ClearScreen ();
      for (int row = 0; row < TRIES; row++) {
         Console.SetCursorPosition (GridStart, Console.CursorTop);
         for (int col = 0; col < WORDSIZE; col++) {
            if (mCursor / WORDSIZE == row && mCursor % WORDSIZE == col && mCursor < ((mRow + 1) * WORDSIZE))
               DrawCell ('◌', ConsoleColor.White);
            else if (MemBuffer[row * WORDSIZE + col] == default)
               DrawCell ('·', ConsoleColor.White);
            else
               DrawAllocatedCell (row, col);
         }
         Console.WriteLine ("\n");
      }

      Console.SetCursorPosition (GridStart, Console.CursorTop);
      Console.WriteLine (string.Join ("*", Enumerable.Repeat ("-", 12)));
      Console.Write ('\n');

      Console.SetCursorPosition (KeyStart, Console.CursorTop);
      for (int i = 1; i <= 26; i++) {
         ConsoleColor kbColor = KeyBuffer[i - 1] switch {
            1 => ConsoleColor.Red,
            2 => ConsoleColor.Blue,
            3 => ConsoleColor.Green,
            _ => ConsoleColor.White
         };

         Console.ForegroundColor = kbColor;
         Console.Write ($"{(char)(i + 64),-5}");
         Console.ResetColor ();

         if (i % 8 == 0) {
            Console.Write ("\n\n");
            Console.SetCursorPosition (KeyStart, Console.CursorTop);
         }
      }
      Console.WriteLine ();

      if (!string.IsNullOrEmpty (statusMessage)) {
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.WriteLine ('\n');
         int MesStart = Math.Max (0, (Console.WindowWidth - statusMessage.Length) / 2);
         Console.SetCursorPosition (MesStart, Console.CursorTop);
         Console.WriteLine (statusMessage);
         Console.ResetColor ();
      }
   }

   void DrawAllocatedCell (int row, int col) {
      ConsoleColor color = ConsoleColor.White;
      if (row < mRow) {
         color = MemBufferColor[row * WORDSIZE + col] switch {
            1 => ConsoleColor.Red,
            2 => ConsoleColor.Blue,
            3 => ConsoleColor.Green,
            _ => ConsoleColor.White
         };
      }
      DrawCell (MemBuffer[row * WORDSIZE + col], color);
   }

   void DrawCell (char character, ConsoleColor color) {
      Console.ForegroundColor = color;
      Console.Write ($"{character,-5}");
      Console.ResetColor ();
   }

   void PrintResult () {
      Console.WriteLine ('\n');
      string resultMsg = HasWon ? "YOU GUESSED IT CORRECTLY!" 
                                : $"{EXPECTED} IS THE WORD! PLEASE TRY AGAIN!";
      Console.ForegroundColor = HasWon ? ConsoleColor.Green : ConsoleColor.Red;
      int MesStart = Math.Max (0, (Console.WindowWidth - resultMsg.Length) / 2);
      Console.SetCursorPosition (MesStart, Console.CursorTop);
      Console.WriteLine (resultMsg);
      Console.ResetColor ();
      Console.ReadKey (true);
   }
   #endregion

   #region Fields ---------------------------------------------------
   int mRow = 0;
   int mCursor = 0;
   int GridStart = (Console.WindowWidth - 21) / 2;
   int KeyStart = (Console.WindowWidth - 36) / 2;
   char[] MemBuffer = new char[TRIES * WORDSIZE];
   int[] MemBufferColor = new int[TRIES * WORDSIZE];
   int[] KeyBuffer = new int[26];
   bool GameOver = false;
   bool HasWon = false;
   string EXPECTED = "";
   string statusMessage = "";
   WordBank wordBank;
   #endregion

   #region Constants ------------------------------------------------
   const int WORDSIZE = 5;
   const int TRIES = 6;
   #endregion
}
#endregion