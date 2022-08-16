namespace ShopManagement.Application.Contracts;

public class PaymentMethod
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    private PaymentMethod(long id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static List<PaymentMethod> GetList()
    {
        return new List<PaymentMethod>
        {
            new PaymentMethod(1, "پرداخت اینترنتی",
                "در این مدل شما به درگاه پرداخت اینترنتی هدایت شده و در لحظه پرداخت وجه را انجام خواهید داد."),
            new PaymentMethod(2, "پرداخت نقدی",
                "در این مدل, ابتدا سفارش ثبت شده و  سپس در هنگام تحویل کالا وجه به صورت نقدی دریافت خواهد شد.")
        };
    }

    public static PaymentMethod GetBy(long id)
    {
        return GetList().FirstOrDefault(x => x.Id == id)!;
    }

}