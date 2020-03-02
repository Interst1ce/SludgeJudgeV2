using System;
using System.Collections;
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
    private AudioSource audioSource;

    [SerializeField]
    AudioClip introAudio;
    [SerializeField]
    AudioClip outroAudio;
    [SerializeField]
    AudioClip missTapAudio;
    //do something about starting background SFX

    public List<StepObject> steps = new List<StepObject>();

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //check finish requirements
        if(currentStep == steps.Count && !audioSource.isPlaying && finished == true) {
            if(outroAudio != null) {
                //play outro audio and once it is finished CallPause();
            } //else pause after delay
        }
        //make initial object glow
        if (!audioSource.isPlaying && introPlayed && currentStep == 0) {
            var glow = steps[0].targets[0].objectTarget.GetComponent<GlowObjectCmd>(); //might throw an error if first target isn't an object
            if (glow != null) glow.StartCoroutine("GlowPulse");
        }
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
                                //oh boy, maybe start a coroutine that checks the slider? Do something with listeners?
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
