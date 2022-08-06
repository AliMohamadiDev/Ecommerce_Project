using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository;

public class ArticleCategoryRepository : RepositoryBase<long, ArticleCategory>, IArticleCategoryRepository
{
    private readonly BlogContext _blogContext;
    public ArticleCategoryRepository(BlogContext context) : base(context)
    {
        _blogContext = context;
    }

    public string GetSlugBy(long id)
    {
        return _blogContext.ArticleCategories.Select(x => new {x.Id, x.Slug}).FirstOrDefault(x => x.Id == id)!.Slug;
    }

    public EditArticleCategory GetDetails(long id)
    {
        return _blogContext.ArticleCategories.Select(x => new EditArticleCategory
        {
            Id = x.Id,
            Name = x.Name,
            CanonicalAddress = x.CanonicalAddress,
            Description = x.Description,
            Keywords = x.Keywords,
            MetaDescription = x.MetaDescription,
            ShowOrder = x.ShowOrder,
            Slug = x.Slug,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle
        }).AsNoTracking().FirstOrDefault(x => x.Id == id)!;
    }

    public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel)
    {
        var query = _blogContext.ArticleCategories.Select(x => new ArticleCategoryViewModel
        {
            Id = x.Id,
            Description = x.Description,
            Name = x.Name,
            Picture = x.Picture,
            ShowOrder = x.ShowOrder,
            CreationDate = x.CreationDate.ToFarsi()
        });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
        {
            query = query.Where(x => x.Name.Contains(searchModel.Name));
        }

        return query.OrderByDescending(x => x.ShowOrder).ToList();
    }
}