// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Reads the word list and gets the top 7 most frequent characters.

#region Program -----------------------------------------------------------------------------------
/// <summary>
/// Opens up a file and iterates over characters to increase their frequency table.
/// Sorts the table by value (frequency) and then takes the Top 7 (key, value) pairs.
/// </summary>

Dictionary<char, int> freqTable = new ();
string content = File.ReadAllText (@"C:\Users\msara\Downloads\Words-List.txt");

foreach (var ch in content) {
   if (ch is >= 'A' and <= 'Z') {
      if (freqTable.TryGetValue (ch, out int value))
         freqTable[ch]++;
      else
         freqTable[ch] = 1;
   }
}

Console.WriteLine ("Printing Top 7 Elements..");
foreach (var pair in freqTable.OrderByDescending (item => item.Value).Take (7)) {
   Console.WriteLine ($"character {pair.Key} was used {pair.Value} times");
}
#endregion