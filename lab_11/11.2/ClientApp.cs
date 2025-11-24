using Northwind.Client.Services;
using Northwind.Shared.Products;
using System.Globalization;

namespace Northwind.Client;

public class ClientApp(ProductApiClient apiClient)
{
    private readonly ProductApiClient _apiClient = apiClient;

    public async Task RunAsync()
    {
        Console.WriteLine("Northwind Products client");
        Console.WriteLine("Введите номер команды и нажмите Enter.");

        var exitRequested = false;
        while (!exitRequested)
        {
            PrintMenu();
            Console.Write("> ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ListProductsAsync();
                    break;
                case "2":
                    await GetProductAsync();
                    break;
                case "3":
                    await CreateProductAsync();
                    break;
                case "4":
                    await UpdateProductAsync();
                    break;
                case "5":
                    await DeleteProductAsync();
                    break;
                case "0":
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("1 - Показать все товары");
        Console.WriteLine("2 - Найти товар по ID");
        Console.WriteLine("3 - Добавить товар");
        Console.WriteLine("4 - Обновить товар");
        Console.WriteLine("5 - Удалить товар");
        Console.WriteLine("0 - Выход");
    }

    private async Task ListProductsAsync()
    {
        var products = await _apiClient.GetProductsAsync();
        if (products.Count == 0)
        {
            Console.WriteLine("В базе нет товаров.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId,3} | {product.ProductName,-35} | Цена: {product.UnitPrice?.ToString("C", CultureInfo.GetCultureInfo("ru-RU")) ?? "n/a"} | Остаток: {product.UnitsInStock ?? 0}");
        }
    }

    private async Task GetProductAsync()
    {
        var id = PromptInt("ID товара");
        if (id is null)
        {
            return;
        }

        var product = await _apiClient.GetProductAsync(id.Value);
        if (product is null)
        {
            Console.WriteLine("Товар не найден.");
            return;
        }

        PrintProduct(product);
    }

    private async Task CreateProductAsync()
    {
        var dto = PromptUpsertDto();
        if (dto is null)
        {
            return;
        }

        var created = await _apiClient.CreateProductAsync(dto);
        if (created is null)
        {
            Console.WriteLine("Не удалось создать товар.");
            return;
        }

        Console.WriteLine("Создан товар:");
        PrintProduct(created);
    }

    private async Task UpdateProductAsync()
    {
        var id = PromptInt("ID товара");
        if (id is null)
        {
            return;
        }

        var dto = PromptUpsertDto();
        if (dto is null)
        {
            return;
        }

        var updated = await _apiClient.UpdateProductAsync(id.Value, dto);
        if (updated is null)
        {
            Console.WriteLine("Не удалось обновить товар.");
            return;
        }

        Console.WriteLine("Обновленный товар:");
        PrintProduct(updated);
    }

    private async Task DeleteProductAsync()
    {
        var id = PromptInt("ID товара");
        if (id is null)
        {
            return;
        }

        var deleted = await _apiClient.DeleteProductAsync(id.Value);
        Console.WriteLine(deleted ? "Товар удален." : "Не удалось удалить товар.");
    }

    private static void PrintProduct(ProductDto product)
    {
        Console.WriteLine($"ID: {product.ProductId}");
        Console.WriteLine($"Название: {product.ProductName}");
        Console.WriteLine($"Поставщик: {product.SupplierId?.ToString() ?? "-"}");
        Console.WriteLine($"Категория: {product.CategoryId?.ToString() ?? "-"}");
        Console.WriteLine($"Фасовка: {product.QuantityPerUnit ?? "-"}");
        Console.WriteLine($"Цена: {product.UnitPrice?.ToString("F2") ?? "-"}");
        Console.WriteLine($"Склад: {product.UnitsInStock?.ToString() ?? "-"}");
        Console.WriteLine($"В заказе: {product.UnitsOnOrder?.ToString() ?? "-"}");
        Console.WriteLine($"Минимум: {product.ReorderLevel?.ToString() ?? "-"}");
        Console.WriteLine($"Снят с продажи: {(product.Discontinued ? "да" : "нет")}");
    }

    private static int? PromptInt(string label)
    {
        Console.Write($"{label}: ");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out var value))
        {
            Console.WriteLine("Введите корректное число.");
            return null;
        }

        return value;
    }

    private static short? PromptShort(string label)
    {
        Console.Write($"{label}: ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        if (!short.TryParse(input, out var value))
        {
            Console.WriteLine("Введите корректное число или оставьте пустым.");
            return null;
        }

        return value;
    }

    private static decimal? PromptDecimal(string label)
    {
        Console.Write($"{label}: ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
        {
            Console.WriteLine("Введите корректное число (используйте точку).");
            return null;
        }

        return value;
    }

    private static bool? PromptBool(string label)
    {
        Console.Write($"{label} (y/n): ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        return input.Trim().ToLowerInvariant() switch
        {
            "y" or "д" => true,
            "n" or "н" => false,
            _ => null
        };
    }

    private static UpsertProductDto? PromptUpsertDto()
    {
        Console.Write("Название*: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Название обязательно.");
            return null;
        }

        var supplierId = PromptInt("Поставщик (пусто если нет)");
        var categoryId = PromptInt("Категория (пусто если нет)");
        var quantityPerUnit = PromptOptional("Фасовка");
        var price = PromptDecimal("Цена");
        var unitsInStock = PromptShort("Остаток");
        var unitsOnOrder = PromptShort("В заказе");
        var reorderLevel = PromptShort("Мин. остаток");
        var discontinued = PromptBool("Снят с продажи") ?? false;

        return new UpsertProductDto(
            name.Trim(),
            supplierId,
            categoryId,
            quantityPerUnit,
            price,
            unitsInStock,
            unitsOnOrder,
            reorderLevel,
            discontinued);
    }

    private static string? PromptOptional(string label)
    {
        Console.Write($"{label}: ");
        var input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
    }
}

