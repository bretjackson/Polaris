using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;

public class UseItem : MonoBehaviour
{
    public string itemName;
    public string itemId;
    public string actionDescription; // "Press E to use [itemName] to [actionDescription]."
    private PlayableDirector director;

    public List<DialogueTree> dTrees;
    public DialogueManager dialogueManager;
    public InstructionManager instructionManager;
    public InventoryManager invManager;

    private new AudioSource audio;

    private bool itemUsed = false;
    private bool instructionsPresent = false;

    private List<int> ids = new List<int>();

    void Awake() 
    {
        director = gameObject.GetComponent<PlayableDirector>();
        // set up required ids list needed to use this item
        // TODO: change this to be a list input
        List<string> idsStr = itemId.Split(',').ToList();
        foreach(string idStr in idsStr) {
            ids.Add(int.Parse(idStr));
        }

        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (instructionsPresent & Input.GetKeyDown("e"))
        {
            itemUsed = true;
            Use();
            instructionsPresent = false;
        }
    }

    void OnTriggerEnter()
    {
        // trigger dialogue depending on conditionals
        CheckConditionals();

        // if invManager contains necessary ids, allow ability to use item
        if (!itemUsed & InvManagerContainsAll())
        {
            instructionManager.StartInstructions("Press E to use " + itemName.ToLower() + " to " + actionDescription.ToLower() + ".", "e");
            instructionsPresent = true;
        }

        // update location-based conditionals if needed
        if (gameObject.tag == "Bushes") {
            dialogueManager.bushesReached = true;
        }
        if (gameObject.tag == "Shed") {
            dialogueManager.shedReached = true;
        }
    }

    bool InvManagerContainsAll() 
    {
        bool invManContainsAll = true;
        foreach(int item in ids) {
            if (!invManager.Contains(item)) {
                invManContainsAll = false;
            }
        }
        return invManContainsAll;
    }
    
    void CheckConditionals() 
    {
        /* Gets current inventory ids from inventory manager and puts those into dialogue manager. 
        Then, triggers dialogue. */
        List<int> invIds = invManager.GetIds();
        dialogueManager.AddConditionals(invIds);
        dialogueManager.StartDialogue(dTrees);
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
        audio.Play();

        dialogueManager.EndDialogue();

        // get rid of collider once item has been used
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
    }
}
