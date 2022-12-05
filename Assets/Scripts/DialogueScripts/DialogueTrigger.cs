using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueTree dialogue;
    public DialogueManager dialogueManager;

    public InventoryManager invManager;

    public void CheckConditionals() {
        List<int> invIds = invManager.GetIds();
        dialogueManager.AddConditionals(invIds);
        dialogueManager.StartDialogue(dialogue);
    }

    public void OnTriggerEnter(Collider other){
        CheckConditionals();
        //dialogueManager.StartDialogue(dialogue);
    }
    public void OnTriggerExit(Collider other){
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
    }
}
