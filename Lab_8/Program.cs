using System;
using Lab8.Models;
using Lab8.Services;

namespace Lab8
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Путь к бинарному файлу
            const string filePath = "movies.dat";
            var database = new MovieDatabase(filePath);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== КАТАЛОГ ФИЛЬМОВ ===");
                Console.WriteLine("1. Просмотр базы данных");
                Console.WriteLine("2. Добавить фильм");
                Console.WriteLine("3. Удалить фильм по ID");
                Console.WriteLine("4. Запрос A: Фильмы жанра с рейтингом выше заданного");
                Console.WriteLine("5. Запрос B: Фильмы после года X (сортировка по рейтингу)");
                Console.WriteLine("6. Запрос C: Средний рейтинг всех фильмов");
                Console.WriteLine("7. Запрос D: Фильм с самым высоким рейтингом");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            database.ShowAll();
                            break;
                        case "2":
                            AddMovieFromConsole(database);
                            break;
                        case "3":
                            int idToDelete = GetValidInt("Введите ID фильма для удаления: ");
                            database.DeleteMovie(idToDelete);
                            break;
                        case "4":
                            string genre = GetValidString("Введите жанр: ");
                            double minRating = GetValidDouble("Введите минимальный рейтинг: ", 0, 10);
                            database.QueryByGenreAndRating(genre, minRating);
                            break;
                        case "5":
                            int year = GetValidInt("Введите год: ");
                            database.QueryByYearSorted(year);
                            break;
                        case "6":
                            database.QueryAverageRating();
                            break;
                        case "7":
                            database.QueryTopRatedMovie();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Неверный пункт меню.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        // --- Приватные методы ввода и проверки ---

        private static void AddMovieFromConsole(MovieDatabase database)
        {
            Console.WriteLine("\n--- Добавление нового фильма ---");
            int id = GetValidInt("Введите ID: ");
            string title = GetValidString("Введите название: ");
            string genre = GetValidString("Введите жанр: ");
            int year = GetValidInt("Введите год выпуска: ");
            double rating = GetValidDouble("Введите рейтинг (0-10): ", 0, 10);
            int duration = GetValidInt("Введите длительность в минутах: ");

            var newMovie = new Movie(id, title, genre, year, rating, duration);
            database.AddMovie(newMovie);
        }

        private static int GetValidInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result) && result > 0)
                {
                    return result;
                }
                Console.WriteLine("Ошибка: введите положительное целое число.");
            }
        }

        private static double GetValidDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (double.TryParse(input, out double result) && result >= min && result <= max)
                {
                    return result;
                }
                Console.WriteLine($"Ошибка: введите число от {min} до {max}.");
            }
        }

        private static string GetValidString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
                Console.WriteLine("Ошибка: строка не может быть пустой.");
            }
        }
    }
}