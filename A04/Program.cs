// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// Reads a word list and finds the top 7 most frequent characters.
// ------------------------------------------------------------------------------------------------
using System.Reflection;

#region Program -----------------------------------------------------
/// <summary>Reads a word list file and displays the top 7 most frequent characters.</summary>
Dictionary<char, int> freqTable = [];
var assembly = Assembly.GetExecutingAssembly ();
var resourceName = assembly.GetManifestResourceNames ().First (name => name.EndsWith ("WordsList.txt"));
using var stream = assembly.GetManifestResourceStream (resourceName)!;
using var reader = new StreamReader (stream);
string content = reader.ReadToEnd ();
foreach (var ch in content) {
   if (ch is >= 'A' and <= 'Z')
      freqTable[ch] = freqTable.GetValueOrDefault (ch) + 1;
}
Console.WriteLine ("Printing top 7 elements:");
foreach (var pair in freqTable.OrderByDescending (item => item.Value).Take (7))
   Console.WriteLine ($"Character {pair.Key} was used {pair.Value} times.");
#endregion