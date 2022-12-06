using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public DialogueTree dialogue;
    public DialogueManager dialogueManager;

    private new AudioSource audio

    void Start()
    {
       audio = GetComponent<AudioSource>();
    }

    void Pickup()
    {
        InventoryManager.instance.Add(item);
        InventoryManager.instance.ListItems();
        Destroy(gameObject);
        audio.Play();

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
