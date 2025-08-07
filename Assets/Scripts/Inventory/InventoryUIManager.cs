using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryUI;

    public InventoryManager inventoryManager;
    public GameObject InventoySlot;

    bool isOpen = false;

    void Start()
    {
        inventoryManager.onInventoryChanged += UpdateUI;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpen)
            {
                Close();
                isOpen = false;
            }
            else
            {
                Open();
                isOpen = true;
            }
        }
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
        foreach (Transform child in inventoryUI.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < inventoryManager.inventory.Count; i++)
        {
            GameObject slot = Instantiate(InventoySlot, inventoryUI.transform);
        }
    }
}
