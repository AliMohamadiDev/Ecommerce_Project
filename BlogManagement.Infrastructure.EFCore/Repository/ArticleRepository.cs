using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository;

public class ArticleRepository : RepositoryBase<long, Article>, IArticleRepository
{
    private readonly BlogContext _blogContext;
    public ArticleRepository(BlogContext context) : base(context)
    {
        _blogContext = context;
    }

    public EditArticle GetDetails(long id)
    {
        return _blogContext.Articles.Select(x => new EditArticle
        {
            Id = x.Id,
            CanonicalAddress = x.CanonicalAddress,
            CategoryId = x.CategoryId,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            PublishDate = x.PublishDate.ToFarsi(),
            ShortDescription = x.ShortDescription,
            Slug = x.Slug,
            Title = x.Title
        }).AsNoTracking().FirstOrDefault(x => x.Id == id)!;
    }

    public Article GetWithCategory(long id)
    {
        return _blogContext.Articles.Include(x => x.Category).FirstOrDefault(x => x.Id == id)!;
    }

    public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
    {
        var query = _blogContext.Articles.Select(x => new ArticleViewModel
        {
            Id = x.Id,
            Category = x.Category.Name,
            Picture = x.Picture,
            PublishDate = x.PublishDate.ToFarsi(),
            ShortDescription = x.ShortDescription,
            Title = x.Title
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Title))
        {
            query = query.Where(x => x.Title.Contains(searchModel.Title));
        }

        if (searchModel.CategoryId > 0)
        {
            query = query.Where(x => x.CategoreyId == searchModel.CategoryId);
        }

        return query.OrderByDescending(x => x.Id).ToList();
    }
}