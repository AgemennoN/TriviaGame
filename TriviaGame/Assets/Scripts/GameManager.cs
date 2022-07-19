using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject ReturnMenuCanvas;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject btnPreviousQuestion;
    [SerializeField] private GameObject btnNextQuestion;
    public GameObject questionPanelPrefab;
    public GameObject statisticsPanelPrefab;
    
    [HideInInspector] public bool isGameEnd;
    private GameObject currentQuestionPanel;
    [HideInInspector] public List<GameObject> questionPanels;

    [HideInInspector] public float nextQuestionWaitDuration = 4.0f;
    [HideInInspector] public bool countdownActive;

    private float countdownTotalTime = 20.4f;
    private float countdownTimeLeft;
    private float oneSec = 0.0f;

    private OptionCanvas optionCanvas;
    [HideInInspector] public bool TglSFx;
    [HideInInspector] public int TimeCounterSoundStart;
    [SerializeField] private AudioSource CountdownSFx;
    [SerializeField] private AudioSource CorrectAnswerSFx;
    [SerializeField] private AudioSource WrongAnswerSFx;
    [SerializeField] private AudioSource DisplayAnswerSFx;
    [SerializeField] private AudioSource TimeIsUpSFx;

    void Start()
    {
        isGameEnd = false;
        questionPanels = new List<GameObject>();
        StaticGameInfo.currentQuestionIndex = -1;

        optionCanvas = GameObject.Find("OptionCanvas").GetComponent<OptionCanvas>();
        
        CheckSFxOption();
        LoadNextQuestion();
    }


    void Update()
    {
        // Count down if the game is not over yet
        if (isGameEnd == false)
        {
            CountDown();
        }
    }

    public void LoadNextQuestion()
    {
        StaticGameInfo.currentQuestionIndex += 1;
        
        // If last qustion is asked, EndGame() and show the results
        if (StaticGameInfo.currentQuestionIndex == StaticGameInfo.totalQuestionNumber)
        {
            currentQuestionPanel = Instantiate(statisticsPanelPrefab, canvas.transform);
            questionPanels.Add(currentQuestionPanel);
            EndGame();
        }
        else // Else bring the next question
        {
            currentQuestionPanel = Instantiate(questionPanelPrefab, canvas.transform);
            questionPanels.Add(currentQuestionPanel);
            ResetCountdown();
        }

    }

    public void CountDown()
    {
        // "countdownActive" is set to false when the question is answered, or when the time ends
        if (countdownActive == true)
        {
            countdownTimeLeft -= Time.deltaTime;
            countdownText.text = ((int)countdownTimeLeft).ToString();

            // Make ticking noise in last "TimeCounterSoundStart" seconds
            if (countdownTimeLeft <= TimeCounterSoundStart)
            {   
                Ticking();
            }
            // When the countdown reaches zero, write Time is Up and show the answer
            // function SetActionButton() with parameter "QuestionPanel.TActBtnFunctions.TimeIsUp" does that
            if (countdownTimeLeft <= 0)
            {
                countdownActive = false;
                currentQuestionPanel.GetComponent<QuestionPanel>().SetActionButton(QuestionPanel.TActBtnFunctions.TimeIsUp);
            }
        }
    }

    // Resets the time, and activate the "countdownActive" 
    private void ResetCountdown()
    {
        countdownTimeLeft = countdownTotalTime;
        countdownActive = true;
    }

    public void EndGame()
    {   // When the game end pervious and next question buttons become enable
        isGameEnd = true;
        countdownText.gameObject.SetActive(false);
        btnPreviousQuestion.SetActive(true);
    }

    public void PreviousQuestion()
    {   // All the panels questions and the statistics panel stores in "questionPanel" List
        // PreviousQuestion enables the previous panel and disable the current panel
        StaticGameInfo.currentQuestionIndex--;

        currentQuestionPanel.SetActive(false);
        currentQuestionPanel = questionPanels[StaticGameInfo.currentQuestionIndex];
        currentQuestionPanel.SetActive(true);

        if (StaticGameInfo.currentQuestionIndex == 0)
        {   // If in the first panel disable the button because there is no previous panel
            btnPreviousQuestion.SetActive(false);
        }

        if (btnNextQuestion.activeSelf == false)
        {
            // If next panel is activated and the "btnNextQuestion" isn't active (n'th panel to n-1'th panel transmission)
            // activate the btnNextQuestion
            btnNextQuestion.SetActive(true);
        }
    }
    public void NextQuestion()
    {   // All the panels questions and the statistics panel stores in "questionPanel" List
        // NextQuestion enables the Next panel and disable the current panel
        StaticGameInfo.currentQuestionIndex++;

        currentQuestionPanel.SetActive(false);
        currentQuestionPanel = questionPanels[StaticGameInfo.currentQuestionIndex];
        currentQuestionPanel.SetActive(true);


        if (StaticGameInfo.currentQuestionIndex == StaticGameInfo.totalQuestionNumber)
        {   // If in the last panel disable the button because there is no next panel
            btnNextQuestion.SetActive(false);
        }

        if (btnPreviousQuestion.activeSelf == false)
        {   // If next panel is activated and the "btnPreviousQuestion" isn't active (1'st panel to 2'nd panel transmission)
            // activate the btnPreviousQuestion
            btnPreviousQuestion.SetActive(true);
        }
    }

    public void ReturnMenuBtn()
    {
        // Ig game is not over yet, First Ask if the User is sure. Then load the scene
        if (isGameEnd == false)
        {
            ReturnMenuCanvas.SetActive(true);
        }
        else if (isGameEnd == true)
        {
            ReturnMenu();
        }
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void Ticking()
    {   // Make ticking noise in every second
        oneSec -= Time.deltaTime;
        if (oneSec <= 0)
        {
            oneSec = 1.0f;
            if (TglSFx == true)
            {
                CountdownSFx.Play();
            }
        }
    }

    public void CheckSFxOption()
    {   // Fetches TglSFx and TimeCounterSoundStart from Option Menu
        TglSFx = optionCanvas.TglSFx;
        int.TryParse(optionCanvas.ClockSoundStartAtInput.text, out TimeCounterSoundStart);
    }


    public void PlayCorrectAnswerAudio()
    {
        if (TglSFx == true)
            CorrectAnswerSFx.Play();
    }

    public void PlayWrongAnswerAudio()
    {
        if (TglSFx == true)
            WrongAnswerSFx.Play();
    }

    public void PlayDisplayAnswerAudio()
    {
        if (TglSFx == true)
            DisplayAnswerSFx.Play();
    }

    public void PlayTimeIsUpAudio()
    {
        if (TglSFx == true)
            TimeIsUpSFx.Play();
    }


}
