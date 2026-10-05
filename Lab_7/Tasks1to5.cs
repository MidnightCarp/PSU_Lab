using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Lab7.Classes
{
    internal static class Tasks1to5
    {
        // ================= ЗАДАНИЕ 1 =================
        public static void Task1()
        {
            string path = "task1.txt";
            GenerateTask1File(path);

            string[] lines = File.ReadAllLines(path);
            int[] numbers = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                numbers[i] = int.Parse(lines[i]);
            }

            int half = numbers.Length / 2;
            int sum1 = 0;
            int sum2 = 0;

            for (int i = 0; i < half; i++) sum1 += numbers[i];
            for (int i = half; i < numbers.Length; i++) sum2 += numbers[i];

            Console.WriteLine($"Разность сумм первой и второй половины: {sum1 - sum2}");
        }

        private static void GenerateTask1File(string path)
        {
            if (File.Exists(path)) return;

            Random rnd = new Random();
            int count = rnd.Next(5, 10) * 2; // Четное количество
            string[] lines = new string[count];

            for (int i = 0; i < count; i++)
            {
                lines[i] = rnd.Next(1, 100).ToString();
            }
            File.WriteAllLines(path, lines);
            Console.WriteLine("Файл task1.txt сгенерирован.");
        }

        // ================= ЗАДАНИЕ 2 =================
        public static void Task2()
        {
            string path = "task2.txt";
            GenerateTask2File(path);

            string content = File.ReadAllText(path);
            string[] parts = content.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            int sum = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                sum += int.Parse(parts[i]);
            }

            Console.WriteLine($"Сумма всех элементов: {sum}");
        }

        private static void GenerateTask2File(string path)
        {
            if (File.Exists(path)) return;

            Random rnd = new Random();
            List<string> lines = new List<string>();

            for (int i = 0; i < 4; i++)
            {
                string line = "";
                int countInLine = rnd.Next(2, 5);
                for (int j = 0; j < countInLine; j++)
                {
                    line += rnd.Next(1, 50).ToString() + " ";
                }
                lines.Add(line.Trim());
            }
            File.WriteAllLines(path, lines);
            Console.WriteLine("Файл task2.txt сгенерирован.");
        }

        // ================= ЗАДАНИЕ 3 =================
        public static void Task3()
        {
            string path = "task3.txt";
            string outPath = "task3_out.txt";
            GenerateTask3File(path);

            string[] lines = File.ReadAllLines(path);
            if (lines.Length == 0) return;

            string shortest = lines[0];
            string longest = lines[0];

            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].Length < shortest.Length) shortest = lines[i];
                if (lines[i].Length > longest.Length) longest = lines[i];
            }

            File.WriteAllLines(outPath, new string[] { shortest, longest });
            Console.WriteLine("Самая короткая и длинная строки записаны в task3_out.txt");
        }

        private static void GenerateTask3File(string path)
        {
            if (File.Exists(path)) return;
            string[] lines = { "Привет", "Это тестовый файл", "C#", "Программирование", "Ок" };
            File.WriteAllLines(path, lines);
            Console.WriteLine("Файл task3.txt сгенерирован.");
        }

        // ================= ЗАДАНИЕ 4 =================
        public static void Task4()
        {
            string path = "task4.bin";
            string outPath = "task4_out.bin";
            GenerateTask4File(path);

            List<int> evenNumbers = new List<int>();

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    int num = reader.ReadInt32();
                    if (num % 2 == 0) evenNumbers.Add(num);
                }
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(outPath, FileMode.Create)))
            {
                for (int i = 0; i < evenNumbers.Count; i++)
                {
                    writer.Write(evenNumbers[i]);
                }
            }
            Console.WriteLine("Четные числа записаны в task4_out.bin");
        }

        private static void GenerateTask4File(string path)
        {
            if (File.Exists(path)) return;
            Random rnd = new Random();
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                for (int i = 0; i < 10; i++)
                {
                    writer.Write(rnd.Next(1, 100));
                }
            }
            Console.WriteLine("Файл task4.bin сгенерирован.");
        }

        // ================= ЗАДАНИЕ 5 =================
        public static void Task5()
        {
            string path = "task5.xml";
            GenerateTask5File(path);

            List<Luggage> items = new List<Luggage>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Luggage>));

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                items = (List<Luggage>)serializer.Deserialize(fs);
            }

            double totalWeight = 0;
            for (int i = 0; i < items.Count; i++) totalWeight += items[i].Weight;
            double avgWeight = totalWeight / items.Count;

            double m = Program.GetValidDouble("Введите допустимое отклонение m: ");

            Console.WriteLine($"Средняя масса: {avgWeight:F2}. Искомый багаж:");
            for (int i = 0; i < items.Count; i++)
            {
                if (Math.Abs(items[i].Weight - avgWeight) <= m)
                {
                    Console.WriteLine($" - {items[i].Name} (масса: {items[i].Weight})");
                }
            }
        }

        private static void GenerateTask5File(string path)
        {
            if (File.Exists(path)) return;
            List<Luggage> items = new List<Luggage>();
            items.Add(new Luggage { Name = "Чемодан", Weight = 20.5 });
            items.Add(new Luggage { Name = "Сумка", Weight = 5.0 });
            items.Add(new Luggage { Name = "Коробка", Weight = 12.3 });
            items.Add(new Luggage { Name = "Рюкзак", Weight = 7.8 });
            items.Add(new Luggage { Name = "Пакет", Weight = 2.1 });

            XmlSerializer serializer = new XmlSerializer(typeof(List<Luggage>));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fs, items);
            }
            Console.WriteLine("Файл task5.xml сгенерирован.");
        }
    }
}