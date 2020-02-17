using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question",menuName = "ScriptableObjects/Question",order = 4)]
public class Question : ScriptableObject {
    public string question;
    public int answer;
    public string[] choices;
    [Header("Answer layout prefab must have same number of buttons as number of choices")]
    public GameObject answerLayout;
}