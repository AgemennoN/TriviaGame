using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Text countdownText;
    public GameObject questionPanelPrefab;
    public GameObject currentQuestionPanel;


    public List<GameObject> questionPanels;


    [HideInInspector] public float nextQuestionWaitDuration = 4.0f;
    private float countdownTotalTime = 10.4f;
    private float countdownTimeLeft;
    private bool countdownActive;
    public bool isGameEnd;

    void Start()
    {
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
            EndGame();
            return;
        }

        currentQuestionPanel = Instantiate(questionPanelPrefab,canvas.transform);
        questionPanels.Add(currentQuestionPanel);

        ResetCountdown();

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
        // Statics screen
        // Previous Question button
        // Next Question button
    }


    public void BackToMainMenu()
    {
        //First Ask if the User is sure then load the scene
        SceneManager.LoadScene("MainMenuScene");
    }





}
