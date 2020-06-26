using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SampleBeakerBottleAnimFix : MonoBehaviour {
    public GameObject sampleBeaker;
    public GameObject sampleBottle;
    GameObject newBeaker;
    GameObject newBottle;

    public List<AnimationClip> beakerAnims = new List<AnimationClip>();
    public List<AnimationClip> bottleAnims = new List<AnimationClip>();

    float beakerDelay;
    float bottleDelay;

    public void AnimFix(int index) {
        if (index < beakerAnims.Count - 1) {
            sampleBeaker.SetActive(false);
            newBeaker = Instantiate(sampleBeaker,sampleBeaker.transform);
            beakerDelay = beakerAnims[index].length;
            newBeaker.GetComponent<Animator>().Play(beakerAnims[index].name);
        }
        if(index < bottleAnims.Count - 1) {
            sampleBottle.SetActive(false);
            newBottle = Instantiate(sampleBottle,sampleBottle.transform);
            bottleDelay = bottleAnims[index].length;
            newBottle.GetComponent<Animator>().Play(bottleAnims[index].name);
        }

        StartCoroutine("FinishAnimFix");
    }

    IEnumerator FinishAnimFix() {
        while(beakerDelay > 0 && bottleDelay > 0) {
            beakerDelay -= Time.deltaTime;
            bottleDelay -= Time.deltaTime;
            yield return null;
        }
        sampleBeaker.SetActive(true);
        sampleBottle.SetActive(true);

        yield return null;
    }
}
