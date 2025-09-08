using UnityEngine;
using TMPro;

public class PowerTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI powerText;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPowerChanged += UpdateText;
            UpdateText(GameManager.Instance.Power); 
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPowerChanged -= UpdateText;
    }

    private void UpdateText(int newPower)
    {
        powerText.text = $"Our power: {newPower}";
    }
}
