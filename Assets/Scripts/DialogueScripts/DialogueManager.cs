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
    private bool firstDialogue = true;

    private List<int> conditionals = null;

    void Start() {
        // dialogueUIText.text = null;
        instructionManager.StartInstructions("Press F to turn on flashlight.", "f");
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
        dialogue = dialogueTree;
        currentSentence = dialogue.startingSentence;
        dialogueCanvas.enabled = true;
        DisplaySentence();
        if(conditionals != null) {
            bool containsAll = true;
            foreach(int id in conditionals) {
                if (!currentSentence.getIds().Contains(id)) {
                    containsAll = false;
                }
            }
            if(!containsAll) {
                AdvanceSentence();
            }
        }
        // if(conditionals == null) {
        //     dialogueCanvas.enabled = true;
        //     DisplaySentence();
        // }
        // else {
        //     //bool isEqual = conditionals.OrderBy(x => x).SequenceEqual(currentSentence.getIds().OrderBy(x => x));
        //     if(IsEqual(conditionals, currentSentence.getIds())) {
        //         // Debug.Log(String.Format("StartIsEqual"));
        //         // Debug.Log(String.Format("Con: " + conditionals[0] + conditionals[1]));
        //         // Debug.Log(String.Format("Sen: " + currentSentence.getIds()[0] + currentSentence.getIds()[1]));
        //         dialogueCanvas.enabled = true;
        //         DisplaySentence();
        //     }
        //     else{
        //         AdvanceSentence();
        //         Debug.Log(String.Format("Start"));
        //     }
        // }
    }

    public void AdvanceSentence(){
        if(dialogueUIText.text == currentSentence.text) {
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
            //Debug.Log(String.Format("Advance: " + currentSentence.getIds()[0] + currentSentence.getIds()));
            DisplaySentence();
            // if(conditionals == null){
            //     DisplaySentence();
            // }
            //else {
                // if(IsEqual(conditionals, nextSentence.getIds())) {
                //     Debug.Log(String.Format("AdvanceIsEqual"));
                //     currentSentence = nextSentence;
                //     DisplaySentence();
                //     //currentSentence = null;
                // }
                // else {
                //     // Debug.Log(String.Format("Con: " + conditionals[0] + conditionals[1]));
                //     // Debug.Log(String.Format("Sen: " + currentSentence.getIds()[0] + currentSentence.getIds()[1]));
                //     if(nextSentence == null) {
                //         currentSentence = nextSentence;
                //         DisplaySentence();
                //     }
                //     AdvanceSentence();
                //     Debug.Log(String.Format("Advance: " + nextSentence.getIds()[0] + nextSentence.getIds()[1]));
                // }
            //}
        }
    }

    bool IsEqual(List<int> a, List<int> b) {
        if(a.Count != b.Count) {
            return false;
        }
        foreach(int i in a) {
            if(!b.Contains(i)) {
                return false;
            }
        }
        return true;
    }

    public void DisplaySentence(){
        if (currentSentence == null){
            EndDialogue();
            return;
        }
        string sentence = "";
        if(conditionals == null) {
            sentence = currentSentence.text;
        }
        else {
            if(IsEqual(conditionals, currentSentence.getIds())) {
                sentence = currentSentence.text;
            }
        }
        //dialogueUIText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if (firstDialogue && sentence != "") {
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
}
