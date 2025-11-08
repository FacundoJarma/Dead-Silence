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
    NotesManager notesManager;
    Lantern lantern;

    [Header("Prefabs para dropear")]
    public GameObject waterBottlePrefab;
    public GameObject mousePrefab;
    public GameObject note1Prefab;
    public GameObject note2Prefab;
    public GameObject note3Prefab;
    public GameObject note4Prefab;
    public GameObject lanternPrefab;

    [Header("Sonidos")]
    AudioSource audioSource;
    public AudioClip dropSound;
    public AudioClip drinkSound; // 👈 Nuevo sonido para tomar agua

    void Start()
    {
        alertManager = FindObjectOfType<AlertManager>();
        playerHealthManager = FindObjectOfType<HealthManager>();
        throwObject = FindObjectOfType<ThrowObject>();
        notesManager = FindObjectOfType<NotesManager>();
        lantern = FindObjectOfType<Lantern>();
        audioSource = GetComponent<AudioSource>();
        onInventoryFocusChanged?.Invoke(0);
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
            actualFocus--;
        else if (scroll < 0f)
            actualFocus++;

        if (actualFocus < 0)
            actualFocus = inventory.Count != 0 ? inventory.Count - 1 : 0;
        else if (actualFocus >= inventory.Count)
            actualFocus = 0;

        if (actualFocus != prevFocus)
        {
            lantern.TurnOff();
            onInventoryFocusChanged?.Invoke(actualFocus);
            prevFocus = actualFocus;
        }

        if (Input.GetMouseButtonDown(1) && inventory.Count > 0)
        {
            Item selectedItem = inventory[actualFocus];
            switch (selectedItem.itemName)
            {
                case "Water Bottle":
                    playerHealthManager.Heal(10);
                    PlayDrinkSound(); // 👈 Reproduce el sonido de beber
                    break;

                case "Bottle":
                    throwObject.Throw("Bottle");
                    break;

                case "Note#1":
                    notesManager.openOrClose(0);
                    break;

                case "Note#2":
                    notesManager.openOrClose(1);
                    break;

                case "Note#3":
                    notesManager.openOrClose(2);
                    break;

                case "Note#4":
                    notesManager.openOrClose(3);
                    break;

                case "Lantern":
                    lantern.Turn();
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

        if (Input.GetKeyDown(KeyCode.Q) && inventory.Count > 0)
        {
            RemoveAndDropItem();
            PlayDropSound();
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

    public void RemoveAndDropItem()
    {
        if (inventory.Count == 0) return;

        Item selectedItem = inventory[actualFocus];
        GameObject prefabToSpawn = null;

        switch (selectedItem.itemName)
        {
            case "Water Bottle":
                prefabToSpawn = waterBottlePrefab;
                break;
            case "Bottle":
                prefabToSpawn = mousePrefab;
                break;
            case "Note#1":
                notesManager.Close();
                prefabToSpawn = note1Prefab;
                break;
            case "Note#2":
                notesManager.Close();

                prefabToSpawn = note2Prefab;
                break;
            case "Note#3":
                notesManager.Close();

                prefabToSpawn = note3Prefab;
                break;
            case "Note#4":
                notesManager.Close();

                prefabToSpawn = note4Prefab;
                break;
            case "Lantern":
                prefabToSpawn = lanternPrefab;
                break;
        }

        if (prefabToSpawn != null)
        {
            Vector3 dropPosition = transform.position + transform.forward * 1.2f + Vector3.up * 0.2f;
            Instantiate(prefabToSpawn, dropPosition, Quaternion.identity);
        }

        inventory.RemoveAt(actualFocus);
        onInventoryChanged?.Invoke();

        if (inventory.Count == 0)
        {
            actualFocus = 0;
        }
        else
        {
            actualFocus %= inventory.Count;
        }

        onInventoryFocusChanged?.Invoke(actualFocus);
    }

    void PlayDropSound()
    {
        if (audioSource != null && dropSound != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(dropSound);
        }
    }

    void PlayDrinkSound() // 👈 Nueva función
    {
        if (audioSource != null && drinkSound != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(drinkSound);
        }
    }
}
