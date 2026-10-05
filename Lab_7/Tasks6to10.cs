using System;
using System.Collections.Generic;
using System.IO;

namespace Lab7.Classes
{
    internal static class Tasks6to10
    {
        // ================= ЗАДАНИЕ 6 =================
        public static void Task6()
        {
            List<int> L = new List<int>() { 1, 2, 3, 4, 2, 5, 2, 6, 7, 2 };
            Console.WriteLine($"Исходный список: {string.Join(", ", L)}");

            int E = Program.GetValidInt("Введите элемент E для удаления: ");

            // Удаление (итерация с конца)
            for (int i = L.Count - 1; i >= 0; i--)
            {
                if (L[i] == E)
                {
                    L.RemoveAt(i);
                }
            }

            Console.WriteLine($"Список после удаления: {string.Join(", ", L)}");
        }

        // ================= ЗАДАНИЕ 7 =================
        public static void Task7()
        {
            LinkedList<int> L = new LinkedList<int>();
            L.AddLast(10);
            L.AddLast(20);
            L.AddLast(30);
            L.AddLast(40);

            Console.WriteLine("Элементы в обратном порядке:");
            LinkedListNode<int> current = L.Last;
            while (current != null)
            {
                Console.Write($"{current.Value} ");
                current = current.Previous;
            }
            Console.WriteLine();
        }

        // ================= ЗАДАНИЕ 8 =================
        public static void Task8()
        {
            // n учебных заведений
            HashSet<string> institutions = new HashSet<string>() { "ВУЗ-1", "ВУЗ-2", "ВУЗ-3" };
            // Перечень фирм
            HashSet<string> allFirms = new HashSet<string>() { "Фирма А", "Фирма Б", "Фирма В", "Фирма Г" };

            // Словарь: Заведение -> Множество фирм, где закупалось
            Dictionary<string, HashSet<string>> purchases = new Dictionary<string, HashSet<string>>();
            purchases.Add("ВУЗ-1", new HashSet<string>() { "Фирма А", "Фирма Б" });
            purchases.Add("ВУЗ-2", new HashSet<string>() { "Фирма Б" });
            purchases.Add("ВУЗ-3", new HashSet<string>() { "Фирма Г" });

            Console.WriteLine("1) В каких фирмах закупка производилась каждым из заведений:");
            foreach (var pair in purchases)
            {
                Console.WriteLine($"   {pair.Key}: {string.Join(", ", pair.Value)}");
            }

            // Фирмы, где закупалось хотя бы одно заведение
            HashSet<string> firmsWithPurchases = new HashSet<string>();
            foreach (var pair in purchases)
            {
                foreach (string firm in pair.Value)
                {
                    firmsWithPurchases.Add(firm);
                }
            }
            Console.WriteLine("\n2) Фирмы, где закупалось хотя бы одно заведение:");
            Console.WriteLine($"   {string.Join(", ", firmsWithPurchases)}");

            // Фирмы, где никто не закупал
            HashSet<string> firmsWithoutPurchases = new HashSet<string>();
            foreach (string firm in allFirms)
            {
                if (!firmsWithPurchases.Contains(firm))
                {
                    firmsWithoutPurchases.Add(firm);
                }
            }
            Console.WriteLine("\n3) Фирмы, где ни одно заведение не закупало компьютеры:");
            Console.WriteLine($"   {(firmsWithoutPurchases.Count > 0 ? string.Join(", ", firmsWithoutPurchases) : "Нет таких фирм")}");
        }

        // ================= ЗАДАНИЕ 9 =================
        public static void Task9()
        {
            string path = "task9.txt";
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "Это тестовый текст на русском языке для проверки звонких согласных.");
                Console.WriteLine("Файл task9.txt создан.");
            }

            string text = File.ReadAllText(path).ToLower();
            string[] words = text.Split(new char[] { ' ', ',', '.', '!', '?', '\n', '\r', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);

            string voicedConsonants = "бвгджзлмнр";
            HashSet<char> foundConsonants = new HashSet<char>();

            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words[i].Length; j++)
                {
                    char c = words[i][j];
                    if (voicedConsonants.IndexOf(c) >= 0)
                    {
                        foundConsonants.Add(c);
                    }
                }
            }

            List<char> sortedList = new List<char>(foundConsonants);
            sortedList.Sort();

            Console.WriteLine("Звонкие согласные в алфавитном порядке:");
            for (int i = 0; i < sortedList.Count; i++)
            {
                Console.Write($"{sortedList[i]} ");
            }
            Console.WriteLine();
        }

        // ================= ЗАДАНИЕ 10 =================
        public static void Task10()
        {
            string path = "task10.txt";
            if (!File.Exists(path))
            {
                string[] lines = {
                    "Иванова Мария",
                    "Петров Сергей",
                    "Бойцова Екатерина",
                    "Петров Иван",
                    "Иванова Наташа"
                };
                File.WriteAllLines(path, lines);
                Console.WriteLine("Файл task10.txt создан.");
            }

            string[] inputLines = File.ReadAllLines(path);
            Dictionary<string, int> nameCounts = new Dictionary<string, int>();

            Console.WriteLine("Сформированные логины:");
            for (int i = 0; i < inputLines.Length; i++)
            {
                string[] parts = inputLines[i].Split(' ');
                if (parts.Length < 2) continue;

                string surname = parts[0];

                if (nameCounts.ContainsKey(surname))
                {
                    nameCounts[surname]++;
                    Console.WriteLine($"{surname}{nameCounts[surname]}");
                }
                else
                {
                    nameCounts.Add(surname, 1);
                    Console.WriteLine(surname);
                }
            }
        }
    }
}