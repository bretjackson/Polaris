using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UseItem : MonoBehaviour
{
    public string itemName;
    public int itemId;
    private PlayableDirector director;

    public DialogueManager dialogueManager;
    public InventoryManager invManager;
    
    private bool itemUsed = false;
    private bool instructionsPresent = false;

    void Awake() 
    {
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (instructionsPresent & Input.GetKeyDown("e"))
        {
            itemUsed = true;
            Use();
            dialogueManager.EndDialogue();
            instructionsPresent = false;
        }
    }

    void OnTriggerEnter()
    {
        if (!itemUsed & invManager.Contains(itemId))
        {
            dialogueManager.DisplayInstructions("Press E to use " + itemName.ToLower() + ".");
            instructionsPresent = true;
        }
    }

    void OnTriggerExit()
    {
        dialogueManager.EndDialogue();
        instructionsPresent = false;
    }

    void Use()
    {
        // trigger the event for that item
        director.Play();
    }
}
