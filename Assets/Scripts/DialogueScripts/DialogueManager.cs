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
    public DialogueTree beginningDialogue;

    private List<int> conditionals = null;
    private bool doNotAdvance = false; // stop advancing sentences once conditional has been found

    public bool shedReached;
    public bool bushesReached;

    void Start() {
        instructionManager.StartInstructions("Press F to turn on flashlight.", "f");
        firstDialogue = true;
        shedReached = false;
        bushesReached = false;
    }

    void Update() {
        if (firstDialogue & instructionManager.currentInstructionKey == null) { // after flashlight on, begin dialogue
            StartDialogue(beginningDialogue);
        }
        if (dialogueUIText.text != "" && dialogueUIText.text != null && Input.GetKeyDown("space")) {
            AdvanceSentence();
        }
    }

    public void AddConditionals(List<int> items) {
        // conditionals = current inventory and locations visited
        conditionals = items;
        // 0 = been to the shed, -1 = been to the bushes
        if (shedReached) {
            conditionals.Add(0);
        }
        if (bushesReached) {
            conditionals.Add(-1);
        }
    }

    private bool ContainsAllConditionals() {
        // checks if all conditionals needed are in currentSentence ids 
        bool containsAll = true;
        List<int> curr = currentSentence.getIds(); 
        foreach(int c in conditionals) {
            print(c);
        }
            foreach(int id in curr) {
                print(id);
                if (!conditionals.Contains(id)) {
                    containsAll = false;
                }
            }
            return containsAll;
    }

    public void StartDialogue(DialogueTree dialogueTree){
        doNotAdvance = false;
        dialogue = dialogueTree;
        currentSentence = dialogue.startingSentence;
        dialogueCanvas.enabled = true;
        DisplaySentence();
        if(conditionals != null) {
            if(!ContainsAllConditionals()) {
                AdvanceSentence();
            }
        }
    }

    public void AdvanceSentence(){
        if(dialogueUIText != null && dialogueUIText.text == currentSentence.text) {
            currentSentence = currentSentence.nextSentence;
            DisplaySentence();
        }
        else {
            if(dialogueUIText.text != "") {
                StopAllCoroutines();
                dialogueUIText.text = currentSentence.text;
            }
        }
        while (dialogueUIText.text == ""){
            currentSentence = currentSentence.nextSentence;
            DisplaySentence();
        }
    }

    public void DisplaySentence(){
        if (currentSentence == null | doNotAdvance){
            EndDialogue();
            return;
        }
        string sentence = "";
        if(conditionals == null) {
            sentence = currentSentence.text;
        }
        else {
            if(ContainsAllConditionals()) { // correct conditional dialogue has been found
                sentence = currentSentence.text;
                doNotAdvance = true;
            }
        }
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
