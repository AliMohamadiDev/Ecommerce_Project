using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace BlogManagement.Application.Contracts.Article;

public class CreateArticle
{
    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Title { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public IFormFile? Picture { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string PictureAlt { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string PictureTitle { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string PublishDate { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Slug { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string Keywords { get; set; }

    [Required(ErrorMessage = ValidationMessage.IsRequired)]
    public string MetaDescription { get; set; }

    public string? CanonicalAddress { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = ValidationMessage.IsRequired)]
    public long CategoryId { get; set; }
}