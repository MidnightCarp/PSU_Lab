using System;

namespace Lab9.Classes
{
    public class Time
    {
        // Приватные поля
        private byte _hours;
        private byte _minutes;

        // Конструктор
        public Time(byte hours, byte minutes)
        {
            long totalMinutes = (long)hours * 60 + minutes;
            NormalizeTime(totalMinutes);
        }

        // Приватный метод нормализации времени (приведение к формату 00:00 - 23:59)
        private void NormalizeTime(long totalMinutes)
        {
            // Корректная обработка отрицательных значений (для операций вычитания)
            totalMinutes = ((totalMinutes % 1440) + 1440) % 1440;

            _hours = (byte)(totalMinutes / 60);
            _minutes = (byte)(totalMinutes % 60);
        }

        // Метод задания 2: Добавление произвольного количества минут
        public Time AddMinutes(uint minutesToAdd)
        {
            long currentTotal = (long)_hours * 60 + _minutes;
            long newTotal = currentTotal + (long)minutesToAdd;

            int resultTotal = (int)(newTotal % 1440);

            Time result = new Time((byte)(resultTotal / 60), (byte)(resultTotal % 60));
            return result;
        }

        // Приватный вспомогательный метод для вычитания минут
        private Time SubtractMinutes(uint minutesToSubtract)
        {
            long currentTotal = (long)_hours * 60 + _minutes;
            long newTotal = currentTotal - (long)minutesToSubtract;

            Time result = new Time(0, 0);
            result.NormalizeTime(newTotal);
            return result;
        }

        // Перегрузка ToString()
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
        public static Time operator -(uint minutes, Time t)
        {
            Time temp = new Time(0, 0);
            Time added = temp.AddMinutes(minutes);
            int tTotal = (t._hours * 60) + t._minutes;
            return added.SubtractMinutes((uint)tTotal);
        }
    }
}