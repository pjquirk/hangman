using System;
using System.Collections.Generic;
using System.Linq;

namespace hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            List<char> letters = new List<char> {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };

            Console.WriteLine("Tell me the secret word (I won't peek):");
            var solution = Console.ReadLine();

            var dictionaryWords = System.IO.File.ReadAllLines("dictionary.txt");
            Console.WriteLine($"There are {dictionaryWords.Length} words in the dictionary.");
            if (!dictionaryWords.Contains(solution))
            {
                Console.WriteLine("Sorry, that word isn't in the dictionary.");
                return;
            }

            var random = new Random();
            const int guesses_allowed = 7;
            string current_guess = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] currentSolution = Enumerable.Range(0, solution.Length).Select(i => '_').ToArray();

            for (var guesses = 0; guesses < guesses_allowed; guesses++)
            {
                Console.WriteLine($"I have {guesses_allowed - guesses} guesses left.");
                Console.WriteLine($"Current guess: "+ current_guess);

                char random_letter;
                do
                {
                    random_letter = letters[random.Next(0, letters.Count)];
                } while (!current_guess.Contains(random_letter));
                letters.Remove(random_letter);

                Console.WriteLine($"Is there a {random_letter}?");
                var word = Console.ReadLine();
                if (word.Contains('y', StringComparison.InvariantCultureIgnoreCase))
                {
                    int index = -1;
                    while ((index = solution.ToUpper().IndexOf(random_letter.ToString().ToUpper(), index+1)) != -1)
                    {
                        currentSolution[index] = random_letter;
                    }
                    guesses--;
                }

                if (!currentSolution.Contains('_'))
                {
                    Console.WriteLine("I won!");
                    return;
                }

                Console.WriteLine($"I'm looking for a word that looks like {new string(currentSolution)}");
                var match = $"^{new string(currentSolution)}$".Replace("_", ".");
                var matchingWords = dictionaryWords.Where(word => System.Text.RegularExpressions.Regex.IsMatch(word, match, System.Text.RegularExpressions.RegexOptions.IgnoreCase)).ToArray();
                current_guess = matchingWords[random.Next(0, matchingWords.Length)].ToUpper();
                Console.WriteLine($"I think the word may be '{current_guess}'...");
            }

            Console.WriteLine("You won! What was the word?");
        }

        static void Hangman(string[] args)
        {
            Console.WriteLine("Hello Finn!");

            var lines = System.IO.File.ReadAllLines("dictionary.txt");
            Console.WriteLine($"There are {lines.Length} words in the dictionary.");

            var random = new Random();
            var word = "";
            while (word.Length <= 2)
            {
                word = lines[random.Next(lines.Length)];
            }
            Console.WriteLine($"We picked the word: {word}");

            var guessed = new char[word.Length];
            for (int i = 0; i < guessed.Length; i++)
            {
                guessed[i] = '_';
            }

            const int guesses_allowed = 7;
            var guesses = 0;
            while (guesses < guesses_allowed)
            {
                Console.WriteLine($"You have {guesses_allowed - guesses} guesses left.");
                Console.WriteLine($"The word is: {string.Join(" ", guessed)}");
                Console.WriteLine("Guess a letter:");
                var guess = Console.ReadLine();
                if (guess.Length != 1)
                {
                    Console.WriteLine("Please enter a single letter.");
                    continue;
                }
                if (word.Contains(guess))
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (word[i] == guess[0])
                        {
                            guessed[i] = guess[0];
                        }
                    }
                }
                else
                {
                    guesses++;
                }
                if (string.Join("", guessed) == word)
                {
                    Console.WriteLine("You win! The word was: " + word);
                    break;
                }
            }
        }
    }
}
