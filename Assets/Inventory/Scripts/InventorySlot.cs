using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem currentItem;
    public SlotTag currentTag;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (Inventory.carriedItem == null) return;
        if (currentTag != SlotTag.None && Inventory.carriedItem.currentItem.itemTag != currentTag) return;
        SetItem(Inventory.carriedItem);
    }

    public void SetItem(InventoryItem item)
    {
        // Reset slots
        Inventory.carriedItem = null;
        item.activeSlot.currentItem = null;

        // Set current slot
        currentItem = item;
        currentItem.activeSlot = this;
        currentItem.transform.SetParent(transform);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.canvasGroup.blocksRaycasts = true;

        // Changing the sprite's size to fit in the slot
        currentItem.FitImage();

        if (currentTag != SlotTag.None)
        {
            Inventory.Instance.EquipEquipment(currentTag, currentItem);
        }
    }
}
