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
        transform.SetAsFirstSibling();  // Change position in hierarchy to prevent blocking other UI elements
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

    // Randomize button places and add them listeners
    private void SetButtons()
    {
        // Random index for correct answer between the four answer buttons
        correctAnswerIdx = Random.Range(0, AnswerBtns.Length);
        string correctAnswer = StaticGameInfo.questionRequest.results[questionIndex].correct_answer;

        // temporary incorrect answers list elements are selected randomly and removed from the list
        // so that all the question answers are ordered randomly each time
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

    // Calls SetActionButton function with CorrectAnswer parameter

    private void CorrectAnswerButtonClick()
    {
        isAnsweredCorrectly = true;

        SetActionButton(TActBtnFunctions.CorrectAnswer);
    }

    // Make the pressed button red, and calls SetActionButton function with WrongAnswer parameter
    private void WrongAnswerButtonClick(int BtnIndex)
    {
        isAnsweredCorrectly = false;

        AnswerBtns[BtnIndex].GetComponent<Image>().color = Color.red;
        SetActionButton(TActBtnFunctions.WrongAnswer);
    }

    // TActBtnFunctions is a type used as parameter in SetActionButton function.
    public enum TActBtnFunctions {TimeIsUp, CorrectAnswer, WrongAnswer, DisplayAnswer };


    public void SetActionButton(TActBtnFunctions ActBtnFunction)
    {
        gameManager.countdownActive = false; // Stops the Countdown.
        switch (ActBtnFunction)
        {
            case TActBtnFunctions.TimeIsUp:
                // If time ends before user answer the question.
                // Write TimeIsUp in action button and make it Red
                // Shows the correct answer and Disable all the buttons
                ActionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Time is Up";
                ActionBtn.GetComponent<Image>().color = Color.red;
                DisableButtons();
                DisplayAnswer();
                gameManager.PlayTimeIsUpAudio();
                
                break;
            case TActBtnFunctions.CorrectAnswer:
                // If user's answer is correct.
                // Write Correct in action button and make it Green
                // Make the correct answer Green and Disable all the buttons
                ActionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Correct";
                ActionBtn.GetComponent<Image>().color = Color.green;
                DisableButtons();
                DisplayAnswer();
                gameManager.PlayCorrectAnswerAudio();

                break;
            case TActBtnFunctions.WrongAnswer:
                // If user's answer is incorrect.
                // Write Wrong! in action button and make it Red
                // Make the correct answer Green and Disable all the buttons
                ActionBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Wrong!";
                ActionBtn.GetComponent<Image>().color = Color.red;
                DisableButtons();
                DisplayAnswer();
                gameManager.PlayWrongAnswerAudio();

                break;
            case TActBtnFunctions.DisplayAnswer:
                // If user pressed ActionButton.
                // Make the action button Red and shows the correct answer then
                // Disable all the buttons
                ActionBtn.GetComponent<Image>().color = Color.red;
                DisableButtons();
                DisplayAnswer();
                gameManager.PlayDisplayAnswerAudio();

                break;
            default:
                break;
        }
    }

    private void DisableButtons()
    {
        foreach (Button AnswerBtn in AnswerBtns)
        {
            AnswerBtn.enabled = false;
        }
        ActionBtn.enabled = false;

        StartCoroutine(NextQuestionCoroutine());
    }

    // After the buttons are disabled NextQuestionBtn is enabled
    // Even if the button is not pressed after "gameManager.nextQuestionWaitDuration" seconds
    // Game moves to the next question itself
    private IEnumerator NextQuestionCoroutine()
    {
        float LeftTime = gameManager.nextQuestionWaitDuration;
        string text = "Next Question In ";  
        // If the current panel is the last question then change the 
        // text in the button, because the next panel is statistics panel not a question panel
        if (questionIndex == StaticGameInfo.totalQuestionNumber - 1)
            text = "Statistics are in ";

        // yields 1 seconds then enable the next panel button
        yield return new WaitForSeconds(1);
        LeftTime -= 1;

        ActionBtn.gameObject.SetActive(false);

        NextQuestionBtn.gameObject.SetActive(true);
        NextQuestionBtn.onClick.AddListener(NextQuestion);

        while (LeftTime >= 0)
        {   // Update the text every second
            NextQuestionBtn.GetComponentInChildren<TextMeshProUGUI>().text = text + ((int)LeftTime) + " Seconds";
            yield return new WaitForSeconds(1);
            LeftTime -= 1;
        }

        // When the time ends move the next panel
        NextQuestion();
    }

    public void NextQuestion()
    {
        // If NextQuestion button is pressed manually have to stop coroutine 
        // because NextQuestionCoroutine() is also calls NextQuestion() function 
        // If the coroutine is not stopped a question might be skipped
        StopCoroutine(NextQuestionCoroutine()); 

        NextQuestionBtn.gameObject.SetActive(false);
        ActionBtn.gameObject.SetActive(true);

        gameManager.LoadNextQuestion();
        gameObject.SetActive(false);
    }

    private void DisplayAnswer()
    {   // Make the correct button green
        AnswerBtns[correctAnswerIdx].GetComponent<Image>().color = Color.green;
    }

}
