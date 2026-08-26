// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Entry point to execute predefined test cases and evaluate interactive user input.
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region Class Program -----------------------------------------------------------------------------
/// <summary>Runs test cases and handles interactive user prompt evaluation.</summary>
class Program {
   /// <summary>Runs test cases and starts interactive input loop.</summary>
   static void Main () {
      Dictionary<string, double> validCases = new () {
         { "-10 ^ 2", 100 }, { "a = 4", 4 }, { "b = 3.5", 3.5 },
         { "a + b", 7.5 }, { "asin sin 90", 90 }, { "atan tan 45", 45 },
         { "sqrt 25", 5 }, { "log 1", 0 }, { "-2 -2", -4 }, { "10/2+3", 8 },
         { "(+10+3)*2", 26 }, { "(a+2) * a", 24 }, { "cos 0", 1 },
         { "exp 2", 7.3890560989 }, { "cos acos 0", 0 }, { "10 -2 -2", 6 },
         { "---5", -5 }, { "-5+10", 5 }, { "-2-4", -6 }, { "---4+5--2+3", 6 },
         { "(2+3)*-4", -20 }, { "(2+3)*+5", 25 }, { "-4*(3+5)", -32 },
         { "-4+5--8", 9 }, { "-4+5-(-8)", 9 }, { "10^(-4+2)", .01 },
         { "-10-10^2", -110 }, { "---4+5--6-2", 5 }, { "sin 45", 0.7071067812 },
         { "sqrt asin-1", double.NaN }, { "sin -45", -0.7071067812 },
         { "-sin 45", -0.7071067812 }, { "cos 45", 0.7071067812 },
         { "cos -45", 0.7071067812 }, { "tan 45", 1.0 }, { "sin(45+45)", 1.0 },
         { "(sin 90)*2", 2 }, { "sin --90", 1.0 }, { "log 10", 2.302585093 },
         { "sqrt 100", 10 }, { "sqrt(10^2)", 10 }, { "(sqrt 100) - 10", 0 },
         { "(tan 45)+10-20", -9 }, { "asin 1", 90 }, { "acos 0", 90 },
         { "(log 10)+5", 7.302585093 }, { "log(10+5)", 2.7080502011 },
         { "(sin 90)--1", 2 }, { "(sin -90)--1", 0 }, { "sqrt(90+10)", 10 },
         { "sqrt(110-10)", 10 }, { "atan 1", 45 }, { "asin -1", -90 },
         { "atan -1", -45 }, { "(atan -1)+45", 0 }, { "exp 1", 2.7182818285 },
         { "exp 1-2", .7182818285 }, { "exp(2-1)", 2.7182818285 },
         { "exp -1", 0.3678794412 }, { "sqrt -100", double.NaN },
         { "log(-10+5)", double.NaN }, { "sin(sqrt-1)", double.NaN }
      };
      List<string> exceptionCases = ["3 + * 5", "(4 + 6", "2 + abc", "6 *", "5 * (3 + 2))"];
      var eval = new Evaluator ();
      // Header for table
      Console.WriteLine ($"| {"Input",-19}| {"Output",-34}| Verdict");
      Console.WriteLine ($"{new string ('-', 20)} {new string ('-', 35)} {new string ('-', 10)}");
      // Valid test cases evaluation
      foreach ((string qns, double ans) in validCases) {
         var answer = double.NaN;
         try { answer = eval.Evaluate (qns); } catch { }
         bool passed = Math.Abs (answer - ans) < 1e-6
            || (double.IsNaN (ans) && double.IsNaN (answer));
         PrintRow (qns, answer.ToString (), passed);
      }
      // Exception test cases evaluation
      foreach (string qns in exceptionCases) {
         try {
            var val = eval.Evaluate (qns);
            PrintRow (qns, val.ToString (), false);
         } catch (Exception ex) {
            PrintRow (qns, ex.Message, true);
         }
      }
      // User input prompt loop
      Console.WriteLine ();
      for (; ; ) {
         Console.Write ("> ");
         string text = Console.ReadLine () ?? "";
         if (text == "exit") break;
         try {
            double result = eval.Evaluate (text);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine (result);
         } catch (Exception e) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine (e.Message);
         }
         Console.ResetColor ();
      }
   }

   #region Implementation -------------------------------------------
   // Prints evaluated results in aligned table rows.
   static void PrintRow (string input, string output, bool passed) {
      Console.Write ($"| {input,-19}| {output,-34}| ");
      Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
      Console.WriteLine (passed ? "PASS" : "FAIL");
      Console.ResetColor ();
   }
   #endregion
}
#endregion