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

public struct InactiveAccessEvent { }
