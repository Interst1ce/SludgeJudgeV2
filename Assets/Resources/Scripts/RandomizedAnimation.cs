using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedAnimation : MonoBehaviour {
    [SerializeField]
    public GameObject Object;
    [SerializeField]
    public StoryManager storymanager;
    [SerializeField]
    public AnimationClip clipA;
    [SerializeField]
    public AnimationClip clipB;
    [SerializeField]
    public AnimationClip clipAA;
    [SerializeField]
    public int steptoactivate;
    int randomint;
    private bool isrunning = false;


    // Update is called once per frame
    void Update() {

        if (storymanager.currentStep + 2 == steptoactivate && isrunning == false) {
            isrunning = true;
            randomint = Random.Range(0,4);
            if (randomint == 3) {
                storymanager.steps[storymanager.currentStep + 2].targets[0].targetAnim = clipA;
                storymanager.steps[storymanager.currentStep + 3].targets[0].targetAnim = clipAA;
                storymanager.steps[storymanager.currentStep + 3].question.correctChoice = 0;
            }
            if (randomint < 3) {
                storymanager.steps[storymanager.currentStep + 2].targets[0].targetAnim = clipB;
                storymanager.steps[storymanager.currentStep + 3].question.correctChoice = 1;
            }


        }
    }
}
