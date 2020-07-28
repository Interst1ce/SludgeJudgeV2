using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPieceTransition : MonoBehaviour {
    public GameObject setPieceOne;
    public GameObject setPieceTwo;
    public GameObject setPieceThree;

    public void TransitionOne() {
        StartCoroutine(ScaleTransition(setPieceTwo,setPieceOne,36.667f));
    }

    public void TransitionTwo() {
        StartCoroutine(ScaleTransition(setPieceThree,null,95f));
    }

    IEnumerator ScaleTransition(GameObject inSetPiece, GameObject outSetPiece, float delay = 0) {
        yield return new WaitForSeconds(delay);
        float t = 0;
        if (outSetPiece != null) {
            while(t < 1) {
                outSetPiece.transform.localScale = Vector3.Lerp(Vector3.one,Vector3.zero,t);
                Debug.Log("" + outSetPiece.transform.localScale);
                t += Time.deltaTime;
                yield return null;
            }
            outSetPiece.SetActive(false);
        }
        t = 0;
        if (inSetPiece != null) {
            inSetPiece.SetActive(true);
            inSetPiece.transform.localScale = Vector3.zero;
            while (t < 1) {
                inSetPiece.transform.localScale = Vector3.Lerp(Vector3.zero,Vector3.one,t);
                t += Time.deltaTime;
                yield return null;
            }
        }
    }
}
