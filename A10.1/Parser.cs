// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joiners at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Parser.cs
// Implementation of a custom file parser.
// ------------------------------------------------------------------------------------------------
namespace Parse;

using static Parser.EState;

#region Class Parser ------------------------------------------------------------------------------
/// <summary> Implements a custom file parser. </summary>
class Parser {
   #region Methods --------------------------------------------------
   /// <summary> Evaluates the given input file path using a state machine. </summary>
   /// <param name="input"> The input file path string to parse. </param>
   /// <returns> A tuple containing (drive, folder, filename, extension). </returns>
   public static (string, string, string, string) Evaluate (string input) {
      var st = A;
      Action<char> none = (char a) => { }, todo;
      string drive = "", directory = "", extension = "";
      /// State Diagram: docs/Diagram.png
      foreach (var ch in input.ToUpper () + '~') {
         todo = none;
         (st, todo) = (st, ch) switch {
            (A, >= 'A' and <= 'Z') => (B, (x) => drive += x),
            (B, ':') => (C, none),
            (C, '\\') => (D, none),
            (D or E, >= 'A' and <= 'Z') => (E, (x) => directory += x),
            (E, '\\') => (F, (x) => directory += x),
            (F or G, >= 'A' and <= 'Z') => (G, (x) => directory += x),
            (G, '\\') => (F, (x) => directory += x),
            (G, '.') => (H, none),
            (H or I, >= 'A' and <= 'Z') => (I, (x) => extension += x),
            (I, '~') => (J, none),
            _ => (Z, none)
         };
         todo (ch);
      }
      if (st is J) {
         var items = directory.Split ('\\');
         string filename = items[^1];
         directory = string.Join ("/", items.SkipLast (1));
         return (drive, directory, filename, extension);
      }
      return (string.Empty, string.Empty, string.Empty, string.Empty);
   }
   #endregion

   #region Enum EState ----------------------------------------------
   /// <summary> Represents the parser states for evaluating file paths. </summary>
   public enum EState {
      A, // Starting state of the parser.
      B, // Drive letter.
      C, // Colon (:) after the drive letter.
      D, // Backslash (\) after the colon.
      E, // Primary folder name.
      F, // Directory backslash (\) delimiter.
      G, // Subfolder or file name.
      H, // Extension dot (.) separator.
      I, // File extension.
      J, // Success / terminal end state.
      Z  // Error state.
   }
   #endregion
}
#endregion