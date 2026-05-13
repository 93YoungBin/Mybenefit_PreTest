public struct MoneyAddedEvent
{
    public int Amount;
}

public struct MoneyChangedEvent
{
    public int Amount;
}

public struct ProductPurchasedEvent
{
    public Product Product;
}

public struct BeverageConsumedEvent
{
    public Product Product;
}

public struct ProductSoldOutEvent
{
    public Product Product;
}

public struct InactiveAccessEvent { }

public struct SystemNotificationEvent
{
    public string Message;
}
