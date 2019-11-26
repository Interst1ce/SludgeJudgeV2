using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationExtras : MonoBehaviour {

    //use when you need to manually trigger transitions for multiple animatiions on the same object
    public void SequentialAnims(string transTitles) {

    }

    //use when you need to play anims on multiple objects at the same time
    /*due to engine limitations you need to call the function multiple times  
    for each additional object you want to animate*/
    public void SimultaneousAnims(Animator animator) {

    }
}
