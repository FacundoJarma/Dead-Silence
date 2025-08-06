using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public int maxSize;

    public void AddItem(Item i)
    {
        //TODO: Chequeos previos
        if (inventory.Count >= maxSize)
        {
            Debug.Log("Inventario lleno. No se puede añadir el item.");
            return;
        }

        inventory.Add(i);
        Debug.Log("Item añadido correctamente!");
    }
}
