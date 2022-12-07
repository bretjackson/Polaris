using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{   
    public TextMeshProUGUI dialogueUIText;
    public Canvas dialogueCanvas;

    public InstructionManager instructionManager;

    private DialogueTree dialogue;
    private Sentence currentSentence = null;

    private bool firstDialogue;
    public List<DialogueTree> beginningDialogue;

    private List<int> conditionals = null;

    public bool shedReached;
    public bool bushesReached;

    void Start() {
        instructionManager.StartInstructions("Press F to turn on flashlight.", "f");
        firstDialogue = true;
        shedReached = false;
        bushesReached = false;
    }

    void Update() {
        if (firstDialogue & instructionManager.currentInstructionKey == null) { // after flashlight turned on, begin dialogue
            StartDialogue(beginningDialogue);
        }
        if (dialogueUIText.text != "" && dialogueUIText.text != null && Input.GetKeyDown("space")) { // TODO: do we need all here still?
            AdvanceSentence();
        }
    }

    public void AddConditionals(List<int> items) {
        /* Sets conditionals as player's current inventory and locations visited 
        Called from where the dialogue is triggered from, using the inventory manager. */
        conditionals = items;

        // 0 = visited shed, -1 = visited bushes
        if (shedReached) {
            conditionals.Add(0);
        }
        if (bushesReached) {
            conditionals.Add(-1);
        }
    }

    private bool ContainsAllConditionals(DialogueTree dTree) {
        /* Returns true if conditionals contains all the ids listed in dTree, false otherwise. */
        bool containsAll = true;
        List<int> reqIDs = dTree.requiredIds;
        foreach(int id in reqIDs) {
            if (!conditionals.Contains(id)) {
                containsAll = false;
            }
        }
        return containsAll;
    }

    public void StartDialogue(List<DialogueTree> dialogueTrees)
    {
        DialogueTree dialogueTree = dialogueTrees[dialogueTrees.Count-1]; // defaults to last dialogueTree
        foreach(DialogueTree d in dialogueTrees) {
            if (ContainsAllConditionals(d)) {
                dialogueTree = d;
                break;
            }
        }
        dialogue = dialogueTree;
        currentSentence = dialogue.startingSentence;
        dialogueCanvas.enabled = true;
        DisplaySentence();
    }

    public void AdvanceSentence(){
        if(dialogueUIText != null && dialogueUIText.text == currentSentence.text) {
            currentSentence = currentSentence.nextSentence;
            DisplaySentence();
        }
        else {
            StopAllCoroutines();
            dialogueUIText.text = currentSentence.text;
        }
    }

    public void DisplaySentence(){
        if (currentSentence == null){
            EndDialogue();
            return;
        }
        string sentence = "";
        sentence = currentSentence.text;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if (instructionManager.currentInstructionKey == null) {
            instructionManager.StartInstructions("Press Space to continue.", "space");
        }
        if (firstDialogue && currentSentence != null) {
            firstDialogue = false;
        }
    }

    IEnumerator TypeSentence(string sentence){
        dialogueUIText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueUIText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void EndDialogue(){
        dialogueCanvas.enabled = false;
        currentSentence = null;
        dialogueUIText.text = null;
        conditionals = null;
    }
}
