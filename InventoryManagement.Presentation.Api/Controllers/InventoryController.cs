using _01_LampshadeQuery.Contracts.Inventory;
using InventoryManagement.Application.Contracts.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Presentation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryQuery _inventoryQuery;
    private readonly IInventoryApplication _inventoryApplication;

    public InventoryController(IInventoryApplication inventoryApplication, IInventoryQuery inventoryQuery)
    {
        _inventoryQuery = inventoryQuery;
        _inventoryApplication = inventoryApplication;
    }

    [HttpGet("{id}")]
    public List<InventoryOperationViewModel> GetOperationBy(long id)
    {
        return _inventoryApplication.GetOperationLog(id);
    }

    [HttpPost]
    public StockStatus CheckStock(IsInStock command)
    {
        return _inventoryQuery.CheckStock(command);
    }
}