using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{   

    private new AudioSource audio;

    public List<DialogueTree> dTrees;
    public DialogueManager dialogueManager;


    // Start is called before the first frame update
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    

    public void OnTriggerEnter(Collider other)
    {   
        audio.Play();

        //CancelInvoke("EventOnEnd"); //in case previously invoked
        Invoke(nameof(PlayDialogue), 3); //execute on clip finished
    }

    void PlayDialogue()
    {
        dialogueManager.StartDialogue(dTrees);
    }

    public void OnTriggerExit(Collider other)
    {   
        GameObject parentObject = gameObject.transform.parent.gameObject;
        GameObject child1 = parentObject.transform.GetChild(0).gameObject;
        Collider collider1 = child1.GetComponent<Collider>();
        collider1.enabled = false;

        GameObject child2 = parentObject.transform.GetChild(1).gameObject;
        Collider collider2 = child2.GetComponent<Collider>();
        collider2.enabled = false;
    }   
    
}
