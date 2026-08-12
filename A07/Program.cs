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

   #region Methods --------------------------------------------------------------------------------
   /// <summary>Iterates through each word and converts them into double</summary>
   static void Main (string[] args) {
      // Test cases covering valid, invalid inputs
      string[] words = new string[] {
         "10.54E23.4E3", "-1.546234E-4", "0", "0.0", "12345", "0.00000325", "10.54E2",
         "1.5E2.5", "1.E3", "1.E+3", "12.54e3.", "12.", "12.e1", ".325", " +625 ", "6.25e0",
         "6.0e0", "6.25e-1", "+6.25E1", "*6.25", "10.625", "15a1", "1.567*2", "+-12",
         "12.-5", ".e1", "-0.325", "  12.456    "
      };

      foreach (var rawInput in words) {
         string userInput = rawInput.Trim ().ToLower ();
         double customResult = Parse (userInput);
         Console.WriteLine ("--------------------------------------------------");
         Console.WriteLine ($"For the input of {userInput}");
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
      double number = 0, exponent = 0, factor = 0.1;

      // Syntax rule tracking flags
      bool hasNumber = false, hasNumberAfterE = false, hasNumberAfterD = false;
      bool hasE = false, hasD = false, hasSign = false, hasSignAfterE = false;
      bool flag = true;

      foreach (var ch in userInput) {
         // Sign must be at the start or directly after 'e'
         if ((ch == '+' || ch == '-') &&
            (!hasSign || !hasSignAfterE) && (!hasNumber || (hasE && !hasNumberAfterE))) {
            if (!hasNumber && !hasSign) {
               signMain = (ch == '+') ? 1 : -1;
               hasSign = true;
            } else if (hasE && !hasNumberAfterE && !hasSignAfterE) {
               signExpo = (ch == '+') ? 1 : -1;
               hasSignAfterE = true;
            } else {
               flag = false;
               break;
            }
         }
         // Decimal point must appear after integer digits and before 'e'
         else if (ch == '.' && !hasD && hasNumber && !hasE) {
            hasD = true;
         }
         // 'e' must appear after digits or after valid fractional part
         else if (ch == 'e' && !hasE && (hasNumber || (hasD && hasNumberAfterD))) {
            hasE = true;
         }
         // Accumulate numerical digits based on current state (integer, fraction, or exponent)
         else if (ch is >= '0' and <= '9') {
            if (hasE) {
               exponent = (exponent * 10) + (ch - '0');
               hasNumberAfterE = true;
            } else if (hasD) {
               number += factor * (ch - '0');
               factor *= 0.1;
               hasNumberAfterD = true;
            } else {
               number = (number * 10) + (ch - '0');
               hasNumber = true;
            }
         }
         // Invalid character encountered
         else {
            flag = false;
            break;
         }
      }
      // check for required digits after '.' and 'e'
      if (!(hasNumber && (!hasD || hasNumberAfterD) && (!hasE || hasNumberAfterE))) {
         flag = false;
      }
      // Return result if valid, else NaN
      return flag ? Math.Round (signMain * number * Math.Pow (10, signExpo * exponent), 3) 
         : double.NaN;
   }
   #endregion
}
#endregion