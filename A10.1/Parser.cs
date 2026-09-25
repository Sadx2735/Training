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
      Action none = () => { }, todo;
      string drive = "", directory = "", extension = "", filename = "";
      /// State Diagram: docs/Diagram.png
      foreach (var ch in input.ToUpper () + '~') {
         (st, todo) = (st, ch) switch {
            (A, >= 'A' and <= 'Z') => (B, () => drive += ch),
            (B, ':') => (C, none),
            (C, '\\') => (D, none),
            (D or E, >= 'A' and <= 'Z') => (E, () => filename += ch),
            (E, '\\') => (D, () => (directory, filename) = (string.IsNullOrEmpty (directory)
                                                 ? filename : $"{directory}/{filename}", "")),
            (E, '.') => (F, none),
            (F or G, >= 'A' and <= 'Z') => (G, () => extension += ch),
            (G, '~') => (H, none),
            _ => (Z, none)
         };
         todo ();
      }
      return (st is H) ? (drive, directory, filename, extension) : ("", "", "", "");
   }
   #endregion

   #region Enum EState ----------------------------------------------
   /// <summary> Represents the parser states for evaluating file paths. </summary>
   public enum EState {
      A, // Starting state of the parser.
      B, // Drive letter.
      C, // Colon (:) after the drive letter.
      D, // Backslash (\) separator - expecting a name.
      E, // Folder or file name.
      F, // Extension dot (.) separator.
      G, // File extension.
      H, // Success / terminal end state.
      Z  // Error state.
   }
   #endregion
}
#endregion