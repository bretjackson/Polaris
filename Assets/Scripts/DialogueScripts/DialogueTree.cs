using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewDialogue", menuName ="Dialogue/Dialogue Tree")]
public class DialogueTree : ScriptableObject
{
    public Sentence startingSentence;
    public List<int> requiredIds; // conditionals
}
