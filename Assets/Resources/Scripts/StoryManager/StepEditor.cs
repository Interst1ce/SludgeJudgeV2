using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Andeart.ReorderableListEditor;


//[CustomEditor(typeof(StepObject))]
public class StepEditor : ReorderableListEditor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();

        serializedObject.ApplyModifiedProperties();
    }
}
