﻿using _0_Framework.Application;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Application.Contracts.Product;

namespace ShopManagement.Application;

public class ProductApplication : IProductApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductApplication(IProductRepository productRepository, IFileUploader fileUploader, IProductCategoryRepository productCategoryRepository)
    {
        _fileUploader = fileUploader;
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
    }

    public OperationResult Create(CreateProduct command)
    {
        var operation = new OperationResult();
        if (_productRepository.Exists(x => x.Name == command.Name))
        {
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        var slug = command.Slug.Slugify();

        var categorySlug = _productCategoryRepository.GetCategorySlugById(command.CategoryId);
        var path = $"{categorySlug}/{slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);

        var product = new Product(command.Name, command.Code, command.ShortDescription,
            command.MetaDescription, picturePath, command.PictureAlt, command.PictureTitle, command.CategoryId,
            slug, command.Keywords, command.MetaDescription);
        _productRepository.Create(product);
        _productRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditProduct command)
    {
        var operation = new OperationResult();
        var product = _productRepository.GetProductWithCategory(command.Id);
        if (product == null)
        {
            return operation.Failed(ApplicationMessages.RecordNotFound);
        }

        if (_productRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
        {
            return operation.Failed(ApplicationMessages.DuplicatedRecord);
        }

        var slug = command.Slug.Slugify();

        var path = $"{product.Category.Slug}/{slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);

        product.Edit(command.Name, command.Code, command.ShortDescription,
            command.Description, picturePath, command.PictureAlt, command.PictureTitle, command.CategoryId,
            slug, command.Keywords, command.MetaDescription);

        _productRepository.SaveChanges();
        return operation.Succeeded();
    }

    public EditProduct GetDetails(long id)
    {
        return _productRepository.GetDetails(id);
    }

    public List<ProductViewModel> GetProducts()
    {
        return _productRepository.GetProducts();
    }

    public List<ProductViewModel> Search(ProductSearchModel searchModel)
    {
        return _productRepository.Search(searchModel);
    }
}