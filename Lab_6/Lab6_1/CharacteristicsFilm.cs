using System;

namespace Lab6_1.Classes
{
    public class CharacteristicsFilm : Movie
    {
        // Приватные поля дочернего класса
        private bool _isBasedOnBook;
        private string _genre;
        private int _rating;

        // Конструктор дочернего класса
        public CharacteristicsFilm(string title, bool isBasedOnBook, string genre, int rating)
            : base(title)
        {
            _isBasedOnBook = isBasedOnBook;
            _genre = genre;
            _rating = rating;
        }

        // Конструктор копирования дочернего класса
        public CharacteristicsFilm(CharacteristicsFilm other)
            : base(other)
        {
            _isBasedOnBook = other._isBasedOnBook;
            _genre = other._genre;
            _rating = other._rating;
        }

        // Метод 1: Возвращает статус экранизации
        public string GetBookStatus()
        {
            if (_isBasedOnBook)
            {
                return "Да, фильм снят по книге.";
            }
            return "Нет, это оригинальный сценарий.";
        }

        // Метод 2: Возвращает текстовую интерпретацию оценки
        public string GetRatingCategory()
        {
            if (_rating >= 8 && _rating <= 10)
            {
                return "Высокая оценка";
            }
            if (_rating >= 5 && _rating <= 7)
            {
                return "Средняя оценка";
            }
            return "Низкая оценка";
        }

        // Метод 3: Геттер для жанра
        public string GetGenre()
        {
            return _genre;
        }

        // Перегрузка ToString() для дочернего класса
        public override string ToString()
        {
            string baseInfo = base.ToString();
            return $"{baseInfo}\nЖанр: {_genre}\nЭкранизация: {GetBookStatus()}\nОценка: {_rating}/10 ({GetRatingCategory()})";
        }
    }
}