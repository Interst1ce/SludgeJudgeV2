using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Effect",menuName = "ScriptableObjects/StoryManager/Extensions/Sound Effect",order = 1)]
public class SoundEffect : ScriptableObject {
    public List<SFX> soundEffects = new List<SFX>();
}

[System.Serializable]
public struct SFX {
    public bool loop;
    public AudioClip soundEffect;
    public float delay;
}
