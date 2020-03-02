using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Andeart.ReorderableListEditor;


//[CustomEditor(typeof(StepObject))]
public class StepEditor : ReorderableListEditor {
    protected override void OnEnable() {
        base.OnEnable();
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}

//[CustomPropertyDrawer(typeof(StepObject.Target))]
public class StepDrawer : PropertyDrawer {
    public override void OnGUI(Rect position,SerializedProperty property,GUIContent label) {
        EditorGUI.BeginProperty(position,label,property);

        var targetType = property.FindPropertyRelative("type");
        var objectTarget = property.FindPropertyRelative("objectTarget");
        var sliderTarget = property.FindPropertyRelative("sliderTarget");
        var targetStep = property.FindPropertyRelative("targetStep");
        var targetAnim = property.FindPropertyRelative("targetAnim");
        var targetAudio = property.FindPropertyRelative("targetAudio");
        var extensions = property.FindPropertyRelative("extensions");

        position = EditorGUI.PrefixLabel(position,GUIUtility.GetControlID(FocusType.Passive),label);

        var indent = EditorGUI.indentLevel;
        //EditorGUI.indentLevel = 0;

        var typeRect = new Rect(position.x,position.y + 20,80,16);
        var objTargetRect = new Rect(position.x + 100,position.y + 20,200,16);
        var sldrTargetRect = new Rect(position.x + 100,position.y + 20,200,16);
        var stepRect = new Rect(position.x + 202,position.y,80,16);
        var animRect = new Rect(position.x,position.y + 40,140,16);
        var audioRect = new Rect(position.x + 160,position.y + 40,140,16);
        var extensionRect = new Rect(position.x,position.y + 60,282,16);

        EditorGUI.PropertyField(typeRect,targetType,GUIContent.none);
        if (targetType.enumValueIndex == 0) {
            EditorGUI.PropertyField(objTargetRect,objectTarget,GUIContent.none);
        }else if(targetType.enumValueIndex == 1) {
            EditorGUI.PropertyField(sldrTargetRect,sliderTarget,GUIContent.none);
        }
        EditorGUI.PropertyField(stepRect,targetStep,GUIContent.none);
        EditorGUI.PropertyField(animRect,targetAnim,GUIContent.none);
        EditorGUI.PropertyField(audioRect,targetAudio,GUIContent.none);
        EditorGUI.PropertyField(extensionRect,extensions,GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property,GUIContent label) {
        SerializedObject childObj = new SerializedObject(property.serializedObject.targetObject as StepObject);
        SerializedProperty ite = childObj.GetIterator();

        float totalHeight = base.GetPropertyHeight(property,label) + EditorGUIUtility.standardVerticalSpacing;

        while (ite.NextVisible(true)) {
            //skip properies that have the same x coord as the next property
            
            //if(!(ite.rectValue.x == GetNextProperty(ite).rectValue.x)) totalHeight += EditorGUI.GetPropertyHeight(ite,label);
        }

        return totalHeight;
    }

    public SerializedProperty GetNextProperty(SerializedProperty property) {
        SerializedObject childObj = new SerializedObject(property.serializedObject.targetObject as StepObject);
        SerializedProperty ite = childObj.GetIterator();

        return ite;
    }
}
