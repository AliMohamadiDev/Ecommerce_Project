using _0_Framework.Application;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application;

public class OrderApplication : IOrderApplication
{
    private readonly IAuthHelper _authHelper;
    private readonly IConfiguration _configuration;
    private readonly IOrderRepository _orderRepository;
    private readonly IShopInventoryAcl _shopInventoryAcl;

    public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration, IShopInventoryAcl shopInventoryAcl)
    {
        _authHelper = authHelper;
        _configuration = configuration;
        _orderRepository = orderRepository;
        _shopInventoryAcl = shopInventoryAcl;
    }

    public void Cancel(long id)
    {
        var order = _orderRepository.Get(id);
        order.Cancel();
        _orderRepository.SaveChanges();
    }

    public long PlaceOrder(Cart cart)
    {
        var currentAccountId = _authHelper.CurrentAccountId();
        var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount, cart.PayAmount);

        foreach (var cartItem in cart.Items)
        {
            var orderItem = new OrderItem(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate);
            order.AddItem(orderItem);
        }

        _orderRepository.Create(order);
        _orderRepository.SaveChanges();
        return order.Id;
    }

    public double GetAmountBy(long id)
    {
        return _orderRepository.GetAmountBy(id);
    }

    public List<OrderItemViewModel> GetItems(long orderId)
    {
        return _orderRepository.GetItems(orderId);
    }

    public string PaymentSucceeded(long orderId, long refId)
    {
        var order = _orderRepository.Get(orderId);
        order.PaymentSucceeded(refId);
        var issueTrackingNo = CodeGenerator.Generate("S");
        order.SetIssueTrackingNo(issueTrackingNo);

        if (!_shopInventoryAcl.ReduceFromInventory(order.Items)) return "";
        _orderRepository.SaveChanges();
        return issueTrackingNo;

    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        return _orderRepository.Search(searchModel);
    }
}