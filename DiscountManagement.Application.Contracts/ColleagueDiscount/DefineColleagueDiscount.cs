using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;

namespace DiscountManagement.Application.Contracts.ColleagueDiscount;

public class DefineColleagueDiscount
{
    [Range(1, 100_000, ErrorMessage = ValidationMessage.IsRequired)]
    public long ProductId { get; set; }

    [Range(1, 99, ErrorMessage = ValidationMessage.IsRequired)]
    public int DiscountRate { get; set; }

    public List<ProductViewModel> Products { get; set; }
}