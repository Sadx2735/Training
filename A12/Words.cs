namespace WordleGame;
class WordBank {

   string[] availableWords;
   string[] dictionaryWords;
   Random randomizer = new Random ();

   public WordBank (string puzzlePath, string dictPath) {
      availableWords = File.ReadAllLines (puzzlePath);
      dictionaryWords = File.ReadAllLines (dictPath);
   }

   public string GetRandomWord () {
      int index = randomizer.Next (availableWords.Length);
      return availableWords[index];
   }

   public bool IsValidWord (string word) {
      return dictionaryWords.Contains (word);
   }
}