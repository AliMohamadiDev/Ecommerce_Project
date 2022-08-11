using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.Comment;
using BlogManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampshadeQuery.Query;

public class ArticleQuery : IArticleQuery
{
    private readonly BlogContext _blogContext;
    private readonly CommentContext _commentContext;

    public ArticleQuery(BlogContext blogContext, CommentContext commentContext)
    {
        _blogContext = blogContext;
        _commentContext = commentContext;
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
                Id = x.Id,
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

        var comments = _commentContext.Comments
            .Where(x => !x.IsCanceled)
            .Where(x => x.IsConfirmed)
            .Where(x => x.Type == CommentType.Article)
            .Where(x => x.OwnerRecordId == article.Id)
            .Select(x => new CommentQueryModel
            {
                Id = x.Id,
                Message = x.Message,
                Name = x.Name,
                ParentId = x.ParentId,
                CreationDate = x.CreationDate.ToFarsi()
            }).OrderByDescending(x => x.Id).ToList();

        foreach (var comment in comments)
        {
            if (comment.ParentId > 0)
            {
                comment.ParentName = comments.FirstOrDefault(x => x.Id == comment.ParentId)!.Name;
            }
        }

        article.Comments = comments;

        return article;
    }
}