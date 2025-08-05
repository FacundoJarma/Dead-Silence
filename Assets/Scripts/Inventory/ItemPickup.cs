using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item;  // Asignas en inspector el Item que representa

    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Interact()
    {
        InventoryManager inventory = player.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(item);
            Destroy(gameObject);
        }
    }
}
