using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeverageView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;

    private Action<BeverageView, Product> _onClickCallback;

    public void Bind(Product product, Action<BeverageView, Product> onClickCallback)
    {
        _onClickCallback = onClickCallback;

        nameText.text = product.Name;

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => _onClickCallback?.Invoke(this, product));
    }
}