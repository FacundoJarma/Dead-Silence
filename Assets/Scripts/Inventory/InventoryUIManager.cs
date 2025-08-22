using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryUI;

    public InventoryManager inventoryManager;
    public GameObject InventoySlot;


    void Start()
    {
        inventoryManager.onInventoryChanged += UpdateUI;
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
