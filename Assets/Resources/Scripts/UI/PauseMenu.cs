using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    GameObject[] toToggle;
    [SerializeField]
    GameObject[] subMenus;
    [SerializeField]
    TextMeshProUGUI chapterTitle;

    bool small = true;

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

        if (small) {
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
        } else {
            gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0,0.83f);
            gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.008f,1);
        }
    }

    public void ToggleSubMenu(int index) {
        foreach (GameObject gameObject in toToggle) {
            if (gameObject.name == "PausePanel") {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
        subMenus[index].SetActive(!subMenus[index].activeSelf);
    }
}
