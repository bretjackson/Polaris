using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<int> conditionals = null;

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

    public void AddConditionals(List<int> items) {
        conditionals = items;
    }

    public void StartDialogue(DialogueTree dialogueTree){
        dialogue = dialogueTree;
        currentSentence = dialogue.startingSentence;
        if(conditionals == null) {
            dialogueCanvas.enabled = true;
            DisplaySentence();
        }
        else {
            bool isEqual = conditionals.OrderBy(x => x).SequenceEqual(currentSentence.getIds().OrderBy(x => x));
            if(isEqual) {
                dialogueCanvas.enabled = true;
                DisplaySentence();
            }
            else{
                AdvanceSentence();
            }
        }
    }

    public void AdvanceSentence(){
        if (dialogueUIText.text == currentSentence.text){
            if(conditionals == null){
                currentSentence = currentSentence.nextSentence;
                DisplaySentence();
            }
            else {
                bool isEqual = conditionals.OrderBy(x => x).SequenceEqual(dialogue.startingSentence.getIds().OrderBy(x => x));
                if(isEqual) {
                    currentSentence = currentSentence.nextSentence;
                    DisplaySentence();
                }
                else {
                    AdvanceSentence();
                }
            }
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
