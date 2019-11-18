using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    GameObject[] toToggle;

    TextMeshProUGUI chapterTitle;

    public void Pause() {
        foreach (GameObject gameObject in toToggle) {
            if (gameObject.name == "ChapterTitle") {
                chapterTitle = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            }
        }
        chapterTitle.text = SceneManager.GetActiveScene().name;

        AudioListener.pause = !AudioListener.pause;
        foreach (GameObject elem in toToggle) {
            elem.SetActive(!elem.activeSelf);
        }
    }
}
