using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Calo-Intake ===");

        var foods = LoadFoods("foods.json");
        if (foods == null || foods.Count == 0)
        {
            Console.WriteLine("No food data found. Put a foods.json file next to the exe.");
            return;
        }

        double totalCalories = 0, totalProtein = 0, totalCarbs = 0, totalFat = 0;

        Console.WriteLine("Type a food name and grams eaten. Commands: 'list', 'done'.");

        while (true)
        {
            Console.Write("Enter food name (or 'list'/'done'): ");
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input)) continue;
            if (input.Equals("done", StringComparison.OrdinalIgnoreCase)) break;
            if (input.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Available foods:");
                foreach (var f in foods) Console.WriteLine("- " + f.Name);
                continue;
            }

            var food = foods.FirstOrDefault(f => f.Name.Equals(input, StringComparison.OrdinalIgnoreCase));
            if (food == null)
            {
                var suggestions = foods.Where(f => f.Name.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0).Take(8).ToList();
                if (suggestions.Count > 0)
                {
                    Console.WriteLine("Not found. Did you mean:");
                    foreach (var s in suggestions) Console.WriteLine("- " + s.Name);
                }
                else
                {
                    Console.WriteLine("Food not found. Try 'list' to see available foods.");
                }
                continue;
            }

            Console.Write("Enter grams eaten: ");
            var gramsText = Console.ReadLine()?.Trim();
            if (!TryParseDouble(gramsText, out double grams) || grams <= 0)
            {
                Console.WriteLine("Invalid grams. Please enter a positive number.");
                continue;
            }

            double factor = grams / 100.0;
            double cals = food.Calories * factor;
            double prot = food.Protein * factor;
            double carbs = food.Carbs * factor;
            double fat = food.Fat * factor;

            totalCalories += cals;
            totalProtein += prot;
            totalCarbs += carbs;
            totalFat += fat;

            Console.WriteLine("\n--- Nutrition ---");
            Console.WriteLine($"{food.Name} — {grams} g");
            Console.WriteLine($"Calories: {cals:F2} kcal");
            Console.WriteLine($"Protein:  {prot:F2} g");
            Console.WriteLine($"Carbs:    {carbs:F2} g");
            Console.WriteLine($"Fat:      {fat:F2} g\n");
        }

        Console.WriteLine("\n=== Totals ===");
        Console.WriteLine($"Calories: {totalCalories:F2} kcal");
        Console.WriteLine($"Protein:  {totalProtein:F2} g");
        Console.WriteLine($"Carbs:    {totalCarbs:F2} g");
        Console.WriteLine($"Fat:      {totalFat:F02} g");
    }

    static List<Food>? LoadFoods(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Food>>(json, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading foods.json: " + ex.Message);
            return null;
        }
    }

    static bool TryParseDouble(string? text, out double value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(text)) return false;
        if (double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value)) return true;
        if (double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out value)) return true;
        return false;
    }
}
