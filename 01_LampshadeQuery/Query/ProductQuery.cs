﻿using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Comment;
using _01_LampshadeQuery.Contracts.Product;
using CommentManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class ProductQuery : IProductQuery
{
    private readonly ShopContext _shopContext;
    private readonly CommentContext _commentContext;
    private readonly DiscountContext _discountContext;
    private readonly InventoryContext _inventoryContext;

    public ProductQuery(ShopContext shopContext, InventoryContext inventoryContext, DiscountContext discountContext, CommentContext commentContext)
    {
        _shopContext = shopContext;
        _commentContext = commentContext;
        _discountContext = discountContext;
        _inventoryContext = inventoryContext;
    }

    public ProductQueryModel GetProductDetails(string slug)
    {
        var inventory = _inventoryContext.Inventory.Select(x =>
            new {x.ProductId, x.UnitPrice, x.InStock}).ToList();

        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new {x.DiscountRate, x.ProductId, x.EndDate}).ToList();

        var product = _shopContext.Products
            .Include(x => x.Category)
            .Include(x=>x.ProductPictures)
            .Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Category = x.Category.Name,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug,
                CategorySlug = x.Category.Slug,
                Code = x.Code,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                Pictures = MapProductPictures(x.ProductPictures)
            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

        if (product is null)
            return new ProductQueryModel();

        var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);

        if (productInventory is not null)
        {
            product.IsInStock = productInventory.InStock;
            var price = productInventory.UnitPrice;
            product.Price = price.ToMoney()!;
            product.DoublePrice = price;

            var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);

            if (discount is not null)
            {
                var discountRate = discount.DiscountRate;
                product.DiscountRate = discountRate;
                product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                product.HasDiscount = discountRate > 0;
                var discountAmount = Math.Round((price * discountRate) / 100);
                product.PriceWithDiscount = (price - discountAmount).ToMoney();
            }
        }

        product.Comments = _commentContext.Comments
            .Where(x => !x.IsCanceled)
            .Where(x => x.IsConfirmed)
            .Where(x => x.Type == CommentType.Product)
            .Where(x=>x.OwnerRecordId == product.Id)
            .Select(x=>new CommentQueryModel
            {
                Id = x.Id,
                Message = x.Message,
                Name = x.Name,
                CreationDate = x.CreationDate.ToFarsi()
            }).OrderByDescending(x => x.Id).ToList();

        return product;
    }

    private static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> pictures)
    {
        return pictures.Select(x => new ProductPictureQueryModel
        {
            IsRemoved = x.IsRemoved,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            ProductId = x.ProductId
        }).Where(x => !x.IsRemoved).ToList();
    }

    public List<ProductQueryModel> GetLatestArrivals()
    {
        var inventory = _inventoryContext.Inventory.Select(x =>
            new {x.ProductId, x.UnitPrice}).ToList();

        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new {x.DiscountRate, x.ProductId}).ToList();

        var products = _shopContext.Products.Include(x => x.Category)
            .Select(product => new ProductQueryModel
            {
                Id = product.Id,
                Category = product.Category.Name,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Slug = product.Slug
            }).AsNoTracking().OrderByDescending(x => x.Id).Take(6).ToList();

        foreach (var product in products)
        {
            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);

            if (productInventory is not null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney()!;

                var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);

                if (discount is not null)
                {
                    var discountRate = discount.DiscountRate;
                    product.DiscountRate = discountRate;
                    product.HasDiscount = discountRate > 0;
                    var discountAmount = Math.Round((price * discountRate) / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }
        }


        return products;
    }

    public List<ProductQueryModel> Search(string value)
    {
        var inventory = _inventoryContext.Inventory.Select(x =>
            new {x.ProductId, x.UnitPrice}).ToList();

        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new {x.DiscountRate, x.ProductId, x.EndDate}).ToList();

        var query = _shopContext.Products
            .Include(x => x.Category)
            .Select(product => new ProductQueryModel
            {
                Id = product.Id,
                Category = product.Category.Name,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Slug = product.Slug,
                CategorySlug = product.Category.Slug,
                ShortDescription = product.ShortDescription
            }).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(value))
        {
            query = query.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value));
        }

        var products = query.OrderByDescending(x => x.Id).ToList();

        foreach (var product in products)
        {
            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);

            if (productInventory is not null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney()!;

                var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);

                if (discount is not null)
                {
                    var discountRate = discount.DiscountRate;
                    product.DiscountRate = discountRate;
                    product.HasDiscount = discountRate > 0;
                    product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                    var discountAmount = Math.Round((price * discountRate) / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }
        }


        return products;
    }

    public List<CartItem> CheckInventoryStatus(List<CartItem> cartItems)
    {
        var inventory = _inventoryContext.Inventory.ToList();

        foreach (var cartItem in cartItems.Where(cartItem =>
                     inventory.Any(x=>x.ProductId == cartItem.Id && x.InStock)))
        {
            var itemInventory = inventory.Find(x => x.ProductId == cartItem.Id);
            cartItem.IsInStock = itemInventory.CalculateCurrentCount() >= cartItem.Count;
        }

        return cartItems;
    }
}