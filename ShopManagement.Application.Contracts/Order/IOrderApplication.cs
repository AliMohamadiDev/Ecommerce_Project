namespace ShopManagement.Application.Contracts.Order;

public interface IOrderApplication
{
    void Cancel(long id);
    long PlaceOrder(Cart cart);
    double GetAmountBy(long id);
    List<OrderItemViewModel> GetItems(long orderId);
    string PaymentSucceeded(long orderId, long refId);
    List<OrderViewModel> Search(OrderSearchModel searchModel);
}