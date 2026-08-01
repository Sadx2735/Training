// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// -------------------------------------------------------------------------------------------------
// Program.cs
// Implements a number guessing game

#region Program -------------------------------------------------------------------------------------------------
/// <summary>Summary for The script</summary>
/// Generates a random target number and prompts the user to guess it within a given range
/// And Comments about the users guess ( exact , Low , High )
/// finally ends when the user guesses the answer Correctly.

int target = new Random ().Next (1, 101);
int trials = 0;

for (; ; ) {
   int number = ReadInt ("Guess a Number between (1-100) : ");
   trials++;
   if (number == target) {
      Console.WriteLine ($"You guessed correctly");
      break;
   } 
   else if (number < target) 
      Console.WriteLine ("Your guess is too low");
   else 
      Console.WriteLine ("Your guess is too high");
}
Console.WriteLine ($"guessed in {trials} trials");

#region Implementations -----------------------------------------------------------------------------------------

/// <summary>
/// Keeps asking user for a valid number
/// </summary>
/// <param name="pmt">Prompt to show in the Console</param>
/// <returns>Returns the Number Guessed by the User</returns>
int ReadInt (string pmt) {
   for (; ; ) {
      Console.Write (pmt);
      string? inp = Console.ReadLine();
      if (int.TryParse (inp, out int result)) 
         return result;
      else 
         Console.WriteLine ("Please enter a Valid Number..");
   }
}
#endregion
#endregion