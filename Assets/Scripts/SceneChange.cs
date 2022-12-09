using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay;
    public string NewLevel;
    public bool triggered; // true if triggered, false if timed

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
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(NewLevel);
    }
}
