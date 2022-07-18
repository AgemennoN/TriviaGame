using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI questionRankText;
    [SerializeField] private TextMeshProUGUI questionText;
    public Button[] AnswerBtns;
    public Button ActionBtn;
    public Button NextQuestionBtn;

    private GameManager gameManager;

    private int questionIndex;
    [HideInInspector] public Question question;
    public bool isAnsweredCorrectly;
    int correctAnswerIdx;

    private void Start()
    {
        transform.SetAsFirstSibling();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        questionIndex = StaticGameInfo.currentQuestionIndex;
        question = StaticGameInfo.questionRequest.results[questionIndex];


        SetTexts();
        SetButtons();
    }



    private void SetTexts()
    {
        categoryText.text = question.category;
        questionRankText.text = "Question " + (questionIndex + 1) + " / " + StaticGameInfo.totalQuestionNumber;
        questionText.text = question.question;

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
                AnswerBtns[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = correctAnswer;
            }
            else
            {
                int randomIncorrectAnswerIdx = Random.Range(0, incorrectAnswers.Count);
                int BtnIndex = i; // to Use the index in delegate
                AnswerBtns[BtnIndex].onClick.AddListener(delegate { WrongAnswerButtonClick(BtnIndex); });
                AnswerBtns[BtnIndex].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = incorrectAnswers[randomIncorrectAnswerIdx];
                incorrectAnswers.RemoveAt(randomIncorrectAnswerIdx);
            }
            i--;
        }

        ActionBtn.onClick.AddListener(delegate { SetActionButton(TActBtnFunctions.DisplayAnswer); });
    }

    private void CorrectAnswerButtonClick()
    {
        isAnsweredCorrectly = true;

        SetActionButton(TActBtnFunctions.CorrectAnswer);
    }

    private void WrongAnswerButtonClick(int BtnIndex)
    {
        isAnsweredCorrectly = false;

        AnswerBtns[BtnIndex].GetComponent<Image>().color = Color.red;

        SetActionButton(TActBtnFunctions.WrongAnswer);
    }

    public enum TActBtnFunctions {TimeIsUp, CorrectAnswer, WrongAnswer, DisplayAnswer };

    public void SetActionButton(TActBtnFunctions ActBtnFunction)
    {
        switch (ActBtnFunction)
        {
            case TActBtnFunctions.TimeIsUp:
                ActionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Time is Up";
                ActionBtn.GetComponent<Image>().color = Color.red;

                DisableButtons();
                DisplayAnswer();

                break;
            case TActBtnFunctions.CorrectAnswer:
                ActionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Correct";
                ActionBtn.GetComponent<Image>().color = Color.green;
                DisableButtons();
                DisplayAnswer();

                break;
            case TActBtnFunctions.WrongAnswer:
                ActionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Wrong!";
                ActionBtn.GetComponent<Image>().color = Color.red;
                DisableButtons();
                DisplayAnswer();

                break;
            case TActBtnFunctions.DisplayAnswer:
                ActionBtn.GetComponent<Image>().color = Color.red;
                DisableButtons();
                DisplayAnswer();

                break;
            default:
                break;
        }
    }

    private void DisableButtons()
    {
        gameManager.countdownActive = false; // Stops Timer to answer the question.
        foreach (Button AnswerBtn in AnswerBtns)
        {
            AnswerBtn.enabled = false;
        }
        ActionBtn.enabled = false;

        StartCoroutine(NextQuestionCoroutine());
    }
    
    
    private IEnumerator NextQuestionCoroutine()
    {
        float LeftTime = gameManager.nextQuestionWaitDuration;
        string text = "Next Question In ";  
        if (questionIndex == StaticGameInfo.totalQuestionNumber - 1)
        {
            text = "Statistics are in ";
        }

        yield return new WaitForSeconds(1);
        LeftTime -= 1;

        ActionBtn.gameObject.SetActive(false);

        NextQuestionBtn.gameObject.SetActive(true);
        NextQuestionBtn.onClick.AddListener(NextQuestion);

        while (LeftTime >= 0)
        {
            NextQuestionBtn.GetComponentInChildren<TextMeshProUGUI>().text = text + ((int)LeftTime) + " Seconds";
            yield return new WaitForSeconds(1);
            LeftTime -= 1;
        }

        NextQuestion();
    }

    public void NextQuestion()
    {
        StopCoroutine(NextQuestionCoroutine()); // Button might be pressed manually
        NextQuestionBtn.gameObject.SetActive(false);
        ActionBtn.gameObject.SetActive(true);

        gameManager.LoadNextQuestion();
        gameObject.SetActive(false);
    }

    private void DisplayAnswer()
    {
        AnswerBtns[correctAnswerIdx].GetComponent<Image>().color = Color.green;
    }

}
