﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiAnimTrigger",menuName = "ScriptableObjects/StoryManager/Extensions/MultiAnimTrigger",order = 3)]
public class MultiAnimTrigger : ScriptableObject {
    public List<animTriggerData> multiAnims = new List<animTriggerData>();
}
[System.Serializable]
public struct animTriggerData {
    public string targetObjPath;
    public string animTrigger;
}