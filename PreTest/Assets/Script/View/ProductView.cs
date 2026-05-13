using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI stockText;
    [SerializeField] private Image productImage;

    private Action<Product> _onClickCallback;
    private Product _product;
    private Coroutine _imageCoroutine;

    public void Bind(Product product, Action<Product> onClickCallback, IResourceLoader<Sprite> imageLoader = null)
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

        LoadImage(imageLoader);
    }

    private void UpdateDisplay()
    {
        nameText.text = _product.Name;
        priceText.text = $"{_product.Price:N0} Won";
        stockText.text = $"{_product.Stock} ea";
    }

    private void ShowOutOfStock()
    {
        //추후 품절 관련 처리 적용 예정
    }

    private void LoadImage(IResourceLoader<Sprite> imageLoader)
    {
        if (imageLoader == null || productImage == null)
        {
            return;
        }

        if (_imageCoroutine != null)
        {
            StopCoroutine(_imageCoroutine);
        }

        _imageCoroutine = StartCoroutine(imageLoader.Co_Load(_product.ImageUrl, OnImageLoaded, OnImageLoadFailed));
    }

    private void OnImageLoaded(Sprite sprite)
    {
        if (productImage != null)
        {
            productImage.sprite = sprite;
        }

        _imageCoroutine = null;
    }

    private void OnImageLoadFailed(string error)
    {
        Debug.Log(error);
        _imageCoroutine = null;
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
