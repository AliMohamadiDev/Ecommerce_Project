using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Article;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampshadeQuery.Query;

public class ArticleQuery : IArticleQuery
{
    private readonly BlogContext _blogContext;

    public ArticleQuery(BlogContext blogContext)
    {
        _blogContext = blogContext;
    }

    public List<ArticleQueryModel> LatestArticles()
    {
        return _blogContext.Articles
            .Include(x => x.Category)
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Title = x.Title,
                Slug = x.Slug,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription
            }).ToList();
    }

    public ArticleQueryModel GetArticleDetails(string slug)
    {
        var article = _blogContext.Articles
            .Include(x => x.Category)
            .Where(x => x.PublishDate <= DateTime.Now)
            .Select(x => new ArticleQueryModel
            {
                Title = x.Title,
                CategoryId = x.CategoryId,
                CategorySlug = x.Category.Slug,
                CategoryName = x.Category.Name,
                Slug = x.Slug,
                CanonicalAddress = x.CanonicalAddress,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi(),
                ShortDescription = x.ShortDescription
            }).FirstOrDefault(x => x.Slug == slug)!;

        article.keywordList = article.Keywords.Split(",").ToList();
        
        return article;
    }
}