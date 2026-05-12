using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class LogController : MonoBehaviour
{
    [SerializeField] private Transform logContent;
    [SerializeField] private LogView logPrefab;
    [SerializeField] private ScrollRect scrollRect;

    private ObjectPool<LogView> _logPool;

    private void Awake()
    {
        _logPool = new ObjectPool<LogView>(
            createFunc: () => Instantiate(logPrefab, logContent),
            actionOnGet: log => log.gameObject.SetActive(true),
            actionOnRelease: log => log.gameObject.SetActive(false),
            actionOnDestroy: log => Destroy(log.gameObject),
            defaultCapacity: 10
        );
    }

    private void OnEnable()
    {
        EventBus.Subscribe<MoneyAddedEvent>(OnMoneyAdded);
        EventBus.Subscribe<ProductPurchasedEvent>(OnProductPurchased);
        EventBus.Subscribe<BeverageConsumedEvent>(OnBeverageConsumed);
        EventBus.Subscribe<InactiveAccessEvent>(OnInactiveAccess);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<MoneyAddedEvent>(OnMoneyAdded);
        EventBus.Unsubscribe<ProductPurchasedEvent>(OnProductPurchased);
        EventBus.Unsubscribe<BeverageConsumedEvent>(OnBeverageConsumed);
        EventBus.Unsubscribe<InactiveAccessEvent>(OnInactiveAccess);
    }

    private void OnMoneyAdded(MoneyAddedEvent e)
    {
        AddLog($"Money : {e.Amount:N0} Won");
    }

    private void OnProductPurchased(ProductPurchasedEvent e)
    {
        AddLog($"ADD : {e.Product.Name}");
    }

    private void OnBeverageConsumed(BeverageConsumedEvent e)
    {
        AddLog($"CONSUME : {e.Product.Name}");
    }

    private void OnInactiveAccess(InactiveAccessEvent e)
    {
        AddLog("Can't use Machine");
    }

    private void AddLog(string message)
    {
        LogView log = _logPool.Get();

        log.transform.SetAsFirstSibling();
        log.Bind(message);

        LayoutRebuilder.ForceRebuildLayoutImmediate(logContent as RectTransform);
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
