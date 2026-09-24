// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Wordle.cs
// Controls the Game display by processing the user input.
// ------------------------------------------------------------------------------------------------

using CustomTrie;
using static System.Console;

namespace WordleGame;

#region Class Wordle ------------------------------------------------------------------------------
/// <summary>Implements the wordle game.</summary>
class Wordle {
   #region Constructors ---------------------------------------------
   /// <summary>Initializes the word bank and loads all its words into the suggestion engine.</summary>
   /// <param name="bank">Object of WordBank.</param>
   public Wordle (WordBank bank) {
      mWordBank = bank;
      mAccWords = mWordBank.GetAllWords ();
      foreach (var word in mAccWords) mSuggestionEngine.Insert (word);
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Runs the Wordle game till the game is over.</summary>
   public void Run () {
      SelectWord ();
      DisplayBoard ();
      while (mState == EGameState.Playing) {
         UpdateGameState (ReadKey (true));
         DisplayBoard ();
      }
      PrintResult ();
   }
   #endregion

   #region Implementation -------------------------------------------
   // Randomly selects a word.
   void SelectWord () => mExpected = mWordBank.GetRandomWord (); // "RIVER";

   // Processes the given key for the game.
   void UpdateGameState (ConsoleKeyInfo key) {
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

   // Validates the current row and updates the game state.
   void ProcessGuess () {
      string guess = new (mMemBuffer, mRow * WORDSIZE, WORDSIZE);
      //if (!mWordBank.IsValidWord (guess)) { mStatusMessage = $"{guess} is not a word"; return; }
      Evaluate (guess); mRow++;
      if (guess == mExpected) mState = EGameState.Won;
      else if (mRow == TRIES) mState = EGameState.Lost;
   }

   // Marks each letter of the guess as Correct, Present or Absent (handles repeated letters).
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
            mCellState[idx] = unmatched.Remove (guess[i]) ? ELetterState.Present : ELetterState.Absent;
         if (mCellState[idx] > mKeyState[key]) mKeyState[key] = mCellState[idx];
      }
   }

   // Returns the hints for the current row (random words when nothing is typed yet).
   string GetHints () {
      if (mState != EGameState.Playing) return "";
      string typed = new string (mMemBuffer, mRow * WORDSIZE, WORDSIZE).TrimEnd ('\0');
      var words = (typed == "") ? mAccWords : mSuggestionEngine.GetSuggestion (typed).ToArray();
      if (words.Length == 0) return "No Possible Words";
      return "Hints : " + string.Join (" , ", words.Shuffle ().Take (HINTS)).ToUpper ();
   }

   // Draws the grid, keyboard, hints and status message.
   void DisplayBoard () {
      Clear ();
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
         DrawCell ((char)('A' + i), mKeyState[i]);
         if ((i + 1) % KEYPERROW == 0) { Write ("\n\n"); CursorLeft = mKeyStart; }
      }
      WriteLine ();

      string hints = GetHints ();
      if (hints != "") {
         WriteLine ('\n');
         WriteCentered (hints, ConsoleColor.Gray);
      }

      if (mStatusMessage != "") {
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

   // Prints the final result of the game.
   void PrintResult () {
      WriteLine ('\n');
      bool won = mState == EGameState.Won;
      WriteCentered (won ? "YOU GUESSED IT CORRECTLY!"
                         : $"{mExpected} IS THE WORD! PLEASE TRY AGAIN!",
                     won ? ConsoleColor.Green : ConsoleColor.Red);
      ReadKey (true);
   }

   // Writes the text centered on the current line in the given color.
   void WriteCentered (string text, ConsoleColor color) {
      ForegroundColor = color;
      CursorLeft = Math.Max (0, (WindowWidth - text.Length) / 2);
      WriteLine (text);
      ResetColor ();
   }

   // Maps a letter state to its display color.
   static ConsoleColor ColorOf (ELetterState state) => state switch {
      ELetterState.Absent => ConsoleColor.Red,
      ELetterState.Present => ConsoleColor.Blue,
      ELetterState.Correct => ConsoleColor.Green,
      _ => ConsoleColor.White
   };
   #endregion

   #region Fields ---------------------------------------------------
   int mCursor, mRow;
   EGameState mState = EGameState.Playing;
   string mExpected = "", mStatusMessage = "";
   WordBank mWordBank;
   Trie mSuggestionEngine = new ();
   string[] mAccWords;
   int mGridStart = (WindowWidth - WORDSPACE) / 2, mKeyStart = (WindowWidth - KEYSPACE) / 2;
   char[] mMemBuffer = new char[TRIES * WORDSIZE];
   ELetterState[] mCellState = new ELetterState[TRIES * WORDSIZE];
   ELetterState[] mKeyState = new ELetterState[26];
   #endregion

   #region Constants ------------------------------------------------
   const int HINTS = 3, KEYPERROW = 8, SPACING = 5, TRIES = 6, WORDSIZE = 5;
   const int KEYSPACE = ((KEYPERROW - 1) * SPACING) + 1;
   const int WORDSPACE = ((WORDSIZE - 1) * SPACING) + 1;
   #endregion

   #region Enums ----------------------------------------------------
   public enum EGameState { Playing, Won, Lost }
   public enum ELetterState { Unknown, Absent, Present, Correct }
   #endregion
}
#endregion