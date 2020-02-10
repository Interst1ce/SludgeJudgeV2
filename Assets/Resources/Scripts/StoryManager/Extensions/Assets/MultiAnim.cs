using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiAnim",menuName = "ScriptableObjects/MultiAnim",order = 1)]
public class MultiAnim : ScriptableObject {
    public List<AnimData> multiAnims = new List<AnimData>();    
}
[System.Serializable]
public struct AnimData {
    public string targetObjPath;
    public string animTitle;
}
