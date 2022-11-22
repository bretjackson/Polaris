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
    private bool firstDialogue = true;

    private List<int> conditionals = null;

    void Start() {
        // dialogueUIText.text = null;
        instructionManager.StartInstructions("Press F to turn on flashlight.", "f");
    }

    void Update() {
        if (dialogueCanvas.enabled == true & Input.GetKeyDown("space")) {
            AdvanceSentence();
        }
    }

    public void AddConditionals(List<int> items) {
        conditionals = items;
    }

    public void StartDialogue(DialogueTree dialogueTree){
        dialogue = dialogueTree;
        currentSentence = dialogue.startingSentence;
        // if(conditionals == null) {
        //     dialogueCanvas.enabled = true;
        //     DisplaySentence();
        // }
        //else {
            //bool isEqual = conditionals.OrderBy(x => x).SequenceEqual(currentSentence.getIds().OrderBy(x => x));
            if(IsEqual(conditionals, currentSentence.getIds())) {
                Debug.Log(String.Format("StartIsEqual"));
                Debug.Log(String.Format("Con: " + conditionals[0] + conditionals[1]));
                Debug.Log(String.Format("Sen: " + currentSentence.getIds()[0] + currentSentence.getIds()[1]));
                dialogueCanvas.enabled = true;
                DisplaySentence();
            }
            else{
                AdvanceSentence();
                Debug.Log(String.Format("Start"));
            }
        //}
    }

    public void AdvanceSentence(){
        if (dialogueUIText.text == currentSentence.text){
            Sentence nextSentence = currentSentence.nextSentence;
            // if(conditionals == null){
            //     DisplaySentence();
            // }
            //else {
                if(IsEqual(conditionals, nextSentence.getIds())) {
                    Debug.Log(String.Format("AdvanceIsEqual"));
                    currentSentence = nextSentence;
                    DisplaySentence();
                }
                else {
                    Debug.Log(String.Format("Advance"));
                    Debug.Log(String.Format("Con: " + conditionals[0] + conditionals[1]));
                    Debug.Log(String.Format("Sen: " + currentSentence.getIds()[0] + currentSentence.getIds()[1]));
                    AdvanceSentence();
                }
            //}
        }
        else {
            StopAllCoroutines();
            dialogueUIText.text = currentSentence.text;
            // currentSentence = null;
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
        string sentence = currentSentence.text;
        //dialogueUIText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if (firstDialogue) {
            instructionManager.StartInstructions("Press Space to continue.", "space");
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
    }
}
