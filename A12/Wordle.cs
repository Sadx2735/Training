using System;
using System.Collections.Generic;
using System.Linq;

namespace WordleGame;
class Wordle {
   const int WORDSIZE = 5;
   const int TRIES = 6;

   int mRow = 0;
   int mCursor = 0;
   int GridStart = (Console.WindowWidth - 21) / 2;
   int KeyStart = (Console.WindowWidth - 36) / 2;

   char[][] MemBuffer = new char[TRIES][];
   int[] MemBufferColor = new int[TRIES * WORDSIZE];
   int[] KeyBuffer = new int[26];

   bool GameOver = false;
   bool HasWon = false;

   string EXPECTED = "";
   string statusMessage = "";
   WordBank wordBank;

   public Wordle (WordBank bank) {
      wordBank = bank;
      for (int i = 0; i < TRIES; i++)
         MemBuffer[i] = new char[WORDSIZE];
   }

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

   private void ClearScreen () {
      Console.Clear ();
   }

   private void SelectWord () {
      EXPECTED = wordBank.GetRandomWord ();
   }

   private void UpdateGameState (ConsoleKeyInfo key) {
      statusMessage = "";
      if (key.Key is >= ConsoleKey.A and <= ConsoleKey.Z && mCursor >= 0 && mCursor < (mRow + 1) * WORDSIZE) {
         (int row, int col) = (mCursor / WORDSIZE, mCursor % WORDSIZE);
         MemBuffer[row][col] = char.ToUpper (key.KeyChar);
         mCursor++;
      } else if (key.Key is ConsoleKey.Backspace && mCursor > mRow * WORDSIZE) {
         mCursor--;
         (int row, int col) = (mCursor / WORDSIZE, mCursor % WORDSIZE);
         MemBuffer[row][col] = default;
      } else if (key.Key is ConsoleKey.Enter && mCursor == (mRow + 1) * WORDSIZE) {
         ProcessGuess ();
      }
   }

   void ProcessGuess () {
      string guessed = new string (MemBuffer[mRow]);

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

   private void CalculateColors (string guessed) {
      HashSet<char> Seen = new HashSet<char> ();
      int[] cBuffer = Enumerable.Repeat (1, WORDSIZE).ToArray ();

      for (int i = 0; i < WORDSIZE; i++) {
         if (MemBuffer[mRow][i] == EXPECTED[i]) {
            cBuffer[i] = 3;
            Seen.Add (MemBuffer[mRow][i]);
         }
      }


      for (int i = 0; i < WORDSIZE; i++) {
         if (cBuffer[i] != 3 && !Seen.Contains (MemBuffer[mRow][i])) {
            cBuffer[i] = EXPECTED.Contains (MemBuffer[mRow][i]) ? 2 : 1;
            Seen.Add (MemBuffer[mRow][i]);
         }
      }

      for (int idx = 0; idx < WORDSIZE; idx++) {
         MemBufferColor[idx + (mRow * WORDSIZE)] = cBuffer[idx];
         int index = MemBuffer[mRow][idx] - 'A';
         KeyBuffer[index] = Math.Max (KeyBuffer[index], cBuffer[idx]);
      }
   }

   private void DisplayBoard () {
      ClearScreen ();
      for (int row = 0; row < TRIES; row++) {
         Console.SetCursorPosition (GridStart, Console.CursorTop);
         for (int col = 0; col < WORDSIZE; col++) {
            if (mCursor / WORDSIZE == row && mCursor % WORDSIZE == col && mCursor < ((mRow + 1) * WORDSIZE))
               DrawCell ('◌', ConsoleColor.White);
            else if (MemBuffer[row][col] == default)
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
      DrawCell (MemBuffer[row][col], color);
   }

   void DrawCell (char character, ConsoleColor color) {
      Console.ForegroundColor = color;
      Console.Write ($"{character,-5}");
      Console.ResetColor ();
   }

   void PrintResult () {
      Console.WriteLine ('\n');
      string resultMsg = HasWon ? "YOU GUESSED IT CORRECTLY!" : $"{EXPECTED} IS THE WORD! PLEASE TRY AGAIN!";
      Console.ForegroundColor = HasWon ? ConsoleColor.Green : ConsoleColor.Red;
      int MesStart = Math.Max (0, (Console.WindowWidth - resultMsg.Length) / 2);
      Console.SetCursorPosition (MesStart, Console.CursorTop);
      Console.WriteLine (resultMsg);
      Console.ResetColor ();
      Console.ReadKey (true);
   }
}