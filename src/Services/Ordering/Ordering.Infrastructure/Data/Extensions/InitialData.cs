namespace Ordering.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
    [
        Customer.Create(CustomerId.Of(new Guid("4b12bcbd-30f1-492e-aaa6-03f661a36c7e")), "francesco", "francesco.sparatore@intellitronika.com"),
        Customer.Create(CustomerId.Of(new Guid("eeaf1c64-cce4-4d74-ba0a-9fcf1379fc3c")), "john", "john@email.com")
    ];

    public static IEnumerable<Product> Products =>
    [
        Product.Create(ProductId.Of(new Guid("e06b0a85-83df-40e0-8906-ae9c47e8f237")), "IPhone X", 500),
        Product.Create(ProductId.Of(new Guid("fc07c3c0-76c5-4c70-a3c2-5159a1664d86")), "Samsung 10", 400),
        Product.Create(ProductId.Of(new Guid("0d478995-1ef9-4454-9b41-1cf2ea6bcb24")), "Huawei Plus", 650),
        Product.Create(ProductId.Of(new Guid("47bdc1f4-3dd5-46b0-8d24-d2eadf621215")), "Xiaomi Mi", 450)
    ];


    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            // Order 1
            var address1 = Address.Of("fra", "spara", "francesco.sparatore@intellitronika.com", "Via Francesco Amici, 6", "Italia", "Italia", "00152");
            var payment1 = Payment.Of("fra", "12345678901", "12/29", "281", 1);
            var order1 = Order.Create(
                            OrderId.Of(Guid.NewGuid()),
                            CustomerId.Of(new Guid("4b12bcbd-30f1-492e-aaa6-03f661a36c7e")),
                            OrderName.Of("ORD_1"),
                            shippingAddress: address1,
                            billingAddress: address1,
                            payment1);
            order1.Add(ProductId.Of(new Guid("e06b0a85-83df-40e0-8906-ae9c47e8f237")), 2, 500);
            order1.Add(ProductId.Of(new Guid("fc07c3c0-76c5-4c70-a3c2-5159a1664d86")), 1, 400);

            // Order 2
            var payment2 = Payment.Of("john", "9876543210", "01/30", "482", 2);
            var address2 = Address.Of("john", "doe", "john@email.com", "Via Francesco Amici, 6", "Italia", "Italia", "00152");
            var order2 = Order.Create(
                            OrderId.Of(Guid.NewGuid()),
                            CustomerId.Of(new Guid("eeaf1c64-cce4-4d74-ba0a-9fcf1379fc3c")),
                            OrderName.Of("ORD_2"),
                            shippingAddress: address2,
                            billingAddress: address2,
                            payment2);
            order2.Add(ProductId.Of(new Guid("0d478995-1ef9-4454-9b41-1cf2ea6bcb24")), 2, 500);
            order2.Add(ProductId.Of(new Guid("47bdc1f4-3dd5-46b0-8d24-d2eadf621215")), 1, 400);

            return [order1, order2];
        }
    }
}
