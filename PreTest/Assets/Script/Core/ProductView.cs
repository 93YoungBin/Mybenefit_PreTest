using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI stockText;

    private Action<Product> _onClickCallback;
    private Product _product;

    public void Bind(Product product, Action<Product> onClickCallback)
    {
        if (_product != null)
        {
            _product.OnStockChanged -= UpdateDisplay;
            _product.OnOutOfStock -= ShowOutOfStock;
        }

        _product = product;
        _onClickCallback = onClickCallback;

        _product.OnStockChanged += UpdateDisplay;
        _product.OnOutOfStock += ShowOutOfStock;

        UpdateDisplay();

        if (!_product.IsInStock)
        { 
            ShowOutOfStock();
        }
    }

    private void UpdateDisplay()
    {
        nameText.text = _product.Name;
        priceText.text = $"{_product.Price:N0} Won";
        stockText.text = $"{_product.Stock} ea";
    }

    private void ShowOutOfStock()
    {
        // 품절 관련 이벤트 추후 처리
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onClickCallback?.Invoke(_product);
    }

    private void OnDestroy()
    {
        if (_product != null)
        {
            _product.OnStockChanged -= UpdateDisplay;
            _product.OnOutOfStock -= ShowOutOfStock;
        }
    }
}