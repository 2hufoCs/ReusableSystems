using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Flags] public enum ItemsNames 
{
    Key = 1 << 0, 
    SleepingPills = 1 << 1, 
    Extinguisher = 1 << 2, 
    NailedBat = 1 << 3
}

public class Inventory : MonoBehaviour
{
    [Header("Items")]
    public static Inventory Instance;
    public static InventoryItem carriedItem;
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    public int Coins { get; private set; }
    [SerializeField] private TextMeshProUGUI coinsTxt;


    public Item[] items;

    [Header("Debug")]
    [SerializeField] Button giveItemButton;
    [SerializeField] Button giveCoinButton;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(Instance);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        giveItemButton.onClick.AddListener(delegate { SpawnItem(); });
        giveCoinButton.onClick.AddListener(delegate { AddCoins(1); });
    }

    void Update()
    {
        if (carriedItem == null) return;

        // Makes the carried item follow the cursor
        // I don't understand how something so simple requires these weird-ass functions
        Canvas parentCanvas = transform.root.GetComponent<Canvas>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.GetComponent<RectTransform>(), Input.mousePosition,
            parentCanvas.worldCamera, out Vector2 movePos);

        carriedItem.transform.position = parentCanvas.transform.TransformPoint(movePos);
    }

    public void SpawnItem(ItemsNames itemName)
    {
        Item itemToSpawn = GetItemByName(itemName.ToString());

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Skip until an empty slot is found
            if (inventorySlots[i].inventoryItem != null) continue;
            if (inventorySlots[i].currentTag != SlotTag.None) continue;

            InventoryItem newItem = Instantiate(itemPrefab, inventorySlots[i].transform);
            newItem.Initialize(itemToSpawn, inventorySlots[i]);
            break;
        }
    }

    public void SpawnItem()
    {
        // Choose a random item
        int random = UnityEngine.Random.Range(0, items.Length);
        ItemsNames itemName = (ItemsNames)Enum.GetValues(typeof(ItemsNames)).GetValue(random);
        SpawnItem(itemName);
    }

    public void RemoveItem(ItemsNames itemName)
    {
        // Remove item
        InventoryItem itemToRemove = FindHeldItem(itemName);
        if (!itemToRemove) return;
        Destroy(itemToRemove.activeSlot.transform.GetChild(0).gameObject);
    }

    // Look for item in item list
    Item GetItemByName(string name)
    {
        foreach (Item item in items)
        {
            if (item.name == name) return item;
        }
        return null;
    }

    List<InventoryItem> GetHeldItems()
    {
        List<InventoryItem> heldItems = new();
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot) heldItems.Add(slot.inventoryItem);
        }
        return heldItems;
    }

    // Look for item in InventorySlots (held items)
    public InventoryItem FindHeldItem(ItemsNames itemName)
    {
        foreach (InventoryItem item in GetHeldItems())
        {
            if (item && item.currentItem.name == itemName.ToString()) return item;
        }
        //Debug.LogError($"wasn't able to find item named {itemName}!!");
        return null;
    }

    public bool FindHeldItems(ItemsNames itemNames)
    {
        foreach (ItemsNames itemName in Enum.GetValues(typeof(ItemsNames)))
        {
            if (!itemNames.HasFlag(itemName)) continue;
            if (!FindHeldItem(itemName)) return false;
        }
        return true;
    }

    // public bool HasHeldItems(ItemsNames[] itemNames)
    // {
    //     foreach (ItemsNames itemName in itemNames)
    //     {
    //         if (!FindHeldItem(itemName)) return false;
    //     }
    //     return true;
    // }

    public void SetCarriedItem(InventoryItem item)
    {
        if (carriedItem != null)
        {
            if (item.activeSlot.currentTag != SlotTag.None && item.activeSlot.currentTag != carriedItem.currentItem.itemTag) return;
            item.activeSlot.SetItem(carriedItem);
        }

        if (item.activeSlot.currentTag != SlotTag.None)
        {
            EquipEquipment(item.activeSlot.currentTag, null);
        }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }

    public void AddCoins(int coins)
    {
        Coins += coins;
        coinsTxt.text = "Coins: " + Coins.ToString();
    }


    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        // Useful to do things when equipping stuff
    }
}
