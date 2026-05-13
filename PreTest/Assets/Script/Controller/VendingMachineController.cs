using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using System;

public class VendingMachineController : MonoBehaviour
{
    [Header("Top Panel")]
    [SerializeField] private TextMeshProUGUI machineIdText;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    [SerializeField] private Image powerLight;

    [Header("Product List")]
    [SerializeField] private Transform productListContent;
    [SerializeField] private ProductView productItemPrefab;

    [Header("Money Buttons")]
    [SerializeField] private Button[] moneyButtons;
    [SerializeField] private int[] moneyAmounts;

    [Header("Inventory")]
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private BeverageView beveragePrefab;

    [Header("Data")]
    [SerializeField] private string _jsonName = "Items";

    private IResourceLoader<MachineData> _dataLoader;
    private IResourceLoader<Sprite> _imageLoader;
    private VendingMachine _machine;
    private ObjectPool<BeverageView> _beveragePool;

    private void Awake()
    {
        _dataLoader = new DataLoader();
        _imageLoader = new ResourcesImageLoader();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<MoneyChangedEvent>(OnMoneyChanged);
        EventBus.Subscribe<ProductPurchasedEvent>(OnProductPurchased);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<MoneyChangedEvent>(OnMoneyChanged);
        EventBus.Unsubscribe<ProductPurchasedEvent>(OnProductPurchased);
    }

    private void Start()
    {
        StartCoroutine(_dataLoader.Co_Load(_jsonName, OnLoadSuccess, OnLoadFail));
    }

    private void OnLoadSuccess(MachineData data)
    {
        _machine = new VendingMachine(data);

        _beveragePool = new ObjectPool<BeverageView>(
            createFunc: () => Instantiate(beveragePrefab, inventoryContent),
            actionOnGet: view => view.gameObject.SetActive(true),
            actionOnRelease: view => view.gameObject.SetActive(false),
            actionOnDestroy: view => Destroy(view.gameObject),
            defaultCapacity: 5
        );

        InitTopPanel();
        InitMoneyButtons();
        InitProductList();
    }

    private void OnLoadFail(string error)
    {
        Debug.LogError($"VendingMachineController Load Fail : {error}");
    }

    private void InitTopPanel()
    {
        machineIdText.text = _machine.MachineId;

        currentMoneyText.text = $"money : {_machine.CurrentMoney:N0} won";

        powerLight.color = _machine.Status == MachineStatus.Active
            ? new Color32(0, 255, 0, 255)
            : new Color32(255, 0, 0, 255);
    }

    private void InitMoneyButtons()
    {
        int count = moneyButtons.Length;

        if (moneyButtons.Length != moneyAmounts.Length)
        {
            Debug.LogWarning($"MoneyButton Error : {moneyButtons.Length} / {moneyAmounts.Length}");

            count = Math.Min(moneyButtons.Length, moneyAmounts.Length);
        }

        for (int i = 0; i < count; i++)
        {
            int amount = moneyAmounts[i];

            moneyButtons[i].onClick.RemoveAllListeners();
            moneyButtons[i].onClick.AddListener(() => _machine.AddMoney(amount));
        }
    }

    private void InitProductList()
    {
        foreach (Product product in _machine.Products)
        {
            ProductView view = Instantiate(productItemPrefab, productListContent);

            view.Bind(product, OnProductClicked, _imageLoader);
        }
    }

    private void OnProductClicked(Product product)
    {
        _machine.Purchase(product);
    }

    private void OnMoneyChanged(MoneyChangedEvent e)
    {
        currentMoneyText.text = $"money : {e.Amount:N0} won";
    }

    private void OnProductPurchased(ProductPurchasedEvent e)
    {
        BeverageView view = _beveragePool.Get();

        view.Bind(e.Product, OnBeverageClicked);
    }

    private void OnBeverageClicked(BeverageView view, Product product)
    {
        EventBus.Publish(new BeverageConsumedEvent { Product = product });

        _beveragePool.Release(view);
    }
}
