using UnityEngine;
using TMPro;

public class CoinDIsplay : MonoBehaviour
{
    public TMP_Text coinText;

    void Start()
    {
        UpdateDisplay();
    }

    void OnEnable()
    {
        UpdateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (coinText != null && PlayerProgression.Instance != null)
            coinText.text = PlayerProgression.Instance.coins.ToString();
    }
}
