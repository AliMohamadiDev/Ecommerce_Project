using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace ShopManagement.Application.Contracts.ProductCategory;

public class CreateProductCategory
{
    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Name { get; set; }

    public string Description { get; set; }

    //[Required(ErrorMessage = ValidationMessage.IsRequired)]
    [FileExtensionLimitation(new string[] {".jpeg", ".jpg", ".png"}, ErrorMessage = ValidationMessage.InvalidFileFormat)]
    [MaxFileSize(3 * 1024 * 1024, ErrorMessage = ValidationMessage.MaxFileSize)]
    public IFormFile Picture { get; set; }

    public string PictureAlt { get; set; }
    public string PictureTitle { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Keywords { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string MetaDescription { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Slug { get; set; }
}