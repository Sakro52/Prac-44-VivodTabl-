using System;
using System.Linq;

// Определение структуры Student
struct Student
{
    public string FullName;
    public string Group;
    public int Informatics;
    public int Physics;
    public int History;
}

class Program
{
    static void Main(string[] args)
    {
        // Ввод количества студентов
        Console.WriteLine("Введите количество студентов:");
        int N;
        while (!int.TryParse(Console.ReadLine(), out N) || N < 0)
        {
            Console.WriteLine("Введите корректное положительное число:");
        }

        // Создание массива студентов
        Student[] students = new Student[N];

        // Ввод данных
        for (int i = 0; i < N; i++)
        {
            Console.WriteLine($"\nВведите данные для студента {i + 1}:");

            Console.Write("ФИО: ");
            students[i].FullName = Console.ReadLine();

            Console.Write("Группа: ");
            students[i].Group = Console.ReadLine();

            Console.Write("Оценка по информатике (0-5): ");
            while (!int.TryParse(Console.ReadLine(), out students[i].Informatics) ||
                   students[i].Informatics < 0 || students[i].Informatics > 5)
            {
                Console.Write("Введите корректную оценку (0-5): ");
            }

            Console.Write("Оценка по физике (0-5): ");
            while (!int.TryParse(Console.ReadLine(), out students[i].Physics) ||
                   students[i].Physics < 0 || students[i].Physics > 5)
            {
                Console.Write("Введите корректную оценку (0-5): ");
            }

            Console.Write("Оценка по истории (0-5): ");
            while (!int.TryParse(Console.ReadLine(), out students[i].History) ||
                   students[i].History < 0 || students[i].History > 5)
            {
                Console.Write("Введите корректную оценку (0-5): ");
            }
        }

        // Вывод всех студентов
        Console.WriteLine("\nСведения о студентах:");
        if (N == 0)
        {
            Console.WriteLine("Информация о студентах отсутствует!");
            return;
        }

        // Вывод шапки таблицы
        Console.WriteLine("┌──────────────────────┬────────────┬──────────────┬──────────┬──────────┐");
        Console.WriteLine("| {0,-20} | {1,-10} | {2,-12} | {3,-8} | {4,-8} |",
            "ФИО", "Группа", "Информатика", "Физика", "История");
        Console.WriteLine("├──────────────────────┼────────────┼──────────────┼──────────┼──────────┤");

        // Вывод данных студентов с ограничением длины строк
        foreach (var student in students)
        {
            string shortName = student.FullName.Length > 20 ? student.FullName.Substring(0, 17) + "..." : student.FullName;
            string shortGroup = student.Group.Length > 10 ? student.Group.Substring(0, 7) + "..." : student.Group;
            Console.WriteLine("| {0,-20} | {1,-10} | {2,-12} | {3,-8} | {4,-8} |",
                shortName, shortGroup, student.Informatics, student.Physics, student.History);
        }
        Console.WriteLine("└──────────────────────┴────────────┴──────────────┴──────────┴──────────┘");

        // Подсчёт среднего балла по предметам с использованием LINQ
        if (N > 0)
        {
            double avgInformatics = students.Average(s => s.Informatics);
            double avgPhysics = students.Average(s => s.Physics);
            double avgHistory = students.Average(s => s.History);

            Console.WriteLine("\nСредний балл по предметам:");
            Console.WriteLine($"Информатика: {avgInformatics:F2}");
            Console.WriteLine($"Физика: {avgPhysics:F2}");
            Console.WriteLine($"История: {avgHistory:F2}");
        }

        // Поиск студентов со средним баллом > 4 с использованием LINQ
        var highAvgStudents = students
            .Select(s => new
            {
                s.FullName,
                s.Group,
                Average = (s.Informatics + s.Physics + s.History) / 3.0
            })
            .Where(s => s.Average > 4)
            .ToList();

        Console.WriteLine("\nСтуденты со средним баллом больше 4:");
        Console.WriteLine("┌──────────────────────┬────────────┬─────────────────┐");
        Console.WriteLine("| {0,-20} | {1,-10} | {2,-15} |",
            "ФИО", "Группа", "Средний балл");
        Console.WriteLine("├──────────────────────┼────────────┼─────────────────┤");

        if (highAvgStudents.Count == 0)
        {
            Console.WriteLine("| {0,-49} |", "Нет студентов со средним баллом больше 4!");
            Console.WriteLine("└──────────────────────┴────────────┴─────────────────┘");
        }
        else
        {
            foreach (var s in highAvgStudents)
            {
                string shortName = s.FullName.Length > 20 ? s.FullName.Substring(0, 17) + "..." : s.FullName;
                string shortGroup = s.Group.Length > 10 ? s.Group.Substring(0, 7) + "..." : s.Group;
                Console.WriteLine("| {0,-20} | {1,-10} | {2,-15:F2} |", shortName, shortGroup, s.Average);
            }
            Console.WriteLine("└──────────────────────┴────────────┴─────────────────┘");
            Console.WriteLine($"\nКоличество студентов со средним баллом больше 4: {highAvgStudents.Count}");
        }
    }
}