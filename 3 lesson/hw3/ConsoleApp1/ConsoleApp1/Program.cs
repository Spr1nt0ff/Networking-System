using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using DatabaseContext;
using CountryLibrary;
using System.Diagnostics.Metrics;

class Program
{
    static void Main()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseSqlServer(@"Server=SPR1NT\SQLEXPRESS;Database=CountriesDB;Integrated Security=SSPI;TrustServerCertificate=true")
            .Options;

        using var db = new ApplicationContext(options);

        db.Database.EnsureCreated();

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Добавить новую страну");
            Console.WriteLine("2 - Обновить данные страны");
            Console.WriteLine("3 - Удалить данные о стране");
            Console.WriteLine("4 - Показать все страны");
            Console.WriteLine("0 - Выход");
            Console.Write("Введите номер действия: ");

            switch (Console.ReadLine())
            {
                case "1": AddCountry(db); break;
                case "2": UpdateCountry(db); break;
                case "3": DeleteCountry(db); break;
                case "4": PrintCountries(db); break;
                case "0": return;
                default: Console.WriteLine("Некорректный ввод, попробуйте снова."); break;
            }
        }
    }

    static void AddCountry(ApplicationContext db)
    {
        var country = new Country
        {
            Name = CheckString("Введите название страны: "),
            Capital = CheckString("Введите столицу: "),
            Population = CheckInt("Введите население: "),
            Area = CheckDouble("Введите площадь (км²): "),
            Continent = CheckString("Введите часть света (Европа, Азия, Африка и т.д.): ")
        };

        db.Countries.Add(country);
        db.SaveChanges();
        Console.WriteLine("Страна успешно добавлена!");
    }

    static void UpdateCountry(ApplicationContext db)
    {
        int countryId = CheckInt("Введите ID страны, которую хотите обновить: ");
        var country = db.Countries.FirstOrDefault(c => c.Id == countryId);
        if (country == null)
        {
            Console.WriteLine("Страна с таким ID не найдена.");
            return;
        }

        Console.WriteLine($"Вы выбрали: {country.Name} (Столица: {country.Capital})");
        country.Name = CheckString("Введите новое название страны: ", country.Name);
        country.Capital = CheckString("Введите новую столицу: ", country.Capital);
        country.Population = CheckInt("Введите новое население: ", country.Population);
        country.Area = CheckDouble("Введите новую площадь: ", country.Area);
        country.Continent = CheckString("Введите новую часть света: ", country.Continent);

        db.SaveChanges();
        Console.WriteLine("Данные о стране успешно обновлены!");
    }

    static void DeleteCountry(ApplicationContext db)
    {
        int countryId = CheckInt("Введите ID страны, которую хотите удалить: ");
        var country = db.Countries.FirstOrDefault(c => c.Id == countryId);
        if (country == null)
        {
            Console.WriteLine("Страна с таким ID не найдена.");
            return;
        }

        db.Countries.Remove(country);
        db.SaveChanges();
        Console.WriteLine("Страна успешно удалена!");
    }

    static void PrintCountries(ApplicationContext db)
    {
        var countries = db.Countries.ToList();
        if (!countries.Any())
        {
            Console.WriteLine("В базе данных нет стран.");
            return;
        }

        Console.WriteLine();

        Console.WriteLine("Список стран:");
        foreach (var country in countries)
        {
            Console.WriteLine($"Id: {country.Id}, Страна: {country.Name}, Столица: {country.Capital}, " +
                              $"Население: {country.Population}, Площадь: {country.Area} км^2, " +
                              $"Часть света: {country.Continent}");
        }
    }

    static string CheckString(string message, string defaultValue = "")
    {
        Console.Write(message);
        string input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
    }

    static int CheckInt(string message, int defaultValue = 0)
    {
        Console.Write(message);
        return int.TryParse(Console.ReadLine(), out int result) ? result : defaultValue;
    }

    static double CheckDouble(string message, double defaultValue = 0)
    {
        Console.Write(message);
        return double.TryParse(Console.ReadLine(), out double result) ? result : defaultValue;
    }
}
