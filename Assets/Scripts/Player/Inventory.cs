using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject rightHand;
    public Transform inventoryBag;
    public Item[] inventory;
    public int inventorySize = 5;
    public GameObject[] itemsWrappers;
    private Item currentItem;
    public Texture2D gunCursor;
    public Text coins;

    int coinCount = 0;
    GameObject tmpItem;
    private Color32 currentItemcolor = new Color32(29, 27, 137, 255);
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
        if (Input.mouseScrollDelta.y > 0 && inventory.Length > 1)
        {
            int currentItemIndex = SearchItemIndexByName(currentItem.itemName);
            int nextItemIndex = currentItemIndex - 1 < 0 ? inventory.Length - 1 : currentItemIndex - 1;
            ChangeCurrentItem(inventory[nextItemIndex]);
        }
        else if (Input.mouseScrollDelta.y < 0 && inventory.Length > 1)
        {
            int currentItemIndex = SearchItemIndexByName(currentItem.itemName);
            int nextItemIndex = currentItemIndex + 1 > inventory.Length - 1 ? 0 : currentItemIndex + 1;
            ChangeCurrentItem(inventory[nextItemIndex]);
        }
    }

    private void OnTriggerEnter(Collider triggerCollider)
    {
        Item trigger = triggerCollider.GetComponent<Item>();
        if (trigger.type == "coin")
        {
            AddCoin(trigger.price, trigger);
        }
        else
        {
            Collect(trigger);
        }
    }

    public void FillInventory()
    {
        for (var i = 0; i < inventory.Length; i++)
        {
            itemsWrappers[i].transform.GetComponent<Image>().sprite = inventory[i].icon;
        }
    }

    void AddCoin(int coinsCollected, Item coin)
    {
        coinCount += coinsCollected;
        coins.text = coinCount.ToString();
        Destroy(coin.gameObject);
    }

    void Collect(Item item)
    {
        if (inventory.Length <= inventorySize)
        {
            tmpItem = Instantiate(item.prefab, transform.position, Quaternion.identity, inventoryBag);
            tmpItem.gameObject.SetActive(false);
            inventory = inventory.Append(tmpItem.GetComponent<Item>());
            ChangeCurrentItem(inventory[inventory.Length - 1]);
            Transform model = item.transform.Find("Sphere").Find("model");
            Transform newItem = Instantiate(model, transform.position, Quaternion.identity, rightHand.transform);
            newItem.name = item.itemName;
            newItem.localPosition = item.inHandPosition;
            newItem.localRotation = Quaternion.Euler(0, 0, 0);
            Destroy(item.gameObject);
            FillInventory();
        }
    }

    void ChangeCurrentItem(Item item)
    {
        currentItem = item;
        for (int i = 1; i < rightHand.transform.childCount; i++)
        {
            if (rightHand.transform.GetChild(i).gameObject.name == currentItem.itemName)
            {
                rightHand.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                rightHand.transform.GetChild(i).gameObject.SetActive(false);
            }
            itemsWrappers[i-1].transform.parent.GetComponent<Image>().color = Color.white;
        }
        HaveGunSwitcher(currentItem.type == "gun");
        itemsWrappers[SearchItemIndexByName(currentItem.itemName)].transform.parent.GetComponent<Image>().color = currentItemcolor;
    }

    void HaveGunSwitcher(bool haveGun)
    {
        gameObject.GetComponent<Attacking>().haveGun = haveGun;
    }

    int SearchItemIndexByName(string itemName)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (itemName == inventory[i].itemName) return i;
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
