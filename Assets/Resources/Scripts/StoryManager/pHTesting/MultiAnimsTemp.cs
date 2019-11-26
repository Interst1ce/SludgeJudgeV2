using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAnimsTemp : MonoBehaviour {
    [System.Serializable]
    public class Anim {
        public Animator animator;
        public Animation animation;
    }

    [System.Serializable]
    public class MultiAnim {
        public Anim[] anims;
    }

    [SerializeField]
    public MultiAnim[] multiAnims;

    public void PlayMultiAnim(int i) {
        for(int j = 0; j < multiAnims[i].anims.Length; j++) {
            multiAnims[i].anims[j].animator.Play(multiAnims[i].anims[j].animation.name);
        }
    }
}
