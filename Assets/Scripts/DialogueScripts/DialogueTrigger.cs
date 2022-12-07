using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<DialogueTree> dTrees;
    public DialogueManager dialogueManager;

    public InventoryManager invManager;

    public void CheckConditionals() 
    {
        /* Gets current inventory ids from inventory manager and puts those into dialogue manager. 
        Then, triggers dialogue. */
        List<int> invIds = invManager.GetIds();
        dialogueManager.AddConditionals(invIds);
        dialogueManager.StartDialogue(dTrees);
    }

    public void OnTriggerEnter(Collider other)
    {
        CheckConditionals();
    }

    public void OnTriggerExit(Collider other)
    {
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
    }
}
