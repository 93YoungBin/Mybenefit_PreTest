using System;
using System.Collections.Generic;

public enum MachineStatus
{
    Active,
    Inactive
}

public class VendingMachine
{
    public string MachineId { get; }
    public MachineStatus Status { get; }
    public int CurrentMoney { get; private set; }
    public IReadOnlyList<Product> Products { get; }

    private const int MaxMoney = 10000;

    public VendingMachine(MachineData data)
    {
        MachineId = data.machineId;

        Status = data.status == "active" ? MachineStatus.Active : MachineStatus.Inactive;

        CurrentMoney = 0;

        List<Product> products = new List<Product>();

        foreach (ProductData productData in data.products)
        {
            products.Add(new Product(productData));
        }

        Products = products.AsReadOnly();
    }

    public bool AddMoney(int amount)
    {
        if (Status == MachineStatus.Inactive)
        {
            EventBus.Publish(new InactiveAccessEvent());
            return false;
        }

        if (CurrentMoney >= MaxMoney)
        {
            EventBus.Publish(new SystemNotificationEvent { Message = "Max Money" });
            return false;
        }

        CurrentMoney = Math.Min(CurrentMoney + amount, MaxMoney);

        EventBus.Publish(new MoneyAddedEvent { Amount = CurrentMoney });
        EventBus.Publish(new MoneyChangedEvent { Amount = CurrentMoney });

        return true;
    }

    public bool Purchase(Product product)
    {
        if (Status == MachineStatus.Inactive)
        {
            EventBus.Publish(new InactiveAccessEvent());
            return false;
        }

        if (!product.IsInStock)
        {
            EventBus.Publish(new SystemNotificationEvent { Message = $"SOLD OUT : {product.Name}" });
            return false;
        }

        if (CurrentMoney < product.Price)
        {
            EventBus.Publish(new SystemNotificationEvent { Message = "Need More Money." });
            return false;
        }

        CurrentMoney -= product.Price;

        product.DeductStock();

        if (!product.IsInStock)
        {
            EventBus.Publish(new ProductSoldOutEvent { Product = product });
        }

        EventBus.Publish(new ProductPurchasedEvent { Product = product });
        EventBus.Publish(new MoneyChangedEvent { Amount = CurrentMoney });

        return true;
    }
}
