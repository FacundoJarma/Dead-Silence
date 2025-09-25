using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public int maxSize;

    int actualFocus = 0;
    int prevFocus = -1;

    public delegate void InventoryChanged();
    public event InventoryChanged onInventoryChanged;

    public delegate void InventoryFocusChanged(float focus);
    public event InventoryFocusChanged onInventoryFocusChanged;

    AlertManager alertManager;
    HealthManager playerHealthManager;
    ThrowObject throwObject;

    void Start()
    {
        alertManager = FindObjectOfType<AlertManager>();
        playerHealthManager = FindObjectOfType<HealthManager>();
        throwObject = FindObjectOfType<ThrowObject>();

        onInventoryFocusChanged?.Invoke(0);
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // scroll arriba
            actualFocus--;
        else if (scroll < 0f) // scroll abajo
            actualFocus++;

        // Wrap-around (vuelve al inicio o al final)
        if (actualFocus < 0)
            actualFocus = inventory.Count != 0 ? inventory.Count - 1 : 0;
        else if (actualFocus >= inventory.Count)
            actualFocus = 0;

        if (actualFocus != prevFocus)
        {
            onInventoryFocusChanged?.Invoke(actualFocus);
            prevFocus = actualFocus;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Item selectedItem = inventory[actualFocus];
            switch (selectedItem.itemName)
            {
                case "Water Bottle":
                    playerHealthManager.Heal(10);
                    break;
                case "Mouse":
                    throwObject.Throw("Mouse");
                    break;
            }
            if (selectedItem.isConsumible)
            {
                inventory.RemoveAt(actualFocus);
                onInventoryChanged?.Invoke();

                actualFocus = 0;
                onInventoryFocusChanged?.Invoke(actualFocus);
            }
        }

    }

    public void AddItem(Item i)
    {
        if (inventory.Count >= maxSize)
        {
            alertManager.DisplayDangerAlert("Inventario lleno.");
            return;
        }

        inventory.Add(i);
        FindObjectOfType<PlayerSFX>().PlaySound("PickUp");
        alertManager.DisplaySuccessAlert("Objeto añadido", 1f);
        onInventoryChanged?.Invoke();
    }
}
