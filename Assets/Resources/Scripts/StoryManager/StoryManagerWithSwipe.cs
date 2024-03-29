﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

[RequireComponent(typeof(AudioSource))]
public class StoryManagerWithSwipe : MonoBehaviour {

    public enum Interaction {
        Tap,
        Swipe
    }
    public enum SwipeDirection {
        Up,
        Down,
        Left,
        Right,
        No
    }

    GameObject qAPanel;

    AudioSource audioSource;

    public int currentStep;
    private bool introPlayed = false;
    private bool finished = false;
    private bool init = false;
    public int pauseDelay = 0;
    public AudioClip introAudio;
    public AudioClip outroAudio;
    public AudioClip missTapAudio;
    public SoundEffect backgroundAudioStart;
    [HideInInspector]
    public SwipeDirection swipeDir;
    public List<Step> steps = new List<Step>();

    //BREAK ALL OF THESE OUT TO SCRIPTABLE OBJECTS

    [System.Serializable]
    public class Step : object {
        public Target[] targets;
        public Interaction targetInteraction;
        public SwipeDirection swipeDir;
        public bool hasQuestion;
        public Question question;
        public UnityEvent otherFunctions;
    }

    [System.Serializable]
    public class Question : object {
        public string question;
        //Do not set this array to be larger than 4
        public string[] choices;
        public int correctChoice;
    }

    [System.Serializable]
    public class Target : object {
        public GameObject objectTarget;
        public int targetStep;
        public AnimationClip targetAnim;
        public AudioClip targetAudio;
    }


    [System.Serializable]
    public class SoundEffect : object {
        public bool loop;
        public AudioClip soundEffect;
        public float delay;
    }

    public void Awake() {
        //qAPanel = GameObject.Find("QAPanel");
        audioSource = GetComponent<AudioSource>();
        currentStep = 0;
    }

    public void Start() {
        //move this to play intro audio when the marker first comes into view
        AudioListener.pause = false;
    }

    //TODO: Break apart intros/outros from update so they don't need to be rechecked every frame and can just be played once the scene is initialized
    //TODO? Turn intros/outros/looping background audio into scriptable objects that are played through extension(s)
    //TODO? Break all non-narration audio out into scriptable objects to be played through extension(s)

    public void Update() {
        if (currentStep == steps.Count && !audioSource.isPlaying && finished == false /*&& !qAPanel.activeSelf*/) {
            finished = true;
            if (outroAudio != null) {
                PlayAudio(outroAudio);
                if (!audioSource.isPlaying) {
                    CallPause();
                }
            } else {
                StartCoroutine(ExecuteAfterTime(pauseDelay));
            }
        }
        if (!audioSource.isPlaying && introPlayed && currentStep == 0) {
            if (steps[0].targets[0].objectTarget.GetComponent<GlowObjectCmd>() != null) {
                steps[0].targets[0].objectTarget.GetComponent<GlowObjectCmd>().StartCoroutine("GlowPulse");
            }
        }


        for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                //call coroutine that detects swipe
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit)) {
                    bool inputMatch = false;
                    DetectInput(new Vector2(0,15));
                    if (swipeDir == steps[currentStep].swipeDir) inputMatch = true;
                    if (inputMatch) {
                        GlowObjectCmd glow = steps[currentStep + 1].targets[0].objectTarget.GetComponent<GlowObjectCmd>();
                        if (glow != null) {
                            glow.StartCoroutine("GlowPulse");
                        } else {
                            GlowObjectCmd[] glows = steps[currentStep + 1].targets[0].objectTarget.GetComponentsInChildren<GlowObjectCmd>();
                            foreach (GlowObjectCmd childGlow in glows) childGlow.StartCoroutine("GlowPulse");
                        }
                        //GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = hit.transform.gameObject.name;

                        Animator lastStepAnim = null;
                        String lastAnimName = "";

                        foreach (Step elem in steps) {
                            foreach (Target target in elem.targets) {
                                if (hit.transform.gameObject == target.objectTarget && currentStep == steps.IndexOf(elem) && !audioSource.isPlaying && !audioSource.loop && (lastStepAnim == null || lastStepAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !lastStepAnim.IsInTransition(0))) {
                                    currentStep = target.targetStep;
                                    for (int j = 0; j < 5; j++) {
                                        StopCoroutine("GlowPulse");
                                    }
                                    if (target.targetAnim != null) {
                                        //play the animation for the step
                                        Animator animator = hit.transform.gameObject.GetComponent<Animator>();
                                        lastStepAnim = animator;
                                        lastAnimName = target.targetAnim.name;
                                        if (animator != null) {
                                            animator.Play(target.targetAnim.name);
                                        } else {
                                            //allows for selecting a level 1 child for the object target
                                            hit.transform.gameObject.GetComponentInParent<Animator>().Play(target.targetAnim.name);
                                        }
                                    }
                                    if (target.targetAudio != null) {
                                        //play audio for the step
                                        PlayAudio(target.targetAudio);
                                    }
                                    if (elem.hasQuestion) {
                                        //send necessary data to the QuestionManager and call Question()
                                        qAPanel.GetComponent<QuestionManager>().question = elem.question.question;
                                        qAPanel.GetComponent<QuestionManager>().choices = elem.question.choices;
                                        qAPanel.GetComponent<QuestionManager>().answer = elem.question.correctChoice;
                                        if (target.targetAudio != null) {
                                            if (target.targetAnim != null) {
                                                if (target.targetAudio.length > target.targetAnim.length) {
                                                    CallQuestion(target.targetAudio.length);
                                                } else {
                                                    CallQuestion(target.targetAnim.length);
                                                }
                                            }
                                            CallQuestion(target.targetAudio.length);
                                        } else if (target.targetAnim != null) {
                                            CallQuestion(target.targetAnim.length);
                                        }
                                        CallQuestion(0);
                                    }
                                    /*if(elem.otherFunctions.GetPersistentEventCount() > 0)*/
                                    elem.otherFunctions.Invoke();
                                } else if (hit.transform.gameObject != target.objectTarget && currentStep == steps.IndexOf(elem) && !audioSource.isPlaying) {
                                    PlayAudio(missTapAudio);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //If checking for tap pass in Vector2.zero for swipeDist, needed because of compiler nonsense
    IEnumerator DetectInput(Vector2 swipeDist) {
        float time = Time.time;
        float timeDelta = 0;

        Vector2 startPos = Vector2.zero;
        Vector2 curPos;
        Vector2 posDelta;

        swipeDir = SwipeDirection.No;
        startPos = Input.mousePosition;

        do {
            timeDelta = Time.time - time;
            if (!Input.GetMouseButton(0)) break;
            curPos = Input.mousePosition;
            posDelta = curPos - startPos;
            if (Mathf.Abs(posDelta.x) > Mathf.Abs(swipeDist.x)) {
                if (Mathf.Sign(posDelta.x) == -1) {
                    yield return swipeDir = SwipeDirection.Left;
                } else if (Mathf.Sign(posDelta.x) == 1) {
                    yield return swipeDir = SwipeDirection.Right;
                }
            } else if (Mathf.Abs(posDelta.y) > Mathf.Abs(swipeDist.y)) {
                if (Mathf.Sign(posDelta.y) == -1) {
                    yield return swipeDir = SwipeDirection.Down;
                } else if (Mathf.Sign(posDelta.y) == 1) {
                    yield return swipeDir = SwipeDirection.Up;
                }
            }
        } while (timeDelta < 0.5f);
    }

    IEnumerator ExecuteAfterTime(float time) {
        yield return new WaitForSeconds(time);
        //Invoke("CallPause", outroAudio.length);
        CallPause();

    }

    public void CallPause() {
        GameObject.Find("PauseUI").GetComponent<PauseMenu>().Pause();
        GameObject.Find("PlayButton").SetActive(false);
    }

    public void CallQuestion(float delay) {
        qAPanel.GetComponent<QuestionManager>().Question(delay);
    }

    public void PlayAudio(AudioClip audio) {
        if (audio != null) {
            audioSource.clip = audio;
            audioSource.Play();
        }
    }
    //probably not necessary but left in incase not having it breaks something
    IEnumerator DelaySoundEffect(AudioSource source,float delay) {
        float elapsedTime = 0;

        while (elapsedTime < delay) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        source.Play();
    }

    public void PlayIntro() {
        if (!introPlayed) {
            introPlayed = true;
            PlayAudio(introAudio);
            //PlaySoundEffects(backgroundAudioStart);
        }
    }
}
