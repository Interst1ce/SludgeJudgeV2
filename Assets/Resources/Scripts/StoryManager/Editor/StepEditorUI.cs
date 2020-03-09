using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

//[CustomEditor(typeof(StepObject))]
public class StepEditorUI : Editor {
    VisualElement rootElement;
    VisualTreeAsset visualTree;

    public void OnEnable() {
        rootElement = new VisualElement();
        visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Resources/Scripts/StoryManager/Editor/StepEditorUI.uxml");
    }

    public override VisualElement CreateInspectorGUI() {
        var root = rootElement;
        Debug.Log(visualTree);
        root.Clear();
        visualTree.CloneTree(root);

        var stepTargetField = root.Q<IntegerField>("stepTarget");
        var targetTypeField = root.Q<EnumField>("targetType");
        var targetField = root.Q<ObjectField>("target");
        var targetAnimField = root.Q<ObjectField>("targetAnim");
        targetAnimField.objectType = typeof(AnimationClip);
        var targetAudioField = root.Q<ObjectField>("targetAudio");
        targetAudioField.objectType = typeof(AudioClip);
        var targetExtensionsField = root.Q<PropertyField>("targetExtensions");

        ScriptableObject selObj = Selection.activeObject as ScriptableObject;
        SerializedObject serObj = new SerializedObject(selObj);
        root.Bind(serObj);

        var newTarget = serObj.FindProperty("newTarget");

        var stepTarget = newTarget.FindPropertyRelative("targetStep");
        var tType = newTarget.FindPropertyRelative("type");
        var targetAnim = newTarget.FindPropertyRelative("targetAnim");
        var targetAudio = newTarget.FindPropertyRelative("targetAudio");
        var targetExtensions = newTarget.FindPropertyRelative("extensions");

        UpdateTargetField((StepObject.TargetType)tType.enumValueIndex,targetField);
        stepTargetField.value = stepTarget.intValue;
        targetAnimField.value = targetAnim.objectReferenceValue;
        targetAudioField.value = targetAudio.objectReferenceValue;

        stepTargetField.RegisterValueChangedCallback(evt => {
            stepTarget.intValue = evt.newValue;
            serObj.ApplyModifiedProperties();
        });

        StepObject.TargetType targetType = 0;
        targetTypeField.Init((StepObject.TargetType)tType.enumValueIndex);
        targetTypeField.RegisterValueChangedCallback(evt => {
            targetType = (StepObject.TargetType)evt.newValue;
            tType.enumValueIndex = (int)targetType;
            UpdateTargetField(targetType,targetField);
            serObj.ApplyModifiedProperties();
            //Debug.Log("" + tType.enumValueIndex);
        });

        targetAnimField.RegisterValueChangedCallback(evt => {
            targetAnim.objectReferenceValue = evt.newValue;
            serObj.ApplyModifiedProperties();
        });

        targetAudioField.RegisterValueChangedCallback(evt => {
            targetAudio.objectReferenceValue = evt.newValue;
            serObj.ApplyModifiedProperties();
        });

        return root;
    }

    public void UpdateTargetField(StepObject.TargetType targetType, ObjectField targetField) {
        switch (targetType) {
            case StepObject.TargetType.Object:
                targetField.objectType = typeof(GameObject);
                targetField.label = "Target Object";
                break;
            case StepObject.TargetType.Slider:
                targetField.objectType = typeof(Slider);
                targetField.label = "Target Slider";
                break;
            default:
                break;
        }
    }

    //public VisualElement CreateModuleUI(string moduleName) {

    //}
}
