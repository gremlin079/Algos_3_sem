using Northwind.Api.Models;

namespace Northwind.Api.Data.Seed;

internal static class ProductSeedData
{
    public static IReadOnlyCollection<Product> Records { get; } =
    [
        new Product { ProductId = 1, ProductName = "Chai", SupplierId = 1, CategoryId = 1, QuantityPerUnit = "10 boxes x 20 bags", UnitPrice = 18.00m, UnitsInStock = 39, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
        new Product { ProductId = 2, ProductName = "Chang", SupplierId = 1, CategoryId = 1, QuantityPerUnit = "24 - 12 oz bottles", UnitPrice = 19.00m, UnitsInStock = 17, UnitsOnOrder = 40, ReorderLevel = 25, Discontinued = false },
        new Product { ProductId = 3, ProductName = "Aniseed Syrup", SupplierId = 1, CategoryId = 2, QuantityPerUnit = "12 - 550 ml bottles", UnitPrice = 10.00m, UnitsInStock = 13, UnitsOnOrder = 70, ReorderLevel = 25, Discontinued = false },
        new Product { ProductId = 4, ProductName = "Chef Anton's Cajun Seasoning", SupplierId = 2, CategoryId = 2, QuantityPerUnit = "48 - 6 oz jars", UnitPrice = 22.00m, UnitsInStock = 53, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
        new Product { ProductId = 5, ProductName = "Grandma's Boysenberry Spread", SupplierId = 3, CategoryId = 2, QuantityPerUnit = "12 - 8 oz jars", UnitPrice = 25.00m, UnitsInStock = 120, UnitsOnOrder = 0, ReorderLevel = 25, Discontinued = false },
        new Product { ProductId = 6, ProductName = "Uncle Bob's Organic Dried Pears", SupplierId = 3, CategoryId = 7, QuantityPerUnit = "12 - 1 lb pkgs.", UnitPrice = 30.00m, UnitsInStock = 15, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
        new Product { ProductId = 7, ProductName = "Northwoods Cranberry Sauce", SupplierId = 3, CategoryId = 2, QuantityPerUnit = "12 - 12 oz jars", UnitPrice = 40.00m, UnitsInStock = 6, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
        new Product { ProductId = 8, ProductName = "Mishi Kobe Niku", SupplierId = 4, CategoryId = 6, QuantityPerUnit = "18 - 500 g pkgs.", UnitPrice = 97.00m, UnitsInStock = 29, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = true },
        new Product { ProductId = 9, ProductName = "Ikura", SupplierId = 4, CategoryId = 8, QuantityPerUnit = "12 - 200 ml jars", UnitPrice = 31.00m, UnitsInStock = 31, UnitsOnOrder = 0, ReorderLevel = 0, Discontinued = false },
        new Product { ProductId = 10, ProductName = "Queso Cabrales", SupplierId = 5, CategoryId = 4, QuantityPerUnit = "1 kg pkg.", UnitPrice = 21.00m, UnitsInStock = 22, UnitsOnOrder = 30, ReorderLevel = 30, Discontinued = false }
    ];
}

