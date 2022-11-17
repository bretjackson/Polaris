using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;

public class UseItem : MonoBehaviour
{
    public string itemName;
    public string itemId;
    private PlayableDirector director;

    public DialogueManager dialogueManager;
    public InventoryManager invManager;
    
    private bool itemUsed = false;
    private bool instructionsPresent = false;

    private int id;
    private List<int> ids = new List<int>();
    private bool multipleIds = false;

    void Awake() 
    {
        director = GetComponent<PlayableDirector>();
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
            dialogueManager.EndDialogue();
            instructionsPresent = false;
        }
    }

    void OnTriggerEnter()
    {
        // check if inventory manager contains all ids listed
        bool invManContainsAll = false;
        if (!multipleIds) { // id used, not ids
            print("one id");
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

        if (!itemUsed & invManContainsAll)
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
