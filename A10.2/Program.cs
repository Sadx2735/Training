// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// For testing the custom DeQueue implementation against System.Collections.Generic.LinkedList.
// ------------------------------------------------------------------------------------------------

using CustomDeQueue;

#region Class Program -----------------------------------------------------------------------------
/// <summary>Tests the custom DeQueue implementation against LinkedList.</summary>
class Program {
   /// <summary>Runs the test suite to verify DeQueue behavior against LinkedList.</summary>
   static void Main () {
      bool isSame = RunTest (100);
      if (isSame) {
         Console.ForegroundColor = ConsoleColor.Green;
         Console.WriteLine ("Passed all tests!");
      } else {
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine ("Failed!");
      }
      Console.ResetColor ();
   }

   #region Implementation -------------------------------------------
   /// Executes operations and validates consistency.
   static bool RunTest (int iterations) {
      bool iSame = true;
      LinkedList<int> ReferenceQ = new ();
      MyDeQueue<int> CustomQ = new ();
      Random rand = new ();
      for (int i = 0; i < iterations; i++) {
         // Verify state integrity (Count & IsEmpty) before each operation
         if (ReferenceQ.Count != CustomQ.Count
            || (ReferenceQ.Count == 0) != CustomQ.IsEmpty()) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine ($"State Mismatch at step {i}!");
            Console.ResetColor ();
            return false;
         }

         double prob = rand.NextDouble ();
         // probability is less than 0.25 do left append / add
         if (prob < 0.25) {
            int val = rand.Next (1, 100);
            ReferenceQ.AddFirst (val);
            CustomQ.PushFront (val);
         }
         // probability is less than 0.50 do right append / add
         else if (prob < 0.50) {
            int val = rand.Next (1, 100);
            ReferenceQ.AddLast (val);
            CustomQ.PushBack (val);
         }
         // probability is less than 0.70 do left pop / remove
         else if (prob < 0.70 && ReferenceQ.Count > 0) {
            int refVal = ReferenceQ.First.Value;
            ReferenceQ.RemoveFirst ();
            int customVal = CustomQ.PopFront ();
            iSame = Verify ("PopFront", i, refVal, customVal);
         }
         // probability is less than 0.90 do right pop / remove
         else if (prob < 0.90 && ReferenceQ.Count > 0) {
            int refVal = ReferenceQ.Last.Value;
            ReferenceQ.RemoveLast ();
            int customVal = CustomQ.PopBack ();
            iSame = Verify ("PopBack", i, refVal, customVal);
         }
         // when more than 0.90 check if peek works correctly this doesnt need that much of 
         // checking as the pop checks this in a more appropriate manner
         else if (ReferenceQ.Count > 0) {
            if (prob < 0.95) {
               int refVal = ReferenceQ.First.Value;
               int customVal = CustomQ.PeekLeft ();
               iSame = Verify ("PeekLeft", i, refVal, customVal);
            } else {
               int refVal = ReferenceQ.Last.Value;
               int customVal = CustomQ.PeekRight ();
               iSame = Verify ("PeekRight", i, refVal, customVal);
            }
         }
         if (!iSame) break;
      }
      return iSame;
   }

   /// <summary>Compares expected and actual outputs for an operation.</summary>
   static bool Verify (string op,int step, int expected, int actual) {
      if (expected == actual) {
         Console.ForegroundColor = ConsoleColor.Green;
         Console.WriteLine ($"At {step,5} {op,-3} : Test passed.");
         Console.WriteLine ($"LinkedList: {expected,3} | MyDeQueue: {actual,3}");
         Console.ResetColor ();
         return true;
      }
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine ($"At {step,5} {op,-8} : Test Failed.");
      Console.WriteLine ($"LinkedList: {expected,3} | MyDeQueue: {actual,3}");
      Console.ResetColor ();
      return false;
   }
   #endregion
}
#endregion