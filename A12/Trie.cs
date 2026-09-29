// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// Trie.cs
// Custom trie (prefix tree) implementation for storing words and suggesting completions.
// ------------------------------------------------------------------------------------------------

namespace CustomTrie;

#region Class Node --------------------------------------------------------------------------------
/// <summary>A single node of the trie holding links to its child nodes.</summary>
class Node {
   #region Properties -----------------------------------------------
   /// <summary>Child nodes, one for each letter from 'A' to 'Z'.</summary>
   public Node[] Links { get; } = new Node[26];

   /// <summary>True if a word ends at this node.</summary>
   public bool IsEnd { get; set; }
   #endregion
}
#endregion

#region Class Trie --------------------------------------------------------------------------------
/// <summary>Implements a Trie that stores words and suggests words for a given prefix.</summary>
public class Trie {
   #region Methods --------------------------------------------------
   /// <summary>Returns up to 3 words in the trie that start with the given prefix.</summary>
   /// <param name="word">The prefix to search for.</param>
   /// <returns>List of matching words (empty if none).</returns>
   public List<string> GetSuggestion (string word) {
      var results = new List<string> ();
      Node temp = mRoot;
      foreach (var ch in word) {
         temp = temp.Links[ch - 'A'];
         if (temp is null) return results;
      }
      DFS (temp, word);
      return results;

      // Helper .....................................................
      // Recursively collects the words below the given node.
      void DFS (Node node, string prefix) {
         if (results.Count == HINTCOUNT) return;
         if (node.IsEnd) results.Add (prefix);
         for (int i = 0; i < 26; i++) {
            var nde = node.Links[i];
            if (nde != null) DFS (nde, prefix + (char)('A' + i));
         }
      }
   }

   /// <summary>Adds a word to the trie.</summary>
   /// <param name="word">The word to add.</param>
   public void Insert (string word) {
      Node temp = mRoot;
      foreach (var ch in word)
         temp = ( temp.Links[ch - 'A'] ) ??= new Node ();
      temp.IsEnd = true;
   }
   #endregion

   #region Fields ---------------------------------------------------
   Node mRoot = new ();
   #endregion

   #region Constants ------------------------------------------------
   const int HINTCOUNT = 3;
   #endregion
}
#endregion