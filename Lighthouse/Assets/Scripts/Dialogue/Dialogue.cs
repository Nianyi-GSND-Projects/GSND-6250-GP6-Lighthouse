using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialogue", menuName = "Game/Dialogue Asset")]
public class DialogueAsset : ScriptableObject
{

    [System.Serializable]
    public struct Line
    {
        [TextArea(3, 5)]
        public string text; 

        [Min(1)]
        public float duration;
    }

    public Line[] lines;
}