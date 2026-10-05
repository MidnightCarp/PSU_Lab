using System;

namespace Lab6_1.Classes
{
    public class Movie
    {
        // Приватное поле
        private string _title;

        // Обычный конструктор
        public Movie(string title)
        {
            _title = title;
        }

        // Конструктор копирования
        public Movie(Movie other)
        {
            _title = other._title;
        }

        // Метод для получения названия
        public string GetTitle()
        {
            return _title;
        }

        // Метод, создающий строку из первого и последнего символов поля
        public string GetFirstLastChars()
        {
            if (string.IsNullOrEmpty(_title))
            {
                return string.Empty;
            }

            if (_title.Length == 1)
            {
                return _title;
            }

            return $"{_title[0]}{_title[_title.Length - 1]}";
        }

        // Перегрузка ToString()
        public override string ToString()
        {
            return $"Название фильма: {_title}";
        }
    }
}