// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Custom Parser to convert string to Double
// ------------------------------------------------------------------------------------------------

#region Class Program -----------------------------------------------------------------------------
/// <summary>Implements a custom Parse method converting a string to a double.</summary>
class Program {

   #region Implementation -------------------------------------------------------------------------
   /// <summary>Iterates through each word and converts them into double</summary>
   static void Main () {
      // Test cases covering valid, invalid inputs
      string[] words = [
      "10.54E23.4E3", "-1.546234E-4", "0", "0.0", "12345", "0.00000325", "10.54E2",
      "1.5E2.5", "1.E3", "1.E+3", "12.54e3.", "12.", "12.e1", ".325", " +625 ", "6.25e0",
      "6.0e0", "6.25e-1", "+6.25E1", "6.25", "10.625", "15a1", "1.5672", "+-12",
      "12.-5", ".e1", "-0.325", " 12.456 "
      ];
      foreach (var rawInput in words) {
         string userInput = rawInput.Trim ();
         double customResult = Parse (userInput);
         Console.WriteLine ("--------------------------------------------------");
         Console.WriteLine ($"Input              : {userInput}");
         Console.WriteLine ($"Via Custom Parse   : {customResult}");
         double builtInResult = double.TryParse (userInput, out double val) ? val : double.NaN;
         Console.WriteLine ($"Via Built-In Parse : {builtInResult}");
      }
   }
   /// <summary>Parses a numeric string input and returns its double representation.</summary>
   /// <param name="userInput">The input string to parse into a double.</param>
   /// <returns>The parsed double value if valid; otherwise double.NaN.</returns>
   static double Parse (string userInput) {
      // Initialize signs, base number, exponent value, and decimal factor
      int signMain = 1, signExpo = 1;
      double number = 0, exponent = 0, factor = 1;
      // Syntax rule tracking flags
      bool iNumber = false, iNumberAfterE = false, iNumberAfterD = false;
      bool iE = false, iD = false, iSign = false, iSignAfterE = false;
      bool iFlag = true;
      foreach (var ch in userInput) {
         // Sign must be at the start or directly after 'e'
         if ((ch == '+' || ch == '-') &&
            (!iSign || !iSignAfterE) && (!iNumber || (iE && !iNumberAfterE))) {
            if (!iNumber && !iSign) {
               signMain = (ch == '+') ? 1 : -1;
               iSign = true;
            } else if (iE && !iNumberAfterE && !iSignAfterE) {
               signExpo = (ch == '+') ? 1 : -1;
               iSignAfterE = true;
            } else {
               iFlag = false;
               break;
            }
         }
         // Decimal point must appear after integer digits and before 'e'
         else if (ch == '.' && !iD && iNumber && !iE) iD = true;
         // 'e' must appear after digits or after valid fractional part
         else if ((ch == 'e' || ch == 'E') && !iE && (iNumber || (iD && iNumberAfterD))) iE = true;
         // Accumulate numerical digits based on current state (integer, fraction, or exponent)
         else if (ch is >= '0' and <= '9') {
            if (iE) {
               exponent = (exponent * 10) + (ch - '0');
               iNumberAfterE = true;
            } else if (iD) {
               number = (number * 10) + (ch - '0');
               factor *= 10;
               iNumberAfterD = true;
            } else {
               number = (number * 10) + (ch - '0');
               iNumber = true;
            }
         }
         // Invalid character encountered
         else {
            iFlag = false;
            break;
         }
      }
      // check for required digits after '.' and 'e'
      if (!(iNumber && (!iD || iNumberAfterD) && (!iE || iNumberAfterE))) iFlag = false;
      // Return result if valid, else NaN
      return iFlag ? Math.Round (((signMain * number) / factor)
         * Math.Pow (10, signExpo * exponent), 3) : double.NaN;
   }
   #endregion
}
#endregion