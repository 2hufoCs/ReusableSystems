using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    [SerializeField] private Item[] itemList = new Item[5];

    [SerializeField]

    public void Start()
    {
        // Spawning random objects in shop, DEBUG ONLY
        for (int i = 0; i < itemList.Length; i++)
        {
            int rnd = Random.Range(0, Inventory.Instance.items.Length);
            itemList[i] = Inventory.Instance.items[rnd];
        }
    }

    public void Buy()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event")
        .GetComponent<EventSystem>().currentSelectedGameObject;

        int coins = Inventory.Instance.Coins;
    }
}
