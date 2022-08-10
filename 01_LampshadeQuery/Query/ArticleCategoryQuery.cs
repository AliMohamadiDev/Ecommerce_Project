using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampshadeQuery.Query;

public class ArticleCategoryQuery : IArticleCategoryQuery
{
    private readonly BlogContext _blogContext;

    public ArticleCategoryQuery(BlogContext blogContext)
    {
        _blogContext = blogContext;
    }

    public ArticleCategoryQueryModel GetArticleCategory(string slug)
    {
        return _blogContext.ArticleCategories
            .Select(x => new ArticleCategoryQueryModel
            {
                Slug = x.Slug,
                Name = x.Name,
                Description = x.Description,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                CanonicalAddress = x.CanonicalAddress,
                Articles = MapArticles(x.Articles)
            }).FirstOrDefault(x => x.Slug == slug)!;
    }

    private static List<ArticleQueryModel> MapArticles(List<Article> articles)
    {
        return articles.Select(x => new ArticleQueryModel
        {
            Slug = x.Slug,
            ShortDescription = x.ShortDescription,
            Title = x.Title,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            PublishDate = x.PublishDate.ToFarsi()
        }).ToList();
    }

    public List<ArticleCategoryQueryModel> GetArticleCategories()
    {
        return _blogContext.ArticleCategories
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryQueryModel
            {
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug,
                ArticlesCount = x.Articles.Count
            }).ToList();
    }
}