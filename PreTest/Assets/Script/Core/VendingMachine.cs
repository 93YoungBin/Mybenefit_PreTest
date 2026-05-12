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

    public event Action<int> OnMoneyChanged;
    public event Action<Product> OnProductPurchased;
    public event Action OnInactiveAccess;

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
            OnInactiveAccess?.Invoke();
            return false;
        }

        CurrentMoney = Math.Min(CurrentMoney + amount, MaxMoney);

        OnMoneyChanged?.Invoke(CurrentMoney);

        return true;
    }

    public bool Purchase(Product product)
    {
        if (Status == MachineStatus.Inactive)
        {
            OnInactiveAccess?.Invoke();
            return false;
        }

        if (!product.IsInStock)
        {
            return false;
        }

        if (CurrentMoney < product.Price)
        {
            return false;
        }

        CurrentMoney -= product.Price;

        product.DeductStock();

        OnProductPurchased?.Invoke(product);

        OnMoneyChanged?.Invoke(CurrentMoney);

        return true;
    }
}