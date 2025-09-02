using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    // Start is called before the first frame update
    public HealthManager healthManager;
    public TextMeshProUGUI healthText;

    public RectTransform staminaBar;
    public RectTransform healthBar;
    public PlayerMovement playerMovement;
    void Start()
    {
        playerMovement.onStaminaChanged += UpdateStaminaBar;
        healthManager.onHealthChanged += UpdateHealthBar;
    }
    void UpdateHealthBar(int currentHealth)
    {
        healthBar.localScale = new Vector3(currentHealth / 100f, 1f, 1f);
    }
    void UpdateStaminaBar(float currentStamina)
    {
        staminaBar.localScale = new Vector3(currentStamina / playerMovement.maxStamina, 1f, 1f);
    }
}
