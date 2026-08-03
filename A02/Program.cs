// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -------------------------------------------------------------------------------------------------
// Program.cs
// Implements a number guessing game
// -------------------------------------------------------------------------------------------------

#region Program -----------------------------------------------------
/// <summary>Generates a random target number and prompts the user to guess it within a given range.</summary>
int target = new Random ().Next (1, 101);
int trials = 0;
for (; ; ) {
   int number = ReadInt ();
   trials++;
   if (number == target) {
      Console.WriteLine ("You guessed correctly!");
      break;
   } else if (number < target)
      Console.WriteLine ("Your guess is too low.");
   else
      Console.WriteLine ("Your guess is too high.");
}
Console.WriteLine ($"Guessed in {trials} trials.");
#endregion

#region Implementation ----------------------------------------------
/// <summary>Keeps asking the user for a valid number.</summary>
/// <returns>Returns the number guessed by the user.</returns>
int ReadInt () {
   for (; ; ) {
      Console.Write ("Guess a number between 1 and 100: ");
      string? input = Console.ReadLine ();
      if (int.TryParse (input, out int result))
         return result;
      Console.WriteLine ("Please enter a valid number.");
   }
}
#endregion
