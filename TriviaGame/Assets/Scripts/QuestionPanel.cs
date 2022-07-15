using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{
    [SerializeField] private Text questionText;
    public Button[] AnswerBtns;

    private GameManager gameManager;

    private int questionIndex;
    private Question question;
    public bool isAnsweredCorrectly;
    int correctAnswerIdx;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        questionIndex = StaticGameInfo.currentQuestionIndex;
        question = StaticGameInfo.questionRequest.results[questionIndex];


        questionText.text = question.question;
        //SetTitle();
        SetButtons();
    }


    private void SetButtons()
    {
        correctAnswerIdx = Random.Range(0, AnswerBtns.Length);
        string correctAnswer = StaticGameInfo.questionRequest.results[questionIndex].correct_answer;
       
        List<string> incorrectAnswers = new List<string>(AnswerBtns.Length - 1);
        incorrectAnswers.AddRange(StaticGameInfo.questionRequest.results[questionIndex].incorrect_answers);

        int i = 3; // i = AnswerBtns.Length - 1 since there are 4 answers,  i = 3
        while (i >= 0)
        {
            if (i == correctAnswerIdx)
            {
                AnswerBtns[i].onClick.AddListener(delegate { CorrectAnswerButtonClick(); });
                AnswerBtns[i].gameObject.GetComponentInChildren<Text>().text = correctAnswer;
            }
            else
            {
                int randomIncorrectAnswerIdx = Random.Range(0, incorrectAnswers.Count);
                AnswerBtns[i].onClick.AddListener(delegate { WrongAnswerButtonClick(); });
                AnswerBtns[i].gameObject.GetComponentInChildren<Text>().text = incorrectAnswers[randomIncorrectAnswerIdx];
                incorrectAnswers.RemoveAt(randomIncorrectAnswerIdx);
            }
            i--;
        }
    }

    private void CorrectAnswerButtonClick()
    {
        Debug.Log("YEEEEEEEEEEEEEEEEEEEEEEEEEEEEY");
        isAnsweredCorrectly = true;

        // Make the Button Green

        // SetActionButton();

        gameObject.SetActive(false);
        gameManager.LoadNextQuestion();
    }

    private void WrongAnswerButtonClick()
    {
        Debug.Log("NOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        isAnsweredCorrectly = false;


        // Make THe Button Red
        // Make the Button[CorrectButtonidx] Green

        // SetActionButton();


        gameManager.LoadNextQuestion();
        gameObject.SetActive(false);
    }

    public void DisplayAnswer()
    {
        // Make the Button[CorrectButtonidx] Green

        // SetActionButton(); 
        // Countdown to next question


        gameObject.SetActive(false);
        gameManager.LoadNextQuestion();

    }

}
