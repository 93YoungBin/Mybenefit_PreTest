using UnityEngine;
using UnityEngine.UI;

public class LogController : MonoBehaviour
{
    [SerializeField] private Transform logContent;
    [SerializeField] private LogView logPrefab;
    [SerializeField] private ScrollRect scrollRect;

    private void OnEnable()
    {
        EventBus.Subscribe<MoneyAddedEvent>(OnMoneyAdded);
        EventBus.Subscribe<ProductPurchasedEvent>(OnProductPurchased);
        EventBus.Subscribe<ProductSoldOutEvent>(OnProductSoldOut);
        EventBus.Subscribe<BeverageConsumedEvent>(OnBeverageConsumed);
        EventBus.Subscribe<InactiveAccessEvent>(OnInactiveAccess);
        EventBus.Subscribe<SystemNotificationEvent>(OnSystemNotification);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<MoneyAddedEvent>(OnMoneyAdded);
        EventBus.Unsubscribe<ProductPurchasedEvent>(OnProductPurchased);
        EventBus.Unsubscribe<ProductSoldOutEvent>(OnProductSoldOut);
        EventBus.Unsubscribe<BeverageConsumedEvent>(OnBeverageConsumed);
        EventBus.Unsubscribe<InactiveAccessEvent>(OnInactiveAccess);
        EventBus.Unsubscribe<SystemNotificationEvent>(OnSystemNotification);
    }

    private void OnMoneyAdded(MoneyAddedEvent e)
    {
        AddLog($"Money : {e.Amount:N0} Won");
    }

    private void OnProductPurchased(ProductPurchasedEvent e)
    {
        AddLog($"ADD : {e.Product.Name}");
    }

    private void OnProductSoldOut(ProductSoldOutEvent e)
    {
        AddLog($"SOLD OUT : {e.Product.Name}");
    }

    private void OnBeverageConsumed(BeverageConsumedEvent e)
    {
        AddLog($"CONSUME : {e.Product.Name}");
    }

    private void OnInactiveAccess(InactiveAccessEvent e)
    {
        AddLog("Can't use Machine");
    }

    private void OnSystemNotification(SystemNotificationEvent e)
    {
        AddLog(e.Message);
    }

    private void AddLog(string message)
    {
        LogView log = Instantiate(logPrefab, logContent);

        log.transform.SetAsFirstSibling();
        log.Bind(message);

        LayoutRebuilder.ForceRebuildLayoutImmediate(logContent as RectTransform);
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
