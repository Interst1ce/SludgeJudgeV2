using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManagerV2 : MonoBehaviour {
    public List<Question> questions;
    Dictionary<Vector2Int,Question> questionDict; //might do list<list<question>> instead

    GameObject qAPanel;
    GameObject questionPanel;
    GameObject answerLayout;
    AudioSource audioSource;

    [SerializeField]
    AudioClip buttonSound;
    [SerializeField]
    AudioClip correctSound;
    [SerializeField]
    AudioClip incorrectSound;

    private void Awake() {
        int indexOffset = 0;
        int prevIndex = 0;
        foreach(Question question in questions) {
            if (question.step == prevIndex) indexOffset++; else indexOffset = 0;
            questionDict.Add(new Vector2Int(question.step,indexOffset), question);
            prevIndex = question.step;
        }
        if (buttonSound != null) {
            audioSource = qAPanel.AddComponent<AudioSource>();
            audioSource.clip = buttonSound;
        }
        if (qAPanel != null) questionPanel = qAPanel.transform.GetChild(0).gameObject;
    }
    void Start() {

    }

    public void StartQuest(int step, int offset) {
        answerLayout = questionDict[new Vector2Int(step,offset)].answerLayout;
        Instantiate(answerLayout,qAPanel.transform);
        UpdateUI();
    }

    public void CheckAnswer(int choice) {
        audioSource.Play();
        if (choice == 0) { //change 0 when correctAnswer variable is available
            if (correctSound != null) {
                audioSource.clip = correctSound;
                audioSource.Play();
            }
            //StartCoroutine(FadeUI(1));
            Invoke("DisableUI",1);
        } else {
            if (incorrectSound != null) {
                audioSource.clip = incorrectSound;
                audioSource.Play();
            }
        }
        audioSource.clip = buttonSound;
    }

    IEnumerator UpdateUI() {
        yield return null;
    }

    public void DisableUI() {
        qAPanel.SetActive(false);
    }
}
