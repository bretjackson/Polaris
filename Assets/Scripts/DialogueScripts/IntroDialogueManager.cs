
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroDialogueManager : MonoBehaviour
{
    public TextMeshProUGUI girlDialogueUIText;
    public TextMeshProUGUI child1DialogueUIText;
    public TextMeshProUGUI child2DialogueUIText;
    public Canvas dialogueCanvas;

    public GameObject continueButton;
    public GameObject optionPanel;
    public TextMeshProUGUI[] optionsUI;

    public DialogueTree dialogue;
    private Sentence currentSentence = null;

    // private void Start() {
    //     StartIntroDialogue(dialogue);
    // }

    // private void StartIntroDialogue(DialogueTree dialogueTree){
    //     dialogue = dialogueTree;
    //     currentSentence = dialogue.startingSentence;
    //     dialogueCanvas.enabled = true;
    //     DisplayIntroSentence();
    // }

    // public void AdvanceIntroSentence(){
    //     currentSentence = currentSentence.nextSentence;
    //     DisplayIntroSentence();
    // }

    // public void DisplayIntroSentence(){
    //     if (currentSentence == null){
    //         EndIntroDialogue();
    //         return;
    //     }
    //     HideOptions();
    //     string sentence = currentSentence.text;
    //     //dialogueUIText.text = sentence;
    //     StopAllCoroutines();
    //     StartCoroutine(TypeIntroSentence(sentence));
    // }

    // IEnumerator TypeIntroSentence(string sentence){
    //     dialogueUIText.text = "";
    //     foreach(char letter in sentence.ToCharArray()){
    //         dialogueUIText.text += letter;
    //         yield return new WaitForSeconds(0.05f);
    //     }

    //     if (currentSentence.HasOptions()){
    //         DisplayOptions();
    //     }
    //     else{
    //         continueButton.SetActive(true);
    //     }
    // }

//     void DisplayOptions(){
//         if (currentSentence.options.Count <= optionsUI.Length){
//             for (int i=0; i < currentSentence.options.Count; i++){
//                 optionsUI[i].text = currentSentence.options[i].text;
//                 optionsUI[i].transform.parent.gameObject.SetActive(true);
//             }
//         }
//         optionPanel.SetActive(true);
//     }

//     void HideOptions(){
//         continueButton.SetActive(false);
//         foreach(TextMeshProUGUI option in optionsUI){
//             option.transform.parent.gameObject.SetActive(false);
//         }
//         optionPanel.SetActive(false);
//     }

//     public void OptionOnClick(int index){
//         Choice option = currentSentence.options[index];
//         if (option.onOptionSelected != null){
//             option.onOptionSelected.Raise();
//         }
//         currentSentence = option.nextSentence;
//         DisplaySentence();
//     }

//     void EndIntroDialogue(){
//         dialogueCanvas.enabled = false;
//     }
}