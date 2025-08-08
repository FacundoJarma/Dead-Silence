using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            Item item = inventoryManager.inventory[i];

            Transform iconTransform = slot.transform.Find("InventorySlot Image"); 
            if (iconTransform != null)
            {
                Image sr = iconTransform.GetComponent<Image>();
                if (sr != null)
                {
                    sr.sprite = item.icon;
                }
            }
        }
    }
}
