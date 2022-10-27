/*
Taken from Bret's in-class demo
https://github.com/bretjackson/Unity3DExample
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController characterController;
    public float speed = 6f;
    public float mouseSensitivity = 2f;

    public Transform cameraTransform;
    public float upLimit = -50;
    public float downLimit = 50;

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    void Move(){
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward*verticalMove + transform.right*horizontalMove;
        characterController.Move(movement*speed*Time.deltaTime);
    }

    void Rotate(){
        float horizontalRot = Input.GetAxis("Mouse X");
        float verticalRot = Input.GetAxis("Mouse Y");

        transform.Rotate(0, horizontalRot*mouseSensitivity, 0);
        cameraTransform.Rotate(verticalRot*mouseSensitivity, 0, 0);

        Vector3 currentRotation = cameraTransform.localEulerAngles;
        if (currentRotation.x > 180){
            currentRotation.x -= 360;
        }
        currentRotation.x  = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
        cameraTransform.localRotation = Quaternion.Euler(currentRotation);
    }
}
