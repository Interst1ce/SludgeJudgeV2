using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPieceTransition : MonoBehaviour {
    public GameObject setPieceOne;
    public GameObject setPieceTwo;
    public GameObject setPieceThree;

    public void TransitionOne() {
        Debug.Log("Starting TransitionOne");
        StartCoroutine(ScaleTransition(setPieceTwo,setPieceOne,35.5f));
        Debug.Log("TransitionOne started");
    }

    public void TransitionTwo() {
        Debug.Log("Starting TransitionTwo");
        StartCoroutine(ScaleTransition(setPieceThree,null,92f));
        Debug.Log("TransitionTwo started");
    }

    IEnumerator ScaleTransition(GameObject inSetPiece, GameObject outSetPiece, float delay = 0) {
        Debug.Log("Waiting for " + delay + " seconds");
        yield return new WaitForSeconds(delay);
        float t = 0;
        if (outSetPiece != null) {
            Debug.Log("Scaling object down");
            while(t < 1) {
                outSetPiece.transform.localScale = Vector3.Lerp(Vector3.one,Vector3.zero,t);
                t += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Deactivating object");
            outSetPiece.SetActive(false);
        }
        t = 0;
        if (inSetPiece != null) {
            Debug.Log("Activating object");
            inSetPiece.SetActive(true);
            Debug.Log("Scaling object up");
            inSetPiece.transform.localScale = Vector3.zero;
            while (t < 1) {
                inSetPiece.transform.localScale = Vector3.Lerp(Vector3.zero,Vector3.one,t);
                t += Time.deltaTime;
                yield return null;
            }
        }
    }
}
