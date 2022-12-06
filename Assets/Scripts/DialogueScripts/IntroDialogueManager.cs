
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroDialogueManager : MonoBehaviour
{
    public TextMeshProUGUI child1DialogueUIText;
    public TextMeshProUGUI child2DialogueUIText;
    public Canvas dialogueCanvas;

    public GameObject optionPanel;
    public TextMeshProUGUI[] optionsUI;

    public DialogueTree dialogue;
    private Sentence currentSentence = null;

    private void Start() {
        StartIntroDialogue(dialogue);
        child1DialogueUIText.text = "";
        child2DialogueUIText.text = "";
    }

    private void StartIntroDialogue(DialogueTree dialogueTree){
        dialogue = dialogueTree;
        currentSentence = dialogue.startingSentence;
        print(currentSentence.text);
        dialogueCanvas.enabled = true;
        DisplayIntroSentence();
    }

    public void DisplayIntroSentence(){
        if (currentSentence == null){
            EndIntroDialogue();
            return;
        }
        HideOptions();
        string sentence = currentSentence.text;
        // child1DialogueUIText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeIntroSentence(sentence, currentSentence.charId));
    }

    IEnumerator TypeIntroSentence(string sentence, int charId){
        TextMeshProUGUI dialogueUIText = GetCorrectUIText(charId);
        dialogueUIText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueUIText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        DisplayOptions();
    }

    TextMeshProUGUI GetCorrectUIText(int id) {
        if (id ==1 ) {
            return child1DialogueUIText;
        }
        else {
            return child2DialogueUIText;
        }
    }

    void DisplayOptions(){
        if (currentSentence.options.Count <= optionsUI.Length){
            for (int i=0; i < currentSentence.options.Count; i++){
                optionsUI[i].text = currentSentence.options[i].text;
                optionsUI[i].transform.parent.gameObject.SetActive(true);
            }
        }
        optionPanel.SetActive(true);
    }

    void HideOptions(){
        foreach(TextMeshProUGUI option in optionsUI){
            option.transform.parent.gameObject.SetActive(false);
        }
        optionPanel.SetActive(false);
    }

    public void OptionOnClick(int index){
        print("clicked!");
        Choice option = currentSentence.options[index];
        if (option.onOptionSelected != null){
            option.onOptionSelected.Raise();
        }
        currentSentence = option.nextSentence;
        DisplayIntroSentence();
    }

    void EndIntroDialogue(){
        dialogueCanvas.enabled = false;
    }
}