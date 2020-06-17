using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimExtras : MonoBehaviour {
    public void PlayMultiAnim(MultiAnim multiAnim) {
        foreach(AnimData anim in multiAnim.multiAnims) {
            Debug.Log(anim.animTitle + " " + anim.targetObjPath);
            GameObject target = GameObject.Find(anim.targetObjPath);
            Animator anmat = target.GetComponent<Animator>();
            if (anmat != null) {
                anmat.Play(anim.animTitle);
            }
        }
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
}
