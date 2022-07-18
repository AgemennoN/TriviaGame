using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CategoryScreenHandler : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject MainMenuScreen;
    [SerializeField] private GameObject ScrollViewContent;
    [SerializeField] private TMP_InputField InputCategory;
    public Category[] categories;
    public GameObject[] categoryButtons;
    private List<string> categoryNames;

    void Awake()
    {
        SetCategoryButtons();

        EnableSearchedCategories();

    }

    private void SetCategoryButtons()
    {
        categories = APIHelper.ApiFetchCategories().trivia_categories;
        categoryNames = new List<string>();
        categoryButtons = new GameObject[categories.Length];

        for (int i = 0; i < categories.Length; i++)
        {
            categoryButtons[i] = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), ScrollViewContent.transform);
            categoryButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = categories[i].name;
            categoryNames.Add(categories[i].name.ToLower());



            Category category = categories[i];  // To use the index with delegate
            Button button = categoryButtons[i].GetComponent<Button>();
            button.onClick.AddListener(delegate { SelectCategory(category); });

            categoryButtons[i].gameObject.SetActive(false);
        }
    }

    public void EnableSearchedCategories()
    {
        if (InputCategory.text.Trim() == "" || InputCategory.text == null)
        {
            for (int i = 0; i < categoryNames.Count; i++)
            {
                categoryButtons[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < categoryNames.Count; i++)
            {
                if (categoryNames[i].Contains(InputCategory.text.ToLower().Trim()))
                    categoryButtons[i].SetActive(true);
                else
                    categoryButtons[i].SetActive(false);
            }
        }
    }

    public void ClearSearchbar()
    {
        InputCategory.text = "";
    }

    public void SelectCategory(Category category)
    {
        StaticGameInfo.selectedCategory = category;


        StaticGameInfo.questionRequest = APIHelper.ApiFetchQuestionsByCategory(category);
        if (StaticGameInfo.questionRequest.response_code == 0)
        {
            StaticGameInfo.totalQuestionNumber = StaticGameInfo.questionRequest.results.Length;
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("response_code = " + StaticGameInfo.questionRequest.response_code);
            //Give warning or smthng
        }
    }

    public void BackToMainMenu()
    {
        gameObject.SetActive(false);
        MainMenuScreen.gameObject.SetActive(true);
    }

}
