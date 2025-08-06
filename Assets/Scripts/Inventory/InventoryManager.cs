using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public int maxSize;

    AlertManager alertManager;
    void Start()
    {
        alertManager = FindObjectOfType<AlertManager>();
    }
    public void AddItem(Item i)
    {
        //TODO: Chequeos previos
        if (inventory.Count >= maxSize)
        {
            alertManager.DisplayDangerAlert("Inventario lleno.");
            return;
        }

        inventory.Add(i);
        alertManager.DisplaySuccessAlert("Objeto añadido", 1f);

    }
}
