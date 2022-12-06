using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroDialogueManager : MonoBehaviour
{
    public TextMeshProUGUI child1DialogueUIText;
    public TextMeshProUGUI child2DialogueUIText;
    public Canvas dialogueCanvas;
    public GameObject blackOutSquare;

    public GameObject optionPanel;
    public TextMeshProUGUI[] optionsUI;

    public DialogueTree dialogue;
    private Sentence currentSentence = null;

    private new AudioSource audio;

    private void Start() {
        StartIntroDialogue(dialogue);
        child1DialogueUIText.text = "";
        child2DialogueUIText.text = "";
        //campfire noises ?
        audio = GetComponent<AudioSource>();
        audio.Play();
    }

    private void StartIntroDialogue(DialogueTree dialogueTree){
        dialogue = dialogueTree;
        currentSentence = dialogue.startingSentence;
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
        child1DialogueUIText.text = "";
        child2DialogueUIText.text = "";
        TextMeshProUGUI dialogueUIText = GetCorrectUIText(charId);
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
        Choice option = currentSentence.options[index];
        if (option.onOptionSelected != null){
            option.onOptionSelected.Raise();
        }
        currentSentence = option.nextSentence;
        DisplayIntroSentence();
    }

    void EndIntroDialogue(){
        dialogueCanvas.enabled = false;
        StartCoroutine(LoadLevelAfterDelay(2));
    }

     IEnumerator LoadLevelAfterDelay(float delay)
     {
        StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainScene");
     }

     IEnumerator FadeToBlack()
     {  
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;
        int fadeSpeed = 1;
        while (objectColor.a < 255) {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
     }
}