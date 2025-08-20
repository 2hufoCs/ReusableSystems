using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        giveItemButton.onClick.AddListener(delegate { SpawnInventoryItem(); });
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

    public void SpawnInventoryItem(Item item = null)
    {
        // If no item given, just spawn a random one (debug, remove once it works)
        if (item == null)
        {
            int random = Random.Range(0, items.Length);
            item = items[random];
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Skip until an empty slot is found
            if (inventorySlots[i].currentItem != null) continue;
            if (inventorySlots[i].currentTag != SlotTag.None) continue;
            InventoryItem newItem = Instantiate(itemPrefab, inventorySlots[i].transform);
            newItem.Initialize(item, inventorySlots[i]);
            break;
        }
    }

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
        Debug.Log(Coins);
    }


    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        // jsp ce que tu mets ici
    }
}
