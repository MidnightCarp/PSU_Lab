using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lab8.Models;

namespace Lab8.Services
{
    public class MovieDatabase
    {
        private readonly List<Movie> _movies;
        private readonly string _filePath;

        public MovieDatabase(string path)
        {
            _filePath = path;
            _movies = new List<Movie>();
            LoadFromFile();
        }

        // Приватный метод загрузки из бинарного файла
        private void LoadFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return;
            }

            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(_filePath, FileMode.Open)))
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        var movie = new Movie
                        {
                            Id = reader.ReadInt32(),
                            Title = reader.ReadString(),
                            Genre = reader.ReadString(),
                            Year = reader.ReadInt32(),
                            Rating = reader.ReadDouble(),
                            DurationMinutes = reader.ReadInt32(),
                        };
                        _movies.Add(movie);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода-вывода при чтении файла: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Нет доступа к файлу: {ex.Message}");
            }
        }

        // Приватный метод сохранения в бинарный файл
        private void SaveToFile()
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(_filePath, FileMode.Create)))
                {
                    writer.Write(_movies.Count);
                    foreach (Movie movie in _movies)
                    {
                        writer.Write(movie.Id);
                        writer.Write(movie.Title);
                        writer.Write(movie.Genre);
                        writer.Write(movie.Year);
                        writer.Write(movie.Rating);
                        writer.Write(movie.DurationMinutes);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода-вывода при сохранении файла: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Нет доступа к файлу: {ex.Message}");
            }
        }

        // 1. Просмотр базы данных
        public void ShowAll()
        {
            if (_movies.Count == 0)
            {
                Console.WriteLine("База данных пуста.");
                return;
            }

            foreach (Movie movie in _movies)
            {
                Console.WriteLine(movie.ToString());
            }
        }

        // 2. Добавление элемента
        public void AddMovie(Movie movie)
        {
            if (_movies.Any(m => m.Id == movie.Id))
            {
                Console.WriteLine("Фильм с таким ID уже существует!");
                return;
            }

            _movies.Add(movie);
            SaveToFile();
            Console.WriteLine("Фильм успешно добавлен.");
        }

        // 3. Удаление элемента по ключу (ID)
        public void DeleteMovie(int id)
        {
            Movie movieToRemove = _movies.FirstOrDefault(m => m.Id == id);
            if (movieToRemove != null)
            {
                _movies.Remove(movieToRemove);
                SaveToFile();
                Console.WriteLine("Фильм успешно удален.");
            }
            else
            {
                Console.WriteLine("Фильм с таким ID не найден.");
            }
        }

        // ================= 4 LINQ ЗАПРОСА =================

        // Запрос 1 (возвращает перечень): Фильмы заданного жанра с рейтингом выше заданного
        public void QueryByGenreAndRating(string genre, double minRating)
        {
            List<Movie> result = _movies
                .Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase) && m.Rating > minRating)
                .ToList();

            Console.WriteLine($"\n--- Фильмы жанра '{genre}' с рейтингом выше {minRating} ---");
            if (result.Count == 0)
            {
                Console.WriteLine("Ничего не найдено.");
                return;
            }

            foreach (Movie movie in result)
            {
                Console.WriteLine(movie.ToString());
            }
        }

        // Запрос 2 (возвращает перечень): Фильмы выпущенные после заданного года, отсортированные по рейтингу
        public void QueryByYearSorted(int year)
        {
            List<Movie> result = _movies
                .Where(m => m.Year > year)
                .OrderByDescending(m => m.Rating)
                .ToList();

            Console.WriteLine($"\n--- Фильмы выпущенные после {year} года (сортировка по рейтингу) ---");
            if (result.Count == 0)
            {
                Console.WriteLine("Ничего не найдено.");
                return;
            }

            foreach (Movie movie in result)
            {
                Console.WriteLine(movie.ToString());
            }
        }

        // Запрос 3 (возвращает одно значение): Средний рейтинг всех фильмов
        public void QueryAverageRating()
        {
            if (_movies.Count == 0)
            {
                Console.WriteLine("\nСредний рейтинг: нет данных.");
                return;
            }

            double average = _movies.Average(m => m.Rating);
            Console.WriteLine($"\n--- Средний рейтинг всех фильмов: {average:F2} ---");
        }

        // Запрос 4 (возвращает одно значение): Название фильма с самым высоким рейтингом
        public void QueryTopRatedMovie()
        {
            if (_movies.Count == 0)
            {
                Console.WriteLine("\nФильм с самым высоким рейтингом: нет данных.");
                return;
            }

            Movie topMovie = _movies.OrderByDescending(m => m.Rating).FirstOrDefault();
            Console.WriteLine($"\n--- Фильм с самым высоким рейтингом: {topMovie.Title} (Рейтинг: {topMovie.Rating:F1}) ---");
        }
    }
}