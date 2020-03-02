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
        var targetField = root.Q<ObjectField>("target");
        targetField.objectType = typeof(GameObject);
        root.Q<EnumField>("targetType").Init(StepObject.TargetType.Object);
        //root.Query(classes: new string[] { "test" }).ForEach((preview) => { preview.Add(CreateModuleUI(preview.name)); });
        return root;
    }

    //public VisualElement CreateModuleUI(string moduleName) {

    //}
}
