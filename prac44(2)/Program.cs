using prac44_2_;
using System;
using System.Linq;
class Program
{
    static void Main(string[] args)
    {
        // Текущий год
        int currentYear = DateTime.Now.Year; // 2025

        // Ввод количества товаров
        Console.WriteLine("Введите количество товаров:");
        int N;
        while (!int.TryParse(Console.ReadLine(), out N) || N < 0)
        {
            Console.WriteLine("Введите корректное положительное число:");
        }

        // Создание массива товаров
        Product[] products = new Product[N];

        // Ввод данных
        for (int i = 0; i < N; i++)
        {
            Console.WriteLine($"\nВведите данные для товара {i + 1}:");

            Console.Write("Наименование: ");
            products[i].Name = Console.ReadLine();

            Console.Write("Изготовитель: ");
            products[i].Manufacturer = Console.ReadLine();

            Console.Write("Количество: ");
            while (!int.TryParse(Console.ReadLine(), out products[i].Quantity) || products[i].Quantity < 0)
            {
                Console.Write("Введите корректное неотрицательное число: ");
            }

            Console.Write("Цена (руб.): ");
            while (!double.TryParse(Console.ReadLine(), out products[i].Price) || products[i].Price < 0)
            {
                Console.Write("Введите корректную неотрицательную цену: ");
            }

            Console.Write("Дата выпуска (дд.мм.гггг): ");
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out products[i].ReleaseDate) ||
                   products[i].ReleaseDate.Year < 1900 || products[i].ReleaseDate.Year > currentYear)
            {
                Console.Write($"Введите корректную дату (01.01.1900–31.12.{currentYear}): ");
            }
        }

        // Вывод всех товаров
        Console.WriteLine("\nСведения о товарах:");
        if (N == 0)
        {
            Console.WriteLine("Информация о товарах отсутствует!");
            return;
        }
        // Вывод шапки таблицы
        Console.WriteLine("┌──────────────────────┬─────────────────┬────────────┬──────────┬──────────────┐");
        Console.WriteLine("| {0,-20} | {1,-15} | {2,-10} | {3,-8} | {4,-12} |",
            "Наименование", "Изготовитель", "Количество", "Цена", "Год выпуска");
        Console.WriteLine("├──────────────────────┼─────────────────┼────────────┼──────────┼──────────────┤");

        // Вывод данных товаров с ограничением длины строк
        foreach (var product in products)
        {
            string shortName = product.Name.Length > 20 ? product.Name.Substring(0, 17) + "..." : product.Name;
            string shortManufacturer = product.Manufacturer.Length > 15 ? product.Manufacturer.Substring(0, 12) + "..." : product.Manufacturer;
            Console.WriteLine("| {0,-20} | {1,-15} | {2,-10} | {3,-8:F2} | {4,-12:dd.MM.yyyy} |",
                shortName, shortManufacturer, product.Quantity, product.Price, product.ReleaseDate);
        }
        Console.WriteLine("└──────────────────────┴─────────────────┴────────────┴──────────┴──────────────┘");

        // Подсчёт общей стоимости товаров текущего года с использованием LINQ
        var currentYearProducts = products
            .Where(p => p.ReleaseDate.Year == currentYear)
            .Select(p => new
            {
                p.Name,
                p.Manufacturer,
                p.Quantity,
                p.Price,
                TotalCost = p.Quantity * p.Price
            })
            .ToList();

        double totalCurrentYearCost = currentYearProducts.Sum(p => p.TotalCost);

        Console.WriteLine($"\nТовары, выпущенные в {currentYear} году:");
        Console.WriteLine("┌──────────────────────┬─────────────────┬────────────┬──────────┬────────────────┐");
        Console.WriteLine("| {0,-20} | {1,-15} | {2,-10} | {3,-8} | {4,-12}|",
            "Наименование", "Изготовитель", "Количество", "Цена", "Общая стоимость");
        Console.WriteLine("├──────────────────────┼─────────────────┼────────────┼──────────┼────────────────┤");

        if (currentYearProducts.Count == 0)
        {
            Console.WriteLine("| {0,-68} |", $"Нет товаров, выпущенных в {currentYear} году!");
            Console.WriteLine("└──────────────────────┴─────────────────┴────────────┴──────────┴────────────┘");
        }
        else
        {
            foreach (var p in currentYearProducts)
            {
                string shortName = p.Name.Length > 20 ? p.Name.Substring(0, 17) + "..." : p.Name;
                string shortManufacturer = p.Manufacturer.Length > 15 ? p.Manufacturer.Substring(0, 12) + "..." : p.Manufacturer;
                Console.WriteLine("| {0,-20} | {1,-15} | {2,-10} | {3,-8:F2} | {4,-12:F2} |",
                    shortName, shortManufacturer, p.Quantity, p.Price, p.TotalCost);
            }
            Console.WriteLine("└──────────────────────┴─────────────────┴────────────┴──────────┴──────────────┘");
            Console.WriteLine($"Общая стоимость товаров {currentYear} года: {totalCurrentYearCost:F2} руб.");
        }

        // Поиск товаров с максимальной и минимальной общей стоимостью
        if (N > 0)
        {
            var productsWithTotalCost = products
                .Select(p => new
                {
                    p.Name,
                    TotalCost = p.Quantity * p.Price
                })
                .ToList();

            var maxCost = productsWithTotalCost.Max(p => p.TotalCost);
            var minCost = productsWithTotalCost.Min(p => p.TotalCost);

            var maxCostProducts = productsWithTotalCost
                .Where(p => p.TotalCost == maxCost)
                .Select(p => p.Name.Length > 20 ? p.Name.Substring(0, 17) + "..." : p.Name)
                .ToList();

            var minCostProducts = productsWithTotalCost
                .Where(p => p.TotalCost == minCost)
                .Select(p => p.Name.Length > 20 ? p.Name.Substring(0, 17) + "..." : p.Name)
                .ToList();

            Console.WriteLine("\nТовары с максимальной общей стоимостью:");
            Console.WriteLine("┌──────────────────────┬─────────────────┐");
            Console.WriteLine("| {0,-20} | {1,-15} |",
                "Наименование", "Общая стоимость");
            Console.WriteLine("├──────────────────────┼─────────────────┤");
            foreach (var name in maxCostProducts)
            {
                Console.WriteLine("| {0,-20} | {1,-15:F2} |", name, maxCost);
            }
            Console.WriteLine("└──────────────────────┴─────────────────┘");

            Console.WriteLine("\nТовары с минимальной общей стоимостью:");
            Console.WriteLine("┌──────────────────────┬─────────────────┐");
            Console.WriteLine("| {0,-20} | {1,-15} |",
                "Наименование", "Общая стоимость");
            Console.WriteLine("├──────────────────────┼─────────────────┤");
            foreach (var name in minCostProducts)
            {
                Console.WriteLine("| {0,-20} | {1,-15:F2} |", name, minCost);
            }
            Console.WriteLine("└──────────────────────┴─────────────────┘");
        }
    }
}