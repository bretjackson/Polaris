using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public List<DialogueTree> dTrees;
    public DialogueManager dialogueManager;
    public InventoryManager inventoryManager;

    // private new AudioSource audio;

    // void Start()
    // {
    //    audio = GetComponent<AudioSource>();
    // }

    void Pickup()
    {
        inventoryManager.Add(item);
        inventoryManager.ListItems();
        // audio.Play();
        Destroy(gameObject);
        
        // trigger dialogue
        if (dialogueManager != null) {
            //dialogueManager.StartDialogue(dialogue);
            CheckConditionals();
        }
    }

    public void CheckConditionals() {
        List<int> invIds = InventoryManager.instance.GetIds();
        dialogueManager.AddConditionals(invIds);
        dialogueManager.StartDialogue(dTrees);
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}
