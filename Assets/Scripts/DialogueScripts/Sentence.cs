using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName ="NewSentence", menuName ="Dialogue/Sentence")]
public class Sentence : ScriptableObject
{
    [TextArea(3, 10)]
    public string text = "text";
    public Sentence nextSentence;
    public string itemId;

    public List<Choice> options = new List<Choice>();

    public bool HasOptions(){
        if (options.Count == 0){
            return false;
        }
        else{
            return true;
        }
    }

    public List<int> getIds(){
        List<int> ids = new List<int>();
        if(itemId == "0") {
            return ids;
        }
        if (itemId.Contains(",")) {
            List<string> idsStr = itemId.Split(',').ToList();
            foreach(string idStr in idsStr) {
                ids.Add(int.Parse(idStr));
            }
        }
        else{
            ids.Add(int.Parse(itemId));
        }
        return ids;
    }
}

[System.Serializable]
public class Choice{
    [TextArea(3, 10)]
    public string text;
    public Sentence nextSentence;
    public GameEvent onOptionSelected;
}
