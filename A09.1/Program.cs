// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// For testing the custom queue implementation against the built-in Queue.
// ------------------------------------------------------------------------------------------------

using Cqueue;

#region Class Program -----------------------------------------------------------------------------
/// <summary>Tests the CustomQ implementation against System.Collections.Generic.Queue.</summary>
class Program {
   #region Methods --------------------------------------------------
   /// <summary>Runs the test to check whether CustomQ matches the built-in queue.</summary>
   static void Main () {
      bool isSame = RunTest (100, 0.50);
      if (isSame) {
         Console.ForegroundColor = ConsoleColor.Green;
         Console.WriteLine ("Passed the test! Both implementations work identically.");
      } else {
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine ("Failed! The custom implementation and built-in queue do not match.");
      }
      Console.ResetColor ();
   }

   /// <summary>Runs random Enqueue/Dequeue operations to verify output parity.</summary>
   /// <param name="times">The total number of test iterations to execute.</param>
   /// <param name="ratio">The probability threshold for triggering a Dequeue operation.</param>
   /// <returns>True if both queues yield identical outputs; otherwise, false.</returns>
   static bool RunTest (int times, double ratio) {
      bool same = true;
      for (int i = 0; i < times; i++) {
         double value = sRand.NextDouble ();
         // Perform Dequeue if ratio condition is met and queues contain elements
         if (value < ratio && sBuiltinQ.Count > 0) {
            int res1 = sBuiltinQ.Dequeue ();
            int res2 = sCustomQ.Dequeue ();
            if (res1 == res2) {
               Console.WriteLine ("----------");
               Console.ForegroundColor = ConsoleColor.Green;
               Console.WriteLine ($"Passed test step {i}! Both work identically.");
               Console.WriteLine ($"Expected: {res1} | Got: {res2}");
            } else {
               Console.WriteLine ("----------");
               Console.ForegroundColor = ConsoleColor.Red;
               Console.WriteLine ($"Failed test step {i}! Outputs differ.");
               Console.WriteLine ($"Expected {res1}, but got {res2}.");
               same = false;
            }
            Console.ResetColor ();
         } else {
            int r = sRand.Next (1, 100);
            sBuiltinQ.Enqueue (r);
            sCustomQ.Enqueue (r);
         }
      }
      return same;
   }
   #endregion

   #region Fields ---------------------------------------------------
   static Queue<int> sBuiltinQ = new ();
   static CustomQ<int> sCustomQ = new ();
   static Random sRand = new ();
   #endregion
}
#endregion