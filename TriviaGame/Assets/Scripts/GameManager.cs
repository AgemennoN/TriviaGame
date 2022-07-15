using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    public GameObject questionPanelPrefab;
    public GameObject currentQuestionPanel;

    public float nextQuestionWaitDuration;
    public float countdown;
    public List<GameObject> questionPanels;


    void Start()
    {
        LoadNextQuestion();
    }

    
    void Update()
    {
        // SHOW TIME
    }

    public void LoadNextQuestion()
    {
        if (questionPanels == null)
        {
            questionPanels = new List<GameObject>();
            StaticGameInfo.currentQuestionIndex = -1;
        }

        StaticGameInfo.currentQuestionIndex += 1;
        currentQuestionPanel = Instantiate(questionPanelPrefab,canvas.transform);
        questionPanels.Add(currentQuestionPanel);

        //START TIMER

    }


    public void BackToMainMenu()
    {
        //FÝrst Ask if the User is sure then load the scene
        SceneManager.LoadScene("MainMenuScene");
    }





}
