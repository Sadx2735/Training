// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Reads a word list and finds the top 7 most frequent characters.
// ------------------------------------------------------------------------------------------------

#region Program -----------------------------------------------------
/// <summary>Reads a word list file and displays the top 7 most frequent characters.</summary>
Dictionary<char, int> freqTable = new ();
string filePath = "WordsList.txt";
string content = File.ReadAllText (filePath);
foreach (var ch in content) {
   if (ch is >= 'A' and <= 'Z') {
      if (freqTable.TryGetValue (ch, out int value))
         freqTable[ch]++;
      else
         freqTable[ch] = 1;
   }
}
Console.WriteLine ("Printing top 7 elements:");
foreach (var pair in freqTable.OrderByDescending (item => item.Value).Take (7))
   Console.WriteLine ($"Character {pair.Key} was used {pair.Value} times.");
#endregion