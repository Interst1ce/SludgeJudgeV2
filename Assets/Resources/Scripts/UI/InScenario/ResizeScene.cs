﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeScene : MonoBehaviour {

    [SerializeField]
    GameObject scene;
    float oldHeight;
    float oldScale;

    public void Init() {
        oldHeight = scene.transform.localPosition.y;
        oldScale = scene.transform.localScale.x;
    }

    public void UpdateScale(float scaleMult) {
        Vector3 newScale = new Vector3(1,1,1) * scaleMult;
        scene.transform.localScale = newScale;
    }

    public void RevertScale(Slider slider) {
        slider.value = oldScale;
    }

    public void UpdateHeight(float height) {
        scene.transform.localPosition = new Vector3(scene.transform.localPosition.x,height,scene.transform.localPosition.z);
    }

    public void RevertHeight(Slider slider) {
        slider.value = oldHeight;
    }

    public void StartSwap() {
        ARController.canSwap = true;
    }

    public void CancelSwap() {
        //scene.transform.localScale = oldTransform.localScale;
        //scene.transform.localRotation = oldTransform.localRotation;
    }

    public void ConfirmUpdate() {
        ARController.canSwap = false;
    }
}
