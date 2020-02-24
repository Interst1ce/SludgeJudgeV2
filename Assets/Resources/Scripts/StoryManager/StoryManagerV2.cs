﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

[RequireComponent(typeof(AudioListener))]
public class StoryManagerV2 : MonoBehaviour {
    public int currentStep;
    private bool introPlayed = false;
    private bool finished = false;

    [SerializeField]
    AudioClip introAudio;
    [SerializeField]
    AudioClip outroAudio;
    [SerializeField]
    AudioClip missTapAudio;
    //do something about starting background SFX

    public List<StepObject> steps = new List<StepObject>();

    private void Awake() {
        //audioSource
    }
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //check finish requirements
        //make initial object glow
        for (int i = 0; i < Input.touchCount; i++) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit)) {
                    foreach (StepObject.Target target in steps[currentStep].targets) {
                        switch (target.type) {
                            case StepObject.TargetType.Object:
                                if (hit.transform == target.objectTarget.transform) {
                                    //stop glow
                                    //play narration audio
                                    //play animations
                                    //start glow for next step if it has an object target, by the way add a depth test to the shader
                                }
                                break;
                            case StepObject.TargetType.Slider:
                                //oh boy
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}