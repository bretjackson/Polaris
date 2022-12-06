using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    Light myLight;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
       myLight = (Light)this.GetComponent("Light");
       source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f")){
            myLight.enabled = !myLight.enabled;
            source.Play();
        }
    }
}
