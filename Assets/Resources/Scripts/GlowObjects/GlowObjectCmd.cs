using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GlowObjectCmd : MonoBehaviour {
    public Color GlowColor;
    public float LerpFactor = 10;

    public Renderer[] Renderers {
        get;
        private set;
    }

    public Color CurrentColor {
        get { return _currentColor; }
    }

    private Color _currentColor;
    private Color _targetColor;

    void Start() {
        Renderers = GetComponentsInChildren<Renderer>();
        GlowController.RegisterObject(this);
    }

    private void TurnOn() {
        _targetColor = GlowColor;
        enabled = true;
    }

    private void TurnOff() {
        _targetColor = Color.black;
        enabled = true;
    }
    
    IEnumerator GlowPulse() {
        int i = 0;
        while (i < 5) {
            //Debug.Log("Turning glow on");
            TurnOn();
            yield return new WaitForSecondsRealtime(1);
            //Debug.Log("Turning glow off");
            TurnOff();
            yield return new WaitForSecondsRealtime(1);
            //StartCoroutine("GlowPulse");
            i++;
        }
    }

    /// <summary>
    /// Update color, disable self if we reach our target color.
    /// </summary>
    private void Update() {
        _currentColor = Color.Lerp(_currentColor,_targetColor,Time.deltaTime * LerpFactor);

        if (_currentColor.Equals(_targetColor)) {
            enabled = false;
        }
    }
}
