using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    Light myLight;

    // Start is called before the first frame update
    void Start()
    {
       myLight = (Light)this.GetComponent("Light");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("f")){
            myLight.enabled = !myLight.enabled;
        }
    }
}
