using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;

    public bool loadNextQuestion;
    public bool isAnsweringQuestion = false;
    public float fillFraction;

    float timerValue;
    float currentTimer;

    // Update is called once per frame
    void Update(){
        UpdateTimer();
    }

    public void CancelTimer() {
        timerValue = 0;
    }

    void UpdateTimer() {
        timerValue -= Time.deltaTime;

        if(timerValue> 0) {
            fillFraction = timerValue / currentTimer;
        }
        else {
            currentTimer = isAnsweringQuestion ? timeToShowCorrectAnswer : timeToCompleteQuestion;
            isAnsweringQuestion = !isAnsweringQuestion;
            timerValue = currentTimer;

            //Se e' finito il timer per mostrare la risposta giusta
            if(isAnsweringQuestion == true) {
                loadNextQuestion = true;
            }
        }
        //Debug.Log(isAnsweringQuestion + ": " + timerValue + " = " + fillFraction);
    }
}
