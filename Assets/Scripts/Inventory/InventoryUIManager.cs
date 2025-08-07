using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryUI;

    public InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager.onInventoryChanged += UpdateUI;
    }


    public void Open()
    {
        inventoryUI.SetActive(true);
    }

    public void Close()
    {
        inventoryUI.SetActive(false);
    }

    public void UpdateUI()
    {

    }
}
