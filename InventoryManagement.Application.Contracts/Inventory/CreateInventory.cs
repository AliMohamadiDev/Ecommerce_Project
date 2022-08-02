using _0_Framework.Application;
using System.ComponentModel.DataAnnotations;
using ShopManagement.Application.Contracts.Product;

namespace InventoryManagement.Application.Contracts.Inventory;

public class CreateInventory
{
    [Range(1, 100000, ErrorMessage = ValidationMessage.IsRequired)]
    public long ProductId { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = ValidationMessage.IsRequired)]
    public double UnitPrice { get; set; }

    public List<ProductViewModel> Products { get; set; }
}