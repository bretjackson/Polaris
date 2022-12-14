using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CutSceneDialogueManager : MonoBehaviour
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

    public bool intro; // slighlty different logic for ending scene needed
    private TextMeshProUGUI dialogueUIText; // needed for ending scene only

    private void Start() 
    {
        StartIntroDialogue(dialogue);
        child1DialogueUIText.text = "";
        child2DialogueUIText.text = "";
        audio = GetComponent<AudioSource>();
        if (!intro)
        {
            dialogueUIText = child1DialogueUIText;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (!intro)
        {
            if (Input.GetKeyDown("space")) 
            {
                AdvanceIntroSentence();
            }
        }
    }

    public void StartIntroDialogue(DialogueTree dialogueTree)
    {
        dialogue = dialogueTree;
        currentSentence = dialogue.startingSentence;
        dialogueCanvas.enabled = true;
        DisplayIntroSentence();
    }

    public void DisplayIntroSentence()
    {
        if (currentSentence == null)
        {
            EndIntroDialogue();
            return;
        }
        if (intro) 
        {
            HideOptions();
        }
        string sentence = currentSentence.text;
        StopAllCoroutines();
        StartCoroutine(TypeIntroSentence(sentence, currentSentence.charId));
    }

    public void AdvanceIntroSentence()
    {
        if(dialogueUIText != null && dialogueUIText.text == currentSentence.text) {
            currentSentence = currentSentence.nextSentence;
            DisplayIntroSentence();
        }
        else {
            StopAllCoroutines();
            dialogueUIText.text = currentSentence.text;
        }
    }

    IEnumerator TypeIntroSentence(string sentence, int charId)
    {
        child1DialogueUIText.text = "";
        child2DialogueUIText.text = "";
        TextMeshProUGUI dialogueUIText = GetCorrectUIText(charId);
        foreach(char letter in sentence.ToCharArray()){
            dialogueUIText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        if (intro)
        {
            DisplayOptions();
        }
    }

    TextMeshProUGUI GetCorrectUIText(int id) 
    { 
        /* Returns the UI text for child 1 or 2 based on id. */
        if (id == 1) {
            return child1DialogueUIText;
        }
        else {
            return child2DialogueUIText;
        }
    }

    void DisplayOptions()
    {
        if (currentSentence.options.Count <= optionsUI.Length){
            for (int i=0; i < currentSentence.options.Count; i++){
                optionsUI[i].text = currentSentence.options[i].text;
                optionsUI[i].transform.parent.gameObject.SetActive(true);
            }
        }
        optionPanel.SetActive(true);
    }

    void HideOptions()
    {
        foreach(TextMeshProUGUI option in optionsUI){
            option.transform.parent.gameObject.SetActive(false);
        }
        optionPanel.SetActive(false);
    }

    public void OptionOnClick(int index)
    {
        Choice option = currentSentence.options[index];
        if (option.onOptionSelected != null){
            option.onOptionSelected.Raise();
        }
        currentSentence = option.nextSentence;
        DisplayIntroSentence();
    }

    void EndIntroDialogue()
    {
        if (intro) 
        {
            StartCoroutine(LoadLevelAfterDelay(2, "MainScene"));
        } 
        else
        {
            GameObject instructionPanel = FindGameObjectInChildWithTag("Instructions");
            instructionPanel.SetActive(true);
        }
    }

    public GameObject FindGameObjectInChildWithTag (string tag)
    {
        Transform t = gameObject.transform;
        for (int i = 0; i < t.childCount; i++) 
        {
            if(t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).gameObject;
            }
                 
        }
        return null;
    }

    public void TriggerNewLevel() // used for in ending scenes
    {
        GameObject blackImage = FindGameObjectInChildWithTag("BlackOut");
        blackImage.SetActive(true);
        StartCoroutine(LoadLevelAfterDelay(5, "EndingScene2"));
    }

    IEnumerator LoadLevelAfterDelay(float delay, string sceneName)
    {
        StartCoroutine(FadeToBlack(delay));
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeToBlack(float delay)
    {  
        yield return new WaitForSeconds(delay / 2);
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