using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;

public class UseItem : MonoBehaviour
{
    public string itemName;
    public string itemId;
    public string actionDescription;
    private PlayableDirector director;

    public InstructionManager instructionManager;
    public InventoryManager invManager;

    public DialogueTree dTree;
    public DialogueManager dialogueManager;

    private bool itemUsed = false;
    private bool instructionsPresent = false;

    private List<int> ids = new List<int>();

    void Awake() 
    {
        director = gameObject.GetComponent<PlayableDirector>();
        // set up required ids list needed to use this item
        List<string> idsStr = itemId.Split(',').ToList();
        foreach(string idStr in idsStr) {
            ids.Add(int.Parse(idStr));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (instructionsPresent & Input.GetKeyDown("e"))
        {
            itemUsed = true;
            Use();
            // dialogueManager.EndDialogue();
            instructionsPresent = false;
        }
    }

    void OnTriggerEnter()
    {
        // check if inventory manager contains all ids listed to use the item
        bool invManContainsAll = true;
        foreach(int item in ids) {
            if (!invManager.Contains(item)) {
                invManContainsAll = false;
            }
        }

        // trigger dialogue from dialogueManager depending on conditionals
        CheckConditionals();

        // if invManager contains necessary ids, allow ability to use item
        if (!itemUsed & invManContainsAll)
        {
            instructionManager.StartInstructions("Press E to use " + itemName.ToLower() + " to " + actionDescription.ToLower() + ".", "e");
            instructionsPresent = true;
        }

        // update location-based conditionals
        if (gameObject.tag == "Bushes") {
            dialogueManager.bushesReached = true;
        }
        if (gameObject.tag == "Shed") {
            dialogueManager.shedReached = true;
        }
        
    }
    
    void CheckConditionals() {
        List<int> invIds = invManager.GetIds();
        dialogueManager.AddConditionals(invIds);
        dialogueManager.StartDialogue(dTree);
    }

    void OnTriggerExit()
    {
        instructionManager.EndInstructions();
        instructionsPresent = false;
        dialogueManager.EndDialogue();
    }

    void Use()
    {   
        // trigger the animation for using this item
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator != null) {
            animator.enabled = true;
        }
        director.Play();

        dialogueManager.EndDialogue();
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
    }
}
