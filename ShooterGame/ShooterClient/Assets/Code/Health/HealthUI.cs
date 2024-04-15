using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image _hpBar;

    public void UpdateHealth(float max, int current)
    {
        _hpBar.fillAmount = current / max;
    }
}