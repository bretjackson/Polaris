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
        InventoryManager.instance.ListItems();
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
        dialogueManager.StartDialogue(dialogue);
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}
