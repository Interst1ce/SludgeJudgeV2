using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.UI;
using UnityEngine;

public class AnimExtras : MonoBehaviour {
    public void PlayMultiAnim(MultiAnim multiAnim) {
        foreach(AnimData anim in multiAnim.multiAnims) {
            Debug.Log(anim.animTitle + " " + anim.targetObjPath);
            GameObject target = GameObject.Find(anim.targetObjPath);
            Debug.Log(target.name);
            Animator anmat = target.GetComponent<Animator>();
            if (anmat != null) {
                Debug.Log("Anim playing");
                anmat.Play(anim.animTitle);
            }
        }
    }

    public void PlayMultiAnim(MultiAnimDelay multiAnim) {
        foreach(AnimDataDelay anim in multiAnim.multiAnims) {
            GameObject target = GameObject.Find(anim.targetObjPath);
            Animator anmat = target.GetComponent<Animator>();
            if(anmat != null) {
                StartCoroutine(MultiAnimDelay(anmat, anim));
            }
        }
    }

    IEnumerator MultiAnimDelay(Animator anmat, AnimDataDelay anim) {
        float t = 0;
        while (t < anim.delay) {
            t += Time.deltaTime;
            yield return null;
        }
        anmat.Play(anim.animTitle);
    }

    public void PlayMultiAnim(MultiAnimTrigger multiAnim) {
        foreach (animTriggerData anim in multiAnim.multiAnims) {
            GameObject target = GameObject.Find(anim.targetObjPath);
            Animator anmat = target.GetComponent<Animator>();
            if (anmat != null) {
                Debug.Log("Triggering Animation");
                anmat.SetTrigger(anim.animTrigger);
            }
        }
    }

    public GameObject sampleBeaker;
    public AnimationClip beakerFill;
    public AnimationClip beakerSpinUp;

    public void PlayBeakerFill () {
        sampleBeaker.GetComponent<Animator>().Play(beakerFill.name);
    }

    public void PlayBeakerSpinUp() {
        sampleBeaker.GetComponent<Animator>().Play(beakerSpinUp.name);
    }
    
    public void TriggerBeakerSpinDown() {
        sampleBeaker.GetComponent<Animator>().SetTrigger("Stop Spin");
    }
}
