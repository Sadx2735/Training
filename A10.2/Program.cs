// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// For testing the custom DeQueue implementation against System.Collections.Generic.LinkedList.
// ------------------------------------------------------------------------------------------------

using DQueue;

#region Class Program -----------------------------------------------------------------------------
/// <summary>Tests the custom DeQueue implementation against LinkedList.</summary>
class Program {
   #region Methods --------------------------------------------------
   /// <summary>Runs the test suite to verify DeQueue behavior against LinkedList.</summary>
   static void Main () {
      bool isSame = RunTest (100);
      if (isSame) {
         Console.ForegroundColor = ConsoleColor.Green;
         Console.WriteLine ("\nPassed all tests!" +
            " Custom DeQueue and LinkedList behave identically.");
      } else {
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine ("\nFailed! " +
            "Output mismatch detected between DeQueue and LinkedList.");
      }
      Console.ResetColor ();
   }

   /// <summary>Executes probabilistic operations and validates state consistency.</summary>
   /// <param name="iterations">The total number of test operations to execute.</param>
   /// <returns>True if both containers yield identical results; otherwise, false.</returns>
   static bool RunTest (int iterations) {
      bool same = true;
      for (int i = 0; i < iterations; i++) {
         // Verify state integrity (Count & IsEmpty) before each operation
         if (sReferenceQ.Count != sCustomQ.Count
            || (sReferenceQ.Count == 0) != sCustomQ.IsEmpty ()) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine ($"State Mismatch at step {i}! " +
               $"Ref Count: {sReferenceQ.Count}, Custom Count: {sCustomQ.Count}");
            Console.ResetColor ();
            return false;
         }

         double prob = sRand.NextDouble ();
         // probability is less than 0.25 do left append / add
         if (prob < 0.25) {
            int val = sRand.Next (1, 100);
            sReferenceQ.AddFirst (val);
            sCustomQ.PushFront (val);
         }
         // probability is less than 0.50 do right append / add
         else if (prob < 0.50) {
            int val = sRand.Next (1, 100);
            sReferenceQ.AddLast (val);
            sCustomQ.PushBack (val);
         }
         // probability is less than 0.70 do left pop / remove
         else if (prob < 0.70 && sReferenceQ.Count > 0) {
            int refVal = sReferenceQ.First.Value;
            sReferenceQ.RemoveFirst ();
            int customVal = sCustomQ.PopFront ();
            same = Verify ("PopFront", i, refVal, customVal);
         }
         // probability is less than 0.90 do right pop / remove
         else if (prob < 0.90 && sReferenceQ.Count > 0) {
            int refVal = sReferenceQ.Last.Value;
            sReferenceQ.RemoveLast ();
            int customVal = sCustomQ.PopBack ();
            same = Verify ("PopBack", i, refVal, customVal);
         }
         // when more than 0.90 check if peek works correctly this doesnt need that much of 
         // checking as the pop checks this in a more appropriate manner
         else if (sReferenceQ.Count > 0) {
            if (prob < 0.95) {
               int refVal = sReferenceQ.First.Value;
               int customVal = sCustomQ.PeekLeft ();
               same = Verify ("PeekLeft", i, refVal, customVal);
            } else {
               int refVal = sReferenceQ.Last.Value;
               int customVal = sCustomQ.PeekRight ();
               same = Verify ("PeekRight", i, refVal, customVal);
            }
         }
         if (!same) break;
      }
      return same;
   }

   /// <summary>Compares expected and actual outputs for an operation.</summary>
   static bool Verify (string op, int step, int expected, int actual) {
      if (expected == actual) {
         Console.ForegroundColor = ConsoleColor.Green;
         Console.WriteLine ($"[{step}] {op} Passed -> Expected: {expected} | Got: {actual}");
         Console.ResetColor ();
         return true;
      }

      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine ($"[{step}] {op} FAILED -> Expected: {expected} | Got: {actual}");
      Console.ResetColor ();
      return false;
   }
   #endregion

   #region Fields ---------------------------------------------------
   static LinkedList<int> sReferenceQ = new ();
   static DeQueue<int> sCustomQ = new ();
   static Random sRand = new ();
   #endregion
}
#endregion