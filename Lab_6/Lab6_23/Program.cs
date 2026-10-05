using Lab6_23.Classes;
using System;

namespace Lab6_23
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Задание 2. Тестирование класса Time ===");

            byte hours = GetValidByte("Введите часы (0-23): ", 0, 23);
            byte minutes = GetValidByte("Введите минуты (0-59): ", 0, 59);

            Time time = new Time(hours, minutes);
            Console.WriteLine($"Создан объект Time: {time}");

            Console.WriteLine("\n--- Тестирование метода AddMinutes ---");
            uint addMins = GetValidUInt("Введите количество минут для добавления: ");
            Time addedTime = time.AddMinutes(addMins);
            Console.WriteLine($"Результат добавления {addMins} минут: {addedTime}");

            Console.WriteLine("\n=== Задание 3. Тестирование перегрузки операций ===");

            // Унарные операции
            Time incTime = ++time;
            Console.WriteLine($"После ++ (инкремент): {incTime}");

            Time decTime = --time;
            Console.WriteLine($"После -- (декремент): {decTime}");

            // Приведение к byte
            byte extractedHours = (byte)time;
            Console.WriteLine($"Явное приведение к byte (часы): {extractedHours}");

            // Приведение к bool
            bool isNotZero = time;
            Console.WriteLine($"Неявное приведение к bool (не ноль?): {isNotZero}");

            // Бинарные операции +
            uint plusMins = GetValidUInt("Введите минуты для операции + : ");
            Time plusResult1 = time + plusMins;
            Time plusResult2 = plusMins + time;
            Console.WriteLine($"Time + {plusMins} = {plusResult1}");
            Console.WriteLine($"{plusMins} + Time = {plusResult2}");

            // Бинарные операции -
            uint minusMins = GetValidUInt("Введите минуты для операции - : ");
            Time minusResult1 = time - minusMins;
            Time minusResult2 = minusMins - time;
            Console.WriteLine($"Time - {minusMins} = {minusResult1}");
            Console.WriteLine($"{minusMins} - Time = {minusResult2}");

            Console.ReadLine();
        }

        // --- Приватные методы проверки вводимых значений ---

        private static byte GetValidByte(string prompt, byte min, byte max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                // Inline-объявление переменной out (современный синтаксис C#)
                if (byte.TryParse(input, out byte result))
                {
                    if (result >= min && result <= max)
                    {
                        return result;
                    }
                }
                Console.WriteLine($"Ошибка: введите число от {min} до {max}.");
            }
        }

        private static uint GetValidUInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (uint.TryParse(input, out uint result))
                {
                    return result;
                }
                Console.WriteLine("Ошибка: введите неотрицательное целое число.");
            }
        }
    }
}