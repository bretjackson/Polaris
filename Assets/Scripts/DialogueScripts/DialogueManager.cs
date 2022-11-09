using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueUIText;
    //public GameObject continueButton;
    public Canvas dialogueCanvas;
    // public GameObject optionPanel;
    // public TextMeshProUGUI[] optionsUI;
    public TextMeshProUGUI instructionsUIText;

    private DialogueTree dialogue;
    private Sentence currentSentence = null;
    private bool firstDialogue = true;
    private string currentInstruction = null;

    void Start() {
        dialogueUIText.text = null;
        DisplayInstructions("Press F to turn on flashlight.");
    }

    void Update() {
        if (dialogueCanvas.enabled == true & Input.GetKeyDown("space")) {
            AdvanceSentence();
        }
        if (instructionsUIText.text == "Press F to turn on flashlight." & Input.GetKeyDown("f")) {
            EndDialogue();
        }
    }

    public void DisplayInstructions(string command){
        if (command == "Press Space to continue.") {
            firstDialogue = false;
        }
        currentInstruction = command;
        dialogueCanvas.enabled = true;
        instructionsUIText.text = currentInstruction;
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
            DisplayInstructions("Press Space to continue.");
        }
    }

    IEnumerator TypeSentence(string sentence){
        dialogueUIText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueUIText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        //continueButton.SetActive(true);
        // if (currentSentence.HasOptions()){
        //     DisplayOptions();
        // }
        // else{
        //     continueButton.SetActive(true);
        // }
    }

    // void DisplayOptions(){
    //     if (currentSentence.options.Count <= optionsUI.Length){
    //         for (int i=0; i < currentSentence.options.Count; i++){
    //             optionsUI[i].text = currentSentence.options[i].text;
    //             optionsUI[i].transform.parent.gameObject.SetActive(true);
    //         }
    //     }
    //     optionPanel.SetActive(true);
    // }

    // void HideOptions(){
    //     continueButton.SetActive(false);
    //     foreach(TextMeshProUGUI option in optionsUI){
    //         option.transform.parent.gameObject.SetActive(false);
    //     }
    //     optionPanel.SetActive(false);
    // }

    // public void OptionOnClick(int index){
    //     Choice option = currentSentence.options[index];
    //     if (option.onOptionSelected != null){
    //         option.onOptionSelected.Raise();
    //     }
    //     currentSentence = option.nextSentence;
    //     DisplaySentence();
    // }

    void EndDialogue(){
        dialogueCanvas.enabled = false;
        instructionsUIText.text = null;
    }
}
