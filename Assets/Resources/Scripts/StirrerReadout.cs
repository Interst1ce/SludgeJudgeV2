using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StirrerReadout : MonoBehaviour {
    public TextMeshPro readout;
    public int targetCount;

    string text = "";
    int counter = 0;

    IEnumerator UpdateText() {
        while(counter < targetCount) {
            text = "" + counter;
            counter++;
            readout.text = text;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
