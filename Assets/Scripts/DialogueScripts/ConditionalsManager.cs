using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionalsManager : MonoBehaviour
{
    public DialogueTree dTree;
    public DialogueManager dialogueManager;

    public InventoryManager invManager;

    public void CheckConditionals() {
        List<int> invIds = invManager.GetIds();
        dialogueManager.AddConditionals(invIds);
        dialogueManager.StartDialogue(dTree);
    }

    // public InventoryManager invManager;
    // public string itemName;
    // public string itemId;

    // public DialogueTree withItem;
    // public DialogueTree noItem;
    // public DialogueManager dialogueManager;

    // void OnTriggerEnter()
    // {
    //     // check if inventory manager contains all ids listed
    //     if (invManager.Contains(int.Parse(itemId))) {
    //         if (dialogueManager != null) {
    //             dialogueManager.StartDialogue(withItem);
    //         }
    //     }
    //     else {
    //         if(dialogueManager != null) {
    //             dialogueManager.StartDialogue(noItem);
    //         }
    //     }
    // }
}
