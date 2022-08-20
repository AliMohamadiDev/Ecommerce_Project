using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Pages;

public class IndexModel : PageModel
{
    public Chart DoughnutDataSet { get; set; }
    public List<Chart> BarLineDataSet { get; set; }

    public List<string> NameList { get; set; }
    public List<int> dataList { get; set; }
    public List<string> ColorList { get; set; }

    private readonly IProductCategoryApplication _productCategoryApplication;

    public IndexModel(IProductCategoryApplication productCategoryApplication)
    {
        _productCategoryApplication = productCategoryApplication;
    }

    public void OnGet()
    {
        var productCategories = _productCategoryApplication.GetProductCategories()
            .Select(x => new {x.Id, x.Name, x.ProductsCount}).ToList();
        NameList = new List<string>();
        dataList = new List<int>();
        ColorList = new List<string>();
        //var p1 = productCategories

        //var NameList = new List<string>();
        //var dataList = new List<long>();
        var random = new Random();
        foreach (var productCategory in productCategories)
        {
            NameList.Add(productCategory.Name);
            dataList.Add((int) productCategory.ProductsCount);
            var color = $"#{random.Next(0x1000000):X6}";
            ColorList.Add(color);
        }

        DoughnutDataSet = new Chart
        {
            Label = "Apple",
            Data = dataList,
            BorderColor = "#ffcdb2",
            BackgroundColor = ColorList.ToArray()
        };

        /* BarLineDataSet = new List<Chart1>();

         foreach (var productCategory in productCategories)
         {
             var chart = new Chart1
             {
                 Label = productCategory.Name,
                 Data = productCategory.ProductsCount,
                 BackgroundColor = $"#{random.Next(0x1000000):X6}",
                 BorderColor = "#b5838d"
             };
             BarLineDataSet.Add(chart);
         }
 */
        BarLineDataSet = new List<Chart>
        {
            new Chart
            {
                Label = "Apple",
                Data = new List<int> {100, 200, 250, 170, 50},
                BackgroundColor = new[] {"#ffcdb2"},
                BorderColor = "#b5838d"
            },
            new Chart
            {
                Label = "Samsung",
                Data = new List<int> {200, 300, 350, 270, 100},
                BackgroundColor = new[] {"#ffc8dd"},
                BorderColor = "#ffafcc"
            },
            new Chart
            {
                Label = "Total",
                Data = new List<int> {300, 500, 600, 440, 150},
                BackgroundColor = new[] {"#0077b6"},
                BorderColor = "#023e8a"
            },
        };
        /*DoughnutDataSet = new Chart
        {
            Label = "Apple",
            Data = new List<int> { 100, 200, 250, 170, 50 },
            BorderColor = "#ffcdb2",
            BackgroundColor = new[] { "#b5838d", "#ffd166", "#7f4f24", "#ef233c", "#003049" }
        };*/
    }

}

public class Chart
{
    [JsonProperty(PropertyName = "label")] public string Label { get; set; }

    [JsonProperty(PropertyName = "data")] public List<int> Data { get; set; }

    [JsonProperty(PropertyName = "backgroundColor")]
    public string[] BackgroundColor { get; set; }

    [JsonProperty(PropertyName = "borderColor")]
    public string BorderColor { get; set; }
}

public class Chart1
{
    [JsonProperty(PropertyName = "label")] public string Label { get; set; }

    [JsonProperty(PropertyName = "data")] public long Data { get; set; }

    [JsonProperty(PropertyName = "backgroundColor")]
    public string BackgroundColor { get; set; }

    [JsonProperty(PropertyName = "borderColor")]
    public string BorderColor { get; set; }
}