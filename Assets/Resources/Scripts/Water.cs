using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Water : MonoBehaviour
{
    public GameObject storyManager;
    public GameObject water;
    public GameObject water2;
    public GameObject parent;
    public StoryManager eventSystem;
    public AudioClip waterSound;
    public bool jarUnder;

    private void Awake() {
        storyManager = this.gameObject;
        water = GameObject.Find("waterv1");
        water2 = GameObject.Find("waterv2");
        parent = GameObject.Find("Faucet");
    }

    private void Start() {
        water.SetActive(false);
        water2.SetActive(false);
    }

    void Update() {
        if(SceneManager.GetActiveScene().name == "Collection") {
            switch (storyManager.GetComponent<StoryManager>().currentStep) {
                case 1:
                    water.SetActive(true);
                    water.transform.position = new Vector3(-0.251f,0.3307735f,0.129f);
                    water.transform.localScale = new Vector3(16.40755f,16.40756f,57.42658f);
                    break;
                case 2:
                    water.SetActive(false);
                    break;
                case 5:
                    water2.SetActive(true);
                    water2.transform.position = new Vector3(-0.251f,0.355f,0.129f);
                    water2.transform.localScale = new Vector3(16.40755f, 16.40756f, 57.42658f);
                    break;
                case 6:
                    StartCoroutine(Lerp(0,100,1));
                    break;
                case 8:
                    jarUnder = true;
                    Invoke("AdjustWater",0.35f);
                    break;
                case 9:
                    jarUnder = false;
                    Invoke("AdjustWater", 0.15f);
                    break;
                case 11:
                    water2.SetActive(false);
                    break;
            }
        } else if(SceneManager.GetActiveScene().name == "Collection Review") {
            switch (storyManager.GetComponent<StoryManager>().currentStep) {
                case 3:
                    water2.SetActive(true);
                    water2.transform.position = new Vector3(-0.251f,0.355f,0.129f);
                    water2.transform.localScale = new Vector3(16.40755f,16.40756f,57.42658f);
                    break;
                case 4:
                    StartCoroutine(Lerp(0,100,1));
                    break;
                case 6:
                    jarUnder = true;
                    Invoke("AdjustWater", 0.45f);
                    break;
                case 7:
                    jarUnder = false;
                    Invoke("AdjustWater", 0.15f);
                    break;
                case 9:
                    water2.SetActive(false);
                    break;
            }
        } 
    }

    IEnumerator Lerp(float start,float target,float time) {
        float elapsedTime = 0;
        while (elapsedTime < time) {
            water2.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,Mathf.Lerp(start,target,elapsedTime));
            //water.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,(water.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) + target * Time.deltaTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public void AdjustWater() {
        if (jarUnder) {
            water2.transform.position = new Vector3(-0.251f,0.487f,0.129f);
            water2.transform.localScale = new Vector3(16.40755f,16.40755f,31f);
        } else {
            water2.transform.position = new Vector3(-0.251f,0.355f,0.129f);
            water2.transform.localScale = new Vector3(16.40755f, 16.40756f, 57.42658f);
        }
    }
}
