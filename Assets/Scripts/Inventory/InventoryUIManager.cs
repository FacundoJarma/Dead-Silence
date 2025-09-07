using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryUI;
    public InventoryManager inventoryManager;

    public GameObject InventoySlot;
    public GameObject[] InventorySlots;

    // Mantiene referencia a los slots instanciados
    private List<GameObject> instantiatedSlots = new List<GameObject>();

    void Start()
    {
        inventoryManager.onInventoryFocusChanged += UpdateUIFocused;
        inventoryManager.onInventoryChanged += UpdateUI;
    }

    public void UpdateUI()
    {
        // Limpia los slots anteriores
        foreach (Transform child in inventoryUI.transform)
        {
            Destroy(child.gameObject);
        }
        instantiatedSlots.Clear();

        // Crea nuevos slots y guarda referencias
        for (int i = 0; i < inventoryManager.inventory.Count; i++)
        {
            GameObject slot = Instantiate(InventoySlot, inventoryUI.transform);
            instantiatedSlots.Add(slot);

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

    public void UpdateUIFocused(float focus)
    {
        // Convierte el focus a índice entero
        int focusInt = Mathf.RoundToInt(focus);

        for (int i = 0; i < instantiatedSlots.Count; i++)
        {
            Transform bgTransform = instantiatedSlots[i].transform.Find("InventorySlot Background");
            if (bgTransform != null)
            {
                bgTransform.gameObject.SetActive(i == focusInt);
            }
        }
    }

}
