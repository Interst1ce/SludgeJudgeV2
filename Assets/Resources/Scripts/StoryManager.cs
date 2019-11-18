using System;
using System.Collections;
using UnityEngine;

public class StoryManager : MonoBehaviour {

    public enum TapOrDrag {
        Tap,
        Drag
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
    public int pauseDelay=0;
    [SerializeField]
    public AudioClip introAudio;
    [SerializeField]
    public AudioClip outroAudio;
    [SerializeField]
    public SoundEffect backgroundAudioStart;

    //[SerializeField]
    //public GameObject slider;

    [SerializeField]
    public Step[] steps;

    [System.Serializable]
    public class Step : object {
        [SerializeField]
        public Target[] targets;
        [SerializeField]
        public SoundEffect[] soundEffects;
        [SerializeField]
        public Highlight[] highlights;
        [SerializeField]
        public AudioClip missTap;
        [SerializeField]
        public int stepOrder;
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
        [SerializeField]
        public bool hasQuestion;
        [SerializeField]
        public Question question;        
    }

    [System.Serializable]
    public class Question : object {
        [SerializeField]
        public String question;
        [SerializeField]
        //Do not set this array to be larger than 4
        public String[] choices;
        [SerializeField]
        public int correctChoice;
    }

    [System.Serializable]
    public class Target : object {
        [SerializeField]
        public GameObject objectTarget;
        [SerializeField]
        public int targetStep;
        [SerializeField]
        public AnimationClip targetAnim;
        [SerializeField]
        public AudioClip targetAudio;
    }


    [System.Serializable]
    public class SoundEffect : object {
        [SerializeField]
        public bool loop;
        [SerializeField]
        public AudioClip soundEffect;
        [SerializeField]
        public float delay;
    }

    [System.Serializable]
    public class Highlight : object {
        [SerializeField]
        public AnimationClip highlightAnim;
        [SerializeField]
        public GameObject highlightTarget;
    }

    public void Awake() {
        qAPanel = GameObject.Find("QAPanel");
        audioSource = GetComponent<AudioSource>();
        currentStep = 0;
    }

    public void Start() {
        //move this to play intro audio when the marker first comes into view
        AudioListener.pause = false;
    }

    public void Update() {
        if (currentStep == steps.Length && !audioSource.isPlaying && finished == false && !qAPanel.activeSelf ) {
            finished = true;
            //if (outroAudio != null)
            //{
            if (outroAudio != null)
            {
                PlayAudio(outroAudio);
                if (!audioSource.isPlaying)
                {
                    CallPause();
                }
            }
            else{
                StartCoroutine(ExecuteAfterTime(pauseDelay));
            }

            //}
           // else
            //{
            //    CallPause();
           // }
        }
        if (!audioSource.isPlaying && introPlayed) {
            if(currentStep <= steps.Length - 1) {
                if (steps[currentStep].highlights != null) {
                    foreach(Highlight highlight in steps[currentStep].highlights) {
                        highlight.highlightTarget.GetComponent<Animator>().Play(highlight.highlightAnim.name);
                    }
                }
            }   
        }
        for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit)) {
                    //GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = hit.transform.gameObject.name;
                    foreach (Step elem in steps) {
                        foreach(Target target in elem.targets) {
                            if (hit.transform.gameObject == target.objectTarget && currentStep == elem.stepOrder && !audioSource.isPlaying && !audioSource.loop) {
                                currentStep = target.targetStep;
                                AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
                                for (int j = 1; j < audioSources.Length; j++) {
                                    Destroy(audioSources[j]);
                                }
                                foreach (Highlight highlight in elem.highlights) {
                                    highlight.highlightTarget.GetComponent<Animator>().Play("New State");
                                }
                                if (target.targetAnim != null) {
                                    //play the animation for the step
                                    //maybe update for next sprint multiple animations to play in sequence
                                    hit.transform.gameObject.GetComponent<Animator>().Play(target.targetAnim.name);
                                }
                                if (target.targetAudio != null) {
                                    //play audio for the step
                                    PlayAudio(target.targetAudio);
                                }
                                if (elem.soundEffects != null) {
                                    PlaySoundEffects(elem.soundEffects);
                                }
                                /*if (elem.hasSlider) {
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
                            } else if (hit.transform.gameObject != target.objectTarget && currentStep == elem.stepOrder && !audioSource.isPlaying) {
                                PlayAudio(elem.missTap);
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
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
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void PlaySoundEffects(SoundEffect[] effects) {
        foreach(SoundEffect effect in effects) {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = effect.soundEffect;
            if (effect.loop) {
                audioSource.loop = true;
            }
            audioSource.PlayDelayed(effect.delay);
        }
    }
    public void PlaySoundEffects(SoundEffect effect) {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = effect.soundEffect;
        if (effect.loop) {
            audioSource.loop = true;
        }
        if (effect.delay == 0) {
            audioSource.Play();
        } else {
            StartCoroutine(DelaySoundEffect(audioSource, effect.delay));
        }
    }

    IEnumerator DelaySoundEffect(AudioSource source, float delay) {
        float elapsedTime = 0;

        while(elapsedTime < delay) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        source.Play();
    }

    public void PlayIntro() {
        if (!introPlayed) {
            introPlayed = !introPlayed;
            PlayAudio(introAudio);
            PlaySoundEffects(backgroundAudioStart);
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
