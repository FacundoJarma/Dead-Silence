using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    // Start is called before the first frame update
    public HealthManager healthManager;
    public TextMeshProUGUI healthText;
    void Start()
    {
        healthManager.onHealthChanged += UpdateHealthBar;
    }
    void UpdateHealthBar(int currentHealth)
    {
        healthText.text = currentHealth.ToString();
    }
}
