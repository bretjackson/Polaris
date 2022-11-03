using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public DialogueTree dialogue;
    public DialogueManager dialogueManager;

    void Pickup()
    {
        InventoryManager.instance.Add(item);
        Destroy(gameObject);

        // trigger dialogue
        dialogueManager.StartDialogue(dialogue);
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}
