using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay;
    public string NewLevel;
    public bool triggered; // true if triggered, false if timed
    public GameObject blackOutSquare;

    void Start()
    {
        if (!triggered)
        {
            StartCoroutine(LoadLevelAfterDelay(delay));
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        TriggerScene(1);
    }

    public void TriggerScene(float delay) {
        StartCoroutine(LoadLevelAfterDelay(delay));
    }
 
    IEnumerator LoadLevelAfterDelay(float delay)
    {
        if (blackOutSquare != null) 
        {
            blackOutSquare.SetActive(true);
            StartCoroutine(FadeToBlack(delay));
            StartCoroutine(FadeMusic(delay));
        }
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(NewLevel);
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

    IEnumerator FadeMusic(float delay)
    {
        float elapsedTime = 0;
        float currentVolume = AudioListener.volume;
 
        while(elapsedTime < delay) {
            elapsedTime += Time.deltaTime;
            AudioListener.volume = Mathf.Lerp(currentVolume, 0, elapsedTime / delay);
            yield return null;
        }
    }
}
