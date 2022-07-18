using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class StatisticsPanel : MonoBehaviour
{
    [SerializeField] private GameObject ResultPanel;
    [SerializeField] private TextMeshProUGUI ScoreTxt;
    [SerializeField] private TextMeshProUGUI CategoryStatsTextPrefab;
    [SerializeField] private Button PlayAgainBtn;
    private GameManager gameManager;
    private int CorrectAnswers;
    private List<TCategory> diffrentCategories;

    void Start()
    {
        transform.SetAsFirstSibling();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        CheckCategories();
        CheckCorrectAnswers();

        DisplayResults();
    }

    private void CheckCategories()
    {
        diffrentCategories = new List<TCategory>();
        List<string> categories = new List<string>();   // To check if the category with the same name exist easly
        for (int i = 0; i < StaticGameInfo.totalQuestionNumber; i++)
        {
            QuestionPanel QP = gameManager.questionPanels[i].GetComponent<QuestionPanel>();

            int cateIndex = categories.FindIndex(cat => cat == QP.question.category);
            if (cateIndex == -1) // New category
            {
                categories.Add(QP.question.category);   // Add category name to list

                TCategory category = new TCategory(QP); // Create a new category class that stores question numbers.
                diffrentCategories.Add(category);
            }
            else                // Existing categort
            {
                diffrentCategories[cateIndex].totalAnswered += 1;
                if (QP.isAnsweredCorrectly)
                {
                    diffrentCategories[cateIndex].correctlyAnswered += 1;
                }
            }
        }
    }

    public class TCategory
    {
        public string name;
        public int totalAnswered;
        public int correctlyAnswered;
        public TCategory(QuestionPanel QP)
        {
            name = QP.question.category;
            totalAnswered = 1;
            if (QP.isAnsweredCorrectly)             // Checks If the question is answered correctly
                correctlyAnswered = 1;
            else
                correctlyAnswered = 0;
        }
    }

    private void CheckCorrectAnswers()
    {
        CorrectAnswers = 0;
        foreach (TCategory category in diffrentCategories)
        {
            CorrectAnswers += category.correctlyAnswered;
        }
    }

    private void DisplayResults()
    {
        string tempText;
        if (diffrentCategories.Count > 1)   // That means Game Played with random categories
        {
            ScoreTxt.text = CorrectAnswers + " / " + StaticGameInfo.totalQuestionNumber;
            foreach (TCategory category in diffrentCategories)
            {
                TextMeshProUGUI CategoryTxt = Instantiate(CategoryStatsTextPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), ResultPanel.transform);
                tempText = category.name + ": " + category.correctlyAnswered + " / " + category.totalAnswered;
                CategoryTxt.text = tempText;
            }

            PlayAgainBtn.onClick.AddListener(PlayAgainWithRandomQuestions);
            tempText = "Play Again with Random Categories";
            PlayAgainBtn.GetComponentInChildren<TextMeshProUGUI>().text = tempText;
        }
        else
        {
            ScoreTxt.text = "In Category: " + StaticGameInfo.selectedCategory.name + "\n";
            ScoreTxt.text += CorrectAnswers + " / " + StaticGameInfo.totalQuestionNumber;

            PlayAgainBtn.onClick.AddListener(PlayAgainWithSelectedCategory);
            tempText = "Play Again with Category: " + StaticGameInfo.selectedCategory.name;
            PlayAgainBtn.GetComponentInChildren<TextMeshProUGUI>().text = tempText;
        }

    }

    private void PlayAgainWithSelectedCategory()
    {
        StaticGameInfo.questionRequest = APIHelper.ApiFetchQuestionsByCategory(StaticGameInfo.selectedCategory);
        if (StaticGameInfo.questionRequest.response_code == 0)
        {
            StaticGameInfo.totalQuestionNumber = StaticGameInfo.questionRequest.results.Length;
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("response_code = " + StaticGameInfo.questionRequest.response_code);
        }
    }

    private void PlayAgainWithRandomQuestions()
    {
        StaticGameInfo.questionRequest = APIHelper.ApiFetchRandomQuestions();

        if (StaticGameInfo.questionRequest.response_code == 0)
        {
            StaticGameInfo.totalQuestionNumber = StaticGameInfo.questionRequest.results.Length;
            SceneManager.LoadScene("GameScene");

        }
        else
        {
            Debug.Log("response_code = " + StaticGameInfo.questionRequest.response_code);
        }
    }

}
