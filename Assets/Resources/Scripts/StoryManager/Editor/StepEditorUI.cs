using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(StepObject))]
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

        ScriptableObject selObj = Selection.activeObject as ScriptableObject;
        SerializedObject serObj = new SerializedObject(selObj);
        root.Bind(serObj);

        StepObject.TargetType targetType = 0;
        var tType = serObj.FindProperty("target").FindPropertyRelative("type");
        var targetTypeField = root.Q<EnumField>("targetType");
        targetTypeField.Init((StepObject.TargetType)tType.enumValueIndex);
        targetTypeField.RegisterValueChangedCallback(evt => {
            targetType = (StepObject.TargetType)evt.newValue;
            Debug.Log("" + (int)targetType);
            tType.enumValueIndex = (int)targetType;
            //Debug.Log("" + tType.enumValueIndex);
        });

        var targetField = root.Q<ObjectField>("target");
        targetField.objectType = typeof(GameObject);

        //root.Query(classes: new string[] { "test" }).ForEach((preview) => { preview.Add(CreateModuleUI(preview.name)); });
        return root;
    }

    

    //public VisualElement CreateModuleUI(string moduleName) {

    //}
}
