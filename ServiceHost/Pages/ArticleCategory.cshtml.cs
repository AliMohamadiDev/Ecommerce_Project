using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ArticleCategoryModel : PageModel
    {
        public ArticleCategoryQueryModel ArticleCategory;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        public List<ArticleQueryModel> LatestArticles;

        private readonly IArticleQuery _articleQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;

        public ArticleCategoryModel(IArticleQuery articleQuery, IArticleCategoryQuery articleCategoryQuery)
        {
            _articleQuery = articleQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }

        public void OnGet(string id)
        {
            LatestArticles = _articleQuery.LatestArticles();
            ArticleCategory = _articleCategoryQuery.GetArticleCategory(id);
            ArticleCategories = _articleCategoryQuery.GetArticleCategories();
        }
    }
}
