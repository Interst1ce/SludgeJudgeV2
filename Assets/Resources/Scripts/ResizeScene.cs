using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeScene : MonoBehaviour {

    [SerializeField]
    GameObject scene;
    Transform oldTransform;

    public void Init() {
        oldTransform = scene.transform;
    }

    public void UpdateScale(float scaleMult) {
        Vector3 newScale = new Vector3(1,1,1) * scaleMult;
        scene.transform.localScale = newScale;
    }

    public void UpdateHeight(float height) {
        scene.transform.localPosition = new Vector3(scene.transform.localPosition.x,height,scene.transform.localPosition.z);

    }

    public void StartSwap() {
        ARController.canSwap = true;
    }

    public void CancelSwap() {
        scene.transform.localScale = oldTransform.localScale;
        scene.transform.localRotation = oldTransform.localRotation;
    }

    public void ConfirmUpdate() {
        ARController.canSwap = false;
    }

    void Update() {

    }
}
