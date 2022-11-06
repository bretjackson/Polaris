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

    public float characterHeight = 4.11f;

    public Animator animator;

    // void Start(){
    //     animator = GetComponent<animator>();
    // }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    void Move(){
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        float gravity = 0f;

        // if the character is not grounded, use raycasting to bring them to ground 
        
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        RaycastHit hit;
        if (!characterController.isGrounded
            & Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask)) {
            gravity = hit.distance - characterHeight;
        }

        Vector3 movement = transform.forward*verticalMove + transform.right*horizontalMove + new Vector3(0, gravity, 0);
        characterController.Move(movement*speed*Time.deltaTime);
        if (movement.sqrMagnitude > 1E-3f){ // I think this means she walks with roatation
            animator.SetBool("isWalking", true); //possible to refactor in so only have to get isWalking once
        }
        else{
            animator.SetBool("isWalking", false);
        }
    }

    void Rotate(){
        float horizontalRot = Input.GetAxis("Mouse X");
        float verticalRot = Input.GetAxis("Mouse Y");

        transform.Rotate(0, horizontalRot*mouseSensitivity, 0);
        cameraTransform.Rotate(-verticalRot*mouseSensitivity, 0, 0);

        Vector3 currentRotation = cameraTransform.localEulerAngles;
        if (currentRotation.x > 180){
            currentRotation.x -= 360;
        }
        currentRotation.x  = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
        cameraTransform.localRotation = Quaternion.Euler(currentRotation);
    }
}
