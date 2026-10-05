using System;

namespace Lab8.Models
{
    public class Movie
    {
        // Свойства
        public int Id { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public int Year { get; set; }

        public double Rating { get; set; }

        public int DurationMinutes { get; set; }

        // Пустой конструктор (нужен для десериализации)
        public Movie()
        {
        }

        // Конструктор с параметрами
        public Movie(int id, string title, string genre, int year, double rating, int duration)
        {
            Id = id;
            Title = title;
            Genre = genre;
            Year = year;
            Rating = rating;
            DurationMinutes = duration;
        }

        // Перегруженный ToString()
        public override string ToString()
        {
            return $"ID: {Id} | Название: {Title} | Жанр: {Genre} | Год: {Year} | Рейтинг: {Rating:F1} | Длительность: {DurationMinutes} мин.";
        }
    }
}