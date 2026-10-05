using System;

namespace Lab6_23.Classes
{
    public class Time
    {
        // Приватные поля
        private byte _hours;
        private byte _minutes;

        // Конструктор
        public Time(byte hours, byte minutes)
        {
            // Внутренняя нормализация на случай выхода за пределы (для операторов)
            int totalMinutes = (hours * 60) + minutes;
            NormalizeTime(totalMinutes);
        }

        // Приватный метод нормализации времени (приведение к формату 00:00 - 23:59)
        private void NormalizeTime(int totalMinutes)
        {
            // Обработка отрицательных значений (вычитание)
            while (totalMinutes < 0)
            {
                totalMinutes += 1440; // Добавляем 24 часа (1440 минут)
            }

            totalMinutes %= 1440; // Отбрасываем лишние сутки

            _hours = (byte)(totalMinutes / 60);
            _minutes = (byte)(totalMinutes % 60);
        }

        // Метод задания 2: Добавление произвольного количества минут
        public Time AddMinutes(uint minutesToAdd)
        {
            int currentTotal = (_hours * 60) + _minutes;
            // Используем long, чтобы избежать переполнения int при больших uint
            long newTotal = currentTotal + (long)minutesToAdd;

            int resultTotal = (int)(newTotal % 1440);

            Time result = new Time((byte)(resultTotal / 60), (byte)(resultTotal % 60));
            return result;
        }

        // Приватный вспомогательный метод для вычитания минут (нужен для оператора -- и -)
        private Time SubtractMinutes(uint minutesToSubtract)
        {
            int currentTotal = (_hours * 60) + _minutes;
            long newTotal = currentTotal - (long)minutesToSubtract;

            Time result = new Time(0, 0);
            result.NormalizeTime((int)(newTotal % 1440));
            return result;
        }

        // Перегрузка ToString() (Задание 2)
        public override string ToString()
        {
            return $"{_hours:D2}:{_minutes:D2}";
        }

        // ==========================================
        // ЗАДАНИЕ 3: Перегрузка операций
        // ==========================================

        // Унарные операции
        public static Time operator ++(Time t)
        {
            return t.AddMinutes(1);
        }

        public static Time operator --(Time t)
        {
            return t.SubtractMinutes(1);
        }

        // Операции приведения типа
        public static explicit operator byte(Time t)
        {
            return t._hours; // минуты отбрасываются
        }

        public static implicit operator bool(Time t)
        {
            return t._hours != 0 || t._minutes != 0;
        }

        // Бинарные операции (Time + uint)
        public static Time operator +(Time t, uint minutes)
        {
            return t.AddMinutes(minutes);
        }

        public static Time operator +(uint minutes, Time t)
        {
            return t.AddMinutes(minutes);
        }

        // Бинарные операции (Time - uint)
        public static Time operator -(Time t, uint minutes)
        {
            return t.SubtractMinutes(minutes);
        }

        // Бинарная операция (uint - Time)
        // Логика: uint трактуется как минуты от начала суток (00:00)
        public static Time operator -(uint minutes, Time t)
        {
            Time temp = new Time(0, 0);
            Time added = temp.AddMinutes(minutes);
            int tTotal = (t._hours * 60) + t._minutes;
            return added.SubtractMinutes((uint)tTotal);
        }
    }
}