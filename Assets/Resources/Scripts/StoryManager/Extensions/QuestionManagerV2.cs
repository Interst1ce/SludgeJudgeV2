using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManagerV2 : MonoBehaviour {
    public List<Question> questions;

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
        if (buttonSound != null) {
            audioSource = qAPanel.AddComponent<AudioSource>();
            audioSource.clip = buttonSound;
        }
    }
    void Start() {

    }

    void Update() {

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

    public void DisableUI() {
        qAPanel.SetActive(false);
    }
}
