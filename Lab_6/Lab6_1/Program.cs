using Lab6_1.Classes;
using System;

namespace Lab6_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Тестирование базового класса ---");
            string title = GetValidString("Введите название фильма: ");

            // Создание объекта базового класса
            Movie baseMovie = new Movie(title);
            Console.WriteLine("\nРезультат метода GetFirstLastChars(): " + baseMovie.GetFirstLastChars());
            Console.WriteLine("Результат ToString(): " + baseMovie.ToString());

            // Тестирование конструктора копирования базового класса
            Movie copyBaseMovie = new Movie(baseMovie);
            Console.WriteLine("Копия (ToString): " + copyBaseMovie.ToString());

            Console.WriteLine("\n--- Тестирование дочернего класса ---");
            bool isBook = GetValidBool("Этот фильм снят по книге? (да/нет): ");
            string genre = GetValidString("Назовите жанр фильма: ");
            int rating = GetValidInt("Дайте оценку по десятибалльной шкале (1-10): ", 1, 10);

            // Создание объекта дочернего класса
            CharacteristicsFilm derivedMovie = new CharacteristicsFilm(title, isBook, genre, rating);
            Console.WriteLine("\nМетод GetBookStatus(): " + derivedMovie.GetBookStatus());
            Console.WriteLine("Метод GetRatingCategory(): " + derivedMovie.GetRatingCategory());
            Console.WriteLine("Результат ToString():\n" + derivedMovie.ToString());

            // Тестирование конструктора копирования дочернего класса
            CharacteristicsFilm copyDerivedMovie = new CharacteristicsFilm(derivedMovie);
            Console.WriteLine("\n--- Копия дочернего класса (ToString) ---");
            Console.WriteLine(copyDerivedMovie.ToString());

            Console.ReadLine();
        }

        // --- Приватные методы для проверки вводимых значений ---

        private static string GetValidString(string prompt)
        {
            string input;
            while (true)
            {
                Console.Write(prompt);
                input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
                Console.WriteLine("Ошибка: строка не может быть пустой. Попробуйте снова.");
            }
        }

        private static bool GetValidBool(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (input != null)
                {
                    input = input.Trim().ToLower();
                    if (input == "да" || input == "yes" || input == "y" || input == "1")
                    {
                        return true;
                    }
                    if (input == "нет" || input == "no" || input == "n" || input == "0")
                    {
                        return false;
                    }
                }
                Console.WriteLine("Ошибка: введите 'да' или 'нет'.");
            }
        }

        private static int GetValidInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                int result;

                if (int.TryParse(input, out result))
                {
                    if (result >= min && result <= max)
                    {
                        return result;
                    }
                }
                Console.WriteLine(string.Format("Ошибка: введите целое число от {0} до {1}.", min, max));
            }
        }
    }
}