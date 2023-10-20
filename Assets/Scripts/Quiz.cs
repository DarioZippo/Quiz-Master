using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly;

    [Header("Button colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    
    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;

    bool firstQuestion = true;

    // Start is called before the first frame update
    void Awake() {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.minValue = 0;
    }

    private void Update() {
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion) {
            if(progressBar.value == progressBar.maxValue) {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;

            firstQuestion = false;
        }
        //Se sto mostrando la risposta giusta per il timer scaduto
        else if(!firstQuestion) {
            if(!hasAnsweredEarly && !timer.isAnsweringQuestion) {
                DisplayScore();
                //Non faccio l'highlight su nessuna risposta
                DisplayAnswer(-1);
                SetButtonState(false);
            }
        }
    }

    public void OnAnswerSelected(int index) {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        //Disabilito dopo aver dato la risposta
        SetButtonState(false);
        timer.CancelTimer();
        DisplayScore();
    }

    private void DisplayScore() {
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    private void DisplayAnswer(int index) {
        Image buttonImage;

        if(index == currentQuestion.GetCorrectAnswerIndex()) {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was;\n" + correctAnswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void GetNextQuestion() {
        if(questions.Count > 0) {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionSeen();
        }
    }

    private void GetRandomQuestion() {
        int index = UnityEngine.Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if(questions.Contains(currentQuestion)) {
            questions.Remove(currentQuestion);
        }
    }

    private void DisplayQuestion() {
        questionText.text = currentQuestion.GetQuestion();

        TextMeshProUGUI buttonText;
        for(int i = 0; i < answerButtons.Length; i++) {
            buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state) {
        for(int i = 0; i < answerButtons.Length; i++) {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites() {
        for(int i = 0; i < answerButtons.Length; i++) {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
