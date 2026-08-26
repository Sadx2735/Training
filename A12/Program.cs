class Program {
   static void Main (String[] args) {
      Console.WriteLine ("Init for Wordle :)");
   }
}

/*
Rough Ideas to have while coding ( Personal Note )
While Getting Input :
1. Have a cursor that points to the first index.
2. Have a temp buffer of size 6.

While pressed :
1. if enter is clicked check if cursor is at size. ie end
2. backspace move back and make the buffer to default.
3. if alphabets are pressed assign and move the pointer towards right
4. if the state reaches the end. just show the result or when all the point goes 2 then also
you can just display the answer.
5. while updating color loop through the buffer and update till state - 1;

Coloring :
1. Give first priority to the same place same color
2. then iterate through the guess and evaluate if its not already seen
3. for the below letter board do the max(existing,considered)
*/