using System.Collections;
using UnityEngine;

public class DataLoaderTest : MonoBehaviour
{
    private IDataLoader _loader = new DataLoader();

    private void Start()
    {
        StartCoroutine(_loader.Co_Load(OnSuccess, OnFail));
    }

    private void OnSuccess(MachineData data)
    {
        Debug.Log($"ID : {data.machineId}");
        Debug.Log($"Statuse : {data.status}");
        Debug.Log($"Count : {data.products.Count}");

        foreach (ProductData productData in data.products)
        {
            Product product = new Product(productData);

            Debug.Log($"[{product.Id}] {product.Name} / {product.Price} / {product.Stock} / {product.IsInStock}");
        }
    }

    private void OnFail(string error)
    {
        Debug.LogError($"Fail :  {error}");
    }
}