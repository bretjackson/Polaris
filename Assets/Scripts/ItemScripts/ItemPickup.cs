using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public List<DialogueTree> dTrees;
    public DialogueManager dialogueManager;
    public InventoryManager inventoryManager;

    public new AudioSource audio;

    void Pickup()
    {   
        inventoryManager.Add(item);
        inventoryManager.ListItems();
        Destroy(gameObject);
        
        // trigger dialogue
        if (dialogueManager != null) {
            //dialogueManager.StartDialogue(dialogue);
            CheckConditionals();
        }
    }

    public void CheckConditionals() 
    {
        /* Gets current inventory ids from inventory manager and puts those into dialogue manager. 
        Then, triggers dialogue. */
        List<int> invIds = InventoryManager.instance.GetIds();
        dialogueManager.AddConditionals(invIds);
        dialogueManager.StartDialogue(dTrees);
    }

    private void OnMouseDown()
    {
        audio.Play();
        Pickup();
    }
}
