using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
//SO sta per Scriptable Objects
public class QuestionSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string question = "Enter new question text here";
    [SerializeField] string[] answer = new string[4];
    [Range(0, 3)]
    [SerializeField] int correctAnswerIndex;

    public string GetQuestion() {
        return question;
    }

    public string GetAnswer(int index) {
        return answer[index];
    }

    public int GetCorrectAnswerIndex() {
        return correctAnswerIndex;
    }
}