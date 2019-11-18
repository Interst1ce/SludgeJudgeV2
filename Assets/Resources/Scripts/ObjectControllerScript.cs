using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class ObjectControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private GameObject ballToResize;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Slider heightSlider;
    [SerializeField]
    private Text debugField;
    private bool objectInstantiated = false;
    private float currSliderVal;
    private float prevSliderVal = 0;
    private float originalHeight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Touch touch = Input.GetTouch(0);
        if (Input.touchCount < 1 || touch.phase != TouchPhase.Began)
        {
            return;
        }
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            if (!objectInstantiated)
            {
                Anchor anc = hit.Trackable.CreateAnchor(hit.Pose);
                ballToResize = Object.Instantiate(prefab, hit.Pose.position, hit.Pose.rotation, anc.transform);
                originalHeight = ballToResize.transform.position.y;
                objectInstantiated = true;
            }
            
        }

    }

    public void ResizeAsset()
    {

        float scale = slider.value;
        ballToResize.gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void RaiseLowerAsset()
    {
        
        
            currSliderVal = heightSlider.value;
            float assetHeight = ballToResize.gameObject.transform.position.y;
        float newAssetHeight = originalHeight + currSliderVal;
        //ballToResize.transform.Translate(new Vector3(0f, currSliderVal, 0f));
        ballToResize.transform.SetPositionAndRotation(new Vector3(ballToResize.transform.position.x, newAssetHeight, ballToResize.transform.position.z), new Quaternion(ballToResize.transform.rotation.x,ballToResize.transform.rotation.y,ballToResize.transform.rotation.z,ballToResize.transform.rotation.w));
        /*if (currSliderVal > prevSliderVal)
        {
            ballToResize.transform.Translate(new Vector3(0f, currSliderVal, 0f));
        }
        else if (currSliderVal < prevSliderVal)
        {
            ballToResize.transform.Translate(new Vector3(0f, -currSliderVal, 0f));
        }*/
        /*if (currSliderVal > prevSliderVal)
        {
            ballToResize.transform.position = new Vector3(ballToResize.transform.position.x,originalHeight+currSliderVal,ballToResize.transform.position.z);
        }
        else if (currSliderVal < prevSliderVal)
        {
            ballToResize.transform.position = new Vector3(ballToResize.transform.position.x, originalHeight - currSliderVal, ballToResize.transform.position.z);
        }*/

        debugField.text = assetHeight.ToString();
            prevSliderVal = currSliderVal;
        
        
    }
}
