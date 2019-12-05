using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimExtras : MonoBehaviour {
    public void PlayMultiAnim(MultiAnim multiAnim) {
        foreach(animData anim in multiAnim.multiAnims) {
            GameObject.Find(anim.targetObjPath).GetComponent<Animator>().Play(anim.animClip.name);
        }
    }

    public void PlayMultiAnim(MultiAnimTrigger multiAnim) {
        foreach (animTriggerData anim in multiAnim.multiAnims) {
            GameObject.Find(anim.targetObjPath).GetComponent<Animator>().SetTrigger(anim.animTrigger);
        }
    }
}
