using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPieceTransition : MonoBehaviour {
    public GameObject setPieceOne;
    public GameObject setPieceTwo;
    public GameObject setPieceThree;

    List<Material> GetTransitionMaterials(GameObject setpiece) {
        List<Material> materials = new List<Material>();

        foreach (Renderer renderer in setpiece.GetComponentsInChildren<Renderer>()) {
            if (!materials.Contains(renderer.material)) materials.Add(renderer.material);
        }

        return materials;
    }


    public void TransitionOne() {
        //List<Material> fadeOut = GetTransitionMaterials(setPieceOne);
        Debug.Log("Starting TransitionOne");
        StartCoroutine(ScaleTransition(setPieceTwo,setPieceOne,35.5f));
        Debug.Log("TransitionOne started");
    }

    

    public void TransitionTwo() {
        //List<Material> fadeIn = GetTransitionMaterials(setPieceThree);
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

    IEnumerator AlphaTransition(List<Material> fadeIn, GameObject inSetPiece, List<Material> fadeOut = null, GameObject outSetPiece = null, float delay = 0) {
        float t = 0;
        yield return new WaitForSeconds(delay);
        if (fadeOut != null) {
            while (t < 1) {
                foreach (Material material in fadeOut) {
                    Color color = material.color;
                    color.a = Mathf.Lerp(255,0,t);
                    material.color = color;
                    t += Time.deltaTime;
                }
                yield return null;
            }
            outSetPiece.SetActive(false);
        }

        if(inSetPiece != null) inSetPiece.SetActive(true);

        if (fadeIn != null) {
            t = 0;
            while (t < 1) {
                foreach (Material material in fadeOut) {
                    Color color = material.color;
                    if (material.name == "Zone123 Plane") {
                        color.a = Mathf.Lerp(0,190,t);
                    } else color.a = Mathf.Lerp(0,255,t);
                    material.color = color;
                    t += Time.deltaTime;
                }
                yield return null;
            }
        }
    }
}
