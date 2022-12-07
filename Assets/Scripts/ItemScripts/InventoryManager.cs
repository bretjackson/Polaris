using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<Item> items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameObject Inventory;
    public GameObject InventoryButton;

    /* item ids: 1 = ax, 2 = screwdriver, 3 = star badge, 4 = detective badge, 
    5 = fauna badge, 6 = water, 7 = flashlight */

    private void Awake()
    {
        instance = this;
    }

    void Update() 
    {
        if (Input.GetKeyDown("tab")) {
            if (Inventory.activeInHierarchy == false) {
                Inventory.SetActive(true);
                InventoryButton.SetActive(false);
                ListItems();
            }
            else {
                Inventory.SetActive(false);
                InventoryButton.SetActive(true);
            }
        }
    }

    public void Add(Item item)
    {
        items.Add(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public void ListItems()
    {
        foreach(Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach(var item in items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            
            itemIcon.sprite = item.icon;
            itemName.text = item.itemName;
        }
    }

    public bool Contains(int itemId)
    {
        foreach(Item i in items) {
            if (i.id == itemId) {
                return true;
            }
        }
        return false;
    }

    public List<int> GetIds() 
    {
        List<int> idList = new List<int>();
        foreach(Item i in items) {
            idList.Add(i.id);
        }
        return idList;
    }
}
