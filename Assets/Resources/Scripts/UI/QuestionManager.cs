using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public string question;
    public int answer;
    public string[] choices;
    List<GameObject> answerLayouts = new List<GameObject>();
    public List<TextMeshProUGUI> textToBeFaded = new List<TextMeshProUGUI>();
    public List<Image> imageToBeFaded = new List<Image>();
    GameObject qAPanel;
    GameObject questionPanel;
    AudioSource audioSource;

    [SerializeField]
    public AudioClip buttonSound;
    [SerializeField]
    public AudioClip correctSound;

    private void Awake() {
        qAPanel = GameObject.Find("QAPanel");
        questionPanel = GameObject.Find("QuestionPanel");
        answerLayouts.Add(GameObject.Find("AnswerLayout2"));
        answerLayouts.Add(GameObject.Find("AnswerLayout3"));
        answerLayouts.Add(GameObject.Find("AnswerLayout4"));
        if(buttonSound != null) {
            audioSource = qAPanel.AddComponent<AudioSource>();
            audioSource.clip = buttonSound;
        }
    }

    private void Start() {
        foreach(GameObject gameObject in answerLayouts) {
            gameObject.SetActive(false);
        }
        qAPanel.SetActive(false);
    }

    public void Question(float delay) {
        qAPanel.SetActive(true);
        StartCoroutine(FadeUI(1,delay,true));
    }

    //FadeUI coroutine
    IEnumerator FadeUI(float targetTime, float delay = 0, bool fadeIn = false) {
        Debug.Log("Coroutine Started");
        textToBeFaded.Add(questionPanel.GetComponentInChildren<TextMeshProUGUI>());
        imageToBeFaded.Add(qAPanel.GetComponent<Image>());
        imageToBeFaded.Add(questionPanel.GetComponent<Image>());


        //depending on the length of choices[] add button children from AnswerLayout2/3/4 to toBeFadedIn[]
        switch (choices.Length) {
            case 2:
                foreach (Transform child in answerLayouts[0].transform) {
                    answerLayouts[0].SetActive(true);
                    textToBeFaded.Add(child.GetChild(0).GetComponent<TextMeshProUGUI>());
                    imageToBeFaded.Add(child.GetComponent<Image>());
                }
                break;
            case 3:
                foreach (Transform child in answerLayouts[1].transform) {
                    answerLayouts[1].SetActive(true);
                    textToBeFaded.Add(child.GetChild(0).GetComponent<TextMeshProUGUI>());
                    imageToBeFaded.Add(child.GetComponent<Image>());
                }
                break;
            case 4:
                foreach (Transform child in answerLayouts[2].transform) {
                    answerLayouts[2].SetActive(true);
                    textToBeFaded.Add(child.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>());
                    imageToBeFaded.Add(child.gameObject.GetComponent<Image>());
                }
                break;
            default:
                Debug.Log("The number of choices that you have must be between 2-4");
                break;

        }
        float elapsedTime = 0;

        //update text boxes
        if (fadeIn) {
            textToBeFaded[0].text = question;
            for (int i = 1; i < textToBeFaded.Count; i++) {
                textToBeFaded[i].text = choices[i - 1];
            }
        }

        if(delay > 0) {
            while (elapsedTime < delay) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        while(elapsedTime < targetTime + delay) {
            if (fadeIn){
                foreach (TextMeshProUGUI elem in textToBeFaded) {
                    elem.color = new Color(0,0,0,Mathf.Lerp(0,1,(elapsedTime / targetTime)));
                    Debug.Log(elem.text + elem.color);
                }
                foreach (Image elem in imageToBeFaded) {
                    if(elem.gameObject.name == "QAPanel") {
                        elem.color = new Color(0.25f,0.25f,0.25f,Mathf.Lerp(0,0.75f,(elapsedTime / targetTime)));
                    } else {
                        elem.color = new Color(1,1,1,Mathf.Lerp(0,1,(elapsedTime / targetTime)));
                    } 
                }
            } else {
                foreach (TextMeshProUGUI elem in textToBeFaded) {
                    elem.color = new Color(0,0,0,Mathf.Lerp(1,0,(elapsedTime / targetTime)));
                }
                foreach (Image elem in imageToBeFaded) {
                    if (elem.gameObject.name == "QAPanel") {
                        elem.color = new Color(0.25f,0.25f,0.25f,Mathf.Lerp(0.75f,0,(elapsedTime / targetTime)));
                    } else {
                        elem.color = new Color(1,1,1,Mathf.Lerp(1,0,(elapsedTime / targetTime)));
                    }
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    //call from OnClick event from AnswerButtons
    public void CheckAnswer(int choice) {
        audioSource.Play();
        if(choice == answer) {
            if(correctSound != null) {
                AudioSource correct = qAPanel.AddComponent<AudioSource>();
                correct.clip = correctSound;
                correct.Play();
            }
            StartCoroutine(FadeUI(1));
            Invoke("DisableUI",1);
        }
    }

    public void DisableUI() {
        qAPanel.SetActive(false);
    }
}
