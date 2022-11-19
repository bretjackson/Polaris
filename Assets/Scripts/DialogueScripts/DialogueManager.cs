using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueUIText;
    public Canvas dialogueCanvas;
    // public TextMeshProUGUI instructionsUIText;
    public InstructionManager instructionManager;

    private DialogueTree dialogue;
    private Sentence currentSentence = null;
    private bool firstDialogue = true;
    // private string currentInstruction = null;

    void Start() {
        // dialogueUIText.text = null;
        instructionManager.StartInstructions("Press F to turn on flashlight.", "f");
    }

    void Update() {
        if (dialogueCanvas.enabled == true & Input.GetKeyDown("space")) {
            AdvanceSentence();
        }
        // if (instructionsUIText.text == "Press F to turn on flashlight." & Input.GetKeyDown("f")) {
        //     EndDialogue();
        // }
    }

    public void StartDialogue(DialogueTree dialogueTree){
        dialogue = dialogueTree;
        currentSentence = dialogue.startingSentence;
        dialogueCanvas.enabled = true;
        DisplaySentence();
    }

    public void AdvanceSentence(){
        if (dialogueUIText.text == currentSentence.text){
            currentSentence = currentSentence.nextSentence;
            DisplaySentence();
        }
        else {
            StopAllCoroutines();
            dialogueUIText.text = currentSentence.text;
            // currentSentence = null;
        }
    }

    public void DisplaySentence(){
        if (currentSentence == null){
            EndDialogue();
            return;
        }
        // HideOptions();
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
        // instructionsUIText.text = null;
        dialogueUIText.text = null;
    }
}
