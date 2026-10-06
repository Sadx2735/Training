// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Wordle.cs
// Wordle game core logic
// ------------------------------------------------------------------------------------------------

using System.Text;

namespace WordleGame;

#region Class Wordle ------------------------------------------------------------------------------
/// <summary>Implements the wordle game logic</summary>
class Wordle {
   #region Constructors ---------------------------------------------
   /// <summary>Initializes the word bank and the secret word</summary>
   public Wordle (WordBank bank, string word) => (mWordBank, mExpected) = (bank, word);
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Appends the current board with color annotations to a file</summary>
   public void SaveBoard (string file) {
      var sb = new StringBuilder ();
      for (int row = 0; row < TRIES; row++) {
         for (int col = 0; col < WORDSIZE; col++) {
            int idx = row * WORDSIZE + col;
            char ch = idx == mCursor && row == mRow ? '◌'
                    : mGrid[idx] == default ? '·' : mGrid[idx];
            sb.Append (Annotate (ch, mCellState[idx]));
         }
         sb.AppendLine ();
      }
      sb.AppendLine ();
      for (int i = 0; i < 26; i++) {
         sb.Append (Annotate ((char)('A' + i), mKeyState[i]));
         if ((i + 1) % KEYPERROW == 0) sb.AppendLine ();
      }
      sb.Append ("\n\n");
      if (!string.IsNullOrEmpty (mStatusMessage)) sb.AppendLine ($"{mStatusMessage}\n\n");
      if (mState != EGameState.InProgress) sb.AppendLine (ResultText ());
      File.AppendAllText (file, sb.ToString ());
   }

   /// <summary>Processes the given key for the game (ignored once the game is over)</summary>
   public void UpdateGameState (ConsoleKeyInfo key) {
      if (mState != EGameState.InProgress) return;
      mStatusMessage = "";
      int rowStart = mRow * WORDSIZE, rowEnd = rowStart + WORDSIZE;
      switch (key.Key) {
         case >= ConsoleKey.A and <= ConsoleKey.Z when mCursor < rowEnd:
            mGrid[mCursor++] = (char)key.Key; break;
         case ConsoleKey.Backspace when mCursor > rowStart:
            mGrid[--mCursor] = default; break;
         case ConsoleKey.Enter when mCursor == rowEnd:
            ProcessGuess (); break;
      }
   }
   #endregion

   #region Implementation -------------------------------------------
   // Wraps a character in brackets based on its state: {} correct, [] present, () absent
   string Annotate (char ch, ELetterState state) => state switch {
      ELetterState.Absent => $"({ch})",
      ELetterState.Present => $"[{ch}]",
      ELetterState.Correct => $"{{{ch}}}",
      _ => $" {ch} "
   };

   // Marks each letter of the guess as Correct, Present or Absent
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

   // Validates the current row and updates the game state
   void ProcessGuess () {
      string guess = new (mGrid, mRow * WORDSIZE, WORDSIZE);
      if (!mWordBank.IsValidWord (guess)) {
         mStatusMessage = "Not a valid word";
         return;
      }
      Evaluate (guess);
      if (guess == mExpected) mState = EGameState.Won;
      else if (++mRow == TRIES) mState = EGameState.Lost;
   }

   // Returns the final result message of the game
   string ResultText () => mState == EGameState.Won ? "YOU GUESSED IT CORRECTLY!"
                                                    : $"{mExpected} IS THE WORD! PLEASE TRY AGAIN!";
   #endregion

   #region Fields ---------------------------------------------------
   int mCursor, mRow;
   EGameState mState;
   string mExpected, mStatusMessage = "";
   WordBank mWordBank;
   char[] mGrid = new char[TRIES * WORDSIZE];
   ELetterState[] mCellState = new ELetterState[TRIES * WORDSIZE];
   ELetterState[] mKeyState = new ELetterState[26];
   #endregion

   #region Constants ------------------------------------------------
   const int KEYPERROW = 8, TRIES = 6, WORDSIZE = 5;
   #endregion

   #region Enums ----------------------------------------------------
   public enum EGameState {
      InProgress,    // Game is running, the player can still enter guesses
      Won,           // Player guessed the word within the allowed tries
      Lost           // All tries are used up without guessing the word
   }

   public enum ELetterState {
      Unknown,       // Letter not yet evaluated (untyped cell or unused key)
      Absent,        // Letter is not in the word
      Present,       // Letter is in the word but at a different position
      Correct        // Letter is in the word at the correct position
   }
   #endregion
}
#endregion