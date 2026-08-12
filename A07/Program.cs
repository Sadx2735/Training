// -------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -------------------------------------------------------------------------------------------------
// Program.cs
// Custom Parser to convert string to Double
// -------------------------------------------------------------------------------------------------

#region Class Program -----------------------------------------------------------------------------
internal class Program {
   #region Methods --------------------------------------------------
   private static void Main (string[] args) {
      // Test Cases 
      string[] words = new string[] {
         "10.54E23.4E3", "-1.546234E-4", "0", "0.0", "12345", "0.00000325", "10.54E2",
         "1.5E2.5", "1.E3", "1.E+3", "12.54e3.", "12.", "12.e1", ".325", " +625 ", "6.25e0",
         "6.0e0", "6.25e-1", "+6.25E1", "*6.25", "10.625", "15a1", "1.567*2", "+-12",
         "12.-5", ".e1", "-0.325", "  12.456    "
      };

      foreach (var rawInput in words) {
         // Sign, MainNumber, Exponent
         string userInput = rawInput.Trim ().ToLower ();
         int signMain = 1;
         int signExpo = 1;

         double number = 0;
         double exponent = 0;
         double factor = 0.1;

         // Condition flags to accept or reject inputs based on syntax rules
         // (e.g., exponent symbol 'e' must not appear before any digits).
         bool hasNumber = false;
         bool hasNumberAfterE = false;
         bool hasNumberAfterD = false;

         bool hasE = false;
         bool hasD = false;

         bool hasSign = false;
         bool hasSignAfterE = false;

         // Overall execution control flag
         bool flag = true;

         foreach (var ch in userInput) {
            // For a sign to be present, it must either be at the start 
            // or directly after 'e' before any exponent digits appear.
            if ((ch == '+' || ch == '-') && (!hasSign || !hasSignAfterE) && (!hasNumber || (hasE && !hasNumberAfterE))) {
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
            // For a decimal point to be present, it must appear after the integer digits 
            // and before any exponent symbol.
            else if (ch == '.' && !hasD && hasNumber && !hasE) {
               hasD = true;
            }
            // For 'e' to be present, it must appear after digits, 
            // or after the fractional part if a decimal point exists.
            else if (ch == 'e' && !hasE && (hasNumber || (hasD && hasNumberAfterD))) {
               hasE = true;
            }
            // For numerical digits (0-9) to be accumulated.
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
            // If the character does not match any valid condition, mark as invalid.
            else {
               flag = false;
               break;
            }
         }

         // Final check to verify that required digits exist after '.' and 'e' if present.
         if (!(hasNumber && (!hasD || hasNumberAfterD) && (!hasE || hasNumberAfterE))) {
            flag = false;
         }

         Console.WriteLine ("----------");
         Console.WriteLine ($"For the input of {userInput}");
         double customResult = Math.Round (signMain * number * Math.Pow (10, signExpo * exponent), 3);
         Console.WriteLine ($"Via Custom Parse   : {(flag ? customResult : double.NaN)}");
         double builtInResult = double.TryParse (userInput, out double val) ? val : double.NaN;
         Console.WriteLine ($"Via Built-In Parse : {builtInResult}");
      }
   }
   #endregion
}
#endregion