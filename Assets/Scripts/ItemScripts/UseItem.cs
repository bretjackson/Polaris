using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UseItem : MonoBehaviour
{
    public string itemName;
    // public GameEvent event; // the animation or change to scene that results from using this item
    private PlayableDirector director;

    public DialogueManager dialogueManager;
    public InventoryManager invManager;
    
    private bool itemUsed = false;
    private bool instructionsPresent = false;

    void Awake() 
    {
        director = GetComponent<PlayableDirector>();
        
        director.Pause();
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
        // probably also trigger dialogue here eventually
        if (!itemUsed & invManager.Contains(2)) // screwdriver = id 2
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
        print("Using item: " + itemName);
        director.Play();
    }
}
