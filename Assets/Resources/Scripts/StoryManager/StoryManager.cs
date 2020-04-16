using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GoogleARCore.Examples.Common;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

[RequireComponent(typeof(AudioSource))]
public class StoryManager : MonoBehaviour {

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
    public enum ManipulationType {
        Transform,
        Rotate,
        Scale
    }
    public enum ManipulationAxis {
        X,
        Y,
        Z
    }

    GameObject qAPanel;

    AudioSource audioSource;

    public int currentStep;
    private bool introPlayed = false;
    private bool finished = false;
    private bool init = false;
    public int pauseDelay = 0;
    [SerializeField]
    public AudioClip introAudio;
    [SerializeField]
    public AudioClip outroAudio;
    [SerializeField]
    public AudioClip missTapAudio;
    [SerializeField]
    public SoundEffect backgroundAudioStart;
    [HideInInspector]
    public SwipeDirection swipeDir;

    //[SerializeField]
    //public GameObject slider;

    //public static Step[] staticSteps;

    public List<Step> steps = new List<Step>();

    //BREAK ALL OF THESE OUT TO SCRIPTABLE OBJECTS

    [System.Serializable]
    public class Step : object {
        public Target[] targets;
        //public SoundEffect[] soundEffects;
        //These variables are for object manipulation using a slider, not being used right now and might be replaced with a two-axis dragging system
        /*[SerializeField]
        public TapOrDrag tapOrDrag;
        [SerializeField]
        public bool hasSlider;
        [SerializeField, Range(0,1)]
        public float sliderTarget;
        [SerializeField]
        public bool manipulateObject;
        [SerializeField]
        public ManipulationType manipulationType;
        [SerializeField]
        public ManipulationAxis manipulationAxis;
        [SerializeField]
        public float manipulationMultiplier;*/
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

        //input detection, type of check depends on current step's selected interaction type,
        //if(swipeDir == steps[currentStep].swipeDir)

        List<GameObject> planes = gameObject.GetComponent<DetectedPlaneGenerator>().planesInScene;
        GameObject scene = gameObject.GetComponent<ARController>().sceneObject;
        if (scene.activeSelf) {
            foreach (GameObject obj in planes) {
                obj.GetComponent<MeshRenderer>().enabled = false;
                obj.GetComponent<DetectedPlaneGenerator>().enabled = false;
            }
            gameObject.GetComponent<DetectedPlaneGenerator>().enabled = false;
        }

        for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                //call coroutine that detects swipe
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit)) {
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
                                /*AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
                                foreach (AudioSource audioSource in audioSources) {
                                    if (audioSources.Length > 1) {
                                        Destroy(audioSource);
                                    } else this.audioSource = audioSource;
                                }*/
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
                                /*if (elem.soundEffects != null) {
                                    PlaySoundEffects(elem.soundEffects);
                                }
                                if (elem.hasSlider) {
                                    if (!slider.activeSelf) {
                                        //activate slider and add an EventListener that calls CheckSlider(Step) everytime the slider value changes
                                        slider.SetActive(true);
                                        slider.GetComponent<Slider>().onValueChanged.AddListener(delegate { CheckSlider(elem); });
                                    } else {
                                        slider.SetActive(false);
                                    }
                                }*/
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

    //If checking for tap pass in Vector2.zero for swipeDist, needed because of compiler nonsense
    IEnumerator DetectInput(Interaction toCheck, Vector2 swipeDist) {
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
                }else if(Mathf.Sign(posDelta.x) == 1) {
                    yield return swipeDir = SwipeDirection.Right;
                }
            }else if(Mathf.Abs(posDelta.y) > Mathf.Abs(swipeDist.y)) {
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

    /*public void PlaySoundEffects(SoundEffect[] effects) {
        foreach (SoundEffect effect in effects) {
            if(effect != null) {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = effect.soundEffect;
                if (effect.loop) {
                    audioSource.loop = true;
                }
                audioSource.PlayDelayed(effect.delay);
            }
        }
    }
    public void PlaySoundEffects(SoundEffect effect) {
        if(effect != null) {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = effect.soundEffect;
            if (effect.loop) {
                audioSource.loop = true;
            }
            audioSource.PlayDelayed(effect.delay);
        }
        /*if (effect.delay == 0) {
            audioSource.Play();
        } else {
            StartCoroutine(DelaySoundEffect(audioSource,effect.delay));
        }/
    }*/

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

    //UPDATE TO WORK WITH MULTIPLE TARGETS
    //adjusts the position/rotation/scale of the object along one axis depending on the value of the slider.
    /*public void CheckSlider(Step step) {
        Vector3 p = step.targets.objectTarget.transform.localPosition;
        Quaternion r = step.targets.objectTarget.transform.localRotation;
        Vector3 s = step.targets.objectTarget.transform.localScale;
        float sliderMultiply = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
        switch (step.manipulationType) {
            case ManipulationType.Transform:
                switch (step.manipulationAxis) {
                    case ManipulationAxis.X:
                        p.x = sliderMultiply;
                        break;
                    case ManipulationAxis.Y:
                        p.y = sliderMultiply;
                        break;
                    case ManipulationAxis.Z:
                        p.z = sliderMultiply;
                        break;
                }
                break;
            case ManipulationType.Rotate:
                switch (elem.manipulationAxis) {
                    case ManipulationAxis.X:
                        GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = ("" + r);
                        GameObject.Find("TextMeshPro Text (1)").GetComponent<TextMeshProUGUI>().text = ("" + (slider.GetComponent<Slider>().value * elem.manipulationMultiplier));
                        elem.objectTarget.transform.Rotate(new Vector3(sliderMultiply,r.y,r.z));
                        break;
                    case ManipulationAxis.Y:
                        elem.objectTarget.transform.Rotate(new Vector3(r.x,sliderMultiply,r.z));
                        break;
                    case ManipulationAxis.Z:
                        elem.objectTarget.transform.Rotate(new Vector3(r.x,r.y,sliderMultiply));
                        break;
                }
                break;
            case ManipulationType.Scale:
                switch (elem.manipulationAxis) {
                    case ManipulationAxis.X:
                        s.x = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
                        break;
                    case ManipulationAxis.Y:
                        s.y = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
                        break;
                    case ManipulationAxis.Z:
                        s.z = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
                        break;
                }
                break;
        }
    }*/
}