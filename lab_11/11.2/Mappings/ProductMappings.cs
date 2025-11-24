using Northwind.Api.Models;
using Northwind.Shared.Products;

namespace Northwind.Api.Mappings;

public static class ProductMappings
{
    public static ProductDto ToDto(this Product product) =>
        new(
            product.ProductId,
            product.ProductName,
            product.SupplierId,
            product.CategoryId,
            product.QuantityPerUnit,
            product.UnitPrice,
            product.UnitsInStock,
            product.UnitsOnOrder,
            product.ReorderLevel,
            product.Discontinued);

    public static Product ToEntity(this UpsertProductDto dto, int id = 0) =>
        new()
        {
            ProductId = id,
            ProductName = dto.ProductName,
            SupplierId = dto.SupplierId,
            CategoryId = dto.CategoryId,
            QuantityPerUnit = dto.QuantityPerUnit,
            UnitPrice = dto.UnitPrice,
            UnitsInStock = dto.UnitsInStock,
            UnitsOnOrder = dto.UnitsOnOrder,
            ReorderLevel = dto.ReorderLevel,
            Discontinued = dto.Discontinued
        };

    public static void UpdateFrom(this Product product, UpsertProductDto dto)
    {
        product.ProductName = dto.ProductName;
        product.SupplierId = dto.SupplierId;
        product.CategoryId = dto.CategoryId;
        product.QuantityPerUnit = dto.QuantityPerUnit;
        product.UnitPrice = dto.UnitPrice;
        product.UnitsInStock = dto.UnitsInStock;
        product.UnitsOnOrder = dto.UnitsOnOrder;
        product.ReorderLevel = dto.ReorderLevel;
        product.Discontinued = dto.Discontinued;
    }
}

