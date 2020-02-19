using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Step",menuName = "ScriptableObjects/StoryManager/Step",order = 0)]
public class StepObject : ScriptableObject {

    public struct Target {
        //targetType
        public GameObject objectTarget;
        public int targetStep;
        public AnimationClip targetAnim;
        public AudioClip targetAudio;
    }
}
