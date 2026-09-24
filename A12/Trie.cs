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
   #region Constructors ---------------------------------------------
   /// <summary>Initializes the links for 26 letters.</summary>
   public Node () => mLinks = new Node[26];
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Returns the child node for the given letter.</summary>
   public Node GetNode (char ch) => mLinks[ch - 'A'];

   /// <summary>Returns true if a word ends at this node.</summary>
   public bool IsEnd () => mEnd;

   /// <summary>Sets the child node for the given letter.</summary>
   public void PutNode (char ch, Node nde) => mLinks[ch - 'A'] = nde;

   /// <summary>Marks that a word ends at this node.</summary>
   public void SetEnd () => mEnd = true;
   #endregion

   #region Fields ---------------------------------------------------
   Node[] mLinks;
   bool mEnd;
   #endregion
}
#endregion

#region Class Trie --------------------------------------------------------------------------------
/// <summary>Implements a Trie that stores words and suggests words for a given prefix.</summary>
public class Trie {
   #region Constructors ---------------------------------------------
   /// <summary>Initializes the root node.</summary>
   public Trie () => mRoot = new Node ();
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Returns all the words in the trie that start with the given prefix.</summary>
   /// <param name="word">The prefix to search for.</param>
   /// <returns>List of matching words (empty if none).</returns>
   public List<string> GetSuggestion (string word) {
      var results = new List<string> ();
      Node temp = mRoot;
      foreach (var ch in word) {
         if (temp.GetNode (ch) is null) return results;
         temp = temp.GetNode (ch);
      }
      DFS (temp, word);
      return results;

      // Helper .....................................................
      // Recursively attempts to search possible words.
      void DFS (Node node, string prefix) {
         if (node.IsEnd ()) results.Add (prefix);
         for (char st = 'A'; st <= 'Z'; st++) {
            var nde = node.GetNode (st);
            if (nde != null) DFS (nde, prefix + st);
         }
      }
   }

   /// <summary>Adds a word to the trie.</summary>
   /// <param name="word">The word to add.</param>
   public void Insert (string word) {
      Node temp = mRoot;
      foreach (var ch in word) {
         if (temp.GetNode (ch) is null) temp.PutNode (ch, new Node ());
         temp = temp.GetNode (ch);
      }
      temp.SetEnd ();
   }
   #endregion

   #region Fields ---------------------------------------------------
   Node mRoot;
   #endregion
}
#endregion