using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class SampleBeakerBottleAnimFix : MonoBehaviour {
    public GameObject sampleBeaker;
    GameObject newBeaker;

    public List<AnimationClip> beakerAnims = new List<AnimationClip>();

    public void AnimFix(int index) {
        if (index < beakerAnims.Count - 1) {
            if(newBeaker == null) {
                sampleBeaker.SetActive(false);
                newBeaker = Instantiate(sampleBeaker,sampleBeaker.transform);
            }
            newBeaker.GetComponent<Animator>().Play(beakerAnims[index].name);
        }
    }
}
