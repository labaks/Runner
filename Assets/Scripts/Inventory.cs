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
    public GameObject[] rightHandItems;
    public Texture2D gunCursor;
    public Text coins;

    int coinCount = 0;
    GameObject tmpItem;
    void Start()
    {
        Cursor.SetCursor(gunCursor, Vector3.zero, CursorMode.ForceSoftware);
        if (inventory.Length > 0)
        {
            currentItem = inventory[0];
            FillInventory();
        }
        coins.text = coinCount.ToString();
    }
    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0 && inventory.Length != 0)
        {
            int currentItemIndex = SearchItemIndexByName(currentItem.gameObject.name);
            int nextItemIndex = currentItemIndex - 1 < 0 ? inventory.Length - 1 : currentItemIndex - 1;
            ChangeCurrentItem(inventory[nextItemIndex]);
        }
        else if (Input.mouseScrollDelta.y < 0 && inventory.Length != 0)
        {
            int currentItemIndex = SearchItemIndexByName(currentItem.gameObject.name);
            int nextItemIndex = currentItemIndex + 1 > inventory.Length - 1 ? 0 : currentItemIndex + 1;
            ChangeCurrentItem(inventory[nextItemIndex]);
        }
    }

    private void OnTriggerEnter(Collider item)
    {
        Item triggered = item.GetComponent<Item>();
        if (triggered.type == "coin")
        {
            AddCoin(triggered.price, triggered);
        }
        else
        {
            Collect(triggered);
        }
    }

    public void FillInventory()
    {
        for (var i = 0; i < inventory.Length; i++)
        {
            itemsWrappers[i].transform.GetComponent<Image>().sprite = inventory[i].icon;
        }
    }

    void AddCoin(int coinsCollected, Item coin) {
        coinCount += coinsCollected;
        coins.text = coinCount.ToString();
        Destroy(coin.gameObject);
    }

    void Collect(Item item)
    {
        if (inventory.Length <= inventorySize)
        {
            tmpItem = item.prefab;
            inventory = inventory.Append(tmpItem.GetComponent<Item>());
            ChangeCurrentItem(inventory[inventory.Length - 1]);
            // Destroy(item.gameObject);
            item.gameObject.SetActive(false);
            FillInventory();
        }
    }

    void ChangeCurrentItem(Item item)
    {
        foreach (GameObject rightHandItem in rightHandItems)
        {
            rightHandItem.SetActive(false);
        }
        HaveGunSwitcher(false);
        currentItem = item;
        switch (item.name)
        {
            case "Gun":
                HaveGunSwitcher(true);
                rightHand.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Knife":
                rightHand.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case "Baseballbeat":
                rightHand.transform.GetChild(2).gameObject.SetActive(true);
                break;
            default: break;
        }
    }

    void HaveGunSwitcher(bool haveGun)
    {
        gameObject.GetComponent<Attacking>().haveGun = haveGun;
    }

    int SearchItemIndexByName(string itemName)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (itemName == inventory[i].gameObject.name) return i;
        }
        return -1;
    }
}

public static class Extensions
{
    public static T[] Append<T>(this T[] array, T item)
    {
        return new List<T>(array) { item }.ToArray();
    }
}
