// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// For testing the custom queue implementation against the built-in Queue.
// ------------------------------------------------------------------------------------------------

using CustomQueue;

#region Class Program -----------------------------------------------------------------------------
/// <summary>Tests the CustomQ implementation against System.Collections.Generic.Queue.</summary>
class Program {
   /// <summary>Runs the test to check whether CustomQ matches the built-in queue.</summary>
   static void Main () {
      bool iSame = RunTest (100, 0.50);
      if (iSame) {
         Console.ForegroundColor = ConsoleColor.Green;
         Console.WriteLine ("Passed the test!");
      } else {
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine ("Test Failed!");
      }
      Console.ResetColor ();
   }

   #region Implementation -------------------------------------------
   // Runs random Enqueue/Dequeue operations to verify output parity.
   static bool RunTest (int times, double ratio) {
      bool hasPassed = true;
      Queue<int> builtinQ = new ();
      MyQueue<int> customQ = new ();
      Random rand = new ();
      for (int i = 0; i < times; i++) {
         double value = rand.NextDouble ();
         // Perform Dequeue if ratio condition is met and queues contain elements
         if (value < ratio && builtinQ.Count > 0) {
            int res1 = builtinQ.Dequeue ();
            int res2 = customQ.Dequeue ();
            if (res1 == res2) {
               Console.WriteLine ("----------");
               Console.ForegroundColor = ConsoleColor.Green;
               Console.WriteLine ($"Step {i} : Test passed.");
               Console.WriteLine ($"Queue: {res1} | MyQueue: {res2}");
            } else {
               Console.WriteLine ("----------");
               Console.ForegroundColor = ConsoleColor.Red;
               Console.WriteLine ($"Step {i} : Test Failed.");
               Console.WriteLine ($"Queue: {res1} | MyQueue: {res2}");
               hasPassed = false;
            }
            Console.ResetColor ();
         } else {
            int r = rand.Next (1, 100);
            builtinQ.Enqueue (r);
            customQ.Enqueue (r);
         }
      }
      return hasPassed;
   }
   #endregion
}
#endregion