using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject rightHand;
    public Item[] inventory;
    public int inventorySize = 5;
    public GameObject[] itemsWrappers;
    private Item currentItem;
    public float collectRadius = 0.5f;
    public LayerMask itemsLayers;
    public GameObject[] rightHandItems;
    void Start()
    {
        if (inventory.Length > 0)
        {
            currentItem = inventory[0];
            FillInventory();
        }
    }

    private void OnTriggerEnter(Collider item)
    {
        Collect(item.GetComponent<Item>());
    }

    public void FillInventory()
    {
        for (var i = 0; i < inventory.Length; i++)
        {
            itemsWrappers[i].transform.GetComponent<Image>().sprite = inventory[i].icon;
        }
    }

    void Collect(Item item)
    {
        if (inventory.Length <= inventorySize)
        {
            inventory = inventory.Append(item);
            ChangeCurrentItem(item);
            Destroy(item.gameObject);
            FillInventory();
        }
    }

    void ChangeCurrentItem(Item item)
    {
        foreach (GameObject rightHandItem in rightHandItems)
        {
            rightHandItem.SetActive(false);
        }
        Debug.Log(item.name);
        switch (item.name)
        {
            case "Gun":
                gameObject.GetComponent<Attacking>().haveGun = true;
                rightHand.transform.GetChild(0).gameObject.SetActive(true);
                break;
            default: break;
        }
    }
}

public static class Extensions
{
    public static T[] Append<T>(this T[] array, T item)
    {
        return new List<T>(array) { item }.ToArray();
    }
}
