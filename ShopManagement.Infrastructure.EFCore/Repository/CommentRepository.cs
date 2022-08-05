using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Comment;
using ShopManagement.Domain.CommentAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class CommentRepository : RepositoryBase<long, Comment>, ICommentRepository
{
    private readonly ShopContext _shopContext;
    public CommentRepository(ShopContext context) : base(context)
    {
        _shopContext = context;
    }

    public List<CommentViewModel> Search(CommentSearchModel searchModel)
    {
        var query = _shopContext.Comments
            .Include(x => x.Product)
            .Select(x => new CommentViewModel
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                Message = x.Message,
                IsCanceled = x.IsCanceled,
                IsConfirmed = x.IsConfirmed,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                CommentDate = x.CreationDate.ToFarsi()
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
        {
            query = query.Where(x => x.Name.Contains(searchModel.Name));
        }
        if (!string.IsNullOrWhiteSpace(searchModel.Email))
        {
            query = query.Where(x => x.Email.Contains(searchModel.Email));
        }

        return query.OrderByDescending(x => x.Id).ToList();
    }
}