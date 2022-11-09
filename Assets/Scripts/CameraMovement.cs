using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform head;
    Transform cameraLocation;

    // Start is called before the first frame update
    void Start()
    {
        cameraLocation = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraLocation.position = head.position;
        // cameraLocation.rotation = head.rotation;
    }
}
