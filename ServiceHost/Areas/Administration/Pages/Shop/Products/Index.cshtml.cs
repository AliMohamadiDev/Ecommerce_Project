using _0_Framework.Infrastructure;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.Shop.Products;

public class IndexModel : PageModel
{
    [TempData] public string Message { get; set; }

    public ProductSearchModel SearchModel;
    public List<ProductViewModel> Products;
    public SelectList ProductCategories;

    private readonly IProductApplication _productApplication;
    private readonly IProductCategoryApplication _productCategoryApplication;

    public IndexModel(IProductApplication productApplication, IProductCategoryApplication productCategoryApplication)
    {
        _productApplication = productApplication;
        _productCategoryApplication = productCategoryApplication;
    }

    [NeedPermission(ShopPermissions.ListProducts)]
    public void OnGet(ProductSearchModel searchModel)
    {
        ProductCategories = new SelectList(_productCategoryApplication.GetProductCategories(), "Id", "Name");
        Products = _productApplication.Search(searchModel);
    }

    public IActionResult OnGetCreate()
    {
        var command = new CreateProduct
        {
            Categories = _productCategoryApplication.GetProductCategories()
        };
        return Partial("./Create", command);
    }

    [NeedPermission(ShopPermissions.CreateProduct)]
    public JsonResult OnPostCreate(CreateProduct command)
    {
        var result = _productApplication.Create(command);
        return new JsonResult(result);
    }

    public IActionResult OnGetEdit(long id)
    {
        var product = _productApplication.GetDetails(id);
        product.Categories = _productCategoryApplication.GetProductCategories();
        return Partial("Edit", product);
    }

    [NeedPermission(ShopPermissions.EditProduct)]
    public JsonResult OnPostEdit(EditProduct command)
    {
        var result = _productApplication.Edit(command);
        return new JsonResult(result);
    }


    public IActionResult OnGetExcel()
    {
        var products = _productApplication.Search(new ProductSearchModel());
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Products");
        var currentRow = 1;

        worksheet.Cell(currentRow, 1).Value = "آی دی";
        worksheet.Cell(currentRow, 2).Value = "نام";
        worksheet.Cell(currentRow, 3).Value = "گروه";
        worksheet.Cell(currentRow, 4).Value = "کد";
        worksheet.Cell(currentRow, 5).Value = "تاریخ ایجاد";

        foreach (var product in products)
        {
            currentRow++;

            worksheet.Cell(currentRow, 1).Value = product.Id;
            worksheet.Cell(currentRow, 2).Value = product.Name;
            worksheet.Cell(currentRow, 3).Value = product.Category;
            worksheet.Cell(currentRow, 4).Value = product.Code;
            worksheet.Cell(currentRow, 5).Value = product.CreationDate;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();

        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "products.xlsx");
    }
}