using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

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

    private IDataLoader _dataLoader;
    private VendingMachine _machine;
    private ObjectPool<BeverageView> _beveragePool;

    private void Awake()
    {
        _dataLoader = new DataLoader();
    }

    private void Start()
    {
        StartCoroutine(_dataLoader.Co_Load(OnLoadSuccess, OnLoadFail));
    }

    private void OnDestroy()
    {
        if (_machine == null)
        {
            return;
        }

        _machine.OnMoneyChanged -= UpdateMoneyText;
        _machine.OnProductPurchased -= OnProductPurchased;
        _machine.OnInactiveAccess -= OnInactiveAccess;
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

        SubscribeEvents();

        InitTopPanel();

        InitProductList();
    }

    private void OnLoadFail(string error)
    {
        Debug.LogError($"[VendingMachineController] {error}");
    }

    private void SubscribeEvents()
    {
        _machine.OnMoneyChanged += UpdateMoneyText;
        _machine.OnProductPurchased += OnProductPurchased;
        _machine.OnInactiveAccess += OnInactiveAccess;
    }

    private void InitTopPanel()
    {
        machineIdText.text = _machine.MachineId;
        currentMoneyText.text = $"{_machine.CurrentMoney:N0} won";

        powerLight.color = _machine.Status == MachineStatus.Active
            ? new Color32(0, 255, 0, 255)
            : new Color32(255, 0, 0, 255);

        InitMoneyButtons();
    }

    private void InitMoneyButtons()
    {
        for (int i = 0; i < moneyButtons.Length; i++)
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
            view.Bind(product, OnProductClicked);
        }
    }

    private void OnProductClicked(Product product)
    {
        _machine.Purchase(product);
    }

    private void UpdateMoneyText(int money)
    {
        currentMoneyText.text = $"{money:N0} won";
    }

    private void OnProductPurchased(Product product)
    {
        BeverageView view = _beveragePool.Get();
        view.Bind(product, OnBeverageClicked);
    }

    private void OnBeverageClicked(BeverageView view, Product product)
    {
        _beveragePool.Release(view);
    }

    private void OnInactiveAccess()
    {
        
    }
}