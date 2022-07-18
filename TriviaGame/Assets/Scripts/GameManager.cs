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
    public GameObject questionPanelPrefab;
    [HideInInspector] public GameObject currentQuestionPanel;
    public GameObject statisticsPanelPrefab;
    public GameObject btnPreviousQuestion;
    public GameObject btnNextQuestion;



    public List<GameObject> questionPanels;


    [HideInInspector] public float nextQuestionWaitDuration = 4.0f;
    private float countdownTotalTime = 20.4f;
    private float countdownTimeLeft;
    [HideInInspector] public bool countdownActive;
    [HideInInspector] public bool isGameEnd;

    void Start()
    {
        isGameEnd = false;
        questionPanels = new List<GameObject>();
        StaticGameInfo.currentQuestionIndex = -1;
        LoadNextQuestion();

    }


    void Update()
    {
        if (isGameEnd == false)
        {
            CountDown();
        }

    }

    public void LoadNextQuestion()
    {

        StaticGameInfo.currentQuestionIndex += 1;

        // This if probably should be in action button
        if (StaticGameInfo.currentQuestionIndex == StaticGameInfo.totalQuestionNumber)
        {
            currentQuestionPanel = Instantiate(statisticsPanelPrefab, canvas.transform);
            questionPanels.Add(currentQuestionPanel);
            EndGame();
        }
        else
        {
            currentQuestionPanel = Instantiate(questionPanelPrefab, canvas.transform);
            questionPanels.Add(currentQuestionPanel);

            ResetCountdown();
        }

    }

    public void CountDown()
    {
        if (countdownActive == true)
        {
            countdownTimeLeft -= Time.deltaTime;
            countdownText.text = ((int)countdownTimeLeft).ToString();

            if (countdownTimeLeft <= 0)
            {
                countdownActive = false;
                currentQuestionPanel.GetComponent<QuestionPanel>().SetActionButton(QuestionPanel.TActBtnFunctions.TimeIsUp);
            }
        }
    }

    private void ResetCountdown()
    {
        countdownTimeLeft = countdownTotalTime;
        countdownActive = true;
    }

    public void EndGame()
    {
        isGameEnd = true;
        countdownText.gameObject.SetActive(false);
        btnPreviousQuestion.SetActive(true);
    }

    public void PreviousQuestion()
    {
        StaticGameInfo.currentQuestionIndex--;

        currentQuestionPanel.SetActive(false);
        currentQuestionPanel = questionPanels[StaticGameInfo.currentQuestionIndex];
        currentQuestionPanel.SetActive(true);


        if (StaticGameInfo.currentQuestionIndex == 0)
        {
            btnPreviousQuestion.SetActive(false);
        }

        if (btnNextQuestion.activeSelf == false)
        {
            btnNextQuestion.SetActive(true);
        }
    }
    public void NextQuestion()
    {
        StaticGameInfo.currentQuestionIndex++;

        currentQuestionPanel.SetActive(false);
        currentQuestionPanel = questionPanels[StaticGameInfo.currentQuestionIndex];
        currentQuestionPanel.SetActive(true);


        if (StaticGameInfo.currentQuestionIndex == StaticGameInfo.totalQuestionNumber)
        {
            btnNextQuestion.SetActive(false);
        }

        if (btnPreviousQuestion.activeSelf == false)
        {
            btnPreviousQuestion.SetActive(true);
        }
    }

    public void ReturnMenuBtn()
    {
        //First Ask if the User is sure then load the scene
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





}
