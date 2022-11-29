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

    //public ConditionalsManager conManager;

    private bool itemUsed = false;
    private bool instructionsPresent = false;

    private int id;
    private List<int> ids = new List<int>();
    private bool multipleIds = false;

    void Awake() 
    {
        director = gameObject.GetComponent<PlayableDirector>();
        // change itemId string to list or int
        if (itemId.Contains(",")) {
            multipleIds = true;
            List<string> idsStr = itemId.Split(',').ToList();
            foreach(string idStr in idsStr) {
                ids.Add(int.Parse(idStr));
            }
        }
        else {
            id = int.Parse(itemId);
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
        // check if inventory manager contains all ids listed
        bool invManContainsAll = false;
        if (!multipleIds) { // id used, not ids
            if (invManager.Contains(id)) {
                invManContainsAll = true;
            }
        } else { // list of ids used
            invManContainsAll = true;
            foreach(int item in ids) {
                if (!invManager.Contains(item)) {
                    invManContainsAll = false;
                }
            }
        }

        //conManager.CheckConditionals();
        CheckConditionals();

        if (!itemUsed & invManContainsAll)
        {
            instructionManager.StartInstructions("Press E to use " + itemName.ToLower() + " to " + actionDescription.ToLower() + ".", "e");
            instructionsPresent = true;
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
        Animator animator = gameObject.GetComponent<Animator>();
        if (animator != null) {
            animator.enabled = true;
        }
        // trigger the event for that item
        director.Play();
        dialogueManager.EndDialogue();
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
    }
}
