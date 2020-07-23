using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiAnimDelay",menuName = "ScriptableObjects/StoryManager/Extensions/MultiAnimDelay",order = 3)]
public class MultiAnimDelay : ScriptableObject {
    public List<AnimDataDelay> multiAnims = new List<AnimDataDelay>();
}
[System.Serializable]
public struct AnimDataDelay {
    public string targetObjPath;
    public string animTitle;
    public float delay;
}

