using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionManager : MonoBehaviour
{

    public TextMeshProUGUI instructionsUIText;
    public Canvas instructionalCanvas;

    private string currentInstruction = null;
    private string currentInstructionKey = null;

    // Update is called once per frame
    void Update()
    {
        // ignore if space because we don't want canvas to be disabled
        if (currentInstructionKey == "space") {
                return;
        }
        // if they've pressed the instructional key, instructions go away
        if (currentInstruction != null) {
            if (Input.GetKeyDown(currentInstructionKey)) {
                EndInstructions();
            }
        }
    }

    public void StartInstructions(string instructions, string key) 
    {
        currentInstructionKey = key;
        currentInstruction = instructions;
        DisplayInstructions();
    }

    void DisplayInstructions()
    {
        if (currentInstruction == null){
            EndInstructions();
            return;
        }
        instructionalCanvas.enabled = true;
        instructionsUIText.text = currentInstruction;
    }

    public void EndInstructions()
    {
        instructionalCanvas.enabled = false;
        currentInstruction = null;
        currentInstructionKey = null;
    }
}
