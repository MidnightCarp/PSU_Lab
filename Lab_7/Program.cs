using System;
using Lab7.Classes;

namespace Lab7
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ ===");
                Console.WriteLine("1. Задание 1 (Текстовые файлы, 1 число на строку)");
                Console.WriteLine("2. Задание 2 (Текстовые файлы, несколько чисел на строку)");
                Console.WriteLine("3. Задание 3 (Текстовые файлы, короткая и длинная строка)");
                Console.WriteLine("4. Задание 4 (Бинарные файлы, четные числа)");
                Console.WriteLine("5. Задание 5 (Бинарные файлы, структуры, XML)");
                Console.WriteLine("6. Задание 6 (List, удаление элементов)");
                Console.WriteLine("7. Задание 7 (LinkedList, обратный порядок)");
                Console.WriteLine("8. Задание 8 (HashSet, закупки)");
                Console.WriteLine("9. Задание 9 (HashSet, звонкие согласные)");
                Console.WriteLine("10. Задание 10 (Dictionary, логины)");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите задание: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Tasks1to5.Task1(); break;
                    case "2": Tasks1to5.Task2(); break;
                    case "3": Tasks1to5.Task3(); break;
                    case "4": Tasks1to5.Task4(); break;
                    case "5": Tasks1to5.Task5(); break;
                    case "6": Tasks6to10.Task6(); break;
                    case "7": Tasks6to10.Task7(); break;
                    case "8": Tasks6to10.Task8(); break;
                    case "9": Tasks6to10.Task9(); break;
                    case "10": Tasks6to10.Task10(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный выбор."); break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        // --- Приватные методы проверки вводимых значений ---

        public static int GetValidInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                {
                    return result;
                }
                Console.WriteLine("Ошибка: введите целое число.");
            }
        }

        public static double GetValidDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (double.TryParse(input, out double result))
                {
                    return result;
                }
                Console.WriteLine("Ошибка: введите число.");
            }
        }

        public static string GetValidString(string prompt)
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