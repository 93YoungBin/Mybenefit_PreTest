using TMPro;
using UnityEngine;

public class LogView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    public void Bind(string message)
    {
        messageText.text = message;
    }
}