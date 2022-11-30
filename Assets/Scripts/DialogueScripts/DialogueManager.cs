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
    public DialogueTree beginningDialogue;

    private DialogueTree dialogue;
    private Sentence currentSentence = null;
    private bool firstDialogue;

    private List<int> conditionals = null;
    private bool doNotAdvance = false; // stop advancing sentences once conditional has been found

    void Start() {
        // dialogueUIText.text = null;
        instructionManager.StartInstructions("Press F to turn on flashlight.", "f");
        firstDialogue = true;
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
        conditionals = items;
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
                // currentSentence = null;
            }
        }
        while (dialogueUIText.text == ""){
            currentSentence = currentSentence.nextSentence;
            DisplaySentence();
        }
    }

    // bool IsEqual(List<int> a, List<int> b) {
    //     if(a.Count != b.Count) {
    //         return false;
    //     }
    //     foreach(int i in a) {
    //         if(!b.Contains(i)) {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

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
        if (firstDialogue && currentSentence != null) {
            instructionManager.StartInstructions("Press Space to continue.", "space");
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

    private bool ContainsAllConditionals() {
        // checks if all conditionals needed are in currentSentence ids 
        bool containsAll = true;
        List<int> curr = currentSentence.getIds();
            foreach(int id in curr) {
                if (!conditionals.Contains(id)) {
                    containsAll = false;
                }
            }
            return containsAll;
    }
}
