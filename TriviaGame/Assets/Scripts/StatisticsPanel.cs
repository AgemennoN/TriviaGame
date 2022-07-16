using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatisticsPanel : MonoBehaviour
{
    [SerializeField] private GameObject ResultPanel;
    private GameManager gameManager;
    private int CorrectAnswers;
    private List<TCategory> diffrentCategories;

    void Start()
    {
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
        GameObject ScoreTxt = new GameObject("ScoreTxt", typeof(Text), typeof(LayoutElement));
        ScoreTxt.transform.parent = ResultPanel.transform;
        ScoreTxt.GetComponent<Text>().text = "Score: " + CorrectAnswers + " / " + StaticGameInfo.totalQuestionNumber;
        ScoreTxt.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        if (diffrentCategories.Count > 1)
        {
            foreach (TCategory category in diffrentCategories)
            {
                GameObject CategoryTxt = new GameObject(category.name, typeof(Text), typeof(LayoutElement));
                CategoryTxt.transform.parent = ResultPanel.transform;
                string text = category.name + ": " + category.correctlyAnswered + " / " + category.totalAnswered;
                CategoryTxt.GetComponent<Text>().text = text;
                CategoryTxt.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

            }
        }

    }

}
