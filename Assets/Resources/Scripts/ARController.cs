using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoogleARCore;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARController : MonoBehaviour {

    Anchor anchor1;
    Anchor anchor2;
    public GameObject sceneObject;
    bool sceneSpawned;
    public static bool canSwap = false;

    List<Anchor> anchors = new List<Anchor>();

    void Awake() {
        Application.targetFrameRate = 60;
        sceneSpawned = false;
        //sceneObject.transform.localScale = new Vector3(0.25f,0.25f,0.25f);
    }

    public void SwapAnchor(TrackableHit hit) {
        if(anchor1 != null) {
            anchor2 = hit.Trackable.CreateAnchor(hit.Pose);
            sceneObject.transform.position = hit.Pose.position;
            sceneObject.transform.rotation = hit.Pose.rotation;
            sceneObject.transform.Rotate(0,180,0,Space.Self);
            sceneObject.transform.parent = anchor2.transform;
            Destroy(anchor1.gameObject);
        } else {
            anchor1 = hit.Trackable.CreateAnchor(hit.Pose);
            sceneObject.transform.position = hit.Pose.position;
            sceneObject.transform.rotation = hit.Pose.rotation;
            sceneObject.transform.Rotate(0,180,0,Space.Self);
            sceneObject.transform.parent = anchor1.transform;
            Destroy(anchor2.gameObject);
        }
        canSwap = false;
    }

    void Update() {

        //Debug.Log(anchors.Count);

        if (Session.Status != SessionStatus.Tracking) {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        } else Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) return;

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

        if(Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit)) {

            //Debug.Log("Raycast");

            Transform rootTransform = transform.parent;
            Vector3 hitPos = hit.Pose.position;

            if(rootTransform != null) {
                hitPos.Scale(rootTransform.transform.localScale);
                rootTransform.localPosition = hitPos * -1;
            }

            if((hit.Trackable is DetectedPlane) && (Vector3.Dot(Camera.main.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) < 0)) {
                return;
            } else {
                if (sceneSpawned) {
                    if (canSwap) {
                        SwapAnchor(hit);
                    }
                } else {
                    sceneSpawned = true;
                    canSwap = false;
                    //Debug.Log("Ground plane hit");
                    DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                    if (detectedPlane.PlaneType == DetectedPlaneType.HorizontalUpwardFacing) {
                        anchor1 = hit.Trackable.CreateAnchor(hit.Pose);
                        sceneObject.SetActive(true);
                        transform.GetComponent<StoryManager>().PlayIntro();
                        sceneObject.transform.position = new Vector3(hit.Pose.position.x,sceneObject.transform.position.y,hit.Pose.position.z);
                        sceneObject.transform.rotation = hit.Pose.rotation;
                        sceneObject.transform.Rotate(0,180,0,Space.Self);
                        sceneObject.transform.parent = anchor1.transform;
                        //Debug.Log("Scene object activated and placed at hit position");
                    }
                }
            }
        }
    }
}
