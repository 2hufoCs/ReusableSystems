using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Range(0, 1)] float slotRatio;
    [SerializeField] Image itemIcon;
    public CanvasGroup canvasGroup;

    public Item currentItem;
    public InventorySlot activeSlot;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
    }

    public void Initialize(Item item, InventorySlot parent)
    {
        activeSlot = parent;
        activeSlot.currentItem = this;
        currentItem = item;
        itemIcon.sprite = item.sprite;

        // Changing the sprite's size to fit in the slot
        FitImage();
    }

    public void FitImage()
    {
        itemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(activeSlot.GetComponent<RectTransform>().sizeDelta.x * slotRatio,
                                                               activeSlot.GetComponent<RectTransform>().sizeDelta.y * slotRatio);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.Instance.SetCarriedItem(this);
        }
    }
}
