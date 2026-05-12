public class Product
{
    public int Id { get; }
    public string Name { get; }
    public string Type { get; }
    public int Price { get; }
    public int Stock { get; private set; }

    public bool IsInStock
    {
        get
        {
            return Stock > 0;
        }
    }

    internal Product(ProductData data)
    {
        Id = data.id;
        Name = data.name;
        Price = data.price;
        Stock = data.stock;
        Type = data.type;
    }

    internal void DeductStock()
    {
        if (Stock > 0)
        { 
            Stock--;
        }
    }
}