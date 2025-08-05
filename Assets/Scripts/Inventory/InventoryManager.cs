using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();

    public void AddItem(Item i)
    {
        //TODO: Chequeos previos

        inventory.Add(i);
        Debug.Log("Item añadido correctamente!");
    }
}
