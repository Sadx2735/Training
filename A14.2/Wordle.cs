// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Wordle.cs
// Controls the Game display by processing the user input.
// ------------------------------------------------------------------------------------------------

using System.Text;
using static System.Console;

namespace WordleGame;

#region Class Wordle ------------------------------------------------------------------------------
/// <summary>Implements the wordle game.</summary>
class Wordle {
   #region Constructors ---------------------------------------------
   /// <summary>Initializes the word bank.</summary>
   /// <param name="bank">Object of WordBank.</param>
   public Wordle (WordBank bank) {
      mWordBank = bank;
      mBuffer = new StringBuilder ();
      mExpected = "STRAW"; //mWordBank.GetRandomWord ();
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the Wordle game till the game is over.</summary>
   public void Run () {
      File.Delete (FILENAME);
      DisplayBoard ();
      while (mState == EGameState.InProgress) {
         UpdateGameState (ReadKey (true));
         DisplayBoard ();
         SaveBoard (FILENAME);
      }
      PrintResult ();
   }

   /// <summary>Writes the current board with color annotations to a file.</summary>
   /// <param name="file">file to which it needs to save the board config</param>
   public void SaveBoard (string file) {
      mBuffer.Clear ();
      for (int row = 0; row < TRIES; row++) {
         for (int col = 0; col < WORDSIZE; col++) {
            int idx = row * WORDSIZE + col;
            char ch = idx == mCursor && row == mRow ? '◌'
                    : mMemBuffer[idx] == default ? '·' : mMemBuffer[idx];
            mBuffer.Append (Annotate (ch, mCellState[idx]));
         }
         mBuffer.AppendLine ();
      }
      mBuffer.AppendLine ();
      for (int i = 0; i < 26; i++) {
         mBuffer.Append (Annotate ((char)('A' + i), mKeyState[i]));
         if ((i + 1) % KEYPERROW == 0) mBuffer.AppendLine ();
      }
      mBuffer.Append ("\n\n");
      if (!string.IsNullOrEmpty (mStatusMessage)) mBuffer.AppendLine ($"{mStatusMessage}\n\n");
      if (mState != EGameState.InProgress) mBuffer.AppendLine (ResultText ());
      File.AppendAllText (file, mBuffer.ToString ());
   }

   /// <summary>Processes the given key for the game.</summary>
   /// <param name="key">Details of the key which got pressed.</param>
   public void UpdateGameState (ConsoleKeyInfo key) {
      mStatusMessage = "";
      int rowStart = mRow * WORDSIZE, rowEnd = rowStart + WORDSIZE;
      switch (key.Key) {
         case >= ConsoleKey.A and <= ConsoleKey.Z when mCursor < rowEnd:
            mMemBuffer[mCursor++] = (char)key.Key; break;
         case ConsoleKey.Backspace when mCursor > rowStart:
            mMemBuffer[--mCursor] = default; break;
         case ConsoleKey.Enter when mCursor == rowEnd:
            ProcessGuess (); break;
      }
   }

   #endregion

   #region Implementation -------------------------------------------
   String Annotate (char ch, ELetterState state) => state switch {
      ELetterState.Absent => $"({ch})",
      ELetterState.Present => $"[{ch}]",
      ELetterState.Correct => $"{{{ch}}}",
      _ => $" {ch} "
   };

   // Maps a letter state to its display color.
   ConsoleColor ColorOf (ELetterState state) => state switch {
      ELetterState.Absent => ConsoleColor.Red,
      ELetterState.Present => ConsoleColor.Blue,
      ELetterState.Correct => ConsoleColor.Green,
      _ => ConsoleColor.White
   };

   // Draws the grid, keyboard, hints and status message.
   void DisplayBoard () {
      Clear ();
      int mGridStart = (WindowWidth - WORDSPACE) / 2, mKeyStart = (WindowWidth - KEYSPACE) / 2;
      for (int row = 0; row < TRIES; row++) {
         CursorLeft = mGridStart;
         for (int col = 0; col < WORDSIZE; col++) {
            int idx = row * WORDSIZE + col;
            char ch = idx == mCursor && row == mRow ? '◌'
                    : mMemBuffer[idx] == default ? '·' : mMemBuffer[idx];
            DrawCell (ch, mCellState[idx]);
         }
         WriteLine ("\n");
      }

      CursorLeft = mGridStart;
      WriteLine (string.Join ("*", Enumerable.Repeat ("-", 12)));
      Write ('\n');

      CursorLeft = mKeyStart;
      for (int i = 0; i < 26; i++) {
         DrawCell (mKeyState[i] == ELetterState.Absent ? ' ' : (char)('A' + i), mKeyState[i]);
         if ((i + 1) % KEYPERROW == 0) { Write ("\n\n"); CursorLeft = mKeyStart; }
      }
      WriteLine ();

      if (!string.IsNullOrEmpty (mStatusMessage)) {
         WriteLine ('\n');
         WriteCentered (mStatusMessage, ConsoleColor.Yellow);
      }
   }

   // Draws a single character in the color of its state.
   void DrawCell (char character, ELetterState state) {
      ForegroundColor = ColorOf (state);
      Write ($"{character,-SPACING}");
      ResetColor ();
   }

   // Marks each letter of the guess as Correct, Present or Absent.
   void Evaluate (string guess) {
      int offset = mRow * WORDSIZE;
      List<char> unmatched = [];
      for (int i = 0; i < WORDSIZE; i++) {
         if (guess[i] == mExpected[i]) mCellState[offset + i] = ELetterState.Correct;
         else unmatched.Add (mExpected[i]);
      }
      for (int i = 0; i < WORDSIZE; i++) {
         int idx = offset + i, key = guess[i] - 'A';
         if (mCellState[idx] != ELetterState.Correct)
            mCellState[idx] = unmatched.Remove (guess[i]) ? ELetterState.Present
                                                          : ELetterState.Absent;
         if (mCellState[idx] > mKeyState[key]) mKeyState[key] = mCellState[idx];
      }
   }

   // Prints the final result of the game.
   void PrintResult () {
      bool won = mState == EGameState.Won;
      WriteLine ('\n');
      WriteCentered (ResultText(), won ? ConsoleColor.Green : ConsoleColor.Red);
      ReadKey (true);
   }

   // Validates the current row and updates the game state.
   void ProcessGuess () {
      string guess = new (mMemBuffer, mRow * WORDSIZE, WORDSIZE);
      if (!mWordBank.IsValidWord (guess)) {
         mStatusMessage = "Not a valid word";
         return;
      }
      Evaluate (guess);
      if (guess == mExpected) mState = EGameState.Won;
      else if (++mRow == TRIES) mState = EGameState.Lost;
   }

   // Returns 
   string ResultText() => (mState == EGameState.Won) ? "YOU GUESSED IT CORRECTLY!"
                                                 : $"{mExpected} IS THE WORD! PLEASE TRY AGAIN!";

   // Writes the text centered on the current line in the given color.
   void WriteCentered (string text, ConsoleColor color) {
      ForegroundColor = color;
      CursorLeft = Math.Max (0, (WindowWidth - text.Length) / 2);
      WriteLine (text);
      ResetColor ();
   }
   #endregion

   #region Fields ---------------------------------------------------
   int mCursor, mRow;
   EGameState mState = EGameState.InProgress;
   string mExpected = "", mStatusMessage = "";
   WordBank mWordBank;
   StringBuilder mBuffer;
   char[] mMemBuffer = new char[TRIES * WORDSIZE];
   ELetterState[] mCellState = new ELetterState[TRIES * WORDSIZE];
   ELetterState[] mKeyState = new ELetterState[26];
   #endregion

   #region Constants ------------------------------------------------
   const string FILENAME = "TESTCASE.txt";
   const int HINTS = 3, KEYPERROW = 8, SPACING = 5, TRIES = 6, WORDSIZE = 5;
   const int KEYSPACE = ((KEYPERROW - 1) * SPACING) + 1;
   const int WORDSPACE = ((WORDSIZE - 1) * SPACING) + 1;
   #endregion

   #region Enums ----------------------------------------------------
   public enum EGameState {
      InProgress,    // Game is running, the player can still enter guesses
      Won,           // Player guessed the word within the allowed tries
      Lost           // All tries are used up without guessing the word
   }

   public enum ELetterState {
      Unknown,       // Letter not yet evaluated (untyped cell or unused key)
      Absent,        // Letter is not in the word (RED)
      Present,       // Letter is in the word but at a different position (BLUE)
      Correct,       // Letter is in the word at the correct position (GREEN)
   }
   #endregion
}
#endregion