using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiAnim", menuName = "ScriptableObjects/MultiAnim",order = 1)]
public class MultiAnim : ScriptableObject {
    public List<animData> multiAnims = new List<animData>();    
}
[System.Serializable]
public struct animData {
    public string targetObjPath;
    public string animTitle;
}
